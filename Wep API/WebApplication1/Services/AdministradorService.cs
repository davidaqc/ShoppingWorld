using Npgsql;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class AdministradorService
{
    private readonly IConfiguration _configuration;

    public AdministradorService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> ValidateAdministradorAsync(string correo, string password)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string sql = "SELECT COUNT(1) FROM Administradores WHERE correo = @correo AND admin_password = @admin_password";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("correo", correo);
                command.Parameters.AddWithValue("admin_password", password);

                int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                return count > 0;
            }
        }
    }
}
