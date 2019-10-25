using System;
using System.Threading.Tasks;
using App.Metrics;
using Micro.KeyStore.Api.Keys;
using Micro.KeyStore.Api.Keys.Models;
using Micro.KeyStore.Api.Keys.Repositories;
using Micro.KeyStore.Api.Keys.Services;
using Micro.KeyStore.Api.Uuid;
using Moq;
using NUnit.Framework;

namespace Micro.KeyStore.UnitTest.Keys
{
    public class KeyServiceTest
    {

        private Mock<IUuidService> GetUuidService(string uuidToReturn)
        {
            var uuid = new Mock<IUuidService>();
            uuid.Setup(x => x.GenerateUuId()).Returns(uuidToReturn);
            return uuid;
        }

        private Mock<IMetrics> GetMockMetrics()
        {
            var metrics = new Mock<IMetrics>();
            return metrics;
        }

        private Mock<IKeyRepository> GetKeyRepository()
        {
            var mockRepo = new Mock<IKeyRepository>();
            mockRepo.Setup(x => x.FindNextShortSha(It.IsAny<string>())).ReturnsAsync("short_sha");
            mockRepo.Setup(x => x.Save(It.IsAny<Key>())).Returns<Key>(Task.FromResult);
            return mockRepo;
        }

        [Test]
        public async Task TestCreateMethodGeneratesUuid()
        {
            var mockUUid = GetUuidService("generated_uuid");
            var mockRepo = GetKeyRepository();
            var keyService = new KeyService(mockRepo.Object, mockUUid.Object, GetMockMetrics().Object);
            var key = new Key
            {
                Body = "something",
                Sha = "some",
                CreatedAt = DateTime.Now
            };
            var newKey = await keyService.CreateKey(key);
            Assert.AreEqual(newKey.Id, "generated_uuid");
        }

        [Test]
        public async Task TestCreateMethodAddsShortSha()
        {
            var mockUUid = GetUuidService("uuid");
            var mockRepo = GetKeyRepository();
            var keyService = new KeyService(mockRepo.Object, mockUUid.Object, GetMockMetrics().Object);
            var key = new Key
            {
                Body = "something",
                Sha = "some",
                CreatedAt = DateTime.Now
            };
            var newKey = await keyService.CreateKey(key);
            Assert.AreEqual(newKey.ShortSha, "short_sha");
        }
    }
}
