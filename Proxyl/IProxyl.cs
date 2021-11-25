using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Proxyl
{
    public interface IProxyl<T> : IAsyncEnumerable<T>
    {
            
    }

    public interface IProxy<TR, TI, out T> : IAsyncEnumerable<T> 
        where TR: IProxylRepository<T> 
        where TI:IProxylIterator<T>
    {
    }


    public abstract class Proxy<T> : IProxy<IProxylRepository<T>, ProxyIterator<T>, T>
    {
        private readonly IProxylRepository<T> _repository;
        private readonly Func<ProxyIterator<T>> _iteratorFactory;
        public IList<T> Items;
        
        protected Proxy(IProxylRepository<T> repository, Func<ProxyIterator<T>> iteratorFactory)
        {
            _repository = repository;
            _iteratorFactory = iteratorFactory;
        }
        public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return _iteratorFactory();
        }

        public async Task<IList<T>> GetItems() => Items ??= await _repository.GetInitialDataAsync();

        public async Task<bool> TryFetchMoreAsync()
        {
            IList<T> more = await _repository.GetMoreDataAsync();
                
            if (more == null || !more.Any()) return false;

            IList<T> items = Items;
            foreach (T t in more)
            {
                items.Add(t);
            }
                
            return true;
        }
    }

    public class ProxyIterator<T> : IProxylIterator<T>
    {
        private readonly Proxy<T> _proxy;
        
        private int _position = -1;

        public T Current => _proxy.Items[_position];

        public ValueTask DisposeAsync() => new(Task.CompletedTask);

        protected ProxyIterator(Proxy<T> proxy)
        {
            _proxy = proxy;
        }
       
        public async ValueTask<bool> MoveNextAsync()
        {
            int newPosition = _position + 1;
            if (await PositionIsValidIndex(newPosition))
            {
                _position = newPosition;
                return true;
            }

            bool fetchedMore = await _proxy.TryFetchMoreAsync();
            if (fetchedMore && await PositionIsValidIndex(newPosition))
            {
                _position = newPosition;
                return true;
            }
            return false;
        }
        
        private async Task<bool> PositionIsValidIndex(int newPosition)
        {
            return newPosition >= 0 && newPosition < (await _proxy.GetItems()).Count;
        }
    }
}