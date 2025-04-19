using Microservices.Common.Enums;

namespace Microservices.Common.Models
{
    public abstract class PagedQueryBase
    {
        private int _pageSize = 10;
        private string _dir = "asc";

        public int PageIndex { get; set; } = 0;
        public int PageSize
        {
            get => _pageSize;
            set
            {
                if (value > 0) { _pageSize = value; }
            }
        }
        public string? Sort { get; set; } = "created";
        public string? Dir
        {
            get => _dir;
            set
            {
                if (value == "desc") { _dir = value; }
            }
        }
        public PaginationDirection Direction { get => _dir == "desc" ? PaginationDirection.Descending : PaginationDirection.Ascending; }
    }
}