using DataLayer.Context;
using DataLayer.Entities.Selling;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class CustomerRepository : Repository<Customer>, ICustomerInterface
{
    public CustomerRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
