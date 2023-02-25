using BLL.Dtos.CustomerDtos;
using BLL.Dtos.WarehouseItemDtos;
using DataLayer.Entities;

namespace BLL.Dtos.WarehouseDtos;

public class WarehouseDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string CreatedDate { get; set; } = string.Empty;

    public IEnumerable<WarehouseItemDto> Items = new List<WarehouseItemDto>();

    public static explicit operator WarehouseDto(Warehouse v)
        => new WarehouseDto()
        {
            Id = v.Id,
            Name = v.Name,
            CreatedDate = v.CreatedDate,
            IsDeleted = v.IsDeleted,
            Items = v.Items.Select(x => (WarehouseItemDto)x)
        };
}