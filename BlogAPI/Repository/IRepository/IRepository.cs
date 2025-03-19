using BlogAPI.Models;
using System.Linq.Expressions;

namespace BlogAPI.Repository.IRepository
{
    public interface IRepository<T> where T: class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null);
        Task CreateAsync(T entity);
        Task RemoveAsync(T entity);
        Task SaveAsync();
    }
}
