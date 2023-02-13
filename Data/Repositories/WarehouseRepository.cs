using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class WarehouseRepository : Repository<Warehouse>, IWarehouseInterface
{
    private readonly AppDbContext _dbContext;

    public WarehouseRepository(AppDbContext dbContext) 
        : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
