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
    public async Task<ActionResult<PaginationArticulo<Almacen>>> GetList(
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

        int previousPage = -1;
        int nextPage = -1;

        if(page > 1)
            previousPage = page - 1 ;
        if(page < total_pages)
            nextPage = page + 1 ;

        return new PaginationArticulo<Almacen>(
            total_objects,
            page,
            results,
            limit,
            total_pages,
            previousPage,
            nextPage
        );
    }

    [HttpGet("{nombre}")]
    
    public async Task<ActionResult<PaginationArticulo<Almacen_ArticuloAll>>> GetById(
        String nombre,
        [FromQuery]
        string? orderby = "nombre",
        [FromQuery]
        int page = 1, 
        [FromQuery]
        int limit = 6)
    {
        Almacen almacen =context.Almacen.FirstOrDefault(item => item.nombre == nombre);
        if (almacen != null){
             List<Almacen_ArticuloAll> results = new List<Almacen_ArticuloAll>();

        var articulos = await context.Almacen_Articulo.Where(item => item.codAlm == almacen.id).ToListAsync();
        foreach (var articulo in articulos)
        {
            var art =  context.Articulo.SingleOrDefault(item=>item.cod == articulo.codArt);
            results.Add(new Almacen_ArticuloAll(articulo.cantidad, art.nombre, art.cod));
        }
        
        var offset =  ( page - 1 ) * limit;
        int total_objects = results.ToList().Count;
        var total_pages = (int)Math.Ceiling((total_objects / (double)limit));
        if(page < 1 || page > total_pages) {
            return BadRequest($"Page {page} not suported");
        }

        int previousPage = -1;
        int nextPage = -1;

        if(page > 1)
            previousPage = page - 1 ;
        if(page < total_pages)
            nextPage = page + 1 ;

        var items = new PaginationArticulo<Almacen_ArticuloAll>(          
            total_objects,
            page,
            results,
            limit,
            total_pages,
            previousPage,
            nextPage
        );
        if(items != null) {
           return items;
           
        }
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
