using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Almacen_Articulo
{
    [Key]
    public int codAlm { get; set; }
    [Key]
    public int codArt { get; set; }
    public int cantidad { get; set; }      
}