namespace api.Utils
{
    /// <summary>
    /// Pagination and sorting parameters for the pagedList routes.
    ///
    /// Bound with [AsParameters], which maps each property from the query string by name — so the
    /// query keys are ?sortBy=, ?sortDirection=, ?page= and ?pageSize= (matched case-insensitively,
    /// hence ?pagesize= works too). This type used to also declare a custom BindAsync reading a
    /// ?sortDir= key and correcting a non-positive page to 1; [AsParameters] never calls BindAsync,
    /// so none of that ran. It was removed rather than left to mislead. Each route validates the
    /// page and page size itself.
    /// </summary>
    public class PaginationModel
    {
        public string? SortBy { get; init; }
        public string? SortDirection { get; init; }
        public int Page { get; init; }
        public int PageSize { get; set; }
    }
}
