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
    public async Task<ActionResult<PaginationArticulo<Articulo>>> GetList(
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
        .Skip(offset)
        .Take(limit)
        .ToListAsync();

        int previousPage = -1;
        int nextPage = -1;

        if(page > 1)
            previousPage = page - 1 ;
        if(page < total_pages)
            nextPage = page + 1 ;

        return new PaginationArticulo<Articulo>(
            total_objects,
            page,
            results,
            limit,
            total_pages,
            previousPage,
            nextPage
            
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DetallesArticulo<Articulo>>> GetByIdAsync(int cod)
    {
        var results = await context.Articulo
        .ToListAsync();
        
        results = (List<Articulo>)results.Where(item => item.cod == cod);

        if(results != null){
            return new DetallesArticulo<Articulo>(
                results
            );
        }
        return BadRequest("No existe el producto, Error");
    }

}
