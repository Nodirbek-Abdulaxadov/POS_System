using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities;

public class BaseEntity
{
    [Key, Required]
    public int Id { get; set; }

    public bool IsDeleted { get; set; } = false;
}
