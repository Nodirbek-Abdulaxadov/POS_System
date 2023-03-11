using DataLayer.Entities;

namespace BLL.Dtos.WarehouseItemDtos;

public class UpdateWarehouseItemDto
{
    public int Id { get; set; }
    public int Quantity { get; set; }
    public string BroughtDate { get; set; } = string.Empty;
    public decimal IncomingPrice { get; set; }
    public decimal SellingPrice { get; set; }

    public int ProductId { get; set; }
    public int AdminId { get; set; }
    public int WarehouseId { get; set; }

    public static explicit operator WarehouseItem(UpdateWarehouseItemDto v)
         => new WarehouseItem()
         {
             Id = v.Id,
             Quantity = v.Quantity,
             AdminId = v.AdminId,
             WarehouseId = v.WarehouseId,
             ProductId = v.ProductId,
             BroughtDate = v.BroughtDate,
             IncomingPrice = v.IncomingPrice,
             SellingPrice = v.SellingPrice
         };
}
