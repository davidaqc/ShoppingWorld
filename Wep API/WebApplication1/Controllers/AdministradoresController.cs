using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Administradores.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdministradoresController : ControllerBase
    {
        private readonly AdministradorService _administradorService;

        public AdministradoresController(AdministradorService administradorService)
        {
            _administradorService = administradorService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Administrador loginRequest)
        {
            if (loginRequest == null || string.IsNullOrEmpty(loginRequest.Correo) || string.IsNullOrEmpty(loginRequest.Admin_Password))
            {
                return BadRequest(new { message = "Correo y contraseña son requeridos." });
            }

            try
            {
                bool isValid = await _administradorService.ValidateAdministradorAsync(loginRequest.Correo, loginRequest.Admin_Password);

                if (isValid)
                {
                    return Ok(new { message = "Login successful" });
                }
                else
                {
                    return Unauthorized(new { message = "Invalid credentials" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
