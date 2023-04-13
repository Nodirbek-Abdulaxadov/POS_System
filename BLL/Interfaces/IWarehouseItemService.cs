using BLL.Dtos.WarehouseItemDtos;
using BLL.Helpers;

namespace BLL.Interfaces;

public interface IWarehouseItemService
{
    Task<PagedList<WarehouseItemViewDto>> GetPagedAsync(int pageSize, int pageNumber, int warehouseId);
    Task<PagedList<WarehouseItemViewDto>> GetAllAsPaged();
    Task<PagedList<WarehouseItemViewDto>> GetArchivedAsync(int pageSize, int pageNumber);
    Task<IEnumerable<WarehouseItemViewDto>> GetAllAsync();

    Task<WarehouseItemViewDto> GetByIdAsync(int id);
    Task<WarehouseItemDto> AddAsync(AddWarehouseItemDto dto);

    Task<WarehouseItemViewDto> Update(UpdateWarehouseItemDto dto);
    Task ActionAsync(int id, ActionType action);
}
