using System.Windows;
namespace Seller.App.Pages;

/// <summary>
/// Interaction logic for ForgotPassword.xaml
/// </summary>
public partial class ForgotPassword : Window
{
    public ForgotPassword()
    {
        InitializeComponent();
        main.Content = new OTP();
    }

    private void back_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow.Close();
        Application.Current.MainWindow = new MainWindow();
        Application.Current.MainWindow.Show();
    }
}