using System;
using System.Reflection;

namespace api.Utils
{

    public class PaginationModel
    {
        public string? SortBy { get; init; }
        public SortDirection SortDirection { get; init; }
        public int Page { get; init; }
        public int PageSize { get; set; }

        public static ValueTask<PaginationModel?> BindAsync(HttpContext context,
                                                       ParameterInfo parameter)
        {
            const string sortByKey = "sortBy";
            const string sortDirectionKey = "sortDir";
            const string currentPageKey = "page";
            const string pagesizeKey = "pagesize";

            Enum.TryParse<SortDirection>(context.Request.Query[sortDirectionKey],
                                         ignoreCase: true, out var sortDirection);
            int.TryParse(context.Request.Query[currentPageKey], out var page);
            page = page == 0 ? 1 : page;
            int.TryParse(context.Request.Query[pagesizeKey], out var pageSize);

            var result = new PaginationModel
            {
                SortBy = context.Request.Query[sortByKey],
                SortDirection = sortDirection,
                Page = page,
                PageSize = pageSize

            };

            return ValueTask.FromResult<PaginationModel?>(result);
        }
    }

    public enum SortDirection
    {
        Default,
        Asc,
        Desc
    }
}

