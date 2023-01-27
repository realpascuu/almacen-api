using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Movimiento
{
    [Key]
    public int id { get; set; }
    public int almacen_salida { get; set; }
    public int almacen_entrada { get; set; }

    public Movimiento(int almacen_entrada, int almacen_salida)
    {
        this.almacen_entrada=almacen_entrada;
        this.almacen_salida=almacen_salida;
    }
    public static Func<Movimiento, object> getFunctionOrderBy(String orderby = "id") {
         switch(orderby.ToLower()) {
            case "almacen_entrada":  return item => item.almacen_entrada;
            case "almacen_salida": return item => item.almacen_salida;
            case "id": default:return item => item.id;
        }
    }
    
}

public class MovimientoCreate 
{   
    public Movimiento movimiento { get; set; }
    public List<ArticuloMovimientoCreate> productos { get; set; }
}

