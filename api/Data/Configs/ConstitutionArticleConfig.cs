using api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace api.Data.Configs
{
    public class ConstitutionArticleConfig : IEntityTypeConfiguration<ConstitutionArticle>
    {
        public void Configure(EntityTypeBuilder<ConstitutionArticle> article)
        {
            article.ToTable("ConstitutionArticle");
            article.HasKey(p => p.Id);
            article.Property(p => p.Id).ValueGeneratedOnAdd();
            article.Property(p => p.TitleNumber).IsRequired();
            article.Property(p => p.Title).IsRequired().HasMaxLength(150);
            article.Property(p => p.ChapterNumber).IsRequired();
            article.Property(p => p.Chapter).IsRequired().HasMaxLength(150);
            article.Property(p => p.ArticleNumber).IsRequired();
            article.Property(p => p.Content).IsRequired();
        }
    }
}
