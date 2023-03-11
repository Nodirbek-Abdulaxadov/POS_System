using BLL.Dtos.ProductDtos;
using BLL.Dtos.TransactionDtos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seller.App.ViewModels
{
    public class TransactionViewModel
    {
        public ObservableCollection<TransactionDto> Transactions = new ObservableCollection<TransactionDto>();

        public TransactionViewModel()
        {
        }

        public void Add(DProduct dProduct)
        {
            Transactions.Add(new TransactionDto()
            {
                Barcode = dProduct.Barcode,
                Brand = dProduct.Brand,
                Color = dProduct.Color,
                Size = dProduct.Size,
                Name = dProduct.Name,
                Price = dProduct.Price,
                Quantity = 1,
                TotalPrice = dProduct.Price,
                AvailableCount = dProduct.AvailableCount
            });
        }

        public void Increment(TransactionDto transaction)
        {
            foreach (var m in Transactions)
            {
                if (m.Barcode == transaction.Barcode)
                {
                    m.Quantity++;
                }
            }
        }
    }
}
