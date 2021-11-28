using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Proxyl
{
    public class Repo<T> : IProxylRepository<T>
    {
        public Task<IList<T>> GetInitialDataAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IList<T>> GetMoreDataAsync()
        {
            throw new NotImplementedException();
        }
    }
}