using Seller.App.Components;
using Seller.App.Services;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Seller.App.Pages
{
    /// <summary>
    /// Interaction logic for PrinterSettings.xaml
    /// </summary>
    public partial class PrinterSettings : Page
    {
        PrintService service = new PrintService();
        public PrinterSettings()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            connected.Text = service.GetSavedPrinterName();
            var list = service.ConnectedPrinters();
            printers.ItemsSource = list;
            printers.SelectedIndex = list.FindIndex(p => p == connected.Text);
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string selected = printers.SelectedItem.ToString();
            service.SavePrinter(selected);
            connected.Text = selected;
        }

        private void test_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string selected = printers.SelectedItem.ToString();
                service.printerName = selected;
                service.Test();
            }
            catch (Exception)
            {
                var messageBox = new MaterialMessageBox("Tanlangan printer ishlamadi🤦‍♂️!", MessageType.Error, MessageButtons.Ok);
                messageBox.ShowDialog();
            }
        }
    }
}
