namespace Microservices.Common.Generics
{
    public class PagedQueryResult<T>
    {
        public PagedQueryResult(IEnumerable<T> items, int recordsCount = 0)
        {
            Items = items;
            RecordsCount = recordsCount;
        }

        public IEnumerable<T> Items { get; private set; }
        public int RecordsCount { get; private set; }
    }
}