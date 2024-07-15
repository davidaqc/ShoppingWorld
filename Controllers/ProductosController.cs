using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Productos.Services;

namespace Productos.Controllers
{

    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;
        private readonly S3Uploader _s3Uploader;

        public ProductosController(ProductoService productoService, S3Uploader s3Uploader)
        {
            _productoService = productoService;
            _s3Uploader = s3Uploader;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            try
            {
                List<object> productos = await _productoService.GetProductosAsync();
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> AddProducto([FromForm] Producto producto, IFormFile file)
        {
            if (producto == null)
            {
                return BadRequest(new { message = "El producto no puede estar vacío" });
            }

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "Debe proporcionar un archivo de imagen para el producto" });
            }

            try
            {
                // Generar un nombre de archivo único
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(Path.GetTempPath(), uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Subir el archivo a S3
                var fileUrl = await _s3Uploader.UploadFileAsync(filePath, uniqueFileName);

                // Actualizar el objeto producto con la URL de la imagen
                producto.ImageUrl = fileUrl;

                // Agregar el producto a la base de datos
                bool result = await _productoService.AddProductoAsync(producto);
                if (result)
                {
                    return Ok(new { message = "Producto agregado correctamente" });
                }
                else
                {
                    return StatusCode(500, new { message = "Error al agregar el producto" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error occurred.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                // Obtener la URL de la imagen antes de eliminar el producto
                string imageUrl = await _productoService.GetProductoImageUrlAsync(id);

                if (string.IsNullOrEmpty(imageUrl))
                {
                    return NotFound(new { message = $"Producto con ID {id} no encontrado." });
                }

                // Eliminar el producto de la base de datos
                bool result = await _productoService.DeleteProductoAsync(id);

                if (result)
                {
                    // Eliminar la imagen de S3 si el producto se eliminó correctamente de la base de datos
                    var uri = new Uri(imageUrl);
                    string fileName = Path.GetFileName(uri.LocalPath);

                    bool s3Result = await _s3Uploader.DeleteFileAsync(fileName);

                    if (s3Result)
                    {
                        return Ok(new { message = $"Producto con ID {id} e imagen asociada eliminados correctamente." });
                    }
                    else
                    {
                        return Ok(new { message = $"Producto con ID {id} eliminado, pero hubo un problema al eliminar la imagen de S3." });
                    }
                }
                else
                {
                    return NotFound(new { message = $"Producto con ID {id} no encontrado." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut]
        public async Task<IActionResult> UpdateProducto([FromForm] Producto producto, IFormFile file)
        {
            if (producto == null)
            {
                return BadRequest(new { message = "Datos del producto no válidos" });
            }

            try
            {
                string fileName;

                if (file != null && file.Length > 0)
                {
                    if (string.IsNullOrEmpty(producto.ImageUrl))
                    {
                        // Generar un nombre de archivo único
                        var fileExtension = Path.GetExtension(file.FileName);
                        fileName = $"{Guid.NewGuid()}{fileExtension}";
                    }
                    else
                    {
                        // Extraer el nombre del archivo existente de la URL
                        var uri = new Uri(producto.ImageUrl);
                        fileName = Path.GetFileName(uri.LocalPath);
                    }

                    var filePath = Path.Combine(Path.GetTempPath(), fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var fileUrl = await _s3Uploader.UploadFileAsync(filePath, fileName);

                    // Actualizar el objeto producto con la nueva URL de la imagen
                    producto.ImageUrl = fileUrl;
                }

                // Actualizar el producto en la base de datos
                bool result = await _productoService.UpdateProductoAsync(producto);
                if (result)
                {
                    return Ok(new { message = "Producto actualizado correctamente" });
                }
                else
                {
                    return StatusCode(500, new { message = "Error al actualizar el producto" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error interno.");
            }
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetProductosCount()
        {
            try
            {
                int count = await _productoService.GetProductosCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
