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

public class Movimiento
{
    public int id { get; set; }
    public int almacen_salida{ get; set; }
    public int almacen_entrada{ get; set; }
    
}


public class ArticuloMovimiento
{
    public int id { get; set; }
    public string articulo{ get; set; }
    public int cantidad { get; set; }
    public int idmovimiento { get; set; }
}

[WebService(Namespace = "http://tempuri.org/")] 
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
[System.Web.Script.Services.ScriptService]
public class Service : System.Web.Services.WebService
{

    //add X numero del articulo a almacen

    [WebMethod]
    public string AddArticuloAlmacen(string almacen, string articulo, int cantidad )
    {
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("update Almacen_Articulo set (cantidad) = ( @cantidad) where almacen like @almacen and articulo like @articulo", connection))
            {
                command.Parameters.AddWithValue("@almacen", almacen);
                command.Parameters.AddWithValue("@articulo", articulo);
                command.Parameters.AddWithValue("@cantidad", cantidad);
                command.ExecuteNonQuery();
            }

            
        }

    }


    //get articulos de un almacen
    [WebMethod]
    public string GetArticulosAlmacen(int almacen)
    {
         var articulos = new List<Almacen_Articulo>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT Articulo.nombre, Almacen.nombre, cantidad FROM Almacen_Articulo, Almacen, Articulo where @almacen = Almacen.id and Almacen_Articulo.almacen = Almacen.nombre and Almacen_articulo.articulo = Articulo.nombre and cantidad != 0 ", connection))
            {
                 command.Parameters.AddWithValue("@almacen", almacen);
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
                           
                            pvp = Convert.ToString(reader["pvp"])
                            imagen = Convert.ToString(reader["imagen"])
                            
                            
                        });
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(articulos);
    }
    //get datos articulo
    [WebMethod]
    public void GetDatosArticulo(int auxid, string nombre) 
    {
         var articulo;
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            var selec ;
            if(nombre != null)
            {
                selec ="SELECT * FROM articulo where nombre like @nombre "
            }else
            {
                selec ="SELECT * FROM articulo where cod like @cod "
            }

            using (var command = new SQLiteCommand( selec, connection))
            {   
                if(nombre!=null)
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                }
                else{
                     command.Parameters.AddWithValue("@cod", auxid);
                }
               
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        articulo = new articulo{

                            cod = auxid
                            nombre = Convert.ToString(reader["nombre"])
                            especificaciones = Convert.ToString(reader["especificaciones"])
                            pvp = Convert.ToString(reader["pvp"])
                            imagen = Convert.ToString(reader["imagen"])
                            categoria = Convert.ToString(reader["categoria"])

                        }
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(articulo);
    }

    //get datos articulo nombre
    [WebMethod]
    public void GetDatosArticuloNombre(string nombre, int offset, int limit) 
    {
         var articulo;
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM articulo where nombre like %@nombre%" , connection))
            {

                 command.Parameters.AddWithValue("@nombre", nombre);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        articulo = new articulo{

                            cod = auxid
                            nombre = Convert.ToString(reader["nombre"])
                            especificaciones = Convert.ToString(reader["especificaciones"])
                            pvp = Convert.ToString(reader["pvp"])
                            imagen = Convert.ToString(reader["imagen"])
                            categoria = Convert.ToString(reader["categoria"])

                        }
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(articulo.SetFirstResult(offset).SetMaxResults(limit));
    }


    //añadir un articulo
    [WebMethod]
    public void AddArticulo(string nombre, string especificaciones, float pvp, string imagen, string categoria)
    {
        //se nos pasa los datos de un producto y lo añadimos a la base de datos
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("INSERT INTO articulo (nombre,especificaciones,pvp,imagen,categoria) VALUES (@nombre , @especificaciones, @pvp,@imagen, @categoria)", connection))
            {
                command.Parameters.AddWithValue("@nombre", nombre);
                command.Parameters.AddWithValue("@especificaciones", especificaciones);
                command.Parameters.AddWithValue("@pvp", pvp);
                command.Parameters.AddWithValue("@imagen", imagen);
                command.Parameters.AddWithValue("@categoria", categoria);
                command.ExecuteNonQuery();
            }
            
            //añadir a los almacenes con cantidad 0
            using (var command = new SQLiteCommand("SELECT nombre FROM Almacen", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                            using (var command = new SQLiteCommand("INSERT INTO Almacen_Articulo (almacen,articulo,cantidad) VALUES (@almacen , @articulo, @cantidad)", connection))
                                {
                                    command.Parameters.AddWithValue("@almacen", Convert.ToInt32(reader["nombre"]));
                                    command.Parameters.AddWithValue("@articulo", nombre);
                                    command.Parameters.AddWithValue("@pvp",0);
                                    
                                    command.ExecuteNonQuery();
                                }
                                                 
                    }
                }
            }
        }
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
    public string GetPedidos(date auxfecha_pedido, bool orden)//fecha nula
    {
        var pedidos = new List<Pedido>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            var selec;
            if(auxfecha_pedido == null)
            {
                selec = "SELECT * FROM Pedido where fecha_factura is null order by fecha_pedido @orden";
            }else
            {
                selec = "SELECT * FROM Pedido where fecha_factura is null and fecha_pedido like @fecha order by fecha_pedido @orden"
            }
            using (var command = new SQLiteCommand(selec, connection))
            {

                if(auxfecha_pedido == null)
                {
                    
                    {}
                }

                if (orden)
                {
                    command.Parameters.AddWithValue("@orden", "ASC");
                }
                else
                {
                    command.Parameters.AddWithValue("@orden", "DESC");
                }
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pedidos.Add(new Pedido
                        {
                            numped = Convert.ToInt32(reader["numped"]),
                            venta = Convert.ToBoolean(reader["venta"])
                            fecha_pedido = Convert.ToDate(reader["fecha_pedido"])
                            fecha_factura = null
                        });
                    }
                }
            }
        }
        return JsonConvert.SerializeObject(pedidos);
    }

    //get factura
    public string GetFactura(date auxfecha_pedido, bool orden)//fecha no nula
    {
        var pedidos = new List<Pedido>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
          var selec;
            if(auxfecha_pedido == null)
            {
                selec = "SELECT * FROM Pedido where fecha_factura is not null order by fecha_pedido @orden";
            }else
            {
                selec = "SELECT * FROM Pedido where fecha_factura is not null and fecha_pedido like @fecha order by fecha_pedido @orden"
            }
            using (var command = new SQLiteCommand(selec, connection))
            {
                
                if(auxfecha_pedido == null)
                {
                     command.Parameters.AddWithValue("@fecha", auxfecha_pedido);
                }

                if (orden)
                {
                    command.Parameters.AddWithValue("@orden", "ASC");
                }
                else
                {
                    command.Parameters.AddWithValue("@orden", "DESC");
                }
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
    public void AddPedido(bool auxventa, date auxfecha_pedido, List<Linped> lineas  )//to do
    {
        // se nos pasa los datos de un pedido y lo añadimos a pedido y a linped

        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            var id
            using (var command = new SQLiteCommand("insert into Pedido (venta, fecha_pedido, fecha_factura) values (@venta, @fecha_pedido, null); SELECT last_insert_rowid()", connection))
            {
                command.Parameters.AddWithValue("@venta", venta);
                command.Parameters.AddWithValue("@fecha_pedido", auxfecha_pedido);
                id = command.ExecuteScalar();
            }

            using(var command = new SQLiteCommand("insert into LinPed (cantidad, codart, pedido) values (@cantidad, @codArt, @pedido)", connection))
            {
                foreach (var linea in lineas)
                {
                    command.Parameters.AddWithValue("@codart", linea.codArt);
                    command.Parameters.AddWithValue("@cantidad", linea.cantidad);
                    command.Parameters.AddWithValue("@pedido", linea.pedido);
                    command.ExecuteNonQuery();
                }
                //to do
            }

        }
    }


    //generar factura
    [WebMethod]
    public void GenerarFactura(int auxpedido)
    {
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("UPDATE pedido SET fecha_factura = @fecha WHERE id = @id", connection))
            {
                command.Parameters.AddWithValue("@fecha", DateTime.Now );
                command.Parameters.AddWithValue("@id", auxpedido);
                command.ExecuteNonQuery();
            }
        }

    }

    //get movimientos
    [WebMethod]
    public string GetMovimientos()
    {
        
        var movimientos = new List<Movimiento>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM Movimiento", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movimientos.Add(new Movimiento
                        {
                            id = Convert.ToInt32(reader["id"])
                            almacen_salida = Convert.ToInt32(reader["almacen_salida"]),
                            almacen_entrada = Convert.ToInt32(reader["almacen_entrada"])
                        });
                    }
                }
            }

        }
        return JsonConvert.SerializeObject(movimientos);
        
    }
    //get datos de un movimiento
    [WebMethod]
    public string GetDatosMovimiento(int movimiento)//to do
    {
        //buscar un movimiento y devolver todos los articulos que se han pasado de un almacen a otro
        var movimientos = new List<Movimiento>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM ArticuloMovimiento where movimiento = @movimiento", connection))
            {

                command.Parameters.AddWithValue("@movimiento", movimiento);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        movimientos.Add(new Movimiento
                        {
                            id = Convert.ToInt32(reader["id"]),
                            articulo = Convert.ToString(reader["articulo"])
                            cantidad = Convert.ToInt32(reader["cantidad"])
                            idmovimiento = movimiento
                        });
                    }
                }
            }
            
        }
        return JsonConvert.SerializeObject(movimientos);
    }
    //update movimiento
    [WebMethod]
    public void AddMovimiento(int almacen_entrada, int almacen_salida,List<ArticuloMovimiento> articulos )//to do
    {
        //añadir movimiento y modificar datos de almacen_articulo

        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            var id;
            using (var command = new SQLiteCommand("insert into Movimiento (almacen_entrada, almacen_salida) values (@almacen_entrada, @almacen_salida);  SELECT last_insert_rowid()", connection))
            {
                command.Parameters.AddWithValue("@venta", venta);
                command.Parameters.AddWithValue("@fecha_pedido", auxfecha_pedido);
                id = command.ExecuteScalar();
            }

            using(var command = new SQLiteCommand("insert into ArticuloMovimiento (articulo, cantidad, idmovimiento) values (@articulo, @cantidad,@idmovimiento)", connection))
            {
                
                 foreach (var articulo in articulos)
                {
         
                    command.Parameters.AddWithValue("@articulo", articulo.articulo);
                    command.Parameters.AddWithValue("@cantidad", articulo.cantidad);
                    command.Parameters.AddWithValue("@idmovimiento", articulo.id);
                    command.ExecuteNonQuery();
                }
            }

        }


    }


    //get almacenes
    [WebMethod]
    public string GetAlmacenes()
    {
        
        var almacenes = new List<Almacen>();
        using (var connection = new SQLiteConnection("Data Source=sqlite.db"))
        {
            connection.Open();
            using (var command = new SQLiteCommand("SELECT * FROM Almacen", connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        almacenes.Add(new Almacen
                        {
                            id = Convert.ToInt32(reader["id"])
                            nombre = Convert.ToString(reader["nombre"]),
                           
                        });
                    }
                }
            }

        }
        return JsonConvert.SerializeObject(almacenes);
        
    }
   
    //get articulos por nombre

}
