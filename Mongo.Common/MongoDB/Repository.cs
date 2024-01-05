using Microsoft.Extensions.Options;
using Mongo.Common.Settings;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Mongo.Common.MongoDB
{
    public class Repository<TCollection> : IRepository<TCollection> where TCollection : IBaseEntity
    {
        private readonly IMongoCollection<TCollection> _collection;

        public Repository(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<TCollection>(typeof(TCollection).Name);
        }

        public IQueryable<TCollection> GetAsQueryable(Expression<Func<TCollection, bool>> expression) =>
            _collection.AsQueryable().Where(expression);

        public IQueryable<TCollection> GetAsQueryable() =>
            _collection.AsQueryable();

        public async Task<TCollection?> GetAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.Find(expression).FirstOrDefaultAsync();

        public async Task CreateAsync(TCollection newDocument) =>
            await _collection.InsertOneAsync(newDocument);

        public async Task CreateManyAsync(ICollection<TCollection> collections) =>
            await _collection.InsertManyAsync(collections);

        public async Task UpdateAsync(Expression<Func<TCollection, bool>> expression, TCollection updateDocument) =>
            await _collection.ReplaceOneAsync(expression, updateDocument);

        public async Task RemoveAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.DeleteOneAsync<TCollection>(expression);

        public async Task<bool> ExistsAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.CountDocumentsAsync<TCollection>(expression) > 0;

        public async Task<long> CountAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.CountDocumentsAsync(expression);

        public async Task<List<TCollection>> GetAsync() =>
            await _collection.Find(i => i.IsDeprecated == false).ToListAsync();

        public async Task<List<TCollection>> GetManyAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.Find(expression).ToListAsync();
    }
}
