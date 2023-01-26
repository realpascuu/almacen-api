using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;
public class Almacen_Articulo
{
    
    [Key]
    public int codAlm { get; set; }
    [Key]
    public int codArt { get; set; }
    public int cantidad { get; set; }      

    public static Func<Almacen_Articulo, object> getFunctionOrderBy(String orderby = "codAlm") {
        switch(orderby.ToLower()) {
            case "codAlm":  return item => item.codAlm;
            case "codArt": default:return item => item.codArt;
            }
    }

   
}