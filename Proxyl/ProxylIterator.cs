using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proxyl
{
    public class ProxylIterator<T> : IProxylIterator<T>
    {
        private readonly IProxyl<T> _proxyl;
        private readonly IProxylRepository<T> _repository;

        private int _position = -1;

        public T Current => _proxyl.Items[_position];

        public ValueTask DisposeAsync() => new(Task.CompletedTask);

        public ProxylIterator(IProxyl<T> proxyl, IProxylRepository<T> repository)
        {
            _proxyl = proxyl;
            _repository = repository;
        }
       
        public async ValueTask<bool> MoveNextAsync()
        {
            int newPosition = _position + 1;
            if (await PositionIsValidIndex(newPosition))
            {
                _position = newPosition;
                return true;
            }

            bool fetchedMore = await TryFetchMore();
            if (fetchedMore && await PositionIsValidIndex(newPosition))
            {
                _position = newPosition;
                return true;
            }
            
            return false;
        }

        private async Task<bool> TryFetchMore()
        {
           IList<T> newItems = await _repository.GetMoreDataAsync();
           bool fetchedMore = newItems != null && newItems.Count != 0;
           if (!fetchedMore) return false;
           
           foreach (T newItem in newItems)
           {
               _proxyl.Items.Add(newItem);
           }

           return true;
        }
        
        private async Task<bool> PositionIsValidIndex(int newPosition)
        {
            return newPosition >= 0 && newPosition < (await _proxyl.GetItemsAsync()).Count;
        }
    }
}