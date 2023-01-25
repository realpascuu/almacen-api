using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class ArticuloMovimiento
{
    [Key]
    public int id { get; set; }
    public string articulo{ get; set; }
    public int cantidad { get; set; }
    public int idmovimiento { get; set; }
}