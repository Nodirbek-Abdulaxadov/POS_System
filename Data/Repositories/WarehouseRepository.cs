using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class WarehouseRepository : Repository<Warehouse>, IWarehouseInterface
{
    private readonly AppDbContext _dbContext;

    public WarehouseRepository(AppDbContext dbContext) 
        : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<IEnumerable<Warehouse>> GetAllWithItemsAsync()
    {
        var items = _dbContext.WarehousesItems.ToList();
        var warehouses = _dbContext.Warehouses.ToList();

        var res = warehouses.Select(w =>
        {
            var list = items.Where(i => i.WarehouseId == w.Id).ToList();
            return new Warehouse()
            {
                Id = w.Id,
                Name = w.Name,
                IsDeleted = w.IsDeleted,
                CreatedDate = w.CreatedDate,
                Items = list
            };
        });

        return Task.FromResult(res);
    }
}
