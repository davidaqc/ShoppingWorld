using Npgsql;

namespace Usuarios.DataAccess
{
    public class UsuarioRepository
    {
        private readonly string _connectionString;

        public UsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> LoginAsync(string correo, string userPassword)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string query = "SELECT user_password FROM Usuarios WHERE correo = @correo";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@correo", correo);
                        var result = await command.ExecuteScalarAsync();

                        if (result == null)
                        {
                            return false; // Usuario no encontrado
                        }

                        string hashedPassword = (string)result;
                        bool passwordMatch = BCrypt.Net.BCrypt.Verify(userPassword, hashedPassword);
                        return passwordMatch;
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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();
                    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.User_Password);
                    string query = "INSERT INTO Usuarios (correo, user_password, nombre, apellidos, numero_identificacion) " +
                                   "VALUES (@correo, @user_password, @nombre, @apellidos, @numero_identificacion)";
                    using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@correo", nuevoUsuario.Correo);
                        command.Parameters.AddWithValue("@user_password", hashedPassword);
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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT COUNT(*) FROM Usuarios";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }
    }
}
