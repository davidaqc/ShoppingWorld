using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Administradores.Services;

[Route("api/[controller]/[action]")]
[ApiController]
public class AdministradoresController : ControllerBase
{
    private readonly AdministradorService _administradorService;
    private readonly IConfiguration _configuration;

    public AdministradoresController(AdministradorService administradorService, IConfiguration configuration)
    {
        _administradorService = administradorService;
        _configuration = configuration;
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
                // Crear el token JWT
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, loginRequest.Correo),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "Administrator")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: creds);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token)
                });
            }
            else
            {
                return Unauthorized(new { message = "Invalid credentials." });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error occurred.");
        }
    }
}
