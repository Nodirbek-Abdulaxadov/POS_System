namespace BLL.Dtos.WarehouseItemDtos;

public class WarehouseItemViewDto : BaseDto
{
    public int Quantity { get; set; }
    public string BroughtDate { get; set; } = string.Empty;
    public decimal IncomingPrice { get; set; }
    public decimal SellingPrice { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public string AdminId { get; set; } = string.Empty;
    public string AdminFullName { get; set; } = string.Empty;
    public int WarehouseId { get; set; }
    public string WarehouseName { get; set; } = string.Empty;
}