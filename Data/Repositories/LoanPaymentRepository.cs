using DataLayer.Context;
using DataLayer.Entities.Selling;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class LoanPaymentRepository : Repository<LoanPayment>, ILoanPaymentInterface
{
    public LoanPaymentRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
