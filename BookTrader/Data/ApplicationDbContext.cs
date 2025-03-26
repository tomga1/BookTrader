using BookTrader.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookTrader.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Libros> Libros { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Libros>()
                .Property(b => b.Precio)
                .HasColumnType("decimal(18,2)");


            base.OnModelCreating(builder);
        }
    }
}
