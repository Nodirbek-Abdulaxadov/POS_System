using System.ComponentModel.DataAnnotations;

namespace DataLayer.Entities;

public class Product : BaseEntity
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;
    public int WarningCount { get; set; }
    [StringLength(50)]
    public string Brand { get; set; } = string.Empty;
    [StringLength(20)]
    public string Color { get; set; } = string.Empty;
    [StringLength(20)]
    public string Size { get; set; } = string.Empty;
    [StringLength(50)]
    public string Barcode { get; set; } = string.Empty;

    public int AdminId { get; set; }
    [StringLength(30)]
    public string AddedDate { get; set; } = string.Empty;

    public IEnumerable<WarehouseItem> Products = new List<WarehouseItem>();
}
