using Npgsql;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Administradores.DataAccess
{
    public class AdministradorRepository
    {
        private readonly string _connectionString;

        public AdministradorRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> ValidateAdministradorAsync(string correo, string password)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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
}
