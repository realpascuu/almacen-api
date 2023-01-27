using almacenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace almacenAPI.Controllers;
[ApiController]
[Route("api/movimientos")]
public class MovimientoController : ControllerBase
{
    private readonly ApplicationDbContext context;
    
    public MovimientoController(ApplicationDbContext _context)
    {
        this.context = _context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Movimiento>>> Get()
    {
        return await context.Movimiento.ToListAsync();
    }

    [HttpGet]
    [Route("page")]
    public async Task<ActionResult<Pagination<Movimiento>>> GetList(
        [FromQuery]
        string? orderby = "almacen_salida",
        [FromQuery]
        int page = 1, 
        [FromQuery]
        int limit = 6,
        [FromQuery]
        int almacen = -1
    )
    {
        var offset =  ( page - 1 ) * limit;
        int total_objects = 0;
        if(almacen > 0) {
            total_objects = context.Movimiento
            .Where(item => item.almacen_entrada == almacen || item.almacen_salida == almacen)
            .ToList().Count;
        } else {
            total_objects = context.Movimiento.ToList().Count;
        }
        var total_pages = (int)Math.Ceiling((total_objects / (double)limit));
        if(page < 1 || page > total_pages) {
            return BadRequest($"Page {page} not suported");
        }
        var results = new List<Movimiento>();
        if(almacen > 0) {
            results = await context.Movimiento
                .Where(item => item.almacen_entrada == almacen || item.almacen_salida == almacen)
                .OrderBy(/*Movimiento.getFunctionOrderBy(orderby)*/item => item.almacen_salida)
                //.ThenBy(Movimiento.getFunctionOrderBy("email"))
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        } else {
            results = await context.Movimiento
                .OrderBy(/*Movimiento.getFunctionOrderBy(orderby)*/item => item.almacen_salida)
                //.ThenBy(Movimiento.getFunctionOrderBy("email"))
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
        }

        return new Pagination<Movimiento>(
            total_objects,
            page,
            results,
            limit,
            total_pages
        );
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovimientoPost>> GetById(int id)
    {
        var movimiento = context.Movimiento.FirstOrDefault(item => item.id == id);
        if(movimiento != null) {
            List<ArticuloMovimientoPost> articulosmovimiento = new List<ArticuloMovimientoPost>();
            var almacen_entrada = context.Almacen.FirstOrDefault(item => item.id == movimiento.almacen_entrada); 
            var almacen_salida = context.Almacen.FirstOrDefault(item => item.id == movimiento.almacen_salida);
            var linpeds = await context.ArticuloMovimiento.Where(item => item.idmovimiento == id).ToListAsync();
            foreach(var linped in linpeds) {
                var articulo = context.Articulo.FirstOrDefault(item => item.cod == linped.articulo);
                if(articulo != null) {
                    articulosmovimiento.Add(new ArticuloMovimientoPost(articulo.nombre, linped.cantidad));
                }
            } 
            return new MovimientoPost(almacen_entrada, almacen_salida, articulosmovimiento);
        }
        return BadRequest($"Movment with id {id} not found");
    }

    [HttpPost]
    public async Task<ActionResult> Post(MovimientoCreate mov_create) {
        try {
            var errors = new List<String>();
            var movimiento = mov_create.movimiento;
            var some_add = false; 
            var added = new List<ArticuloMovimientoFinalCreate>();
            context.Add(movimiento);
            foreach(var producto in mov_create.productos) {
                var alm_art = context.Almacen_Articulo.FirstOrDefault(item => item.codAlm == movimiento.almacen_salida && item.codArt == producto.articulo);
                Console.WriteLine(alm_art);
                if(alm_art != null && alm_art.cantidad >= producto.cantidad) {
                    some_add = true;
                    alm_art.cantidad = alm_art.cantidad - producto.cantidad;
                    var alm_art_entrada = context.Almacen_Articulo.FirstOrDefault(item => item.codAlm == movimiento.almacen_entrada && item.codArt == producto.articulo);
                    if(alm_art_entrada != null) {
                        alm_art_entrada.cantidad = alm_art_entrada.cantidad + producto.cantidad; 
                    } else {
                        Console.WriteLine(movimiento.id);
                        var new_almacen_art = new Almacen_Articulo(movimiento.almacen_entrada, producto.articulo, producto.cantidad);
                        context.Add(new_almacen_art);
                    }
                    
                    var art_mov = new ArticuloMovimientoFinalCreate(producto.articulo, producto.cantidad, movimiento.id);
                    //context.Add(art_mov);
                    added.Add(art_mov);
                } else {
                    errors.Add("Movement not supported");
                }
            }
            if (some_add == true) {
                await context.SaveChangesAsync();
                Console.WriteLine(movimiento.id);
                
                foreach(var add in added) {
                    context.Add(new ArticuloMovimiento(add.articulo, add.cantidad, movimiento.id));
                }
                await context.SaveChangesAsync();
            }
            return Ok(errors);
        } catch(Exception e) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
        }
    }
}
