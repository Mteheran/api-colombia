using api;
using Microsoft.EntityFrameworkCore;

namespace api.Tests.Unit;

/// <summary>
/// Guards against LINQ that cannot be translated to SQL by the real PostgreSQL
/// provider. The integration tests use the EF Core InMemory provider, which
/// evaluates queries on the client and therefore silently accepts expressions
/// (like <c>ToUpperInvariant()</c>) that blow up at runtime against Npgsql with
/// "could not be translated". Here we configure the actual Npgsql provider and
/// force translation via <see cref="EntityFrameworkQueryableExtensions.ToQueryString"/>,
/// which compiles the query to SQL without ever opening a database connection.
/// </summary>
public class QueryTranslationTests
{
    private static DBContext CreateNpgsqlContext()
    {
        // A syntactically valid connection string is enough: ToQueryString()
        // compiles the query to SQL but never connects to the database.
        var options = new DbContextOptionsBuilder<DBContext>()
            .UseNpgsql("Host=localhost;Database=translation_test;Username=test;Password=test")
            .Options;

        return new DBContext(options);
    }

    [Fact]
    public void TelevisionChannelByName_QueryTranslatesToSql()
    {
        using var db = CreateNpgsqlContext();
        var search = "SAMPLE";

        // Mirrors GET /api/v1/TelevisionChannel/name/{name}
        var query = db.TelevisionChannels
            .Where(x => x.Name.ToUpper().Contains(search));

        var sql = query.ToQueryString();

        Assert.Contains("upper(", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void HigherEducationInstitutionByName_QueryTranslatesToSql()
    {
        using var db = CreateNpgsqlContext();
        var search = "SAMPLE";

        // Mirrors GET /api/v1/HigherEducationInstitution/name/{name}
        var query = db.HigherEducationInstitutions
            .Where(x => x.Name.ToUpper().Contains(search));

        var sql = query.ToQueryString();

        Assert.Contains("upper(", sql, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ToUpperInvariant_IsNotTranslatable_Reproduction()
    {
        // Reproduces the reported bug: ToUpperInvariant() has no SQL mapping in
        // the Npgsql provider, so translating it throws InvalidOperationException.
        // This is exactly what the previous by-name implementation did and what
        // this test locks out from coming back.
        using var db = CreateNpgsqlContext();
        var search = "SAMPLE";

        var query = db.TelevisionChannels
            .Where(x => x.Name.ToUpperInvariant().Contains(search));

        var ex = Assert.Throws<InvalidOperationException>(() => query.ToQueryString());
        Assert.Contains("could not be translated", ex.Message);
    }
}
