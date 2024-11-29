using api.Models;
using api.Utils;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using NativeCommunityEndpointMetadataMessages = api.Utils.Messages.EndpointMetadata.NativeCommunityEndpoint;
using Microsoft.AspNetCore.Mvc; 
using static api.Utils.Functions;
using static api.Utils.Messages.EndpointMetadata;

namespace api.Routes
{
    public static class NativeCommunityRoutes
    {
        public static void RegisterNativeCommunityAPI(WebApplication app)
        {
            const string API_NATIVE_COMMUNITY_COMPLETE = $"{Util.API_ROUTE}{Util.API_VERSION}{Util.NATIVE_COMMUNITY_ROUTE}";
            app.MapGet(API_NATIVE_COMMUNITY_COMPLETE, (DBContext db,
                [FromQuery, SwaggerParameter(Description = Swagger.sortedBy)] string? sortBy,
                [FromQuery, SwaggerParameter(Description = Swagger.sortDirection)] string? sortDirection) =>
            {
                 var queryNativeCommunities = db.NativeCommunities.AsQueryable(); 
                (queryNativeCommunities, var isValidSort) = ApplySorting(queryNativeCommunities, sortBy, sortDirection);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }

                var listNativeCommunitys = queryNativeCommunities.ToList();
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
 
                var sortBy = pagination.SortBy ?? string.Empty; 
                var sortDirectionStr = pagination.SortDirection?.ToString() ?? string.Empty;
 
                var queryNativeCommunities = db.NativeCommunities.AsQueryable(); 
                (queryNativeCommunities, var isValidSort) = ApplySorting(queryNativeCommunities, sortBy, sortDirectionStr);

                if (!isValidSort)
                {
                    return Results.BadRequest(RequestMessages.BadRequest);
                }
 
                var totalRecords = await queryNativeCommunities.CountAsync();
 
                var pagedNativeCommunities = await queryNativeCommunities
                    .Skip((pagination.Page - 1) * pagination.PageSize)
                    .Take(pagination.PageSize)
                    .ToListAsync();

                if (!pagedNativeCommunities.Any())
                {
                    return Results.NotFound();
                }
 
                var paginationResponse = new PaginationResponseModel<NativeCommunity>
                {
                    Page = pagination.Page,
                    PageSize = pagination.PageSize,
                    TotalRecords = totalRecords,
                    Data = pagedNativeCommunities
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
