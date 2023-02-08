using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class ProductRepository : Repository<Product>, IProductInterface
{
    public ProductRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
