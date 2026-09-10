using System.Globalization;
using api.Tests.TestData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace api.Tests.Infrastructure;

/// <summary>
/// Hosts the API against an in-memory database seeded exactly once.
///
/// The API is read-only — every route is a GET and nothing mutates data — so there is no reason
/// to tear down and re-seed between tests. Sharing one seeded host across the whole suite
/// replaces ~30 host boots and ~30 full re-seeds with one of each.
/// </summary>
public class ApiColombiaFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"TestDatabase_{Guid.NewGuid()}";
    private readonly Lock _seedLock = new();
    private bool _seeded;

    /// <summary>
    /// Requests allowed per minute by the shared per-IP limiter. Effectively unlimited here so a
    /// growing suite never trips it; <see cref="RateLimitedFactory"/> overrides it to a handful.
    /// </summary>
    protected virtual int PermitLimit => 1_000_000;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("RateLimiting:PermitLimit", PermitLimit.ToString(CultureInfo.InvariantCulture));

        builder.ConfigureServices(services =>
        {
            // 1. Remove the production DbContext registration
            services.RemoveAll(typeof(DbContextOptions<DBContext>));

            // 2. Create a NEW internal service provider for EF Core.
            // This is the key to preventing the provider conflict with Npgsql.
            var efInternalServiceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // 3. Register the DbContext using the isolated provider
            services.AddDbContext<DBContext>(options =>
            {
                options.UseInMemoryDatabase(_databaseName);
                options.UseInternalServiceProvider(efInternalServiceProvider);
            });
        });
    }

    /// <summary>Seeds on first call and does nothing thereafter. Safe to call from every test.</summary>
    public void EnsureSeeded()
    {
        lock (_seedLock)
        {
            if (_seeded)
            {
                return;
            }

            using var scope = Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DBContext>();
            dbContext.Database.EnsureCreated();
            TestSeeder.Seed(dbContext);
            _seeded = true;
        }
    }
}

/// <summary>
/// A factory deliberately kept out of the shared collection, for tests whose assertions depend on
/// per-host state that the rest of the suite's traffic would pollute — recorded request metrics,
/// rate-limit counters.
/// </summary>
public class IsolatedFactory : ApiColombiaFactory;

/// <summary>
/// Drops the limit low enough to prove the limiter in a handful of requests instead of the ~120
/// the rate-limit tests used to fire.
/// </summary>
public sealed class RateLimitedFactory : ApiColombiaFactory
{
    public const int Permits = 5;

    protected override int PermitLimit => Permits;
}
