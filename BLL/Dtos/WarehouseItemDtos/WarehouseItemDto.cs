using DataLayer.Entities;

namespace BLL.Dtos.WarehouseItemDtos;

public class WarehouseItemDto : BaseDto
{
    public int Quantity { get; set; }
    public string BroughtDate { get; set; } = string.Empty;
    public decimal IncomingPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public int ProductId { get; set; }
    public int AdminId { get; set; }
    public int WarehouseId { get; set; }

    public static explicit operator WarehouseItemDto(WarehouseItem v)
        => new WarehouseItemDto()
        {
            Id = v.Id,
            Quantity = v.Quantity,
            SellingPrice = v.SellingPrice,
            ProductId = v.ProductId,
            AdminId = v.AdminId,
            WarehouseId = v.WarehouseId,
            BroughtDate = v.BroughtDate,
            IncomingPrice = v.IncomingPrice,
            IsDeleted = v.IsDeleted,
        };
}