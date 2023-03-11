using BLL.Dtos.WarehouseItemDtos;
using BLL.Helpers;

namespace BLL.Interfaces;

public interface IWarehouseItemService
{
    Task<PagedList<WarehouseItemDto>> GetPagedAsync(int pageSize, int pageNumber, int warehouseId);
    Task<IEnumerable<WarehouseItemDto>> GetAllAsync(int warehouseId);

    Task<WarehouseItemDto> GetByIdAsync(int id);
    Task<WarehouseItemDto> AddAsync(AddWarehouseItemDto dto);

    Task<WarehouseItemDto> Update(UpdateWarehouseItemDto dto);
    Task RemoveAsync(int id);
}
