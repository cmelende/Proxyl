using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proxyl
{
    public interface IProxylRepository<T>
    {
        Task<IList<T>> GetInitialDataAsync();
        Task<IList<T>> GetMoreDataAsync();
    }
}