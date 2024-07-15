using Productos.DataAccess;

namespace Productos.Services
{
    public class ProductoService
    {
        private readonly ProductoRepository _productoRepository;

        public ProductoService(ProductoRepository productoRepository)
        {
            _productoRepository = productoRepository;
        }

        public async Task<List<object>> GetProductosAsync()
        {
            return await _productoRepository.GetProductosAsync();
        }

        public async Task<bool> AddProductoAsync(Producto nuevoProducto)
        {
            return await _productoRepository.AddProductoAsync(nuevoProducto);
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            return await _productoRepository.DeleteProductoAsync(id);
        }

        public async Task<bool> UpdateProductoAsync(Producto productoActualizado)
        {
            return await _productoRepository.UpdateProductoAsync(productoActualizado);
        }

        public async Task<int> GetProductosCountAsync()
        {
            return await _productoRepository.GetProductosCountAsync();
        }

        public async Task<string> GetProductoImageUrlAsync(int id)
        {
            return await _productoRepository.GetProductoImageUrlAsync(id);
        }

    }
}
