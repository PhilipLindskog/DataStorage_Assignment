using System.Linq.Expressions;

namespace Data.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> expression);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);
        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity);
    }
}