using System.ComponentModel.DataAnnotations;
using System.Text;

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

    public String generatePasswordHash(String random_password)
    {
        var crypt = new System.Security.Cryptography.SHA256Managed();
        var hash = new System.Text.StringBuilder();
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(random_password));
        foreach (byte theByte in crypto)
        {
            hash.Append(theByte.ToString("x2"));
        }
        return hash.ToString();
        
    } 

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

public class LoginModel {
    public String email { get; set; }
    public String password { get; set; }
}