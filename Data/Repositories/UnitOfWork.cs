using DataLayer.Context;
using DataLayer.Interfaces;

namespace DataLayer.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork( AppDbContext dbContext,
                       ICustomerInterface customerInterface,
                       ILoanInterface loanInterface,
                       ILoanPaymentInterface loanPaymentInterface,
                       IProductInterface productInterface,
                       IReceiptInterface receiptInterface,
                       ITransactionInterface transactionInterface,
                       IWarehouseInterface warehouseInterface,
                       IWarehouseItemInterface warehouseItemInterface,
                       ICategoryInterface categories)
    {
        _dbContext = dbContext;
        Customers = customerInterface;
        Loans = loanInterface;
        LoanPayments = loanPaymentInterface;
        Products = productInterface;
        Receipts = receiptInterface;
        Transactions = transactionInterface;
        Warehouses = warehouseInterface;
        WarehouseItems = warehouseItemInterface;
        Categories = categories;
    }
    public ICustomerInterface Customers { get; }

    public ILoanInterface Loans { get; }

    public ILoanPaymentInterface LoanPayments { get; }

    public IProductInterface Products { get; }

    public IReceiptInterface Receipts { get; }

    public ITransactionInterface Transactions { get; }

    public IWarehouseInterface Warehouses { get; }

    public IWarehouseItemInterface WarehouseItems { get; }

    public ICategoryInterface Categories { get; }

    public void Dispose()
            => GC.SuppressFinalize(this);

    public async Task SaveAsync()
        => await _dbContext.SaveChangesAsync();
}
