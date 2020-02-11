using System;
using System.Linq;
using System.Threading.Tasks;
using Micro.KeyStore.Api.Configs;
using Micro.KeyStore.Api.Keys.Models;
using Micro.KeyStore.Api.Keys.Repositories;
using Micro.KeyStore.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace Micro.KeyStore.IntegrationTest.Keys
{
    public class KeyServiceTest
    {
        [SetUp]
        public void Setup()
        {
            var db = GetDatabaseInstance();
            db.Database.Migrate();
            CleanDb(db);
        }

        private static void CleanDb(ApplicationContext db)
        {
            db.Keys.RemoveRange(db.Keys.ToList());
            db.SaveChanges();
        }
        private static IOptions<DatabaseConfig> GetDatabaseConfig()
        {
            var options = new Mock<IOptions<DatabaseConfig>>();
            var databaseConfig = new DatabaseConfig
            {
                Host = "localhost",
                Name = "starter_db",
                Password = "secret",
                Port = 15433,
                User = "starter"
            };
            options.Setup(x => x.Value).Returns(databaseConfig);
            return options.Object;
        }

        private static ApplicationContext GetDatabaseInstance()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            return new ApplicationContext(optionsBuilder.Options, GetDatabaseConfig());
        }

        [Test]
        public async Task TestCreate()
        {
            var db = GetDatabaseInstance();
            var repository = new KeyRepository(db);
            await repository.Save(new Key {Body = "bdy", Id = "12", Sha = "sha", CreatedAt = DateTime.Now, ShortSha = "sh"});
            var key = await repository.FindById("12");
            Assert.AreEqual(key.ShortSha, "sh");
            Assert.AreEqual(key.Sha, "sha");
            Assert.AreEqual(key.Body, "bdy");
        }

        [Test]
        public async Task TestFindByIdReturnsNullIfNotFound()
        {
            var db = GetDatabaseInstance();
            var repository = new KeyRepository(db);
            var key = await repository.FindById("124");
            Assert.IsNull(key);
        }

        [Test]
        public async Task TestCreateShortSha()
        {
            var db = GetDatabaseInstance();
            CleanDb(db);
            var repository = new KeyRepository(db);
            await repository.Save(new Key {Body = "key1", Id = "1", Sha = "sha_temp123", CreatedAt = DateTime.Now, ShortSha = "s"});
            await repository.Save(new Key {Body = "key2", Id = "2", Sha = "sha_temp234", CreatedAt = DateTime.Now, ShortSha = "sh"});
            await repository.Save(new Key {Body = "key3", Id = "3", Sha = "sha_temp231", CreatedAt = DateTime.Now, ShortSha = "sha"});
            await repository.Save(new Key {Body = "key4", Id = "4", Sha = "pak_test", CreatedAt = DateTime.Now, ShortSha = "p"});
            await repository.Save(new Key {Body = "key5", Id = "5", Sha = "lom", CreatedAt = DateTime.Now, ShortSha = "l"});
            var actualSha = await repository.FindNextShortSha("sna_temp232");
            Assert.AreEqual("sna_", actualSha);
        }

        [Test]
        public async Task TestCreateShortShaWorksForFirstCharacters()
        {
            var db = GetDatabaseInstance();
            CleanDb(db);
            var repository = new KeyRepository(db);
            await repository.Save(new Key {Body = "key1", Id = "1", Sha = "sha_temp123", CreatedAt = DateTime.Now, ShortSha = "s"});
            await repository.Save(new Key {Body = "key2", Id = "2", Sha = "sha_temp234", CreatedAt = DateTime.Now, ShortSha = "sh"});
            await repository.Save(new Key {Body = "key3", Id = "3", Sha = "sha_temp231", CreatedAt = DateTime.Now, ShortSha = "sha"});
            await repository.Save(new Key {Body = "key4", Id = "4", Sha = "pak_test", CreatedAt = DateTime.Now, ShortSha = "p"});
            await repository.Save(new Key {Body = "key5", Id = "5", Sha = "lom", CreatedAt = DateTime.Now, ShortSha = "l"});
            var actualSha = await repository.FindNextShortSha("kite_something");
            Assert.AreEqual("kite", actualSha);
        }
    }
}
