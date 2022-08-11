using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business;
using Moq;
using NUnit.Framework;

namespace API.Tests;

public class QueryTest
{
    [Test]
    public async Task TestKeysReturnsKeys()
    {
        var mockKeyService = new Mock<IKeyService>();
        mockKeyService.Setup(x => x.FetchAllKeys()).ReturnsAsync(new List<Key>
        {
            new() {Body = "body1", Id = "id1"},
            new() {Body = "body2", Id = "id2"}
        });
        var query = new Query();
        var queryResponse = await query.Keys(mockKeyService.Object);
        var enumerable = queryResponse.ToList();
        Assert.AreEqual(enumerable.First().Body, "body1");
        Assert.AreEqual(enumerable.First().Id, "id1");
        Assert.AreEqual(enumerable.ElementAt(1).Body, "body2");
        Assert.AreEqual(enumerable.ElementAt(1).Id, "id2");
    }
}
