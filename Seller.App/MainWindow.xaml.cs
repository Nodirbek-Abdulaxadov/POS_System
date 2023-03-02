using Seller.App.Services;
using System.Windows;
using System.Windows.Media;

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
