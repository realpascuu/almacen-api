using Microsoft.EntityFrameworkCore;
using almacenAPI.Models;

namespace almacenAPI;
public class ApplicationDbContext: DbContext 
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    
    public DbSet<Usuario> Usuario { get; set; }
}