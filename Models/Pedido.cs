using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Pedido
{
    [Key]
    public int numped { get; set; }
    public bool venta { get; set; }
    public DateTime fecha_pedido  { get; set; }
    public DateTime? fecha_factura  { get; set; }

    public Pedido(bool venta)
    {
        this.venta = venta;
        this.fecha_pedido = DateTime.Now;
        this.fecha_factura = null;
    }
}

public class PedidoPost 
{
    public bool venta { get; set; }
    public List<LinpedPost> lineas { get; set; }
}