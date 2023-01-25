using almacenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SQLite;
using System.Data;
using System.Text.Json;
using System.Net;
using Newtonsoft.Json;

namespace almacenAPI.Controllers;
[ApiController]
[Route("api/articulos")]
public class ArticulosController : ControllerBase
{
    private readonly ApplicationDbContext context;
    
    public ArticulosController(ApplicationDbContext _context)
    {
        this.context = _context;
    }

    [HttpGet]
    public ActionResult Get()
    {
        var articulos = new List<Articulo>();
        using (SQLiteConnection connection = new SQLiteConnection("Data Source= db\\Almacen.db"))
        {
            connection.Open();
           
            using (var command = new SQLiteCommand("SELECT * FROM articulo", connection))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        articulos.Add(
                            new Articulo{
                            cod = Convert.ToInt32(reader["cod"]),
                            nombre = Convert.ToString(reader["nombre"]),
                            pvp = Convert.ToInt32(reader["pvp"]),
                            imagen = Convert.ToString(reader["imagen"]),
                            categoria = Convert.ToString(reader["categoria"])
                        });
                    }
                }
            }
            //Create my object
            var myData = new
            {
                Host = @"sftp.myhost.gr",
                UserName = "my_username",
                Password = "my_password",
                SourceDir = "/export/zip/mypath/",
                articulos = articulos
            };
            string jsonData = JsonConvert.SerializeObject(myData);
            return Ok(jsonData);
        }
    }
}
