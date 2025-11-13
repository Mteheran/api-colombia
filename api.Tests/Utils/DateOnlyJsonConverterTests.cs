using System.Text.Json;
using api.Utils;

namespace api.Tests.Utils;

public class DateOnlyJsonConverterTests
{
    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new DateOnlyJsonConverter());
        return options;
    }

    [Fact]
    public void Serialize_DateOnly_UsesRoundTripFormat()
    {
        // Arrange
        var date = new DateOnly(2025, 11, 12);
        var options = CreateOptions();

        // Act
        var json = JsonSerializer.Serialize(date, options);

        // Assert
        Assert.Equal("\"2025-11-12\"", json);
    }

    [Fact]
    public void Deserialize_ValidDateTimeString_ReturnsDateOnly()
    {
        // Arrange
        var json = "\"2025-11-12T00:00:00\""; // valid ISO date-time
        var options = CreateOptions();

        // Act
        var result = JsonSerializer.Deserialize<DateOnly>(json, options);

        // Assert
        Assert.Equal(new DateOnly(2025, 11, 12), result);
    }

    [Fact]
    public void Deserialize_InvalidString_ThrowsJsonException()
    {
        // Arrange
        var json = "\"not a date\"";
        var options = CreateOptions();

        // Act & Assert
        Assert.ThrowsAny<JsonException>(() => JsonSerializer.Deserialize<DateOnly>(json, options));
    }
}