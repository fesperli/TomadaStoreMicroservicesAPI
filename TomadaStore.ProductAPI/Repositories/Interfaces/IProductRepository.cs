using TomadaStore.Models.DTOs.Product;

namespace TomadaStore.ProductAPI.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<List<ProductResponseDTO>> GetProducts();
        Task<ProductResponseDTO> GetProductById(int id);
        Task<ProductResponseDTO> CreateProduct(ProductResponseDTO productDto);
        Task<bool> UpdateProduct(ProductResponseDTO productDto);
        Task<bool> DeleteProduct(int id);
    }
}
