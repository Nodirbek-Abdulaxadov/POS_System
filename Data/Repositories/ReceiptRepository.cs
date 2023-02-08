using DataLayer.Context;
using DataLayer.Entities.Selling;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class ReceiptRepository : Repository<Receipt>, IReceiptInterface
{
    public ReceiptRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
