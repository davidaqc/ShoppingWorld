using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Usuarios.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;

        public UsuariosController(UsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Usuario request)
        {
            try
            {
                bool loginSuccessful = await _usuarioService.LoginAsync(request.Correo, request.User_Password);
                if (loginSuccessful)
                {
                    return Ok(true);
                }
                else
                {
                    return BadRequest(new { message = "El correo o la contraseña son incorrectos" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Registrar([FromBody] Usuario nuevoUsuario)
        {
            try
            {
                bool registrationSuccessful = await _usuarioService.RegistrarAsync(nuevoUsuario);
                if (registrationSuccessful)
                {
                    return Ok(new { message = "Usuario registrado exitosamente" });
                }
                else
                {
                    return BadRequest(new { message = "Error al registrar el usuario" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.GetUsuariosAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{correo}")]
        public async Task<IActionResult> DeleteUsuario(string correo)
        {
            try
            {
                bool eliminacionExitosa = await _usuarioService.DeleteUsuarioAsync(correo);
                if (eliminacionExitosa)
                {
                    return Ok(new { message = "Usuario eliminado exitosamente" });
                }
                else
                {
                    return NotFound(new { message = "Usuario no encontrado" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{nombre}")]
        public async Task<IActionResult> GetUsuariosByNombre(string nombre)
        {
            try
            {
                var usuarios = await _usuarioService.GetUsuariosByNombreAsync(nombre);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUsuariosCount()
        {
            try
            {
                int count = await _usuarioService.GetUsuariosCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
