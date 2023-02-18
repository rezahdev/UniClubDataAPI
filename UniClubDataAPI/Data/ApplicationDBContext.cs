using Microsoft.EntityFrameworkCore;
using UniClubDataAPI.Models;

namespace UniClubDataAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) {  }

        public DbSet<Club> Clubs { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Club>().HasData(new Club()
            {
                Id = 1,
                Name = "Test Club",
                UniversityId = 1,
                Email = "asd@asd.com",
                Description = "Test description",
                LogoUrl = "logo.jpg",
                WebsiteUrl = "test.com",
                InstagramUrl = "instagram.com",
                FacebookUrl = "facebook.com",
                TwitterUrl = "twitter.com",
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            });
        }
    }
}
