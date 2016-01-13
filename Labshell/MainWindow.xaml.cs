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

        private RealTimeCheck rtc = new RealTimeCheck();

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
            rtc.Start();
            if (CacheService.GetLab() != null)
            {
                this.lab.Content = CacheService.GetLab().name;
            }
            else
            {
                this.lab.Content = "未配置";
            }
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
            if (CacheService.GetAdminToken() == null)
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
                    CacheService.DeleteStuList(student);
                    break;
                }
            }
            this.studentList.Items.Refresh();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
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
                        Student student = new Student() { Number = lr.data.account, Name = lr.data.name, Token=lr.token};
                        ReservationResult rr = rf.GetValidity(CacheService.GetLab().id, student.Token);
                        if (rr.code == "200")
                        {
                            try
                            {
                                CacheService.AddStuList(student);
                                students.Add(student);
                                this.studentList.Items.Refresh();
                            }
                            catch (ArgumentException)
                            {
                                LSMessageBox.Show("登陆异常", "已经存在该学生");
                            }

                            this.experiment.Content = rr.data.name;
                        }
                        else
                        {
                            LSMessageBox.Show("登陆异常", rr.message);
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
            CacheService.ClearStudentList();
        }

    }
}
