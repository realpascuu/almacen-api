using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Usuario
{
    [Key]
    public String email { get; set; }
    public String password { get; set; }
    public String nombre { get; set; }
    public String apellidos { get; set; }
    public String dni { get; set; }
    public String telefono { get; set; }
    public String calle { get; set; }
    public String codpos { get; set; }
    public String fechanac { get; set; }
}