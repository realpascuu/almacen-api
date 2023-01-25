using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Movimiento
{
    [Key]
    public int id { get; set; }
    public int almacen_salida { get; set; }
    public int almacen_entrada { get; set; }
    
}