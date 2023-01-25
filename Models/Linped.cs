using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Linped
{
    [Key]
    public int linea { get; set; }
    public int cantidad { get; set; }  
    public int codArt { get; set; }
    public Pedido pedido  { get; set; }
}
