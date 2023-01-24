using almacenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace almacenAPI.Controllers;
[ApiController]
[Route("api/users")]
public class UsuariosController : ControllerBase
{
    private readonly ApplicationDbContext context;
    
    public UsuariosController(ApplicationDbContext _context)
    {
        this.context = _context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Usuario>>> Get()
    {
        return await context.Usuario.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult> Post(Usuario usuario) {
        context.Add(usuario);
        await context.SaveChangesAsync();
        return Ok();
    }

}
