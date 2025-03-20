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
        public DbSet<LocalUser> LocalUsers { get; set; }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        //    modelBuilder.Entity<PostModel>().HasData(
        //        new PostModel
        //        {
        //            Id = 1,
        //            UserName = "Test",
        //            Title = "Test",
        //            ImageUrl = "Test",
        //            Post = "test",
        //            Created_date = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 12, 0, 0), DateTimeKind.Utc),
        //            Updated_date = DateTime.SpecifyKind(new DateTime(2024, 1, 1, 12, 0, 0), DateTimeKind.Utc)
        //        }
        //        );


        //    modelBuilder.Entity<LocalUser>().HasData(
        //        new LocalUser
        //        {
        //            Id = 1,
        //            UserName = "Test",
        //            Name = "test",
        //            Password = "Test",
        //            Role = "Test",
        //            Email = "Test"
        //        }
        //        );
        //}


    }
}
