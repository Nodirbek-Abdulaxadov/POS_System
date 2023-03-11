using Seller.App.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Seller.App.Pages
{
    /// <summary>
    /// Interaction logic for PrinterSetup.xaml
    /// </summary>
    public partial class PrinterSetup : Window
    {
        PrintService service = new PrintService();
        public PrinterSetup()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            connected.Text = service.GetSavedPrinterName();
            printers.ItemsSource = service.ConnectedPrinters();
            printers.SelectedIndex = 0;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            string selected = printers.SelectedItem.ToString();
            service.SavePrinter(selected);
            this.Close();
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
                MessageBox.Show("Tanlangan printer ishlamadi🤷‍♂");
            }
        }
    }
}
