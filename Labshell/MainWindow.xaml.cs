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

namespace Labshell
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Student> students = new List<Student>();

        public MainWindow()
        {
            InitializeComponent();
            initData();
        }

        private void initData() 
        {
            this.number.WateMark = "请输入学号";
            this.password.WateMark = "请输入密码";
            Student student1 = new Student() { Number = "1435846", Name = "潘岩"};
            students.Add(student1);
            Student student2 = new Student() { Number = "091116", Name = "郭意亮" };
            students.Add(student2);
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
            AdminLogin adminLogin = new AdminLogin();
            adminLogin.Show();
            adminLogin.Owner = this;
            this.Hide();
        }

        private void RemoveLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;
            MessageBox.Show(label.Tag.ToString());
        }
    }
}
