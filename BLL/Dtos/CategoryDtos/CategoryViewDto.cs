using DataLayer.Entities;

namespace BLL.Dtos.CategoryDtos;

public class CategoryViewDto : BaseDto
{
    public string Name { get; set; } = string.Empty;

    public static explicit operator CategoryViewDto(Category v)
        => new CategoryViewDto()
        {
            Id = v.Id,
            Name = v.Name,
        };
}