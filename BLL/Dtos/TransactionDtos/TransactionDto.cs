using BLL.Dtos.ProductDtos;

namespace BLL.Dtos.TransactionDtos;

public class TransactionDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public int AvailableCount { get; set; }
    public int OrderId { get; set; }
}