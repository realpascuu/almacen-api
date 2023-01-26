using System.ComponentModel.DataAnnotations;

namespace almacenAPI.Models;

public class Linped
{
    [Key]
    public int linea { get; set; }
    public int cantidad { get; set; }  
    public int codArt { get; set; }
    public int pedido  { get; set; }

    public Linped(int cantidad, int codArt, int pedido)
    {
        this.cantidad = cantidad;
        this.codArt = codArt;
        this.pedido = pedido;
    }
}

public class LinpedPost
{
    public int cantidad { get; set; }  
    public int codArt { get; set; }
}