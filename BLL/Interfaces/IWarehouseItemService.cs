using BLL.Dtos.WarehouseItemDtos;
using BLL.Helpers;

namespace BLL.Interfaces;

public interface IWarehouseItemService
{
    Task<PagedList<WarehouseItemDto>> GetPagedAsync(int pageSize, int pageNumber, int warehouseId);
    Task<PagedList<WarehouseItemDto>> GetArchivedAsync(int pageSize, int pageNumber);
    Task<IEnumerable<WarehouseItemDto>> GetAllAsync(int warehouseId);

    Task<WarehouseItemDto> GetByIdAsync(int id);
    Task<WarehouseItemDto> AddAsync(AddWarehouseItemDto dto);

    Task<WarehouseItemDto> Update(UpdateWarehouseItemDto dto);
    Task ActionAsync(int id, ActionType action);
}
