using almacenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace almacenAPI.Controllers;
[ApiController]
[Route("api/pedidos")]
public class PedidosController : ControllerBase
{
    private readonly ApplicationDbContext context;
    
    public PedidosController(ApplicationDbContext _context)
    {
        this.context = _context;
    }

    [HttpGet]
    public async Task<ActionResult<List<Pedido>>> Get()
    {
        return await context.Pedido.ToListAsync();
    }

    [HttpGet]
    [Route("page")]
    public async Task<ActionResult<PaginationArticulo<Pedido>>> GetList(
        [FromQuery]
        string? orderby = "fecha_pedido",
        [FromQuery]
        int page = 1, 
        [FromQuery]
        int limit = 6,
        [FromQuery]
        int factura_value = 0
    )
    {
        var factura = !(factura_value == 0);
        var offset =  ( page - 1 ) * limit;
        int total_objects = context.Pedido.ToList().Count;
        var total_pages = (int)Math.Ceiling((total_objects / (double)limit));
        if(page < 1 || page > total_pages) {
            return BadRequest($"Page {page} not suported");
        }

        var results = new List<Pedido>();
        if(factura == true) {
            results = await context.Pedido
            .Where(item => item.fecha_factura != null)
            .OrderBy(item => item.fecha_pedido)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        } else {
            results = await context.Pedido
            .Where(item => item.fecha_factura == null)
            .OrderBy(item => item.fecha_pedido)
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        }

        int previousPage = -1;
        int nextPage = -1;

        if(page > 1)
            previousPage = page - 1 ;
        if(page < total_pages)
            nextPage = page + 1 ;

        return new PaginationArticulo<Pedido>(
            total_objects,
            page,
            results,
            limit,
            total_pages,
            previousPage,
            nextPage
        );
    }

    // TODO: detalles pedido se quiere linped
    [HttpGet("{numped}")]
    public async Task<ActionResult<PedidoWithLinpeds>> GetById(int numped)
    {
        var pedido = context.Pedido.FirstOrDefault(item => item.numped == numped);
        var results = new List<PedidoDetails>();
        if(pedido != null) {
            var linpeds = await context.Linped.Where(item => item.pedido == numped).ToListAsync();
            foreach(var linped in linpeds) {
                var articulo = context.Articulo.Where(item => item.cod == linped.codArt).FirstOrDefault();
                if(articulo != null) {
                    results.Add(new PedidoDetails(articulo.nombre, linped.cantidad, articulo.pvp));
                }
            }
            return new PedidoWithLinpeds(pedido, results);
        }
        return BadRequest($"Pedido with numped {numped} not found");
    }

    [HttpPost]
    public async Task<ActionResult<List<String>>> Post(PedidoPost pedido) {
        try {
            if (pedido.lineas.Count == 0) {
                return BadRequest($"Order can not be empty");
            }
            var errors = new List<String>();
            var ped = new Pedido(pedido.venta);
            context.Add(ped);
            context.SaveChanges();
            var some_added = false;
            foreach(LinpedPost linped in pedido.lineas) {
                if(pedido.venta == true) {
                    var linped_added = false;
                    var almacenes = await context.Almacen_Articulo.Where(item => item.codArt == linped.codArt).ToListAsync();
                    foreach(Almacen_Articulo almacen in almacenes) {
                        if (linped.cantidad <= almacen.cantidad) {
                            some_added = true;
                            linped_added = true;
                            almacen.cantidad = almacen.cantidad - linped.cantidad;
                            context.Add(new Linped(linped.cantidad, linped.codArt, ped.numped));
                            break;
                        }
                    }
                    if(linped_added == false) {
                        errors.Add($"Linped {linped.codArt} no tiene existencias suficientes. Realice algún movimiento");
                    }
                } else {
                    some_added = true;
                    var almacen = context.Almacen.FirstOrDefault();
                    var almacen_articulo = context.Almacen_Articulo.Where(item => item.codArt == linped.codArt && item.codAlm == almacen.id).FirstOrDefault();
                    if(almacen_articulo != null) {
                        almacen_articulo.cantidad = almacen_articulo.cantidad + linped.cantidad; 
                    } else {
                        var new_almacen_art = new Almacen_Articulo(almacen.id, linped.codArt, linped.cantidad);
                        context.Add(new_almacen_art);
                    }
                    context.Add(new Linped(linped.cantidad, linped.codArt, ped.numped));
                }

            }
            if(some_added == true) {
                await context.SaveChangesAsync();
                return Ok(errors);
            } else {
                context.Remove(ped);
                return BadRequest(errors);
            }
            return Ok();
        } catch(Exception e) {
            Console.WriteLine(e);
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
        }
    }

    // TODO: no se edita, se genera factura
    [HttpPut("{numped}/generar-factura")]
    public async Task<ActionResult<Pedido>> Put(int numped) {
        var pedido = context.Pedido.SingleOrDefault(item => item.numped == numped);
        if(pedido != null) {
            pedido.fecha_factura = DateTime.Now;
            await context.SaveChangesAsync();
            return pedido;
        }
        return BadRequest($"Order with numped {numped} not found");
    }

    [HttpDelete("{numped}")]
    public async Task<ActionResult> Delete(int numped) {
        var pedido = context.Pedido.SingleOrDefault(item => item.numped == numped);
        if(pedido != null) {
            // borrar todas las lineas de pedido
            var linpeds = await context.Linped.Where(item => item.pedido == numped).ToListAsync();
            foreach(Linped l in linpeds) {
                context.Linped.Remove(l);    
            }
            context.Pedido.Remove(pedido);
            await context.SaveChangesAsync();
            return Ok();
        }
        return BadRequest($"Order with numped {numped} not found");
    }
}
