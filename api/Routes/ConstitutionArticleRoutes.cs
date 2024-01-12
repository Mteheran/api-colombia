using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using ConstitutionArticleMetadataMessages = api.Utils.Messages.EndpointMetadata.ConstitutionArticleEndpoint;

namespace api.Routes
{
    public static class ConstitutionArticleRoutes
    {
        public static void RegisterConstitutionArticleAPI(WebApplication app)
        {
            const string API_CONSTITUTION_ARTICLE_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.CONSTITUTION_ARTICLE}";
            app.MapGet(API_CONSTITUTION_ARTICLE_COMPLETE, (DBContext db) =>
            {
                var listConstitutionArticles = db.ConstitutionArticles
                .OrderBy(p=> p.Id).ToList();
                return Results.Ok(listConstitutionArticles);
            })
            .Produces<List<ConstitutionArticle>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_LIST_SUMMARY,
                 description: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_LIST_DESCRIPTION
                 ));

            app.MapGet($"{API_CONSTITUTION_ARTICLE_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var city = await db.ConstitutionArticles
                                    .SingleOrDefaultAsync(p => p.Id == id);
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<City?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_BYID_SUMMARY,
                 description: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_BYID_DESCRIPTION
                 ));


            app.MapGet($"{API_CONSTITUTION_ARTICLE_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbConstitutionArticles = db.ConstitutionArticles.ToList();
                var ConstitutionArticles = Functions.FilterObjectListPropertiesByKeyword<ConstitutionArticle>(dbConstitutionArticles, wellFormedKeyword);

                if (!ConstitutionArticles.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(ConstitutionArticles.OrderBy(p => p.Id));
            })
            .Produces<List<ConstitutionArticle>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_SEARCH_SUMMARY,
                 description: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_SEARCH_DESCRIPTION
                 ));


            app.MapGet($"{API_CONSTITUTION_ARTICLE_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var ConstitutionArticles = db.ConstitutionArticles.OrderBy(p => p.Id).Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
                if (!await ConstitutionArticles?.AnyAsync())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<ConstitutionArticle>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = await ConstitutionArticles.CountAsync(),
                    Data = await ConstitutionArticles.ToListAsync()
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<ConstitutionArticle>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_PAGEDLIST_SUMMARY,
                description: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_PAGEDLIST_DESCRIPTION
                ));

            app.MapGet($"{API_CONSTITUTION_ARTICLE_COMPLETE}/byChapterNumber/{{chapternumber}}", (int chapternumber, DBContext db) =>
            {
                var dbConstitutionArticles = db.ConstitutionArticles.Where(p=> p.ChapterNumber == chapternumber).ToList();
             
                if (!dbConstitutionArticles.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(dbConstitutionArticles.OrderBy(p => p.Id));
            })
           .Produces<List<ConstitutionArticle>>(200)
           .WithMetadata(new SwaggerOperationAttribute(
               summary: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_SEARCH_SUMMARY,
                description: ConstitutionArticleMetadataMessages.MESSAGE_CONSTITUTION_ARTICLE_SEARCH_DESCRIPTION
                ));

        }
    }
}

