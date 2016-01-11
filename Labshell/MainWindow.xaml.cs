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

namespace Labshell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Student> students = new List<Student>();

        private AccountFactory af = new AccountFactory();

        public MainWindow()
        {
            InitializeComponent();
            initData();
        }

        private void initData() 
        {
            this.studentList.ItemsSource = students;
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
                    Student student = new Student() { Number = lr.data.account, Name = lr.data.name };
                    students.Add(student);
                    this.studentList.Items.Refresh();
                    CacheService.AddStuList(student);
                }
                else if (lr.code == "801")
                {
                    LSMessageBox.Show("登录错误", "用户名或密码错误");
                }
            }
            else
            {
                LSMessageBox.Show("网络错误", "网络异常");
            }
        }
    }
}
