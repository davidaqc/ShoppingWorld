using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class UsuarioService
{
    private readonly IConfiguration _configuration;

    public UsuarioService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> LoginAsync(string correo, string userPassword)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Usuarios WHERE correo = @correo AND user_password = @user_password";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@user_password", userPassword);

                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        return reader.Read();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public async Task<bool> RegistrarAsync(Usuario nuevoUsuario)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Usuarios (correo, user_password, nombre, apellidos, numero_identificacion) VALUES (@correo, @user_password, @nombre, @apellidos, @numero_identificacion)";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@correo", nuevoUsuario.Correo);
                    command.Parameters.AddWithValue("@user_password", nuevoUsuario.User_Password);
                    command.Parameters.AddWithValue("@nombre", nuevoUsuario.Nombre);
                    command.Parameters.AddWithValue("@apellidos", nuevoUsuario.Apellidos);
                    command.Parameters.AddWithValue("@numero_identificacion", nuevoUsuario.NumeroIdentificacion);

                    int result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public async Task<List<Usuario>> GetUsuariosAsync()
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                string query = "SELECT correo, user_password, nombre, apellidos, numero_identificacion FROM Usuarios";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        List<Usuario> usuarios = new List<Usuario>();
                        while (await reader.ReadAsync())
                        {
                            Usuario usuario = new Usuario
                            {
                                Correo = reader.GetString(0),
                                User_Password = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellidos = reader.GetString(3),
                                NumeroIdentificacion = reader.GetInt32(4)
                            };
                            usuarios.Add(usuario);
                        }
                        return usuarios;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public async Task<bool> DeleteUsuarioAsync(string correo)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Usuarios WHERE correo = @correo";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    int result = await command.ExecuteNonQueryAsync();
                    return result > 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public async Task<List<Usuario>> GetUsuariosByNombreAsync(string nombre)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            try
            {
                await connection.OpenAsync();
                string query = "SELECT correo, user_password, nombre, apellidos, numero_identificacion FROM Usuarios WHERE nombre LIKE @nombre";
                using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", $"%{nombre}%");

                    using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        List<Usuario> usuarios = new List<Usuario>();
                        while (await reader.ReadAsync())
                        {
                            Usuario usuario = new Usuario
                            {
                                Correo = reader.GetString(0),
                                User_Password = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Apellidos = reader.GetString(3),
                                NumeroIdentificacion = reader.GetInt32(4)
                            };
                            usuarios.Add(usuario);
                        }
                        return usuarios;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public async Task<int> GetUsuariosCountAsync()
    {
        int count = 0;
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string sql = "SELECT COUNT(*) FROM Usuarios";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                count = Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }

        return count;
    }

}
