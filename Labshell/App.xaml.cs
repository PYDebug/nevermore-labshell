using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Labshell.Factory;
using Labshell.Service;
using Labshell.Result;
using Labshell.Model;

namespace Labshell
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private MachineFactory mf = new MachineFactory();

        protected override void OnStartup(StartupEventArgs e)
        {
            MachineResult mr = mf.GetMachine(CacheService.GetMac());
            if (mr != null)
            {
                if (mr.code == "200")
                {
                    if (mr.data != null)
                    {
                        CacheService.SetMac(mr.data.macAddress);
                        CacheService.SetLab(mr.data.labId);
                        CacheService.SetLaunchPath(mr.data.launchPath);
                        foreach (String p in mr.data.listenPath)
                        {
                            CacheService.AddListenPath(new ListenPath { Path = p });
                        }
                    }
                }
                else if(mr.code == "402")
                {
                    LSMessageBox.Show("提示","当前机器没有配置");
                }
            }
            else
            {
                LSMessageBox.Show("网络错误","网络异常");
                
            }
            base.OnStartup(e);
        }
    }
}
