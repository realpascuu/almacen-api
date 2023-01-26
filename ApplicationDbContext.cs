using Microsoft.EntityFrameworkCore;
using almacenAPI.Models;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace almacenAPI;
public class ApplicationDbContext: DbContext 
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Pedido> Pedido { get; set; }
    public DbSet<Linped> Linped { get; set; }
    public DbSet<Articulo> Articulo { get; set; }
    public DbSet<Almacen_Articulo> Almacen_Articulo { get; set; }
    public DbSet<Almacen> Almacen { get; set; }
    public DbSet<Categoria> Categoria { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Almacen_Articulo>()
            .HasKey(b => new { b.codArt, b.codAlm });
    }
}