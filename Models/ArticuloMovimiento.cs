using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class ArticuloMovimiento
{
    [Key]
    public int id { get; set; }
    public int articulo{ get; set; }
    public int cantidad { get; set; }
    public int idmovimiento { get; set; }
    public ArticuloMovimiento(int articulo, int cantidad, int idmovimiento)
    {
        this.articulo=articulo;
        this.cantidad=cantidad;
        this.idmovimiento=idmovimiento;
    }
}


public class ArticuloMovimientoPost
{
    
    public String nombre { get; set; }
    public int cantidad { get; set; }

    public ArticuloMovimientoPost(String nombre, int cantidad)
    {
        this.nombre = nombre;
        this.cantidad = cantidad;
    }
  
}

public class MovimientoPost
{
    public List<ArticuloMovimientoPost> articulosmovimiento {get;set;}
    public Almacen? almacen_entrada{ get; set; }
    public Almacen? almacen_salida { get; set; }

    public MovimientoPost(Almacen? almacen_entrada, Almacen? almacen_salida, List<ArticuloMovimientoPost> articulosmovimiento)
    {
        this.almacen_entrada = almacen_entrada;
        this.almacen_salida = almacen_salida;
        this.articulosmovimiento = articulosmovimiento;
    }
}
