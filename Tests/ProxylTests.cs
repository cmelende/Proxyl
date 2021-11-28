using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Proxyl;

namespace Tests
{
    [TestFixture]
    public class ProxylTests
    {
        [Test]
        public async Task GetItemsAsync_ReturnsCorrectList()
        {
            var repo = A.Fake<IProxylRepository<int>>();
            var proxylIterator = A.Fake<IProxylIterator<int>>();
            var iteratorFactory = new Func<IProxyl<int>, IProxylIterator<int>>(_ => proxylIterator);
            A.CallTo(() => repo.GetInitialDataAsync()).Returns(new List<int> { 0, 1, 2 });

            var proxy = new Proxyl<int>(repo, iteratorFactory);

            IList<int> results = await proxy.GetItemsAsync();

            Assert.That(results.Count, Is.EqualTo(3));
            Assert.That(results[0], Is.EqualTo(0));
            Assert.That(results[1], Is.EqualTo(1));
            Assert.That(results[2], Is.EqualTo(2));
        }

        [Test]
        public void GetAsyncEnumerator_ReturnsCorrectEnumerator()
        {
            var repo = A.Fake<IProxylRepository<int>>();
            var proxylIterator = A.Fake<IProxylIterator<int>>();
            var iteratorFactory = new Func<IProxyl<int>, IProxylIterator<int>>(_ => proxylIterator);
            
            var proxyl = new Proxyl<int>(repo, iteratorFactory);

            IAsyncEnumerator<int> enumerator = proxyl.GetAsyncEnumerator();
            
            Assert.That(enumerator, Is.EqualTo(proxylIterator));
        }
    }
}
