using BookTrader.Models;
using Microsoft.EntityFrameworkCore;

namespace BookTrader.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        public DbSet<Categorias> Categorias { get; set; }
    }
}
