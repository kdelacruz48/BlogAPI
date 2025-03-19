using BlogAPI.Models;
using System.ComponentModel;
using System.Linq.Expressions;

namespace BlogAPI.Repository.IRepository
{
    public interface IPostRepository : IRepository<PostModel>
    {
        Task<PostModel> UpdateAsync(PostModel entity);

    }
}
