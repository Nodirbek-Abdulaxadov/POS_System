using Seller.App.Components;
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

namespace Seller.App.Pages;

/// <summary>
/// Interaction logic for OTP.xaml
/// </summary>
public partial class OTP : Page
{
    public OTP()
    {
        InitializeComponent();
    }

    private void send_btn_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(phone_input.Text))
        {
            new MaterialMessageBox("Telefon raqamingizni kiriting!", MessageType.Error, MessageButtons.Ok).ShowDialog();
            return;
        }
        else if (phone_input.Text.Length < 7)
        {
            new MaterialMessageBox("Telefon raqam noto'g'ri kiritildi!", MessageType.Error, MessageButtons.Ok).ShowDialog();
            return;
        }
        else
        {
            using var authService = new AuthService();
            var request = authService.SendOtp(phone_input.Text);
            request.Wait();
            var result = request.Result;
            if (result != null)
            {
                new MaterialMessageBox("Yuborildi!", MessageType.Success, MessageButtons.Ok).ShowDialog();
                return;
            }
        }
    }
}