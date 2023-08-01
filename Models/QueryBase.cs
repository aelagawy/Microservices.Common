using Microservices.Common.Enums;

namespace Microservices.Common.Models
{
    public abstract class QueryBase
    {
        public QueryBase(int pageIndex = 0, int pageSize = 10, string sort = "created", string dir = "asc")
        {
            PageIndex = pageIndex;
            PageSize = pageSize != 0 ? pageSize : 10;
            Sort = sort;
            Direction = dir == "desc" ? PaginationDirection.Descending : PaginationDirection.Ascending;
        }

        public int PageIndex { get; }
        public int PageSize { get; }
        public string Sort { get; }
        public PaginationDirection Direction { get; }
    }
}