using System;
using System.Reflection;
using api.Models;

namespace api.Utils
{
    public class PaginationModel
    {
        public string? SortBy { get; init; }
        public string? SortDirection { get; init; } 
        public int Page { get; init; }
        public int PageSize { get; set; }
        public static ValueTask<PaginationModel?> BindAsync(HttpContext context, ParameterInfo parameter)
    {
        const string sortByKey = "sortBy";
        const string sortDirectionKey = "sortDir";
        const string currentPageKey = "page";
        const string pagesizeKey = "pagesize";

        var sortBy = context.Request.Query[sortByKey].ToString();
        var sortDirectionString = context.Request.Query[sortDirectionKey].ToString();
        
        int.TryParse(context.Request.Query[currentPageKey], out var page);
        page = page == 0 ? 1 : page;  

        int.TryParse(context.Request.Query[pagesizeKey], out var pageSize);
 
        sortBy = string.IsNullOrEmpty(sortBy) ? null : sortBy;

            var result = new PaginationModel
            {
                SortBy = sortBy,  
                SortDirection = sortDirectionString, 
                Page = page,
                PageSize = pageSize
            }; 
        return ValueTask.FromResult<PaginationModel?>(result);
        }
    }
}