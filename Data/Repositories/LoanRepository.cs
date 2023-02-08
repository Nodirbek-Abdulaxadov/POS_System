using DataLayer.Context;
using DataLayer.Entities.Selling;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class LoanRepository : Repository<Loan>, ILoanInterface
{
    public LoanRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
