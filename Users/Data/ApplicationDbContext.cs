using Microsoft.EntityFrameworkCore;
using Users.Api.Models;

namespace Users.Api.Data
{
    public class ApplicationDbContext :DbContext
    {
        public DbSet<User> Users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }



    }
}
