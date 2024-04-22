using Backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Access
{
    public class TiendaContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Purchase> Purchases { get; set;}
    }
}
