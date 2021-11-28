using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests.Utilities
{
    public static class AsyncEnumerableExtensions
    {
        public static async Task<IList<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var list = new List<T>();
            await foreach (T element in enumerable)
            {
                list.Add(element);
            }

            return list;
        }
    }
}