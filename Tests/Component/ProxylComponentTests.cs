using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Proxyl;
using Tests.Utilities;

namespace Tests.Component
{

    [TestFixture]
    public class ProxylComponentTests
    {
        [Test]
        public async Task VerifyEnumerationOverWholeList()
        {
            var repo = new FakeIntRepository(3);
            var iteratorFactory = new Func<IProxyl<int>, IProxylIterator<int>>(p => new ProxylIterator<int>(p, repo));
            var proxyl = new Proxyl<int>(repo, iteratorFactory);

            IList<int> list = await proxyl.ToListAsync();
            
            Assert.That(list.Count, Is.EqualTo(12));
        }
    }

    public class FakeIntRepository : IProxylRepository<int>
    {
        private readonly int _getMoreMaxCount;
        private int _counter;

        public FakeIntRepository(int getMoreMaxCount)
        {
            _getMoreMaxCount = getMoreMaxCount;
        }
        public async Task<IList<int>> GetInitialDataAsync()
        {
            await Task.Delay(3);

            return new List<int> { 1, 2, 3 };
        }

        public async Task<IList<int>> GetMoreDataAsync()
        {
            await Task.Delay(3);

            if (_counter++ < _getMoreMaxCount) 
                return new List<int> { 3, 4, 5 };
            return await Task.FromResult<IList<int>>(null);
        }
    }
}