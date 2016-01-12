﻿using System;
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
using Labshell.Util;

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
            MachineResult mr = mf.GetMachine(MachineUtil.GetMac());
            if (mr != null)
            {
                if (mr.code == "200")
                {
                    if (mr.data != null)
                    {
                        //CacheService.SetMachineId(mr.data.id);
                        //CacheService.SetMac(mr.data.macAddress);
                        //CacheService.SetLab(mr.data.labId);
                        //CacheService.SetLaunchPath(mr.data.launchPath);
                        //foreach (String p in mr.data.listenPath)
                        //{
                        //    CacheService.AddListenPath(new ListenPath { Path = p });
                        //}
                        CacheService.SetMachineConf(mr);
                    }
                }
                else if(mr.code == "402")
                {
                    LSMessageBox.Show("提示","当前机器没有配置");
                }
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                LSMessageBox.Show("网络错误", "获取机器配置错误，请确认网络连接并重新打开");
                this.Shutdown();
                //MainWindow mainWindow = new MainWindow();
                //mainWindow.Show();
            }
            base.OnStartup(e);
        }
    }
}
