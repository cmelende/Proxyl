using System.Collections.Generic;
using System.Threading.Tasks;
using FakeItEasy;
using NUnit.Framework;
using Proxyl;

namespace Tests
{
    [TestFixture]
    public class ProxylIteratorTests
    {
        [Test]
        public async Task MoveNextAsync_ReturnsTrueNotAtEndOfList()
        {
            var ints = new List<int>{1,2,3};
            var proxyl = A.Fake<IProxyl<int>>();
            var repo = A.Fake<IProxylRepository<int>>();
            var iterator = new ProxylIterator<int>(proxyl, repo);

            A.CallTo(() => proxyl.GetItemsAsync()).Returns(ints);
            
            bool moveNextAsync = await iterator.MoveNextAsync();
            
            Assert.That(moveNextAsync, Is.True);
        }
        
        [Test]
        public async Task Current_ReturnsValidItem()
        {
            const int value = 1;
            var ints = new List<int>{1,2,3};
            var proxyl = A.Fake<IProxyl<int>>();
            var repo = A.Fake<IProxylRepository<int>>();
            var iterator = new ProxylIterator<int>(proxyl, repo);

            A.CallTo(() => proxyl.Items).Returns(ints);
            A.CallTo(() => proxyl.GetItemsAsync()).Returns(ints);
            
            await iterator.MoveNextAsync();
            int result = iterator.Current;
            
            Assert.That(result, Is.EqualTo(value));
        }
    }
}