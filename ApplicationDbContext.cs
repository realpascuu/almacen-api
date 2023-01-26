using Microsoft.EntityFrameworkCore;
using almacenAPI.Models;

namespace almacenAPI;
public class ApplicationDbContext: DbContext 
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Usuario> Usuario { get; set; }
    public DbSet<Almacen> Almacen { get; set; }
    public DbSet<Articulo> Articulo { get; set; }
}