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
using Labshell.Factory;
using Labshell.Service;
using Labshell.Result;
using Labshell.Util;

namespace Labshell
{
    /// <summary>
    /// ConfigWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ConfigWindow : Window
    {
        private List<Lab> labs = new List<Lab>();
        private List<ListenPath> paths = new List<ListenPath>();

        private LabFactory lf = new LabFactory();

        private MachineFactory mf = new MachineFactory();

        private RealTimeCheck rtc = new RealTimeCheck();

        public ConfigWindow()
        {
            InitializeComponent();
            initData();
        }

        private void initData()
        {
            rtc.SetLabel(this.netInfo);
            rtc.SetImage(this.netState);
            rtc.Start();

            List<Lab> lablist = lf.AllLab();
            if (lablist != null)
            {
                labs = lablist;
            }
            else
            {
                LSMessageBox.Show("网络错误","网络异常");
            }

            this.labList.ItemsSource = labs;
            this.labList.DisplayMemberPath = "Name";
            this.labList.SelectedValuePath = "Id";

            this.pathList.ItemsSource = paths;

            if (CacheService.GetLab() != -1)
            {
                int labindex = -1;
                int labid = CacheService.GetLab();
                foreach (Lab lab in labs)
                {
                    labindex++;
                    if (lab.Id == labid)
                        break;
                }

                if (labindex == -1)
                {
                    labindex = 0;
                }
                this.labList.SelectedIndex = labindex;
            }

            if (CacheService.GetLaunchPath() != null)
            {
                this.pathText.Text = CacheService.GetLaunchPath();
            }

            if (CacheService.GetListenPath().Count != 0)
            {
                foreach(ListenPath lp in CacheService.GetListenPath())
                {
                    this.paths.Add(lp);
                }
                this.pathList.Items.Refresh();
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

        private void LabList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.labName.Content = this.labs[this.labList.SelectedIndex].Name;
            this.labNumber.Content = this.labs[this.labList.SelectedIndex].Number;
            this.machineNumber.Content = this.labs[this.labList.SelectedIndex].MachineNumber;
            this.studentNumber.Content = this.labs[this.labList.SelectedIndex].StudentNumber;
        }

        private void RemoveButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Button btn = sender as Button;
            foreach (ListenPath path in paths)
            {
                if (path.Path.Equals(btn.Tag.ToString()))
                {
                    paths.Remove(path);
                    break;
                }
            }
            this.pathList.Items.Refresh();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            this.Owner.Show();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            string filter = "启动文件(*.exe)|*.exe";
            System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();
            fd.Title = "请选择启动项";
            fd.Filter = filter;
            fd.FileName = this.pathText.Text.Trim();

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.pathText.Text = fd.FileName;
            }
        }

        private void OpenBrowser(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            
            if(fbd .ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ListenPath lp = new ListenPath() { Path = fbd.SelectedPath };
                this.paths.Add(lp);
                this.pathList.Items.Refresh();
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CacheService.GetMachineId() == -1)
            {
                List<String> lpath = new List<String>();
                foreach (ListenPath lp in paths)
                {
                    lpath.Add(lp.Path);
                }
                MachineResult mr = mf.AddMachine(MachineUtil.GetMac(), (int)this.labList.SelectedValue, this.pathText.Text.ToString(), lpath);
                if (mr != null)
                {
                    if (mr.code == "200")
                    {
                        LSMessageBox.Show("提示", "配置成功");
                        CacheService.SetMachineConf(mr);
                        this.Close();
                        this.Owner.Show();
                    }
                    else
                    {
                        LSMessageBox.Show("配置异常",mr.message);
                    }
                }
                else
                {
                    LSMessageBox.Show("网络错误", "网络异常");
                }
            }
            else
            {
                List<String> lpath = new List<String>();
                foreach (ListenPath lp in paths)
                {
                    lpath.Add(lp.Path);
                }
                MachineResult mr = mf.UpdateMachine(CacheService.GetMachineId(), MachineUtil.GetMac(), (int)this.labList.SelectedValue, this.pathText.Text.ToString(), lpath);
                if (mr != null)
                {
                    if (mr.code == "200")
                    {
                        LSMessageBox.Show("提示", "更新成功");
                        CacheService.SetMachineConf(mr);
                        this.Close();
                        this.Owner.Show();
                    }
                    else 
                    {
                        LSMessageBox.Show("配置异常", mr.message);
                    }
                }
                else
                {
                    LSMessageBox.Show("网络错误", "网络异常");
                }
            }
        }

        
    }
}
