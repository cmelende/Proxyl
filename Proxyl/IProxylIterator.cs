using System.Collections.Generic;

namespace Proxyl
{
    public interface IRedditApiProxyListIterator<out T> : IAsyncEnumerator<T>{ }
}