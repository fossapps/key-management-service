using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Storage;

namespace Business.Tests;

public class KeyServiceTest
{
    [Test]
    public async Task CreatePublicKeyTest()
    {
        var mockKeyRepo = new Mock<IKeyRepository>();
        mockKeyRepo
            .Setup(x => x.Create(It.IsAny<Storage.Key>()))
            .Returns((Storage.Key input) =>
            {
                return Task.FromResult(new Storage.Key
                {
                    Body = input.Body,
                    CreatedAt = DateTime.Now.ToUniversalTime(),
                    Id = "key_something"
                });
            });
        var service = new KeyService(mockKeyRepo.Object);
        var key = await service.Create("my-public-key");
        Assert.AreEqual("my-public-key", key.Body);
        Assert.AreEqual("key_something", key.Id);
    }

    [Test]
    public async Task FetchAllKeysTest()
    {
        var mockKeyRepo = new Mock<IKeyRepository>();
        var mockKeys = new List<Storage.Key>
        {
            new() { Body = "1", Id = "key_1"},
            new() { Body = "2", Id = "key_2"},
        };
        mockKeyRepo.Setup(x => x.FindAllKeys()).ReturnsAsync(mockKeys.AsEnumerable());
        var service = new KeyService(mockKeyRepo.Object);
        var keys = await service.FetchAllKeys();
        var enumerable = keys.ToList();
        Assert.AreEqual("1", enumerable.First().Body);
        Assert.AreEqual("key_1", enumerable.First().Id);
        Assert.AreEqual("2", enumerable.ElementAt(1).Body);
        Assert.AreEqual("key_2", enumerable.ElementAt(1).Id);
    }

    [Test]
    public async Task CleanupKeysTest()
    {
        var mockKeyRepo = new Mock<IKeyRepository>();
        mockKeyRepo.Setup(x => x.CleanupKeys(It.IsAny<DateTime>())).Returns((DateTime x) =>
        {
            Assert.AreEqual(-1, x.Subtract(DateTime.Now).Hours);
            return Task.CompletedTask;
        });
        var service = new KeyService(mockKeyRepo.Object);
        await service.CleanupKeys(1);
    }
}
