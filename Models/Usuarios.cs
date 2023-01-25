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
    public DateTime fechanac { get; set; }

    public static Func<Usuario, object> getFunctionOrderBy(String orderby = "email") {
        switch(orderby.ToLower()) {
            case "nombre": return item => item.nombre;
            case "apellidos": return item => item.apellidos;
            case "dni": return item => item.dni;
            case "telefono": return item => item.telefono;
            case "calle": return item => item.calle;
            case "codpos": return item => item.codpos;
            case "email": default: return item => item.email;
        }
    }
}
