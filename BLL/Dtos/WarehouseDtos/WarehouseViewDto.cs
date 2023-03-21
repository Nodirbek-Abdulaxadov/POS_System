using DataLayer.Entities;

namespace BLL.Dtos.WarehouseDtos;

public class WarehouseViewDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string CreatedDate { get; set; } = string.Empty;

    public static explicit operator WarehouseViewDto(Warehouse v)
        => new WarehouseViewDto()
        {
            Id = v.Id,
            Name = v.Name,
            CreatedDate = v.CreatedDate
        };
}
