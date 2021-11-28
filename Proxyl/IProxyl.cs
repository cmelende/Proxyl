using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proxyl
{
    public interface IProxyl<T> : IAsyncEnumerable<T>
    {
        IList<T> Items { get; }
        Task<IList<T>> GetItemsAsync();
    }
}