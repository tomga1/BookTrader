﻿using BookTrader.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace BookTrader.Data
{
    public class ApplicationDbContext : IdentityDbContext<Users>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Categorias> Categorias { get; set; }
        public DbSet<Libros> Libros { get; set; }
        public DbSet<Sugerencias> Sugerencias { get; set; }

        public DbSet<Condiciones> Condiciones { get; set; }
        public DbSet<IdiomasEntity> Idiomas { get; set; }
        public DbSet<Paises> Paises { get; set; }
        public DbSet<Provincias> Provincias { get; set; }
        public DbSet<Localidades> Localidades { get; set; }
        public DbSet<FormatoEntity> Formato { get; set; }
        public DbSet<SubCategorias> SubCategorias { get; set; }
        public DbSet<EstadoPublicacionEntity> EstadoPublicacionEntity { get; set; }
        public DbSet<PlanesSuscripcion> PlanesSuscripcion { get; set; }
        public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }




        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Libros>()
                .Property(b => b.Precio)
                .HasColumnType("decimal(18,2)");


            base.OnModelCreating(builder);



        }
    }
}
