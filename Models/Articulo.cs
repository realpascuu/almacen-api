using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Articulo
{
    [Key]
    public int cod  { get; set; }
    public string nombre { get; set; }
    public string especificaciones  { get; set; }
    public float pvp { get; set; }
    public string imagen { get; set; }
    public string categoria { get; set; }


    public static Func<Articulo, object> getFunctionOrderBy(String orderby = "nombre") {
        switch(orderby.ToLower()) {
            case "nombre": default: return item => item.nombre;
           
        }
    }
}