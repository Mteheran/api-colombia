using System.Globalization;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using api;
using api.Routes;
using Microsoft.AspNetCore.Http.Json;
using api.Utils;
using api.Utils.Metrics;
using System.Net;
using System.IO.Compression;
using api.Const;
using api.Mcp;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(static options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Version = VersionInfo.CurrentVersion,
        Title = "API Colombia",
        Description = "Open and free API that contains general information about Colombia",
        TermsOfService = new Uri("https://github.com/Mteheran/api-colombia"),
        Contact = new OpenApiContact
        {
            Name = "Miguel Teheran",
            Url = new Uri("https://mteheran.dev")
        }
    });
});

// Response compression (Brotli + Gzip). This is the single biggest lever for
// outbound "data out": JSON API responses and text assets are shrunk ~70-90%.
builder.Services.AddResponseCompression(static options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[]
    {
        "application/json",
        "image/svg+xml",
    });
});
builder.Services.Configure<BrotliCompressionProviderOptions>(static o => o.Level = CompressionLevel.Optimal);
builder.Services.Configure<GzipCompressionProviderOptions>(static o => o.Level = CompressionLevel.Optimal);

builder.Services.AddOutputCache(static options =>
{
    options.AddBasePolicy(static builder =>
        builder.Expire(TimeSpan.FromDays(7))
               // Vary the cache by every query string value so ?sortBy=, ?page=,
               // ?pageSize= etc. produce distinct cache entries.
               .SetVaryByQuery("*"));
});

// Shared per-IP rate limiting for the public resource groups that opt in via
// Util.PublicRateLimitPolicy (Holiday, City, Department, ...). Responses are
// cached aggressively, so a client that respects the cache never approaches the
// limit; only per-second bursts are cut. All opted-in groups share one per-IP budget.
builder.Services.AddRateLimiter(static options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy(Util.PublicRateLimitPolicy, static httpContext =>
    {
        // Partition by client IP. If the API runs behind a reverse proxy
        // (Azure App Service, Cloudflare) consider partitioning by the first
        // X-Forwarded-For value instead, since RemoteIpAddress may be the proxy.
        var partitionKey = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetSlidingWindowLimiter(partitionKey, static _ =>
            new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 6,   // 10-second segments → gradual expiry
                QueueLimit = 0,
                AutoReplenishment = true
            });
    });

    options.OnRejected = static async (context, ct) =>
    {
        // Always advertise Retry-After. The sliding window limiter doesn't attach
        // RetryAfter metadata, so fall back to the segment length (Window /
        // SegmentsPerWindow = 10s) — the point at which the oldest segment frees a permit.
        var retrySeconds = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
            ? (int)retryAfter.TotalSeconds
            : 10;

        context.HttpContext.Response.Headers.RetryAfter =
            retrySeconds.ToString(CultureInfo.InvariantCulture);

        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(
            new { message = "Rate limit exceeded. Please retry later." }, ct);
    };
});

builder.Services.AddCors(static options =>
{
    options.AddPolicy(name: Util.CorsPolicyName,
                      static policy  =>
                      {
                          policy.WithMethods("GET");
                          policy.AllowAnyOrigin();
                      });
});

builder.Configuration.AddEnvironmentVariables();

// Prefer environment variable override if provided
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                        ?? builder.Configuration["DATABASE_CONNECTION"];

builder.Services.AddNpgsql<DBContext>(connectionString);

builder.Services.Configure<JsonOptions>(static options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

// Live-data MCP server hosted alongside the REST API (see api/Mcp).
builder.AddApiColombiaMcp();

// Lightweight public request analytics: in-memory collector on the hot path,
// with a background service flushing hourly rollups to Postgres.
builder.Services.AddSingleton<RequestMetricsCollector>();
builder.Services.AddHostedService<RequestMetricsFlusher>();

var app = builder.Build();

InfoRoutes.RegisterInfoAPI(app);
MetricsRoutes.RegisterMetricsAPI(app);
CountryRoutes.RegisterCountryAPI(app);
RegionRoutes.RegisterRegionAPI(app);
DepartmentRoutes.RegisterDepartmentAPI(app);
CityRoutes.RegisterCityAPI(app);
PresidentRoutes.RegisterPresidentApi(app);
TouristAttractionRoutes.RegisterTouristAttractionAPI(app);
CategoryNaturalAreaRoutes.RegisterCategoryNaturalAreaAPI(app);
NaturalAreaRoutes.RegisterNaturalAreaAPI(app);
MapsRoutes.RegisterCountryAPI(app);
InvasiveSpecieRoutes.RegisterInvasiveSpecieAPI(app);
NativeCommunityRoutes.RegisterNativeCommunityAPI(app);
IndigenousReservationRoutes.RegisterIndigenousReservationAPI(app);
AirportRoutes.RegisterAirportAPI(app);
ConstitutionArticleRoutes.RegisterConstitutionArticleAPI(app);
RadioRoutes.RegisterRadioRoutesAPI(app);
HolidayRoutes.RegisterHolidayAPI(app);
TypicalDishRoutes.RegisterTypicalDishAPI(app);
TraditionalFairAndFestivalRoutes.RegisterTraditionalFairAndFestivalAPI(app);
IntangibleHeritageRoutes.RegisterIntangibleHeritageAPI(app);
HeritageCityRoutes.RegisterHeritageCityAPI(app);
PostalCodeRoutes.RegisterPostalCodeAPI(app);
UrbanCenterRoutes.RegisterUrbanCenterAPI(app);

// Record request analytics as the outermost middleware so its counting stream
// wraps the compressed output and measures the actual bytes leaving the server.
// Purely in-memory, O(1) per request.
app.UseMiddleware<RequestMetricsMiddleware>();

// Compress everything downstream (static files + JSON API responses).
app.UseResponseCompression();

app.UseStatusCodePages(static context => {
    var request = context.HttpContext.Request;
    var response = context.HttpContext.Response;

    if (response.StatusCode == (int)HttpStatusCode.NotFound
    && request.Path.Value != null
    && !request.Path.Value.Contains("/api"))
    {
        response.Redirect("/");
    }

    return Task.CompletedTask;
});

app.UseDefaultFiles();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = static ctx =>
    {
        // Far-future caching for static assets (images/fonts/css/js). The
        // static-file middleware still emits ETag/Last-Modified, so browsers
        // get 304s once max-age expires.
        ctx.Context.Response.Headers[HeaderNames.CacheControl] = "public, max-age=604800"; // 7 days
    }
});
app.UseCors(Util.CorsPolicyName);
// Placed before UseOutputCache so every request (including cache hits) counts
// against the limit — bursts consume egress bandwidth even when served from cache.
app.UseRateLimiter();
app.UseSwagger();
app.UseSwaggerUI();
app.UseOutputCache();

// Streamable HTTP MCP endpoint at /api/v1/mcp.
app.MapApiColombiaMcp();

// Browser-based MCP tester (wwwroot/mcp-inspector.html) served at /mcp.
app.MapGet("/mcp", (IWebHostEnvironment env) =>
    Results.File(Path.Combine(env.WebRootPath, "mcp-inspector.html"), "text/html"))
   .ExcludeFromDescription();

// Public metrics dashboard (wwwroot/metrics.html) served at /metrics; it reads
// the JSON API at /api/v1/metrics.
app.MapGet("/metrics", (IWebHostEnvironment env) =>
    Results.File(Path.Combine(env.WebRootPath, "metrics.html"), "text/html"))
   .ExcludeFromDescription();

app.Run();

public partial class Program{

}
