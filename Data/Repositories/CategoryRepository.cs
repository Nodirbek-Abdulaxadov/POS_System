using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class CategoryRepository : Repository<Category>, ICategoryInterface
{
    public CategoryRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}