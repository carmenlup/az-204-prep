using Microsoft.EntityFrameworkCore;
using WebApp.Data.Entities;

namespace WebApp.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public required DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().ToTable("Products");
        }
    }
}
