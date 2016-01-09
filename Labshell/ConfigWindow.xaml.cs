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
using Labshell.Command;
using Microsoft.Win32;

namespace Labshell
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private List<Lab> labs = new List<Lab>();
        private List<ListenPath> paths = new List<ListenPath>();
        public OpenFileCommand openFileCommand {get; set;}

        public ConfigWindow()
        {
            InitializeComponent();
            initData();
        }

        private void initData()
        {
            Lab lab1 = new Lab() { Id = 1, Name = "本部-扭转115", ExperimentName="扭转实验", MachineNumber = 10, StudentNumber = 20 };
            Lab lab2 = new Lab() { Id = 2, Name = "本部-扭转117", ExperimentName="扭转实验", MachineNumber = 20, StudentNumber = 40 };
            labs.Add(lab1);
            labs.Add(lab2);
            this.labList.ItemsSource = labs;
            this.labList.DisplayMemberPath = "Name";
            this.labList.SelectedValuePath = "Id";

            ListenPath listen1 = new ListenPath() { Path = "test" };
            paths.Add(listen1);
            this.pathList.ItemsSource = paths;
            this.DataContext = this;
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

        private void LabList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.labName.Content = this.labs[this.labList.SelectedIndex].Name;
            this.expName.Content = this.labs[this.labList.SelectedIndex].ExperimentName;
            this.machineNumber.Content = this.labs[this.labList.SelectedIndex].MachineNumber;
            this.studentNumber.Content = this.labs[this.labList.SelectedIndex].StudentNumber;
        }

        private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            string filter = "启动文件(*.exe)|*.exe";
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "请选择启动项";
            fd.Filter = filter;
            fd.FileName = this.pathText.Text.Trim();

            if (fd.ShowDialog() == true)
            {
                this.pathText.Text = fd.FileName;
            }
        }

        
    }
}
