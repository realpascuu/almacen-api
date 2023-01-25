using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Pedido
{
    [Key]
    public int numped { get; set; }
    public bool venta { get; set; }
    public DateTime fecha_pedido  { get; set; }
    public DateTime fecha_factura  { get; set; }
}