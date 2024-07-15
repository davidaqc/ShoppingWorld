using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Categorias.DataAccess
{
    public class CategoriaRepository
    {
        private readonly string _connectionString;
        private readonly S3Uploader _s3Uploader;

        public CategoriaRepository(IConfiguration configuration, S3Uploader s3Uploader)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _s3Uploader = s3Uploader;
        }

        public async Task<List<object>> GetCategoriasAsync()
        {
            List<object> categorias = new List<object>();

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Recuperar URLs de las imágenes de los productos asociados
                        List<string> imageUrls = new List<string>();
                        string selectImagesSql = "SELECT image_url FROM Productos WHERE id_categoria = @id_categoria";
                        using (NpgsqlCommand selectImagesCommand = new NpgsqlCommand(selectImagesSql, connection))
                        {
                            selectImagesCommand.Parameters.AddWithValue("@id_categoria", id_categoria);
                            using (var reader = await selectImagesCommand.ExecuteReaderAsync())
                            {
                                while (await reader.ReadAsync())
                                {
                                    var imageUrl = reader["image_url"] as string;
                                    if (!string.IsNullOrEmpty(imageUrl))
                                    {
                                        imageUrls.Add(imageUrl);
                                    }
                                }
                            }
                        }

                        // Eliminar productos asociados
                        string deleteProductosSql = "DELETE FROM Productos WHERE id_categoria = @id_categoria";
                        using (NpgsqlCommand deleteProductosCommand = new NpgsqlCommand(deleteProductosSql, connection))
                        {
                            deleteProductosCommand.Parameters.AddWithValue("@id_categoria", id_categoria);
                            await deleteProductosCommand.ExecuteNonQueryAsync();
                        }

                        // Eliminar categoría
                        string deleteCategoriaSql = "DELETE FROM Categorias WHERE id_categoria = @id_categoria";
                        using (NpgsqlCommand deleteCategoriaCommand = new NpgsqlCommand(deleteCategoriaSql, connection))
                        {
                            deleteCategoriaCommand.Parameters.AddWithValue("@id_categoria", id_categoria);
                            int result = await deleteCategoriaCommand.ExecuteNonQueryAsync();
                            await transaction.CommitAsync();

                            // Eliminar imágenes de S3
                            foreach (var imageUrl in imageUrls)
                            {
                                string fileName = imageUrl.Substring(imageUrl.LastIndexOf("/") + 1);
                                await _s3Uploader.DeleteFileAsync(fileName);
                            }

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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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
            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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

            using (NpgsqlConnection connection = new NpgsqlConnection(_connectionString))
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
}
