using System.Collections.Generic;

namespace Proxyl
{
    public interface IProxylIterator<out T> : IAsyncEnumerator<T>{ }
}