using Seller.App.Components;
using Seller.App.Pages;
using Seller.App.Services;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace Seller.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Selling? selling;
        Notifier? notifier = null;
        private bool networkFailed = false;

        public MainWindow()
        {
            InitializeComponent();
            selling = new Selling();
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
        }

        private async void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (!NetworkService.IsConnected())
            {
                CheckNetwork();
            }
            else
            {
                if (phone_input.Text != "" && password_input.Password != "")
                {
                    using var authService = new AuthService();
                    try
                    {
                        var res = await authService.LoginAsync(phone_input.Text, password_input.Password);
                        if (res.Item1 == true)
                        {
                            info.Foreground = Brushes.LightGreen;

                            var messageBox = new MaterialMessageBox("Successfully logged in!", MessageType.Success, MessageButtons.Ok);
                            var result = messageBox.ShowDialog();

                            Application.Current.MainWindow.Close();
                            Application.Current.MainWindow = new Selling();
                            Application.Current.MainWindow.Show();
                        }
                        else
                        {
                            notifier.ShowError("Invalid phoneNumber or password!");
                        }
                    }
                    catch (Exception ex)
                    {
                        notifier.ShowError(ex.Message);
                    }
                }
                else
                {
                    notifier.ShowError("All fields must be non empty!");
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CheckNetwork();
            using var tokenService = new TokenService();
            if (tokenService.TokenExist())
            {
                Thread t = new Thread(VerifyToken);
                t.SetApartmentState(ApartmentState.STA);
                t.IsBackground = true;
                t.Start();
            }
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

        private void close_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = new MaterialMessageBox("Are you sure exit app!", MessageType.Confirmation, MessageButtons.YesNo);
            var result = messageBox.ShowDialog();
            if (result == true)
            {
                Application.Current.Shutdown();
            }
        }

        private void VerifyToken()
        {
            using var authService = new AuthService();
            var result = authService.RefreshToken();
            result.Wait();
            if (result.Result)
            {
                Application.Current.Dispatcher.BeginInvoke(
                 DispatcherPriority.Background,
                 new Action(() => {
                     new MaterialMessageBox("Successfully logged in!", MessageType.Success, MessageButtons.Ok).ShowDialog();
                     Application.Current.MainWindow.Close();
                     Application.Current.MainWindow = new Selling();
                     Application.Current.MainWindow.Show();
                 }));
            }
            else
            {
                Application.Current.Dispatcher.BeginInvoke(
                 DispatcherPriority.Background,
                 new Action(() => {
                     notifier.ShowError("Token expired!");
                 }));
            }
        }

        private void forgot_password_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = new ForgotPassword();
            Application.Current.MainWindow.Show();
        }
    }
}
