using BLL.Dtos.ProductDtos;
using BLL.Dtos.TransactionDtos;
using Seller.App.Components;
using Seller.App.Services;
using Seller.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Seller.App.Pages
{
    /// <summary>
    /// Interaction logic for Selling.xaml
    /// </summary>
    public partial class Selling : Window
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();
        DispatcherTimer dispatcherTimer2 = new DispatcherTimer();

        ProductAPIService? product;
        List<DProduct> productViews = new List<DProduct>();
        TransactionViewModel vm;
        Notifier notifier;
        bool networkFailed = false;
        int activeTextboxIndex = 0;

        public Selling()
        {
            dispatcherTimer.Tick += DispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
            dispatcherTimer.Start();
            dispatcherTimer2.Tick += DispatcherTimer_Tick2;
            dispatcherTimer2.Interval = new TimeSpan(0, 0, 3);
            //dispatcherTimer2.Start();
            InitializeComponent();
            notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.TopLeft,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(3),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(3));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });

            vm = new TransactionViewModel();
            transactions_table.ItemsSource = vm.Transactions;
        }

        private void DispatcherTimer_Tick2(object? sender, EventArgs e)
        {
            CheckNetwork();
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
                SetTotalPrice();
            }
        }

        private List<DProduct>? GetProductViews()
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
            SetTotalPrice();
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
            else
            {
                notifier.ShowWarning("Bu mahsulotdan boshqa qolmadi!");
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
            SetTotalPrice();
        }

        private void SetTotalPrice()
        {
            var totalPrice = vm.Transactions.Sum(tr => tr.TotalPrice);
            total.Text = totalPrice.ToString();
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            vm.Transactions.Clear();
            total.Clear();
            naqd.Clear();
            plastik.Clear();
            chegirma.Clear();
            barcode_input.Clear();
            productViews.AddRange(GetProductViews());
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Transactions.Count > 0)
            {
                using PrintService printService = new PrintService();
                printService.printerName = "XP-80";
                printService.Print(vm.Transactions.ToList(), "Nodirbek Abdulaxadov", 1);
            }
        }

        private void setings_Click(object sender, RoutedEventArgs e)
        {
            Settings settings = new Settings();
            Application.Current.MainWindow.Opacity = 0.5;
            settings.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
        }

        private void CheckNetwork()
        {
            start:
            if (!NetworkService.IsConnected())
            {
                networkFailed = true;
                var messageBox = new MaterialMessageBox("Internet connection failed!", MessageType.Error, MessageButtons.Retry);
                var result = messageBox.ShowDialog();
                if (result == true)
                {
                    goto start;
                }
            }
            else if (networkFailed == true)
            {
                notifier.ShowSuccess("You are online!");
                networkFailed = false;
            }
        }

        private void numbers_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            WriteNumberToTextbox(button.Content.ToString());
        }

        private void WriteNumberToTextbox(string number)
        {
            switch (activeTextboxIndex)
            {
                case 1: 
                    {
                        naqd.Text += number;
                    } break;
                case 2:
                    {
                        plastik.Text += number;
                    }
                    break;
                case 3:
                    {
                        chegirma.Text += number;
                    }
                    break;
                case 4:
                    {
                        barcode_input.Text += number;
                    }
                    break;
            }
        }

        private void tb_GotFocus(object sender, RoutedEventArgs e)
        {
            activeTextboxIndex = int.Parse(((TextBox)sender).Uid);
        }

        private void removetb_Click(object sender, RoutedEventArgs e)
        {
            switch (activeTextboxIndex)
            {
                case 1: CutText(ref naqd); break;
                case 2: CutText(ref plastik); break;
                case 3: CutText(ref chegirma); break;
                case 4: CutText(ref barcode_input); break;
            }
        }

        private void CutText(ref TextBox textbox)
        {
            if (textbox == null || textbox.Text.Length == 0) return;
            if (textbox.Text.Length == 1)
            {
                textbox.Text = "";
                return;
            }

            textbox.Text = textbox.Text.Substring(0, textbox.Text.Length - 1);
        }

        private void scanerlash_Click(object sender, RoutedEventArgs e)
        {
            barcode_input.Focus();
        }
    }
}
