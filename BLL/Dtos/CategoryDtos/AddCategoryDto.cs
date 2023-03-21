using System.ComponentModel.DataAnnotations;

namespace BLL.Dtos.CategoryDtos;

public class AddCategoryDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
}