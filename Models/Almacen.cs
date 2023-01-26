using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Almacen
{
 
    
    [Key]
    public int id  { get; set; }
    public String localidad  { get; set; }
    public String nombre  { get; set; }

    public static Func<Almacen, object> getFunctionOrderBy(String orderby = "email") {
        switch(orderby.ToLower()) {
            case "localidad":  return item => item.localidad;
            case "nombre": default:return item => item.nombre;
        }
    }

   public   List<Almacen_Articulo> getArticulosAlmacen(ApplicationDbContext context)
   {       
        return  context.Almacen_Articulo.Where(item => item.codAlm == this.id).ToList() ;
   }
}