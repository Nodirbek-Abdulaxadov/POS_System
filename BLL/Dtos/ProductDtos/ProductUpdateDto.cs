using DataLayer.Entities;

namespace BLL.Dtos.ProductDtos;

public class ProductUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public int WarningCount { get; set; }

    public static explicit operator Product(ProductUpdateDto v)
        => new Product()
        {
            Id = v.Id,
            Name = v.Name,
            Brand = v.Brand,
            Color = v.Color,
            Size = v.Size,
            Barcode = v.Barcode,
            WarningCount = v.WarningCount,
        };
}