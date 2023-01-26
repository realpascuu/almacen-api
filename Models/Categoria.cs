using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Categoria
{
    [Key]
    public String categoria  { get; set; }
    public String empresa  { get; set; }
}
