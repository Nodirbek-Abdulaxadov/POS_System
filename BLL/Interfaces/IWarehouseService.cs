using BLL.Dtos.WarehouseDtos;

namespace BLL.Interfaces;

public interface IWarehouseService
{
    Task<IEnumerable<WarehouseViewDto>> GetAllAsync();
    Task<IEnumerable<WarehouseDto>> GetAllWithItemsAsync();

    Task<WarehouseViewDto> GetByIdAsync(int id);
    Task<WarehouseViewDto> AddAsync(AddWarehouseDto dto);

    Task<WarehouseViewDto> UpdateAsync(WarehouseUpdateDto dto);
    Task RemoveAsync(int id);
}