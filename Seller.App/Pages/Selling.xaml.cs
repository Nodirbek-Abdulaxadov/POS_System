using BLL.Dtos.ProductDtos;
using BLL.Dtos.TransactionDtos;
using Seller.App.Services;
using Seller.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Seller.App.Pages
{
    /// <summary>
    /// Interaction logic for Selling.xaml
    /// </summary>
    public partial class Selling : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        ProductAPIService product;
        List<DProduct> productViews = new List<DProduct>();
        TransactionViewModel vm;

        public Selling()
        {
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            InitializeComponent();

            vm = new TransactionViewModel();
            transactions_table.ItemsSource = vm.Transactions;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            productViews.AddRange(GetProductViews());
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            clock.Text = DateTime.Now.ToString("hh:mm:ss tt");
            date.Text = DateTime.Now.ToLongDateString();
        }

        private void logout_btn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.D4)
            {
                barcode_input.Focus();
            }
            if (e.Key == Key.Enter)
            {
                var tr = productViews.FirstOrDefault(i => i.Barcode == barcode_input.Text);
                if (tr != null && !vm.Transactions.Any(t => t.Barcode == barcode_input.Text))
                {
                    vm.Add(tr);
                }
                barcode_input.Clear();
            }
        }

        private List<DProduct> GetProductViews()
        {
            product = new ProductAPIService();
            var result = product.GetProducts();
            result.Wait();
            var res = result.Result;
            return res;
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            var item = transactions_table.SelectedItem as TransactionDto;
            vm.Transactions.Remove(item);
        }

        private void minus_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = transactions_table.SelectedItem as TransactionDto;
            if (item.Quantity > 1)
            {
                foreach (var m in vm.Transactions)
                {
                    if (m.Barcode == item.Barcode)
                    {
                        m.Quantity--;
                        m.TotalPrice = m.Price * m.Quantity;
                    }
                }
            }
            Refresh();
            return;
        }

        private void plus_btn_Click(object sender, RoutedEventArgs e)
        {
            var item = transactions_table.SelectedItem as TransactionDto;
            if (item.Quantity < item.AvailableCount)
            {
                foreach (var m in vm.Transactions)
                {
                    if (m.Barcode == item.Barcode)
                    {
                        m.Quantity++;
                        m.TotalPrice = m.Price * m.Quantity;
                    }
                }
            }
            Refresh();
            return;
        }

        private void Refresh()
        {
            ObservableCollection<TransactionDto> transactions = new ObservableCollection<TransactionDto>();
            foreach (var transaction in vm.Transactions)
            {
                transactions.Add(transaction);
            }
            vm.Transactions.Clear();
            vm.Transactions = transactions;

            transactions_table.ItemsSource = vm.Transactions;

            var totalPrice = vm.Transactions.Sum(tr => tr.TotalPrice);
            total.Text = totalPrice.ToString();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            vm.Transactions.Clear();
            total.Clear();
            productViews.AddRange(GetProductViews());
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Transactions.Count > 1)
            {
                using PrintService printService = new PrintService();
                printService.printerName = "XP-80";
                printService.Print(vm.Transactions.ToList(), "Nodirbek Abdulaxadov", 1);
            }
        }
    }
}
