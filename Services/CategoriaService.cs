using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Categorias.DataAccess;

namespace Categorias.Services
{
    public class CategoriaService
    {
        private readonly CategoriaRepository _categoriaRepository;

        public CategoriaService(CategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<List<object>> GetCategoriasAsync()
        {
            return await _categoriaRepository.GetCategoriasAsync();
        }

        public async Task<bool> DeleteCategoriaAsync(int id_categoria)
        {
            return await _categoriaRepository.DeleteCategoriaAsync(id_categoria);
        }

        public async Task<bool> CreateCategoriaAsync(Categoria nuevaCategoria)
        {
            return await _categoriaRepository.CreateCategoriaAsync(nuevaCategoria);
        }

        public async Task<bool> UpdateCategoriaAsync(Categoria categoriaActualizada)
        {
            return await _categoriaRepository.UpdateCategoriaAsync(categoriaActualizada);
        }

        public async Task<int> GetCategoriasCountAsync()
        {
            return await _categoriaRepository.GetCategoriasCountAsync();
        }
    }
}
