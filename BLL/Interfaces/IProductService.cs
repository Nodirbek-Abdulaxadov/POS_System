using BLL.Dtos.ProductDtos;
using BLL.Helpers;

namespace BLL.Interfaces;

public interface IProductService
{
    Task<List<DProduct>> GetDProducts();
    Task<PagedList<ProductViewDto>> GetProductsAsync(int pageSize, int pageNumber);
    Task<PagedList<ProductViewDto>> GetArchivedProductsAsync(int pageSize, int pageNumber);
    Task<IEnumerable<ProductViewDto>> GetAllAsync();

    Task<ProductViewDto> GetByIdAsync(int id);
    Task<ProductViewDto> AddAsync(AddProductDto dto);

    Task<ProductViewDto> UpdateAsync(ProductUpdateDto dto);
    Task ActionAsync(int id, ActionType action);
}
