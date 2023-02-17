using Microsoft.EntityFrameworkCore;
using UniClubDataAPI.Models;

namespace UniClubDataAPI.Data
{
    public class ApplicationDBContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; }
    }
}
