using api.Models;

namespace api.Tests.TestData;

internal static class ConstitutionArticleSeed
{
    public const int TotalRows = 2;

    private const string SharedContent =
        "La ley señalará las funciones que el Presidente de la República podrá elegir.";

    /// <summary>Both rows share ChapterNumber 1, so `/byChapterNumber/1` returns two articles.</summary>
    public static IReadOnlyList<ConstitutionArticle> Rows() =>
    [
        new()
        {
            Id = 1,
            TitleNumber = 1,
            Title = "DE LA RAMA EJECUTIVA",
            ChapterNumber = 1,
            Chapter = "DE LA FUNCION ADMINISTRATIVA",
            ArticleNumber = 1,
            Content = SharedContent
        },
        new()
        {
            Id = 2,
            TitleNumber = 2,
            Title = "DE LA RAMA LEGISLATIVA",
            ChapterNumber = 1,
            Chapter = "DE LA FUNCION LEGISLATIVA",
            ArticleNumber = 2,
            Content = SharedContent
        }
    ];
}
