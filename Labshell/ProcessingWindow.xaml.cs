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
using Labshell.Model;
using AForge.Video.DirectShow;
using System.ComponentModel;
using AForge.Video;
using System.Drawing;

namespace Labshell
{
    /// <summary>
    /// ProcessingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessingWindow : Window
    {
        private List<UploadFile> upfiles = new List<UploadFile>();
        private VideoCaptureDevice device;

        public ProcessingWindow()
        {
            InitializeComponent();
            initData();
            this.Loaded += new RoutedEventHandler(Window_Loaded);
        }

        private void initData()
        {
            UploadFile up1 = new UploadFile() { FileName = "test1.txt", FileType = UploadFile.EXPERIMENT, Status = UploadFile.SUCCESS };
            upfiles.Add(up1);
            UploadFile up2 = new UploadFile() { FileName = "test2.png", FileType = UploadFile.PHOTO, Status = UploadFile.WAIT };
            upfiles.Add(up2);
            this.fileList.ItemsSource = upfiles;
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            device.SignalToStop();
            device.WaitForStop(); 
            Application.Current.Shutdown();
        }

        private void MinButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MouseDownEventHandle(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void OpenLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;
            MessageBox.Show(label.Tag.ToString());
        }

        private void ExitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            device.SignalToStop();
            device.WaitForStop(); 
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 设定初始视频设备  
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {   // 默认设备  
                device = new VideoCaptureDevice(videoDevices[0].MonikerString);
                device.NewFrame += new NewFrameEventHandler(videoSourcePlayer_NewFrame);
                device.DesiredFrameSize = new System.Drawing.Size(160, 120);
                device.DesiredFrameRate = 1;
                device.Start();
            }
        }

        private void videoSourcePlayer_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            
            GC.Collect();

            picture.Image = bitmap;
        }
    }
}
