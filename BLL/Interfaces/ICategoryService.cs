using BLL.Dtos.CategoryDtos;
using BLL.Helpers;

namespace BLL.Interfaces;

public interface ICategoryService
{
    Task<PagedList<CategoryViewDto>> GetCategoriesAsync(int pageSize, int pageNumber);
    Task<PagedList<CategoryViewDto>> GetArchivedCategoriesAsync(int pageSize, int pageNumber);
    Task<IEnumerable<CategoryViewDto>> GetAllAsync();

    Task<CategoryViewDto> GetByIdAsync(int id);
    Task<CategoryViewDto> AddAsync(AddCategoryDto dto);

    Task<CategoryViewDto> UpdateAsync(UpdateCategoryDto dto);
    Task ActionAsync(int id, ActionType action);
}