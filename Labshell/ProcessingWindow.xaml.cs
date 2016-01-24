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
using Labshell.JsonForm;

namespace Labshell
{
    /// <summary>
    /// ProcessingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ProcessingWindow : Window
    {
        private List<UploadFile> upfiles = new List<UploadFile>();

        private VideoCaptureDevice device;

        private VideoCaptureDevice device2;

        private RecordFactory rf = new RecordFactory();

        private RealTimeCheck rtc = new RealTimeCheck();

        private CaptureService cs = new CaptureService();

        private CaptureService cs2 = new CaptureService();

        private ListenService ls = new ListenService("EXPERIMENT_RECORD_FILE");

        //private ListenService ps = new ListenService("EXPERIMENT_RECORD_IMAGE");

        public ProcessingWindow()
        {
            InitializeComponent();
            initVideo();
            initData();
        }

        private void initData()
        {
            this.fileList.ItemsSource = upfiles;

            //配置实时监控
            rtc.SetLabel(this.netInfo);
            rtc.SetImage(this.netState);
            rtc.SetVedioImage(this.videoState);
            rtc.SetVedioLabel(this.videoInfo);
            rtc.Start();

            //配置监听实验数据上传
            ls.SetFilter("*.txt");
            ls.SetListBox(this.fileList);
            ls.SetPaths(CacheService.Instance.GetListenPath());

            //配置摄像头
            cs.SetSavePath(System.Environment.CurrentDirectory + "/photo");
            cs.SetDeviceId(1);
            cs.SetListBox(this.fileList);

            cs2.SetSavePath(System.Environment.CurrentDirectory + "/photo");
            cs2.SetDeviceId(2);
            cs2.SetListBox(this.fileList);

            this.groupLabel.Content = "当前共"+CacheService.Instance.GetStudentList().Count+"人组队";
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (device != null)
            {
                device.SignalToStop();
                device.WaitForStop();
            }
            if (device2 != null)
            {
                device2.SignalToStop();
                device2.WaitForStop();
            }
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
            bool? result = LSMessageBox.YNShow("退出", "确认完成实验？");
            if (result == true)
            {
                List<int> ids = new List<int>();
                foreach (Student s in CacheService.Instance.GetStudentList())
                {
                    ids.Add(s.RecordId);
                }
                AttachResult ar = rf.FinishExperiment(CacheService.Instance.ExperimentId, ids, CacheService.Instance.GetStuToken());
                if (ar != null)
                {
                    if (ar.code == "200")
                    {
                        if (device != null)
                        {
                            device.SignalToStop();
                            device.WaitForStop();
                        }
                        if (device2 != null)
                        {
                            device2.SignalToStop();
                            device2.WaitForStop();
                        }
                        Application.Current.Shutdown();
                    }
                    else
                    {
                        LSMessageBox.Show("完成实验异常", ar.message);
                    }
                }
                else
                {
                    LSMessageBox.Show("网络错误", "网络异常");
                }
            }
        }

        private void initVideo1()
        {
            // 设定初始视频设备  
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            this.vedioGrid.Visibility = System.Windows.Visibility.Visible;
            this.noneGrid.Visibility = System.Windows.Visibility.Collapsed;
            // 默认设备  
            device = new VideoCaptureDevice(videoDevices[0].MonikerString);
            device.NewFrame += new NewFrameEventHandler(videoSourcePlayer_NewFrame);
            device.Start();
            cs.SetDevice(this.device);
            cs.Start();//启动自动拍照线程
        }

        private void initVideo2()
        {
            // 设定初始视频设备  
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            this.vedio2Grid.Visibility = System.Windows.Visibility.Visible;
            this.none2Grid.Visibility = System.Windows.Visibility.Collapsed;
            // 默认设备  
            device2 = new VideoCaptureDevice(videoDevices[1].MonikerString);
            device2.NewFrame += new NewFrameEventHandler(videoSourcePlayer_NewFrame2);
            device2.Start();
            cs2.SetDevice(this.device2);
            cs2.Start();//启动自动拍照线程
        }

        private void initVideo()
        {
            // 设定初始视频设备  
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 1)
            {
                initVideo1();
                this.vedio2Grid.Visibility = System.Windows.Visibility.Collapsed;
                this.none2Grid.Visibility = System.Windows.Visibility.Visible;
            }
            else if (videoDevices.Count == 2)
            {
                initVideo1();
                initVideo2();
            }
            else if (videoDevices.Count == 0)
            {
                this.vedioGrid.Visibility = System.Windows.Visibility.Collapsed;
                this.noneGrid.Visibility = System.Windows.Visibility.Visible;

                this.vedio2Grid.Visibility = System.Windows.Visibility.Collapsed;
                this.none2Grid.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void videoSourcePlayer_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            
            GC.Collect();

            picture.Image = bitmap;
        }

        private void videoSourcePlayer_NewFrame2(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            GC.Collect();

            picture2.Image = bitmap;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                String token = CacheService.Instance.GetStuToken();
                
                FileResult fr = rf.UploadFile(openFileDialog.FileName, token);

                //获取文件名字
                String file_name = openFileDialog.FileName.Split('\\')[openFileDialog.FileName.Split('\\').Length-1];

                if (fr != null)
                {
                    if (fr.code == "200")
                    {
                        //关联文件与记录
                        List<Attach> attaches = new List<Attach>();
                        foreach(Student s in CacheService.Instance.GetStudentList())
                        {
                            Attach a = new Attach { subjectId = s.RecordId, ownerId = s.Id, type = "EXPERIMENT_RECORD_FILE" };
                            attaches.Add(a);
                        }
                        
                        AttachResult ar = rf.AttachRecordWithFile(CacheService.Instance.ExperimentId, fr.data.id, attaches, CacheService.Instance.GetStuToken());

                        if (ar != null)
                        {
                            if (ar.code == "200")
                            {
                                UploadFile up = new UploadFile() { FileName = file_name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.SUCCESS, FilePath = openFileDialog.FileName, Id = fr.data.id };
                                upfiles.Add(up);
                                fileList.Items.Refresh();
                            }
                            else
                            {
                                LSMessageBox.Show("关联文件错误", ar.message);
                                UploadFile up = new UploadFile() { FileName = file_name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.FAIL, FilePath = openFileDialog.FileName, Id = -1 };
                                upfiles.Add(up);
                                fileList.Items.Refresh();
                            }
                        }
                        else
                        {
                            LSMessageBox.Show("网络错误", "网络异常");
                            UploadFile up = new UploadFile() { FileName = file_name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.FAIL, FilePath = openFileDialog.FileName, Id = -1 };
                            upfiles.Add(up);
                            fileList.Items.Refresh();
                        }
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

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                LSMessageBox.Show("摄像头异常", "没检测到摄像头，请确保摄像头安装正常");
            }
            else
            {
                initVideo1();
            }
        }

        private void RefreshButton2_Click(object sender, RoutedEventArgs e)
        {
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0)
            {
                LSMessageBox.Show("摄像头异常", "没检测到摄像头，请确保摄像头安装正常");
            }
            else if (videoDevices.Count < 2)
            {
                LSMessageBox.Show("摄像头异常", "没检测到第二个摄像头，请确保摄像头安装正常");
            }
            else
            {
                initVideo2();
            }
        }
    }
}
