using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Productos.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ProductoService _productoService;

        public ProductosController(ProductoService productoService)
        {
            _productoService = productoService;
        }

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

        [HttpPost]
        public async Task<IActionResult> AddProducto([FromBody] Producto nuevoProducto)
        {
            if (nuevoProducto == null)
            {
                return BadRequest(new { message = "El producto no puede estar vacío" });
            }

            try
            {
                bool result = await _productoService.AddProductoAsync(nuevoProducto);
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
                return StatusCode(500, new { message = $"Error interno del servidor: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                bool result = await _productoService.DeleteProductoAsync(id);
                if (result)
                {
                    return Ok(new { message = $"Producto con ID {id} eliminado correctamente." });
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromBody] Producto productoActualizado)
        {
            if (productoActualizado == null)
            {
                return BadRequest(new { message = "El producto no puede estar vacío" });
            }

            try
            {
                bool result = await _productoService.UpdateProductoAsync(id, productoActualizado);
                if (result)
                {
                    return Ok(new { message = $"Producto con ID {id} actualizado correctamente." });
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
