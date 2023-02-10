using DataLayer.Entities;

namespace BLL.Dtos.WarehouseDtos;

public class WarehouseUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CreatedDate { get; set; } = string.Empty;

    public static explicit operator Warehouse(WarehouseUpdateDto v)
        => new Warehouse()
        {
            Id = v.Id,
            Name = v.Name,
            IsDeleted = false,
            CreatedDate = v.CreatedDate
        };
}