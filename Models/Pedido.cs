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

public class PedidoDetails 
{
    public String nombre { get; set; } 
    public int cantidad { get; set; } 
    public float precio { get; set; } 

    public PedidoDetails(String nombre, int cantidad, float precio) {
        this.cantidad = cantidad;
        this.precio = precio;
        this.nombre = nombre;
    }
}

public class PedidoWithLinpeds {
    public Pedido pedido { get; set; }
    public List<PedidoDetails> linpeds { get; set; }

    public PedidoWithLinpeds(Pedido pedido, List<PedidoDetails> linpeds) {
        this.pedido = pedido;
        this.linpeds = linpeds;
    }
}
