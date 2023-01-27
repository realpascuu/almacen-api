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
    
    public int articulo{ get; set; }
    public int cantidad { get; set; }
  
}

public class MovimientoPost
{
    public List<ArticuloMovimientoPost> articulosmovimiento {get;set;}
    public int almacen_entrada{ get; set; }
    public int almacen_salida { get; set; }
}
