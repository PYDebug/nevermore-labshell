using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Net;
using Labshell.Util;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Net.NetworkInformation;

namespace Labshell.Service
{
    class RealTimeCheck
    {
        private Label label;

        private Action<Label, String> updateLabelAction;

        private Action<Image, BitmapImage> updateImageAction;

        BitmapImage good = new BitmapImage(new Uri(@"images/ic-good.png",UriKind.Relative));

        BitmapImage exception = new BitmapImage(new Uri(@"images/ic-exception.png", UriKind.Relative));

        private Image image;

        private bool netStatus;

        private Task t;

        public RealTimeCheck()
        {
            netStatus = true;
            updateLabelAction = new Action<Label, string>(UpdateNetInfo);
            updateImageAction = new Action<Image, BitmapImage>(UpdateNetImage);
        }

        public void Start()
        {
            t = Task.Factory.StartNew(Check);
        }

        private void Check()
        {
            while (true)
            {
                try
                {
                    PingReply pr;
                    Ping ping = new Ping();
                    pr = ping.Send(ServerURL.PING_URL);
                    if (pr.Status == IPStatus.Success)
                    {
                        if (label != null && !this.netStatus)
                        {
                            label.Dispatcher.BeginInvoke(updateLabelAction, label, "网络正常");
                            this.image.Dispatcher.BeginInvoke(updateImageAction, image, good);
                            this.netStatus = true;
                        }
                    }
                    else
                    {
                        if (label != null && this.netStatus)
                        {
                            label.Dispatcher.BeginInvoke(updateLabelAction, label, "网络异常");
                            this.image.Dispatcher.BeginInvoke(updateImageAction, image, exception);
                            this.netStatus = false;
                        }
                    }
                }
                catch (Exception)
                {
                    if (label != null && this.netStatus)
                    {
                        label.Dispatcher.BeginInvoke(updateLabelAction, label, "网络异常");
                        this.image.Dispatcher.BeginInvoke(updateImageAction, image, exception);
                        this.netStatus = false;
                    }
                }

                Thread.Sleep(5000);
            }
        }

        private void UpdateNetImage(Image i, BitmapImage bt)
        {
            i.Source = bt;
        }

        private void UpdateNetInfo(Label l, String content)
        {
            l.Content = content;
        }

        public void SetLabel(Label l)
        {
            this.label = l;
        }

        public void SetImage(Image i)
        {
            this.image = i;
        }

        public void SetNetStatus(bool i)
        {
            this.netStatus = i;
        }
    }
}
