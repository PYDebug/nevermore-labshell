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
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private List<Lab> labs = new List<Lab>();

        public ConfigWindow()
        {
            InitializeComponent();
            initData();
        }

        private void initData()
        {
            Lab lab1 = new Lab() { Id = 1, Name = "本部-扭转115", MachineNumber = 10, StudentNumber = 20 };
            Lab lab2 = new Lab() { Id = 2, Name = "本部-扭转117", MachineNumber = 20, StudentNumber = 40 };
            labs.Add(lab1);
            labs.Add(lab2);
            this.labList.ItemsSource = labs;
            this.labList.DisplayMemberPath = "Name";
            this.labList.SelectedValuePath = "Id";
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
    }
}
