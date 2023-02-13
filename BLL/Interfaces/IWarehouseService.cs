using BLL.Dtos.WarehouseDtos;
using BLL.Helpers;

namespace BLL.Interfaces;

public interface IWarehouseService
{
    Task<PagedList<WarehouseViewDto>> GetWarehousesAsync(int pageSize, int pageNumber);
    Task<IEnumerable<WarehouseViewDto>> GetAllAsync();

    Task<WarehouseViewDto> GetByIdAsync(int id);
    Task<WarehouseViewDto> AddAsync(AddWarehouseDto dto);

    Task<WarehouseViewDto> UpdateAsync(WarehouseUpdateDto dto);
    Task RemoveAsync(int id);
}