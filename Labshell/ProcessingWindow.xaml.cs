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

namespace Labshell
{
    /// <summary>
    /// ProcessingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessingWindow : Window
    {
        private List<UploadFile> upfiles = new List<UploadFile>();

        public ProcessingWindow()
        {
            InitializeComponent();
            initData();
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
            Application.Current.Shutdown();
        }
    }
}
