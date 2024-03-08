using Product.Application.DTO;

namespace Product.Application.Interface
{
    public interface IProductService
    {
        Task<long> Create(ProductDTO requestDTO);
        Task Update(UpdateProductDTO requestDTO, long id);
        Task Delete(long productId);
        Task<IEnumerable<ProductResponseDTO>> Get();
    }
}
