using BLL.Dtos.ProductDtos;
using BLL.Dtos.TransactionDtos;
using Seller.App.Components;
using Seller.App.Services;
using Seller.App.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

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
                    notificationLifetime: TimeSpan.FromSeconds(1),
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
            Load();
        }

        private void GetProductViews()
        {
            product = new ProductAPIService();
            var result = product.GetProducts();
            result.Wait();
            var r = result.Result;
            productViews.AddRange(r ?? new List<DProduct>());
            Application.Current.Dispatcher.BeginInvoke(
                  DispatcherPriority.Background,
                  new Action(() => {
                      notifier.ShowSuccess("Ma'lumotlar yangilandi!");
                  }));
        }

        private void Load()
        {
            CheckNetwork();
            Thread t = new Thread(GetProductViews);
            t.SetApartmentState(ApartmentState.STA);
            t.IsBackground = true;
            t.Start();
        }

        private void DispatcherTimer_Tick(object? sender, EventArgs e)
        {
            clock.Text = DateTime.Now.ToString("hh:mm:ss tt");
            date.Text = DateTime.Now.ToLongDateString();
        }

        private void logout_btn_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MaterialMessageBox("Are you sure logout from profile!", MessageType.Confirmation, MessageButtons.YesNo);
            var result = messageBox.ShowDialog();
            if (result == true)
            {
                using var tokenService = new TokenService();
                tokenService.RemoveCreditionals();
                Application.Current.MainWindow.Close();
                Application.Current.MainWindow = new MainWindow();
                Application.Current.MainWindow.Show();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.Key == Key.D4)
            //{
            //    barcode_input.Focus();
            //}
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
            if (vm != null)
            {
                var totalPrice = vm.Transactions.Sum(tr => tr.TotalPrice);
                if (!string.IsNullOrEmpty(chegirma.Text))
                {
                    totalPrice -= decimal.Parse(chegirma.Text.Replace(" ", ""));
                }
                if (totalPrice > 0)
                {
                    total.Text = ConvertToMoneyFormat(totalPrice.ToString());
                }
                else
                {
                    total.Text = "0";
                }
            }
        }

        private void refresh_Click(object sender, RoutedEventArgs e)
        {
            vm.Transactions.Clear();
            total.Clear();
            naqd.Clear();
            plastik.Clear();
            chegirma.Clear();
            barcode_input.Clear();
            Load();
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            if (vm.Transactions.Count > 0)
            {
                using var selling = new SellingService();
                var receipt = selling.CreateEmptyReceipt();
                receipt.SellerId = "Some guid";
                receipt.Discount = decimal.Parse(chegirma.Text.Replace(" ", "")); ;
                receipt.PaidCard = decimal.Parse(plastik.Text.Replace(" ", ""));
                receipt.PaidCash = decimal.Parse(naqd.Text.Replace(" ", ""));
                //receipt.Transactions = vm.Transactions.ToList();

                using PrintService printService = new PrintService();
                printService.printerName = "XP-80";
                printService.Print(receipt);
            }
            else
            {
                notifier.ShowWarning("Mahsulot qo'shing!");
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
                else
                {
                    messageBox = new MaterialMessageBox("Are you sure exit app?", MessageType.Confirmation, MessageButtons.YesNo);
                    result = messageBox.ShowDialog();
                    if (result == true)
                    {
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        goto start;
                    }
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
                        if (naqd.Text == "0") naqd.Clear();
                        naqd.Text += number;
                        naqd.Text = ConvertToMoneyFormat(naqd.Text);
                    } break;
                case 2:
                    {
                        if (plastik.Text == "0") plastik.Clear();
                        plastik.Text += number;
                        plastik.Text = ConvertToMoneyFormat(plastik.Text);
                    }
                    break;
                case 3:
                    {
                        if (chegirma.Text == "0") chegirma.Clear();
                        chegirma.Text += number;
                        chegirma.Text = ConvertToMoneyFormat(chegirma.Text);
                    }
                    break;
                case 4:
                    {
                        barcode_input.Text += number;
                    }
                    break;
            }
            SetTotalPrice();
        }

        private string ConvertToMoneyFormat(string text)
        {
            if (text.Length < 3 || string.IsNullOrEmpty(text)) return text;

            text = Reverse(text.Replace(" ", ""));
            string result = string.Empty;
            for (int i = 0; i < text.Length; i ++)
            {
                result += text[i];
                if ((i+1) % 3 == 0)
                {
                    result += " ";
                }
            }

            return Reverse(result).Trim();
        }

        private string Reverse(string text)
        {
            string reversed = string.Empty;
            foreach (char c in text.Reverse())
            {
                reversed += c;
            }

            return reversed;
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

        private void naqd_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender; 
            switch (activeTextboxIndex)
            {
                case 1:
                    {
                        naqd.Text = ConvertToMoneyFormat(naqd.Text);
                    }
                    break;
                case 2:
                    {
                        plastik.Text = ConvertToMoneyFormat(plastik.Text);
                    }
                    break;
                case 3:
                    {
                        chegirma.Text = ConvertToMoneyFormat(chegirma.Text);
                    }
                    break;
                case 4:
                    {

                    }break;
            }
            SetTotalPrice();
        }

        private void close_btn_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MaterialMessageBox("Are you sure exit app!", MessageType.Confirmation, MessageButtons.YesNo);
            var result = messageBox.ShowDialog();
            if (result == true)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
