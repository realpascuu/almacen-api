using almacenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace almacenAPI.Controllers;
[ApiController]
[Route("api/articulos")]
public class ArticuloController : ControllerBase
{
    private readonly ApplicationDbContext context;
    
    public ArticuloController(ApplicationDbContext _context)
    {
        this.context = _context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Articulo>>> Get()
    {
        return await context.Articulo.ToListAsync();
    }
     [HttpGet]
    [Route("page")]
    public async Task<ActionResult<Pagination<Articulo>>> GetList(
        [FromQuery]
        string? orderby = "nombre",
        [FromQuery]
        int page = 1, 
        [FromQuery]
        int limit = 6
    )
    {
        var offset =  ( page - 1 ) * limit;
        int total_objects = context.Articulo.ToList().Count;
        var total_pages = (int)Math.Ceiling((total_objects / (double)limit));
        if(page < 1 || page > total_pages) {
            return BadRequest($"Page {page} not suported");
        }

        var results = await context.Articulo
        .OrderBy(/*Articulo.getFunctionOrderBy(orderby)*/item => item.nombre)
        //.ThenBy(Articulo.getFunctionOrderBy("nombre"))
        .Skip(offset)
        .Take(limit)
        .ToListAsync();

        return new Pagination<Articulo>(
            total_objects,
            page,
            results,
            limit,
            total_pages
        );
    }

    
}
