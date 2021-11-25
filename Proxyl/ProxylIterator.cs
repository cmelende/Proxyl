using System.Threading.Tasks;

namespace Proxyl
{
    public abstract class ProxylIterator<T> : IProxylIterator<T>
    {
        private readonly Proxyl<T> _proxy;
        
        private int _position = -1;

        public T Current => _proxy.Items[_position];

        public ValueTask DisposeAsync() => new(Task.CompletedTask);

        protected ProxylIterator(Proxyl<T> proxy)
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