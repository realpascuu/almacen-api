using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace almacenAPI.Models;

public class Almacen_Articulo
{
    public int codAlm { get; set; }
    public int codArt { get; set; }
    public int cantidad { get; set; }

    public Almacen_Articulo(int codAlm, int codArt, int cantidad)
    {
        this.cantidad = cantidad;
        this.codAlm = codAlm;
        this.codArt = codArt;
    }   
}