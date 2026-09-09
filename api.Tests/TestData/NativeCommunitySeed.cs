using api.Models;

namespace api.Tests.TestData;

internal static class NativeCommunitySeed
{
    public const int TotalRows = 3;

    private static readonly string[] SampleImages =
        ["https://example.com/image1.jpg", "https://example.com/image2.jpg"];

    public static IReadOnlyList<NativeCommunity> Rows() =>
    [
        new() { Id = 1, Name = "Sample Community",  Description = "Sample description",  Languages = "Spanish, English", Images = SampleImages },
        new() { Id = 2, Name = "Another Community", Description = "Another description", Languages = "Spanish, English", Images = SampleImages },
        new() { Id = 3, Name = "Third Community",   Description = "Third description",   Languages = "Spanish, English", Images = SampleImages }
    ];
}
