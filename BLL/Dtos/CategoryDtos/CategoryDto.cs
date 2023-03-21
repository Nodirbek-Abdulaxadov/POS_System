using BLL.Dtos.ProductDtos;

namespace BLL.Dtos.CategoryDtos;

public class CategoryDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public List<DProduct> Products = new List<DProduct>();
}