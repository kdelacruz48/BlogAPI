using BlogAPI.Controllers;
using BlogAPI.Data;
using BlogAPI.Models;
using BlogAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlogAPI.Repository
{
    public class PostRepository : Repository<PostModel>, IPostRepository
    {
        //I may want other update methods as If I expand so executing this outside repository
        private readonly ApplicationDbContext _db;
        public PostRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
       

        public async Task<PostModel> UpdateAsync(PostModel entity)
        {
            entity.Updated_date = DateTimeOffset.Now.ToUniversalTime();
            _db.APIPosts.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
        }
    }
}
