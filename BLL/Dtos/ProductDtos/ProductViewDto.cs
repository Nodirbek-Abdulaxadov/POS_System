using DataLayer.Entities;

namespace BLL.Dtos.ProductDtos;

public class ProductViewDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public int WarningCount { get; set; } = 0;
    public string AdminId { get; set; } = string.Empty;

    public static explicit operator ProductViewDto(Product v)
        => new ProductViewDto()
        {
            Id = v.Id,
            Name = v.Name,
            Brand = v.Brand,
            Color = v.Color,
            Size = v.Size,
            Barcode = v.Barcode,
            IsDeleted = v.IsDeleted,
            WarningCount = v.WarningCount,
            AdminId = v.AdminId
        };
}