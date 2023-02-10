using DataLayer.Entities;

namespace DataLayer.Interfaces;

public interface IWarehouseInterface : IRepository<Warehouse>
{
    Task<IEnumerable<Warehouse>> GetAllWithItemsAsync();
}
