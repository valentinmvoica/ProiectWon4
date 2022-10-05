using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Models
{
    public class CatalogueDbContext : DbContext
    {
        private readonly string connectionString;
        public DbSet<Student> Students { get; set; }
        public DbSet<Address> Addresses { get; set; }

        public CatalogueDbContext(IConfiguration configuration)
        {
            this.connectionString = configuration.GetConnectionString("LocalDb");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.connectionString);
        }

    }
}
