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
    public async Task<ActionResult<DetallesArticulo<Articulo>>> GetById()
    {
         //List<Articulo> producto = new List<Articulo>();
         var res = context.Articulo.FirstOrDefault(item => item.cod == Convert.ToInt32(ControllerContext.RouteData.Values["id"]));
    
        if(res!=null){
        //producto.Add(res);
         return new DetallesArticulo<Articulo>(
              res.nombre,
              res.especificaciones,
              res.pvp,
              res.cod,
              res.categoria
         );
        }
        /*
        var results = await context.Articulo
        .ToListAsync();
        
        foreach (var res in results.ToList())
            {
                if(res.cod == cod){
                    List<Articulo> producto = new List<Articulo>();
                    producto.Add(res);
                    return new DetallesArticulo<Articulo>(
                        producto
                    );
                }
            }
            /*
        if(results != null){
            return new DetallesArticulo<Articulo>(
                results
            );
        }
        */
        return BadRequest("No existe el producto, Error");
    }


    [HttpGet]
    [Route("categorias")]
    public async Task<ActionResult<ListaCategorias<Categoria>>> GetList( )
    {

        var results = await context.Categoria.ToListAsync();

        return new ListaCategorias<Categoria>(
           results
        );
    }
    

 [HttpPost]
 [Route("crear")]
    public async Task<ActionResult> Post([FromBody] Articulo json) {
        try {
            var result = context.Add(json);
            await context.SaveChangesAsync();
            return Ok();
        } catch(Exception) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
        }
    }
}
