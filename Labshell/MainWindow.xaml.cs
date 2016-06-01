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
using Labshell.Model;
using Labshell.Factory;
using Labshell.Result;
using Labshell.Service;
using Labshell.Util;
using Microsoft.Win32;
using Labshell.JsonForm;
using System.Diagnostics;
using System.ComponentModel;

namespace Labshell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Student> students = new List<Student>();

        private AccountFactory af = new AccountFactory();

        private ReservationFactory rf = new ReservationFactory();

        private RecordFactory rcf = new RecordFactory();

        private RealTimeCheck rtc = new RealTimeCheck();

        //动态获取的数据

        private String virtualExp;

        public MainWindow()
        {
            InitializeComponent();
            initData();
        }

        private void initData() 
        {
            this.studentList.ItemsSource = students;
            rtc.SetLabel(this.netInfo);
            rtc.SetImage(this.netState);
            rtc.SetVedioImage(this.videoState);
            rtc.SetVedioLabel(this.videoInfo);
            rtc.Start();
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

        private void AdminLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CacheService.Instance.AdminToken == null)
            {
                AdminLogin adminLogin = new AdminLogin();
                adminLogin.Show();
                adminLogin.Owner = this;
                this.Hide();
            }
            else
            {
                ConfigWindow configWindow = new ConfigWindow();
                configWindow.Show();
                configWindow.Owner = this;
                this.Hide();
            }
        }

        private void RemoveLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;
            foreach (Student student in students)
            {
                if (student.Number == label.Tag.ToString())
                {
                    this.students.Remove(student);
                    CacheService.Instance.DeleteStuList(student);
                    break;
                }
            }
            this.studentList.Items.Refresh();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (CacheService.Instance.LaunchPath == null || CacheService.Instance.LaunchPath.Equals(""))
            {
                LSMessageBox.Show("启动异常","没配置启动路径");
                return;
            }
            List<UserRecord> records = new List<UserRecord>();
            foreach (Student s in CacheService.Instance.GetStudentList())
            {
                UserRecord ur = new UserRecord
                {
                    clazzId = s.ClassId,
                    experimentId = CacheService.Instance.ExperimentId,
                    studentId = s.Id,
                    labId = CacheService.Instance.Lab.id,
                    machineId = CacheService.Instance.MachineId,
                    slotId = 1
                };
                records.Add(ur);
            }
            rcf.GetRecord(CacheService.Instance.ExperimentId, records, CacheService.Instance.GetStuToken());
            launch(CacheService.Instance.LaunchPath);
            ProcessingWindow processingWindow = new ProcessingWindow();
            processingWindow.Show();
            processingWindow.Owner = this;
            this.Hide();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            LoginResult lr = af.Login(this.number.Text, this.password.Password);
            if (lr != null)
            {
                if (lr.code == "200")
                {
                    if (AccountUtil.IsRole(AccountUtil.STUDENT, lr.data.roles))
                    {
                        Student student = new Student() { Number = lr.data.account, Name = lr.data.name, Token=lr.token, Id = lr.data.id};
                        ReservationResult rr = rf.GetValidity(CacheService.Instance.Lab.id, student.Token);
                        if (rr != null)
                        {
                            if (rr.code == "200")
                            {
                                try
                                {
                                    if (rr.data.clazz == null)
                                    {
                                        student.ClassId = -1;
                                        CacheService.Instance.AddStuList(student);
                                        students.Add(student);
                                        this.studentList.Items.Refresh();
                                    }
                                    else
                                    {
                                        student.ClassId = rr.data.clazz.id;
                                        //LSMessageBox.Show("班级ID", rr.data.clazz.id+"");
                                        CacheService.Instance.AddStuList(student);
                                        students.Add(student);
                                        this.studentList.Items.Refresh();
                                    }
                                }
                                catch (ArgumentException)
                                {
                                    LSMessageBox.Show("登陆异常", "已经存在该学生");
                                }

                                this.experiment.Content = rr.data.experiment.name;
                                CacheService.Instance.ExperimentId = rr.data.experiment.id;
                                this.virtualExp = rr.data.experiment.virtual_exp_link;
                                if (rr.data.experiment.virtual_exp_link == null || rr.data.experiment.virtual_exp_link == "")
                                {
                                    this.virtualexp.IsEnabled = false;
                                }
                                else
                                {
                                    this.virtualexp.IsEnabled = true;
                                }
                            }
                            else
                            {
                                LSMessageBox.Show("登陆异常", rr.message);
                            }
                        }
                        else
                        {
                            LSMessageBox.Show("网络错误", "网络异常");
                        }
                    }
                    else
                    {
                        LSMessageBox.Show("登录异常","当前角色不是学生");
                    }
                    this.number.Clear();
                    this.password.Clear();
                }
                else
                {
                    LSMessageBox.Show("登录错误", lr.message);
                }
            }
            else
            {
                LSMessageBox.Show("网络错误", "网络异常");
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            this.students.Clear();
            this.studentList.Items.Refresh();
            CacheService.Instance.ClearStudentList();
        }

        private void EnterNotLogin_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CacheService.Instance.LaunchPath == null || CacheService.Instance.LaunchPath.Equals(""))
            {
                LSMessageBox.Show("启动异常", "没配置启动路径");
                return;
            }
            launch(CacheService.Instance.LaunchPath);
            CacheService.Instance.ClearStudentList();
            ProcessingWindow processingWindow = new ProcessingWindow();
            processingWindow.Show();
            processingWindow.Owner = this;
            this.Hide();
        }

        private void VirtualExp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"http\shell\open\command\");
            String path = key.GetValue("").ToString();
            if (path.Contains("\""))
            {
                path = path.TrimStart('"');
                path = path.Substring(0, path.IndexOf('"'));
            }
            key.Close();
            System.Diagnostics.Process.Start(path, this.virtualExp);
        }

        private void launch(String path)
        {
            Process p = new Process();
            p.StartInfo.FileName = path;
            try
            { 
                p.Start();
            }
            catch (Win32Exception err)
            {
                LSMessageBox.Show("启动异常",err.Message);
            }
        }

    }
}
