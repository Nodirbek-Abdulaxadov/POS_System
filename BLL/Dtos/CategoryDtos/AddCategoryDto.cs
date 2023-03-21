using DataLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos.CategoryDtos;

public class AddCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    public static explicit operator Category(AddCategoryDto v)
        => new Category()
        {
            Name = v.Name,
            IsDeleted = false
        };
}