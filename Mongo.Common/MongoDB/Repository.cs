using Microsoft.Extensions.Options;
using Mongo.Common.Settings;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Mongo.Common.MongoDB
{
    public class Repository<TCollection> : IRepository<TCollection> where TCollection : IBaseEntity
    {
        private readonly IMongoCollection<TCollection> _collection;

        public Repository(MongoDbSettings mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
            _collection = mongoDatabase.GetCollection<TCollection>(typeof(TCollection).Name);
        }

        /// <summary>
        /// Gets the list of document as queryable collection
        /// </summary>
        /// <param name="expression">Lamba expression</param>
        /// <returns></returns>
        public IQueryable<TCollection> GetAsQueryable(Expression<Func<TCollection, bool>> expression) =>
            _collection.AsQueryable().Where(expression);

        /// <summary>
        /// Gets the list of document as queryable collection
        /// </summary>
        /// <returns></returns>
        public IQueryable<TCollection> GetAsQueryable() =>
            _collection.AsQueryable();

        /// <summary>
        /// Gets a single document
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<TCollection?> GetAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.Find(expression).FirstOrDefaultAsync();

        /// <summary>
        /// Creates a single document
        /// </summary>
        /// <param name="newDocument"></param>
        /// <returns></returns>
        public async Task CreateAsync(TCollection newDocument) =>
            await _collection.InsertOneAsync(newDocument);

        public async Task CreateManyAsync(ICollection<TCollection> collections) =>
            await _collection.InsertManyAsync(collections);

        /// <summary>
        /// Updates a single document
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="updateDocument"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Expression<Func<TCollection, bool>> expression, TCollection updateDocument) =>
            await _collection.ReplaceOneAsync(expression, updateDocument);

        /// <summary>
        /// Deletes a single document
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task RemoveAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.DeleteOneAsync<TCollection>(expression);

        /// <summary>
        /// Deletes many documents based on a condition
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RemoveManyAsync(Expression<Func<TCollection, bool>> expression, CancellationToken cancellationToken) =>
            await _collection.DeleteManyAsync<TCollection>(expression, cancellationToken);

        /// <summary>
        /// Checks whether a document exists based on a condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.CountDocumentsAsync<TCollection>(expression) > 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<long> CountAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.CountDocumentsAsync(expression);

        /// <summary>
        /// Gets the list of document as a collection
        /// </summary>
        /// <returns></returns>
        public async Task<List<TCollection>> GetAsync() =>
            await _collection.Find(i => i.IsDeprecated == false).ToListAsync();

        /// <summary>
        /// Gets the list of document as a collection based on a condition
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public async Task<List<TCollection>> GetManyAsync(Expression<Func<TCollection, bool>> expression) =>
            await _collection.Find(expression).ToListAsync();
    }
}