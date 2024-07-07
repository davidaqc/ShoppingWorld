using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

public class ProductoService
{
    private readonly IConfiguration _configuration;

    public ProductoService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<object>> GetProductosAsync()
    {
        List<object> productos = new List<object>();
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string sql = @"
                SELECT p.id_producto, p.nombre_producto, p.descripcion, p.detalles, p.precio, p.stock, c.nombre_categoria
                FROM Productos p
                INNER JOIN Categorias c ON p.id_categoria = c.id_categoria
                ORDER BY p.id_producto";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                using (NpgsqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var producto = new
                        {
                            id_producto = reader.GetInt32(0),
                            nombre_producto = reader.GetString(1),
                            descripcion = reader.IsDBNull(2) ? null : reader.GetString(2),
                            detalles = reader.IsDBNull(3) ? null : reader.GetString(3),
                            precio = reader.GetDecimal(4),
                            stock = reader.GetInt32(5),
                            nombre_categoria = reader.GetString(6)
                        };

                        productos.Add(producto);
                    }
                }
            }
        }

        return productos;
    }

    public async Task<bool> AddProductoAsync(Producto nuevoProducto)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string sql = @"
                INSERT INTO Productos (nombre_producto, descripcion, detalles, precio, stock, id_categoria)
                VALUES (@nombre_producto, @descripcion, @detalles, @precio, @stock, @id_categoria)";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("nombre_producto", nuevoProducto.NombreProducto);
                command.Parameters.AddWithValue("descripcion", nuevoProducto.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("detalles", nuevoProducto.Detalles ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("precio", nuevoProducto.Precio);
                command.Parameters.AddWithValue("stock", nuevoProducto.Stock);
                command.Parameters.AddWithValue("id_categoria", nuevoProducto.IdCategoria);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> DeleteProductoAsync(int id)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string checkSql = "SELECT COUNT(1) FROM Productos WHERE id_producto = @id";
            using (NpgsqlCommand checkCommand = new NpgsqlCommand(checkSql, connection))
            {
                checkCommand.Parameters.AddWithValue("id", id);
                int count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                if (count == 0)
                {
                    return false; // Producto no encontrado
                }
            }

            string sql = "DELETE FROM Productos WHERE id_producto = @id";
            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("id", id);
                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }
    }

    public async Task<bool> UpdateProductoAsync(int id, Producto productoActualizado)
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string checkSql = "SELECT COUNT(1) FROM Productos WHERE id_producto = @id";
            using (NpgsqlCommand checkCommand = new NpgsqlCommand(checkSql, connection))
            {
                checkCommand.Parameters.AddWithValue("id", id);
                int count = Convert.ToInt32(await checkCommand.ExecuteScalarAsync());

                if (count == 0)
                {
                    return false; // Producto no encontrado
                }
            }

            string sql = @"
                UPDATE Productos
                SET nombre_producto = @nombre_producto,
                    descripcion = @descripcion,
                    detalles = @detalles,
                    precio = @precio,
                    stock = @stock,
                    id_categoria = @id_categoria
                WHERE id_producto = @id";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("nombre_producto", productoActualizado.NombreProducto);
                command.Parameters.AddWithValue("descripcion", productoActualizado.Descripcion ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("detalles", productoActualizado.Detalles ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("precio", productoActualizado.Precio);
                command.Parameters.AddWithValue("stock", productoActualizado.Stock);
                command.Parameters.AddWithValue("id_categoria", productoActualizado.IdCategoria);
                command.Parameters.AddWithValue("id", id);

                int rowsAffected = await command.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }
    }

    public async Task<int> GetProductosCountAsync()
    {
        int count = 0;
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
        {
            await connection.OpenAsync();

            string sql = "SELECT COUNT(*) FROM Productos";

            using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
            {
                count = Convert.ToInt32(await command.ExecuteScalarAsync());
            }
        }

        return count;
    }
}
