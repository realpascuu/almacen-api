using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Almacen
{
    [Key]
    public int id  { get; set; }
    public String localidad  { get; set; }
    public String nombre  { get; set; }
}