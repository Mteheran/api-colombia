using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using NativeCommunityEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.NativeCommunityEndpoint;

namespace api.Routes
{
    public static class NativeCommunityRoutes
    {
        public static void RegisterNativeCommunityAPI(WebApplication app)
        {
            const string API_NATIVE_COMMUNITY_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.NATIVE_COMMUNITY_ROUTE}";
            app.MapGet(API_NATIVE_COMMUNITY_COMPLETE, (DBContext db) =>
            {
                var listNativeCommunitys = db.NativeCommunities.ToList();
                return Results.Ok(listNativeCommunitys);
            })
            .Produces<List<NativeCommunity>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_LIST_SUMMARY,
                 description: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_LIST_DESCRIPTION
                 ));

            app.MapGet($"{API_NATIVE_COMMUNITY_COMPLETE}/{{id}}", async (int id, DBContext db) =>
            {
                if (id <= 0)
                {
                    return Results.BadRequest();
                }

                var city = await db.NativeCommunities.SingleOrDefaultAsync(p => p.Id == id);
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<City?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_BYID_SUMMARY,
                 description: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_BYID_DESCRIPTION
                 ));

            app.MapGet($"{API_NATIVE_COMMUNITY_COMPLETE}/name/{{name}}", (string name, DBContext db) =>
            {
                var city = db.NativeCommunities.Where(x => x.Name.ToUpper().Equals(name.Trim().ToUpper())).ToList();
                if (city is null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(city);
            })
            .Produces<List<City>?>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_BYNAME_SUMMARY,
                description: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_BYNAME_DESCRIPTION
                ));

            app.MapGet($"{API_NATIVE_COMMUNITY_COMPLETE}/search/{{keyword}}", (string keyword, DBContext db) =>
            {
                string wellFormedKeyword = keyword.Trim().ToUpper().Normalize();
                var dbNativeCommunitys = db.NativeCommunities.ToList();
                var NativeCommunitys = Functions.FilterObjectListPropertiesByKeyword<NativeCommunity>(dbNativeCommunitys, wellFormedKeyword);

                if (!NativeCommunitys.Any())
                {
                    return Results.NotFound();
                }

                return Results.Ok(NativeCommunitys);
            })
            .Produces<List<NativeCommunity>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
                summary: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_SEARCH_SUMMARY,
                 description: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_SEARCH_DESCRIPTION
                 ));


            app.MapGet($"{API_NATIVE_COMMUNITY_COMPLETE}/pagedList", async ([AsParameters] PaginationModel pagination, DBContext db) =>
            {
                if (pagination.Page <= 0 || pagination.PageSize <= 0)
                {
                    return Results.BadRequest();
                }

                var nativeCommunities = db.NativeCommunities.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
                if (!await nativeCommunities?.AnyAsync())
                {
                    return Results.NotFound();
                }

                var paginationResponse = new PaginationResponseModel<NativeCommunity>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = await nativeCommunities.CountAsync(),
                    Data = await nativeCommunities.ToListAsync()
                };

                return Results.Ok(paginationResponse);
            })
            .Produces<PaginationResponseModel<NativeCommunity>>(200)
            .WithMetadata(new SwaggerOperationAttribute(
               summary: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_PAGEDLIST_SUMMARY,
                description: NativeCommunityEndpointMetadataMessages.MESSAGE_NATIVE_COMMUNITY_PAGEDLIST_DESCRIPTION
                ));
        }
    }
}
