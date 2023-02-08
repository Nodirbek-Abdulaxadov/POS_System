using DataLayer.Context;
using DataLayer.Entities.Selling;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class TransactionRepository : Repository<Transaction>, ITransactionInterface
{
    public TransactionRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
