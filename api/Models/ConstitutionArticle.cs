namespace api.Models
{
    public class ConstitutionArticle
    {
        public int Id { get; set; }
        public int TitleNumber { get; set; }
        public string Title { get; set; }
        public int ChapterNumber { get; set; }
        public string Chapter { get; set; }
        public int ArticleNumber { get; set; }
        public string Content { get; set; }
    }
}
