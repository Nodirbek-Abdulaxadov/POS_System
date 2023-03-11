using DataLayer.Entities;

namespace BLL.Dtos.ProductDtos;

public class AddProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public int WarningCount { get; set; }

    public int AdminId { get; set; }

    public static explicit operator Product(AddProductDto v)
        => new Product()
        {
            Name = v.Name,
            Brand = v.Brand,
            Color = v.Color,
            Size = v.Size,
            Barcode = v.Barcode,
            AdminId = v.AdminId,
            WarningCount = v.WarningCount,
            AddedDate = DateTime.Now.ToString(),
            IsDeleted = false
        };
}