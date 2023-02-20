using System;
namespace api.Utils
{
	public class PaginationResponseModel<T>
	{
        public int Page { get; init; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public List<T> Data { get; set; }
    }
}

