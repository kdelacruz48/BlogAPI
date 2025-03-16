using BlogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogAPI.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
           
        }


        public DbSet<PostModel> APIPosts { get; set; }


    }
}
