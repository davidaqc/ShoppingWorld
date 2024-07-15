using Npgsql;

namespace Productos.DataAccess
{
    public class ProductoRepository
    {
        private readonly string _connectionString;

        public ProductoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<object>> GetProductosAsync()
        {
            List<object> productos = new List<object>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = @"
                SELECT p.id_producto, p.nombre_producto, p.descripcion, p.detalles, p.precio, p.stock, p.image_url, c.nombre_categoria, c.id_categoria
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
                                image_url = reader.IsDBNull(6) ? null : reader.GetString(6),
                                nombre_categoria = reader.GetString(7),
                                id_categoria = reader.GetInt32(8)
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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = @"
                    INSERT INTO Productos (nombre_producto, descripcion, detalles, precio, stock, id_categoria, image_url)
                    VALUES (@nombre_producto, @descripcion, @detalles, @precio, @stock, @id_categoria, @image_url)";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("nombre_producto", nuevoProducto.NombreProducto);
                    command.Parameters.AddWithValue("descripcion", nuevoProducto.Descripcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("detalles", nuevoProducto.Detalles ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("precio", nuevoProducto.Precio);
                    command.Parameters.AddWithValue("stock", nuevoProducto.Stock);
                    command.Parameters.AddWithValue("id_categoria", nuevoProducto.IdCategoria);
                    command.Parameters.AddWithValue("image_url", nuevoProducto.ImageUrl);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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

        public async Task<bool> UpdateProductoAsync(Producto updatedProducto)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = @"
            UPDATE Productos
            SET nombre_producto = @nombre_producto,
                descripcion = @descripcion,
                detalles = @detalles,
                precio = @precio,
                stock = @stock,
                id_categoria = @id_categoria,
                image_url = @image_url
            WHERE id_producto = @id_producto";

                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("nombre_producto", updatedProducto.NombreProducto);
                    command.Parameters.AddWithValue("descripcion", updatedProducto.Descripcion ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("detalles", updatedProducto.Detalles ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("precio", updatedProducto.Precio);
                    command.Parameters.AddWithValue("stock", updatedProducto.Stock);
                    command.Parameters.AddWithValue("id_categoria", updatedProducto.IdCategoria);
                    command.Parameters.AddWithValue("image_url", updatedProducto.ImageUrl);
                    command.Parameters.AddWithValue("id_producto", updatedProducto.IdProducto);

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<int> GetProductosCountAsync()
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT COUNT(*) FROM Productos";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

        public async Task<string> GetProductoImageUrlAsync(int id)
        {
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string sql = "SELECT image_url FROM Productos WHERE id_producto = @id";
                using (NpgsqlCommand command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("id", id);
                    var result = await command.ExecuteScalarAsync();

                    return result != null ? result.ToString() : null;
                }
            }
        }

    }
}
