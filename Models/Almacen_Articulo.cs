using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace almacenAPI.Models;
public class Almacen_Articulo
{

    
    [Key]
    public int codAlm { get; set; }
    public int codArt { get; set; }
    public int cantidad { get; set; }      
    public Almacen_Articulo(int codAlm, int codArt, int cantidad)
    {
        this.cantidad = cantidad;
        this.codAlm = codAlm;
        this.codArt = codArt;
    }   

    public static Func<Almacen_Articulo, object> getFunctionOrderBy(String orderby = "codAlm") {
        switch(orderby.ToLower()) {
            case "codAlm":  return item => item.codAlm;
            case "codArt": default:return item => item.codArt;
            }
    }  

}

public class Almacen_ArticuloAll
{
    
    public int cantidad { get; set; }    
    public string articulo{get;set;}

     public Almacen_ArticuloAll( int cantidad, string articulo)
    {
        this.cantidad = cantidad;
       
        this.articulo = articulo;
    }  

}