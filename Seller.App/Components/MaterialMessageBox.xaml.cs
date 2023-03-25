using System.Windows;
using System.Windows.Media;

namespace Seller.App.Components
{
    /// <summary>
    /// Interaction logic for MaterialMessageBox.xaml
    /// </summary>
    public partial class MaterialMessageBox : Window
    {
        public MaterialMessageBox(string Message, MessageType Type, MessageButtons Buttons)
        {
            InitializeComponent();
            txtMessage.Text = Message;
            switch (Type)
            {

                case MessageType.Info:
                    {
                        txtTitle.Text = "Info";
                        cardHeader.Background = new SolidColorBrush(Color.FromRgb(13, 110, 253));
                    }
                    break;
                case MessageType.Confirmation:
                    txtTitle.Text = "Confirmation";
                    break;
                case MessageType.Success:
                    {
                        txtTitle.Text = "Success";
                        cardHeader.Background = new SolidColorBrush(Color.FromRgb(0, 128, 0));
                    }
                    break;
                case MessageType.Warning:
                    {
                        txtTitle.Text = "Warning";
                        cardHeader.Background = new SolidColorBrush(Color.FromRgb(251, 255, 0));
                    }
                    break;
                case MessageType.Error:
                    {
                        txtTitle.Text = "Error";
                        cardHeader.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    break;
            }
            switch (Buttons)
            {
                case MessageButtons.OkCancel:
                    btnYes.Visibility = Visibility.Collapsed; 
                    btnNo.Visibility = Visibility.Collapsed;
                    btnRetry.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.YesNo:
                    btnOk.Visibility = Visibility.Collapsed; 
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnRetry.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.Ok:
                    btnOk.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Collapsed; 
                    btnNo.Visibility = Visibility.Collapsed;
                    btnRetry.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.Retry:
                    {
                        btnRetry.Visibility = Visibility.Visible;
                        btnOk.Visibility = Visibility.Collapsed;
                        btnCancel.Visibility = Visibility.Visible;
                        btnYes.Visibility = Visibility.Collapsed;
                        btnNo.Visibility = Visibility.Collapsed;
                    }
                    break;
            }
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
    public enum MessageType
    {
        Info,
        Confirmation,
        Success,
        Warning,
        Error,
    }
    public enum MessageButtons
    {
        OkCancel,
        YesNo,
        Ok,
        Retry
    }
}