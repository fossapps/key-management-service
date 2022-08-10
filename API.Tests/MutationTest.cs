using System.Threading.Tasks;
using Business;
using Moq;
using NUnit.Framework;

namespace API.Tests;

public class MutationTest
{
    [Test]
    public async Task TestRegisterPublicKeyCallsKeyServiceWithPublicKey()
    {
        var mockKeyService = new Mock<IKeyService>();
        mockKeyService.Setup(x => x.Create("my-public-key")).ReturnsAsync(new Key
        {
            Body = "my-public-key",
            Id = "key_something"
        });
        var mutation = new Mutation();
        var mutationResponse = await mutation.RegisterPublicKey(mockKeyService.Object, "my-public-key");
        Assert.AreEqual(mutationResponse.Body, "my-public-key");
        Assert.AreEqual(mutationResponse.Id, "key_something");
    }
}
