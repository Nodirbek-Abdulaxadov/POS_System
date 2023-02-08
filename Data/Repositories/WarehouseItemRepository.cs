using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Interfaces;
namespace DataLayer.Repositories
{
    public class WarehouseItemRepository : Repository<WarehouseItem>, IWarehouseItemInterface
    {
        public WarehouseItemRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
    }
}
