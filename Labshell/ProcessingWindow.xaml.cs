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
using Labshell.Service;
using Labshell.Factory;
using Labshell.Result;

namespace Labshell
{
    /// <summary>
    /// ProcessingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessingWindow : Window
    {
        private List<UploadFile> upfiles = new List<UploadFile>();
        private VideoCaptureDevice device;

        private RecordFactory rf = new RecordFactory();

        private RealTimeCheck rtc = new RealTimeCheck();

        private CaptureService cs = new CaptureService();

        private ListenService ls = new ListenService();

        public ProcessingWindow()
        {
            InitializeComponent();
            initVideo();
            initData();
        }

        private void initData()
        {
            this.fileList.ItemsSource = upfiles;

            rtc.SetLabel(this.netInfo);
            rtc.SetImage(this.netState);
            rtc.Start();

            cs.SetDevice(this.device);
            cs.SetSavePath(System.Environment.CurrentDirectory + "/photo");
            cs.Start();

            ls.SetListBox(this.fileList);
            ls.SetPaths(CacheService.GetListenPath());

            this.groupLabel.Content = "当前共"+CacheService.GetStudentList().Count+"人组队";
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
            String doc = label.Tag.ToString().Substring(0, label.Tag.ToString().Length - label.Tag.ToString().Split('\\')[label.Tag.ToString().Split('\\').Length-1].Length);
            System.Diagnostics.Process.Start("explorer.exe ", doc);
        }

        private void ExitButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            device.SignalToStop();
            device.WaitForStop(); 
            Application.Current.Shutdown();
        }

        private void initVideo()
        {
            // 设定初始视频设备  
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count > 0)
            {   // 默认设备  
                device = new VideoCaptureDevice(videoDevices[0].MonikerString);
                device.NewFrame += new NewFrameEventHandler(videoSourcePlayer_NewFrame);
                device.Start();
            }
        }

        private void videoSourcePlayer_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            
            GC.Collect();

            picture.Image = bitmap;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String token = CacheService.GetStuToken();
                
                FileResult fr = rf.UploadFile(openFileDialog.FileName, token);

                //获取文件名字
                String file_name = openFileDialog.FileName.Split('\\')[openFileDialog.FileName.Split('\\').Length-1];

                if (fr != null)
                {
                    if (fr.code == "200")
                    {
                        UploadFile up = new UploadFile() { FileName = file_name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.SUCCESS, FilePath = openFileDialog.FileName, Id = fr.data.id };
                        upfiles.Add(up);
                        fileList.Items.Refresh();
                    }
                    else
                    {
                        LSMessageBox.Show("上传文件错误", fr.message);
                        UploadFile up = new UploadFile() { FileName = file_name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.FAIL, FilePath = openFileDialog.FileName, Id = -1 };
                        upfiles.Add(up);
                        fileList.Items.Refresh();
                    }
                }
                else
                {
                    LSMessageBox.Show("网络错误","网络异常");
                    UploadFile up = new UploadFile() { FileName = file_name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.FAIL, FilePath = openFileDialog.FileName, Id = -1 };
                    upfiles.Add(up);
                    fileList.Items.Refresh();
                }
            }
        }
    }
}
