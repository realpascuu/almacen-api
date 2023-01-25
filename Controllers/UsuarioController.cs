using almacenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

    [HttpGet]
    [Route("page")]
    public async Task<ActionResult<Pagination<Usuario>>> GetList(
        [FromQuery]
        string? orderby = "email",
        [FromQuery]
        int page = 1, 
        [FromQuery]
        int limit = 6
    )
    {
        var offset =  ( page - 1 ) * limit;
        int total_objects = context.Usuario.ToList().Count;
        var total_pages = (int)Math.Ceiling((total_objects / (double)limit));
        if(page < 1 || page > total_pages) {
            return BadRequest($"Page {page} not suported");
        }

        var results = await context.Usuario
        .OrderBy(/*Usuario.getFunctionOrderBy(orderby)*/item => item.email)
        //.ThenBy(Usuario.getFunctionOrderBy("email"))
        .Skip(offset)
        .Take(limit)
        .ToListAsync();

        return new Pagination<Usuario>(
            total_objects,
            page,
            results,
            limit,
            total_pages
        );
    }

    [HttpGet("{email}")]
    public async Task<ActionResult<Usuario>> GetById(String email)
    {
        var user = context.Usuario.FirstOrDefault(item => item.email == email);
        if(user != null) {
            return user;
        }
        return BadRequest($"User with email {email} not found");
    }

    [HttpPost]
    public async Task<ActionResult> Post(Usuario usuario) {
        try {
            context.Add(usuario);
            await context.SaveChangesAsync();
            return Ok();
        } catch(Exception) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
        }
    }

    [HttpPut("{email}")]
    public async Task<ActionResult<Usuario>> Put(String email, Usuario usuario) {
        if(usuario.email != email) {
            return BadRequest($"User with email {email} mismatch");
        }
        var user = context.Usuario.SingleOrDefault(item => item.email == email);
        if(user != null) {
            user.password = usuario.password;
            user.nombre = usuario.nombre;
            user.apellidos = usuario.apellidos;
            user.dni = usuario.telefono;
            user.calle = usuario.calle;
            user.codpos = usuario.codpos;
            user.fechanac = usuario.fechanac;
            await context.SaveChangesAsync();
            return user;
        }
        return BadRequest($"User with email {email} not found");
    }

    [HttpDelete("{email}")]
    public async Task<ActionResult> Delete(String email) {
        var user = context.Usuario.SingleOrDefault(item => item.email == email);
        if(user != null) {
            context.Usuario.Remove(user);
            await context.SaveChangesAsync();
            return Ok();
        }
        return BadRequest($"User with email {email} not found");
    }
}
