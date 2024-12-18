using System.Text.Json.Serialization;
using api;
using api.Routes;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http.Json;
using api.Utils;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(static options =>
{
    options.EnableAnnotations();
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = Version.CurrentVersion,
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

builder.Services.AddOutputCache(static options =>
{
    options.AddBasePolicy(static builder => 
        builder.Expire(TimeSpan.FromDays(7)));
});

builder.Services.AddCors(static options =>
{
    options.AddPolicy(name: "corsApiColombia",
                      static policy  =>
                      {
                          policy.WithMethods("GET");
                          policy.AllowAnyOrigin();
                      });
});


builder.Configuration.AddEnvironmentVariables();

builder.Services.AddNpgsql<DBContext>(
    builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.Configure<JsonOptions>(static options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.SerializerOptions.Converters.Add(new DateOnlyJsonConverter());
});

var app = builder.Build();

InfoRoutes.RegisterInfoAPI(app);
CountryRoutes.RegisterCountryAPI(app);
RegionRoutes.RegisterRegionAPI(app);
DepartmentRoutes.RegisterDepartmentAPI(app);
CityRoutes.RegisterCityAPI(app);
PresidentRoutes.RegisterPresidentApi(app);
TuristicAttactionRoutes.RegisterTuristicAttactionAPI(app);
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
app.UseStaticFiles();
app.UseCors("corsApiColombia");
app.UseSwagger();
app.UseSwaggerUI();
app.UseOutputCache();

app.Run();
