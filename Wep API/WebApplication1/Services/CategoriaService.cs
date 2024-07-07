using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class CategoriaService
{
    private readonly IConfiguration _configuration;

    public CategoriaService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<object>> GetCategoriasAsync()
    {
        List<object> categorias = new List<object>();
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string sql = "SELECT id_categoria, nombre_categoria FROM Categorias ORDER BY id_categoria";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var categoria = new
                        {
                            IdCategoria = reader.GetInt32(0),
                            NombreCategoria = reader.GetString(1)
                        };

                        categorias.Add(categoria);
                    }
                }
            }
        }

        return categorias;
    }

    public async Task<bool> DeleteCategoriaAsync(int id_categoria)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    // Eliminar productos asociados
                    string deleteProductosSql = "DELETE FROM Productos WHERE id_categoria = @id_categoria";
                    using (NpgsqlCommand deleteProductosCommand = new NpgsqlCommand(deleteProductosSql, connection))
                    {
                        deleteProductosCommand.Parameters.AddWithValue("@id_categoria", id_categoria);
                        await deleteProductosCommand.ExecuteNonQueryAsync();
                    }

                    string deleteCategoriaSql = "DELETE FROM Categorias WHERE id_categoria = @id_categoria";
                    using (NpgsqlCommand deleteCategoriaCommand = new NpgsqlCommand(deleteCategoriaSql, connection))
                    {
                        deleteCategoriaCommand.Parameters.AddWithValue("@id_categoria", id_categoria);
                        int result = await deleteCategoriaCommand.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();
                        return result > 0;
                    }
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }

    public async Task<bool> CreateCategoriaAsync(Categoria nuevaCategoria)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string insertCategoriaSql = "INSERT INTO Categorias (nombre_categoria) VALUES (@nombre_categoria)";
            using (NpgsqlCommand insertCategoriaCommand = new NpgsqlCommand(insertCategoriaSql, connection))
            {
                insertCategoriaCommand.Parameters.AddWithValue("@nombre_categoria", nuevaCategoria.NombreCategoria);
                int result = await insertCategoriaCommand.ExecuteNonQueryAsync();
                return result > 0;
            }
        }
    }

    public async Task<bool> UpdateCategoriaAsync(Categoria categoriaActualizada)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string updateCategoriaSql = "UPDATE Categorias SET nombre_categoria = @nombre_categoria WHERE id_categoria = @id_categoria";
            using (NpgsqlCommand updateCategoriaCommand = new NpgsqlCommand(updateCategoriaSql, connection))
            {
                updateCategoriaCommand.Parameters.AddWithValue("@id_categoria", categoriaActualizada.IdCategoria);
                updateCategoriaCommand.Parameters.AddWithValue("@nombre_categoria", categoriaActualizada.NombreCategoria);
                int result = await updateCategoriaCommand.ExecuteNonQueryAsync();
                return result > 0;
            }
        }
    }

    public async Task<int> GetCategoriasCountAsync()
    {
        int count = 0;
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string sql = "SELECT COUNT(*) FROM Categorias";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                count = Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }

        return count;
    }
}
