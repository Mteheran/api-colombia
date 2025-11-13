using api.Utils;

namespace api.Tests.Utils;

public class StringExtensionsTests
{
    [Fact]
    public void FirstCharToUpper_Throws_OnNull()
    {
        string? input = null;
        Assert.Throws<ArgumentNullException>(() => StringExtensions.FirstCharToUpper(input!));
    }

    [Fact]
    public void FirstCharToUpper_Throws_OnEmpty()
    {
        var input = string.Empty;
        Assert.Throws<ArgumentException>(() => input.FirstCharToUpper());
    }

    [Theory]
    [InlineData("hello", "Hello")]
    [InlineData("h", "H")]
    [InlineData("HELLO", "Hello")]
    [InlineData("hELLo", "Hello")]
    [InlineData("ñandú", "Ñandú")]
    [InlineData("api", "Api")]
    public void FirstCharToUpper_NormalizesCasing(string input, string expected)
    {
        var result = input.FirstCharToUpper();
        Assert.Equal(expected, result);
    }
}