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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Seller.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void login_btn_Click(object sender, RoutedEventArgs e)
        {
            using var authService = new AuthService();
            var res = await authService.LoginAsync(phone_input.Text, password_input.Password);
            if (res.Item1 == true)
            {
                info.Foreground = Brushes.LightGreen;
                info.Text = res.Item2;
            }
            else
            {
                info.Text = res.Item2;

                MessageBox.Show(res.Item2);
            }
        }
    }
}
