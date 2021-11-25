using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proxyl
{
    
    public abstract class Proxyl<T> : IProxyl<T>
    {
        private readonly IProxylRepository<T> _retriever;
        private readonly IProxylIterator<T> _iterator;

        public IList<T> Items;

        protected Proxyl(IProxylRepository<T> retriever, IProxylIterator<T> iterator)
        {
            _retriever = retriever;
            _iterator = iterator;
        }
            
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new()) => _iterator;

        public async Task<IList<T>> GetItems() => Items ??= await _retriever.GetInitialDataAsync();

        public async Task<bool> TryFetchMoreAsync()
        {
            IList<T> more = await _retriever.GetMoreDataAsync();
                
            if (more == null || !more.Any()) return false;

            IList<T> items = Items;
            foreach (T t in more)
            {
                items.Add(t);
            }
                
            return true;
        }
    }
}
