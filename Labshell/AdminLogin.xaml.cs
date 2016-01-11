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
using Labshell.Factory;
using Labshell.Result;
using Labshell.Model;
using Labshell.Service;

namespace Labshell
{
    /// <summary>
    /// AdminLogin.xaml 的交互逻辑
    /// </summary>
    public partial class AdminLogin : Window
    {
        private AccountFactory af = new AccountFactory();

        public AdminLogin()
        {
            InitializeComponent();
            initData();
        }

        private void initData()
        {
            
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

        private void BackLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Owner.Show();
            this.Close();
        }

        private void EnterButton_Click(object sender, RoutedEventArgs e)
        {
            LoginResult lr = af.Login(this.number.Text, this.password.Password);
            if (lr != null)
            {
                if (lr.code == "200")
                {
                    CacheService.SetAdminToken(lr.token);
                    ConfigWindow configWindow = new ConfigWindow();
                    configWindow.Show();
                    configWindow.Owner = this.Owner;
                    this.Close();
                }
                else
                {
                    LSMessageBox.Show("登录错误", lr.message);
                }
            }
            else
            {
                LSMessageBox.Show("网络错误","网络异常");
            }
        }
    }
}
