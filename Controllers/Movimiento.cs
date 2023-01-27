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
        int limit = 6
    )
    {
        var offset =  ( page - 1 ) * limit;
        int total_objects = context.Movimiento.ToList().Count;
        var total_pages = (int)Math.Ceiling((total_objects / (double)limit));
        if(page < 1 || page > total_pages) {
            return BadRequest($"Page {page} not suported");
        }

        var results = await context.Movimiento
        .OrderBy(/*Movimiento.getFunctionOrderBy(orderby)*/item => item.almacen_salida)
        //.ThenBy(Movimiento.getFunctionOrderBy("email"))
        .Skip(offset)
        .Take(limit)
        .ToListAsync();

        return new Pagination<Movimiento>(
            total_objects,
            page,
            results,
            limit,
            total_pages
        );
    }

    [HttpGet("{almacen}")]
    public async Task<ActionResult<List<Movimiento>>> GetById(int almacen)
    {
        return await context.Movimiento.Where(item => item.almacen_entrada == almacen || item.almacen_salida == almacen).ToListAsync();
    }

/*
    [HttpPost]
    public async Task<ActionResult> Post( MovimientoPost movimiento) {
        try {
            var almacen_e = context.Almacen.SingleOrDefault(item => item.id == movimiento.almacen_entrada);
            var almacen_s = context.Almacen.SingleOrDefault(item => item.id == movimiento.almacen_salida);
            if(almacen_e != null && almacen_s != null && movimiento.articulosmovimiento.Count!= 0) {
                var mov = new Movimiento(movimiento.almacen_entrada, movimiento.almacen_salida);
                context.Add(mov);
                var idmov = context.Movimiento.Last().id;

                    foreach(ArticuloMovimientoPost art in movimiento.articulosmovimiento)
                    {
                        var alm_art_s = context.Almacen_Articulo.SingleOrDefault(item => item.codAlm == almacen_s.id & item.codArt == art.articulo && item.cantidad<art.cantidad  );
                        if (alm_art_s!=null)
                        {
                            var alm_art_e = context.Almacen_Articulo.SingleOrDefault(item => item.codAlm == almacen_e.id &&   item.codArt == art.articulo );
                            if (alm_art_e!=null)
                            {
                                alm_art_e.cantidad += art.cantidad;
                                alm_art_s.cantidad -= art.cantidad;
                            }else
                            {
                                alm_art_s.cantidad -= art.cantidad;
                                context.Add(new Almacen_Articulo(almacen_e.id ,art.articulo,art.cantidad));
                            }
                            context.Add(new ArticuloMovimiento( art.articulo,art.cantidad,idmov) );
                        }else
                        {
                            throw new Exception("Fallo con el primer almacen");
                        }

                    }

                await context.SaveChangesAsync();
                return Ok();
            }
            throw new Exception("Fallo parametros");
        } catch(Exception) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
        }
    }
   
    */
}
