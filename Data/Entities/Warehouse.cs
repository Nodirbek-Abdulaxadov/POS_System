using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities;

public class Warehouse : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Entity { get; set; } = string.Empty;
    [Required]
    [StringLength(30)]
    public string CreatedDate { get; set; } = string.Empty;

    public IEnumerable<WarehouseItem> Items = new List<WarehouseItem>();
}
