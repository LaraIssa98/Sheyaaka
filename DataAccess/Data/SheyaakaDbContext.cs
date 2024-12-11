using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess.Data
{
    public class SheyaakaDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public SheyaakaDbContext(DbContextOptions<SheyaakaDbContext> options) : base(options) { }

        



    }
}
