using DataLayer.Entities;

namespace BLL.Dtos.ProductDtos;

public class ProductViewDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;
    public string Size { get; set; } = string.Empty;
    public string Barcode { get; set; } = string.Empty;
    public int AdminId { get; set; }

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
            AdminId = v.AdminId
        };
}