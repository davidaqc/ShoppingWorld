using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Categorias.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly CategoriaService _categoriaService;

        public CategoriasController(CategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategorias()
        {
            try
            {
                List<object> categorias = await _categoriaService.GetCategoriasAsync();
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpDelete("{id_categoria}")]
        public async Task<IActionResult> DeleteCategoria(int id_categoria)
        {
            try
            {
                bool eliminacionExitosa = await _categoriaService.DeleteCategoriaAsync(id_categoria);
                if (eliminacionExitosa)
                {
                    return Ok(new { message = "Categoría y productos asociados eliminados exitosamente" });
                }
                else
                {
                    return NotFound(new { message = "Categoría no encontrada" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategoria([FromBody] Categoria nuevaCategoria)
        {
            try
            {
                bool creacionExitosa = await _categoriaService.CreateCategoriaAsync(nuevaCategoria);
                if (creacionExitosa)
                {
                    return Ok(new { message = "Categoría creada exitosamente" });
                }
                else
                {
                    return BadRequest(new { message = "Error al crear la categoría" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPut("{id_categoria}")]
        public async Task<IActionResult> UpdateCategoria(int id_categoria, [FromBody] Categoria categoriaActualizada)
        {
            try
            {
                categoriaActualizada.IdCategoria = id_categoria;
                bool actualizacionExitosa = await _categoriaService.UpdateCategoriaAsync(categoriaActualizada);
                if (actualizacionExitosa)
                {
                    return Ok(new { message = "Categoría actualizada exitosamente" });
                }
                else
                {
                    return NotFound(new { message = "Categoría no encontrada" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoriasCount()
        {
            try
            {
                int count = await _categoriaService.GetCategoriasCountAsync();
                return Ok(count);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
