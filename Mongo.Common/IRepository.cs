using System.Linq.Expressions;

namespace Mongo.Common
{
    public interface IRepository<TCollection>
    {
        Task<long> CountAsync(Expression<Func<TCollection, bool>> expression);
        Task CreateAsync(TCollection newDocument);
        Task<bool> ExistsAsync(Expression<Func<TCollection, bool>> expression);
        IQueryable<TCollection> GetAsQueryable(Expression<Func<TCollection, bool>> expression);
        IQueryable<TCollection> GetAsQueryable();
        Task<List<TCollection>> GetAsync();
        Task<TCollection?> GetAsync(Expression<Func<TCollection, bool>> expression);
        Task RemoveAsync(Expression<Func<TCollection, bool>> expression);
        Task UpdateAsync(Expression<Func<TCollection, bool>> expression, TCollection updateDocument);
        Task CreateManyAsync(ICollection<TCollection> collections);
        Task<List<TCollection>> GetManyAsync(Expression<Func<TCollection, bool>> expression);
    }
}
