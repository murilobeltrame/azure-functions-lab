using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Driver;
using SomeExampleFunctions.Models;

namespace SomeExampleFunctions.Shared
{
    public class DatabaseClient
    {
        private readonly IMongoCollection<Integration> _collection;

        public DatabaseClient()
        {
            var settings = MongoClientSettings.FromUrl(
                new MongoUrl(Configuration.ValueOf("DatabaseConnectionString"))
            );
            settings.SslSettings = new SslSettings
            {
                EnabledSslProtocols = SslProtocols.Tls12
            };
            var client = new MongoClient(settings);
            var db = client.GetDatabase(Configuration.ValueOf("DatabaseName"));
            _collection = db.GetCollection<Integration>(Configuration.ValueOf("DatabaseCollection"));
        }

        public async Task Add(Integration item) =>
            await _collection.InsertOneAsync(item);

        public async Task Upsert(Integration item)
        {
            var filter = Builders<Integration>.Filter.Eq("_id", item.ObjectId);
            await _collection.ReplaceOneAsync(filter, item, new ReplaceOptions
            {
                IsUpsert = true
            });
        }
    }
}
