namespace Microservices.Common.Generics
{
    public class PagedQueryResult<T>(IEnumerable<T> items, int recordsCount = 0)
    {
        public IEnumerable<T> Items { get; private set; } = items;
        public int RecordsCount { get; private set; } = recordsCount;
    }
}