using AForge.Video.DirectShow;
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
using ZXing;

namespace Seller.App
{
    /// <summary>
    /// Interaction logic for ScanWindow.xaml
    /// </summary>
    public partial class ScanWindow : Window
    {
        public ScanWindow()
        {
            InitializeComponent();
        }

        VideoCaptureDevice? videoCapture;
        FilterInfoCollection? filterInfoCollection;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in filterInfoCollection )
            {
                video_devices.Items.Add(device.Name);
            }
            video_devices.SelectedIndex = 0;
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            videoCapture = new VideoCaptureDevice(filterInfoCollection[video_devices.SelectedIndex].MonikerString);
            videoCapture.NewFrame += VideoCapture_NewFrame;
            videoCapture.Start();
        }

        private void VideoCapture_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            LuminanceSource bitmap = (LuminanceSource)eventArgs.Frame.Clone();
            IBarcodeReader reader = new BarcodeReader();
            var result = reader.Decode(bitmap);

            if (result != null )
            {
                result_l.Text = result.ToString();
            }
        }
    }
}
