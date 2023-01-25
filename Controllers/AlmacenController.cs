using almacenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace almacenAPI.Controllers;
[ApiController]
[Route("api/almacenes")]
public class AlmacenController : ControllerBase
{
    private readonly ApplicationDbContext context;
    
    public AlmacenController(ApplicationDbContext _context)
    {
        this.context = _context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Almacen>>> Get()
    {
        return await context.Almacen.ToListAsync();
    }

    [HttpGet]
    [Route("page")]
    public async Task<ActionResult<Pagination<Almacen>>> GetList(
        [FromQuery]
        string? orderby = "nombre",
        [FromQuery]
        int page = 1, 
        [FromQuery]
        int limit = 6
    )
    {
        var offset =  ( page - 1 ) * limit;
        int total_objects = context.Almacen.ToList().Count;
        var total_pages = (int)Math.Ceiling((total_objects / (double)limit));
        if(page < 1 || page > total_pages) {
            return BadRequest($"Page {page} not suported");
        }

        var results = await context.Almacen
        .OrderBy(/*Almacen.getFunctionOrderBy(orderby)*/item => item.nombre)
        //.ThenBy(Almacen.getFunctionOrderBy("nombre"))
        .Skip(offset)
        .Take(limit)
        .ToListAsync();

        return new Pagination<Almacen>(
            total_objects,
            page,
            results,
            limit,
            total_pages
        );
    }

    [HttpGet("{nombre}")]
    public async Task<ActionResult<Almacen>> GetById(String nombre)
    {
        var almacen = context.Almacen.FirstOrDefault(item => item.nombre == nombre);
        if(almacen != null) {
            return almacen;
        }
        return BadRequest($"Almacen with name {nombre} not found");
    }

    [HttpPost]
    public async Task<ActionResult> Post(Almacen almacen) {
        
        try {
            var result = context.Add(almacen);
            await context.SaveChangesAsync();
            return Ok();
        } catch(Exception) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
        }
    }


    [HttpDelete("{nombre}")]
    public async Task<ActionResult> Delete(String nombre) {


        var almacen = context.Almacen.SingleOrDefault(item => item.nombre == nombre);
        if(almacen != null) {
           
            context.Almacen.Remove(almacen);
            await context.SaveChangesAsync();
            return Ok();
           
            
        }
        return BadRequest($"Almacen with name {nombre} not found");
    }
}
