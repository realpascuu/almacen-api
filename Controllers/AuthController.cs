using almacenAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace almacenAPI.Controllers;

[ApiController]
[Route("api/auth/login")]
public class LoginController : ControllerBase
{
    private readonly ApplicationDbContext context;
    
    public LoginController(ApplicationDbContext _context)
    {
        this.context = _context;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ObjectResult PostLogin(LoginModel info) 
    {
        try {
            var user = context.Usuario.Where(b => b.email == info.email).FirstOrDefault();
            if (user == null) {
                return StatusCode(StatusCodes.Status400BadRequest, $"User with email {info.email} not found");
            }

            var hash_password = user.generatePasswordHash(info.password);
            if(hash_password != user.password) {
                return StatusCode(StatusCodes.Status400BadRequest, "Password not valid");
            }
            return StatusCode(StatusCodes.Status200OK, "Usuario logueado");
        } catch(Exception) {
            return StatusCode(StatusCodes.Status500InternalServerError, "Error updating data");
        }
    }
}