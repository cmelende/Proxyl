using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Proxyl
{
    public class Proxyl<T> : IProxyl<T>
    {
        private readonly IProxylRepository<T> _repository;
        private readonly Func<IProxyl<T>, IProxylIterator<T>> _iteratorFactory;
        public IList<T> Items { get; private set; }
        
        public Proxyl(IProxylRepository<T> repository, Func<IProxyl<T>, IProxylIterator<T>> iteratorFactory)
        {
            _repository = repository;
            _iteratorFactory = iteratorFactory;
        }
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new())
        {
            return _iteratorFactory(this);
        }

        public async Task<IList<T>> GetItemsAsync() => Items ??= await _repository.GetInitialDataAsync();
    }
}