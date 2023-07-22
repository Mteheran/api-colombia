using System;
namespace api.Utils
{
	public class PaginationResponseModel<T>
	{
        public int Page { get; init; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int PageCount
        {
            get
            {
                return (int)Math.Ceiling(Convert.ToDecimal(TotalRecords) / Convert.ToDecimal(PageSize));
            }
        }
        public List<T> Data { get; set; }
    }
}

