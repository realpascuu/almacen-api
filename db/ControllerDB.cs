using System;
using System.Data;
using System.Data.SQLite;
using System.Web.Services;
using Newtonsoft.Json;



public class Usuario
{

    public string email  { get; set; }
    public string password  { get; set; }
    public string nombre  { get; set; }
    public string apellidos { get; set; }
    public string dni  { get; set; }
    public int telefono  { get; set; }
    public string calle  { get; set; }
    public int codpos  { get; set; }
    public date fechanac { get; set; }
}

public class Almacen
{
    public int id  { get; set; }
    public string localidad  { get; set; }
    public string nombre  { get; set; }
}


public class Categoria
{
    public string categoria  { get; set; }
    public string empresa  { get; set; }
}

public class Articulo
{

    public int cod  { get; set; }
    public string nombre { get; set; }
    public string especificaciones  { get; set; }
    public float pvp { get; set; }
    public string imagen { get; set; }
    public string categoria { get; set; }
}


public class Almacen_Articulo
{
    public String almacen { get; set; }
    public String articulo { get; set; }
    public int cantidad { get; set; }      
}


public class Pedido
{
    public int numped { get; set; }
    public bool venta { get; set; }
    public date fecha_pedido  { get; set; }
    public date fecha_factura  { get; set; }
}

public class Linped
{
    public int linea { get; set; }
    public int cantidad { get; set; }  
    public int codArt { get; set; }
    public int pedido  { get; set; }
}


[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Service : System.Web.Services.WebService
{
    //get productos de un almacen
    [WebMethod]
    public string GetProductosAlmacen(int almacen)
    {
         var articulos = new List<Almacen_Articulo>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT Articulo.nombre, Almacen.nombre, cantidad FROM Almacen_Articulo, Almacen, Articulo where "+ almacen +" = Almacen.id and codAlm ="+ almacen + "and codArt = Articulo.cod", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                         articulos.Add(new Almacen_Articulo
                        {
                            almacen = Convert.ToString(reader["Almacen.nombre"]),
                            articulo = Convert.ToString(reader["Articulo.nombre"])
                            cantidad = Convert.ToInt32(reader["cantidad"])
                        });
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(articulos);
    }
    //get articulos todos almacenes
    [WebMethod]
    public string GetArticulos()
    {
        var articulos = new List<Articulo>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM articulo", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        articulos.Add(new Articulo
                        {
                            cod = Convert.ToInt32(reader["cod"]),
                            nombre = Convert.ToString(reader["nombre"])
                            especificaciones = Convert.ToString(reader["especificaciones"])
                            pvp = Convert.ToString(reader["pvp"])
                            imagen = Convert.ToString(reader["imagen"])
                            categoria = Convert.ToString(reader["categoria"])
                            
                        });
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(articulos);
    }
    //get datos articulo
    [WebMethod]
    public void GetArticulo(int auxid) //to do
    {
        //devolver un articulo
    }
    //añadir un articulo
    [WebMethod]
    public void AddArticulo()//to do
    {
        //se nos pasa los datos de un producto y lo añadimos a la base de datos
    }

    //get usuario para el login
    [WebMethod]
    public string GetUsuario(string password, string auxemail)
    {
        var usuario;
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM usuario where password like "+ password +"and email like " + auxemail, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario = new Usuario{

                            email = auxemail
                            password = password
                            nombre = Convert.ToString(reader["nombre"])
                            apellidos = Convert.ToString(reader["apellidos"])
                            dni = Convert.ToString(reader["pvp"])
                            telefono = Convert.ToInt32(reader["telefono"])
                            calle = Convert.ToString(reader["calle"])
                            codpos = Convert.ToInt32(reader["codpos"])
                            fechanac =Convert.ToDate(reader["fechanac"])

                        }
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(usuario);
    }


    //get pedidos todos 
    [WebMethod]
    public string GetPedidos()
    {
        var pedidos = new List<Pedido>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM Pedido", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pedidos.Add(new Pedido
                        {
                            numped = Convert.ToInt32(reader["numped"]),
                            venta = Convert.ToBoolean(reader["venta"])
                            fecha_pedido = Convert.ToDate(reader["fecha_pedido"])
                            fecha_factura = Convert.ToDate(reader["fecha_factura"])
                        });
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(pedidos);
    }
    //get datos un pedido
    [WebMethod]
    public string GetDatosPedido(int auxpedido)
    {
        var pedido = new List<Linped>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM Linped where pedido ="+ auxpedido, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pedido.Add(new Linped
                        {
                           linea = Convert.ToInt32(reader["linea"]),
                           cantidad = Convert.ToInt32(reader["cantidad"])
                           codArt = Convert.ToInt32(reader["codArt"])
                           pedido = auxpedido
                        });
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(pedidos);
    }
    //añadir un pedido
    [WebMethod]
    public void AddPedido()//to do
    {
        // se nos pasa los datos de un pedido y lo añadimos a pedido y a linped
    }


    //generar factura
    [WebMethod]
    public void GenerarFactura(int auxpedido)//to do
    {
        //buscar el pedido y modificar su fecha de factura
    }

    //get movimientos
    [WebMethod]
    public string GetMovimientos()//to do
    {
        /*
        var movimientos = new List<>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM Pedido", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pedidos.Add(new Pedido
                        {
                            numped = Convert.ToInt32(reader["numped"]),
                            venta = Convert.ToBoolean(reader["venta"])
                            fecha_pedido = Convert.ToDate(reader["fecha_pedido"])
                            fecha_factura = Convert.ToDate(reader["fecha_factura"])
                        });
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(pedidos);
        */
    }
    //get datos de un movimiento
    [WebMethod]
    public string GetDatosMovimiento(int movimiento)//to do
    {
        //buscar un movimiento y devolver todos los articulos que se han pasado de un almacen a otro
    }
    //update movimiento
    [WebMethod]
    public void UpdateMovimiento()//to do
    {
        //añadir movimiento y modificar datos de almacen_articulo
    }


}
