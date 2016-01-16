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
using AForge.Video.DirectShow;

namespace Labshell.Service
{
    class RealTimeCheck
    {
        private Label label;

        private Label vedio_label;

        private Action<Label, String> updateLabelAction;

        private Action<Image, BitmapImage> updateImageAction;

        private Action<Label, String> updateVedioLabelAction;

        private Action<Image, BitmapImage> updateVedioImageAction;

        BitmapImage good = new BitmapImage(new Uri(@"images/ic-good.png",UriKind.Relative));

        BitmapImage exception = new BitmapImage(new Uri(@"images/ic-exception.png", UriKind.Relative));

        private Image image;

        private Image vedio_image;

        private bool netStatus;

        private bool vedio_status;

        private Task t;

        public RealTimeCheck()
        {
            netStatus = true;
            vedio_status = true;
            updateLabelAction = new Action<Label, String>(UpdateNetInfo);
            updateImageAction = new Action<Image, BitmapImage>(UpdateNetImage);
            updateVedioLabelAction = new Action<Label, String>(UpdateVedioInfo);
            updateVedioImageAction = new Action<Image, BitmapImage>(UpdateVedioImage);
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

                    FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
                    if (videoDevices.Count == 0 && this.vedio_status)
                    {
                        vedio_label.Dispatcher.BeginInvoke(updateVedioLabelAction, vedio_label, "摄像头异常");
                        this.vedio_image.Dispatcher.BeginInvoke(updateVedioImageAction, vedio_image, exception);
                        this.vedio_status = false;
                    }
                    else if(videoDevices.Count !=0 && !this.vedio_status)
                    {
                        vedio_label.Dispatcher.BeginInvoke(updateVedioLabelAction, vedio_label, "摄像头正常");
                        this.vedio_image.Dispatcher.BeginInvoke(updateVedioImageAction, vedio_image, good);
                        this.vedio_status = true;
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

        private void UpdateVedioImage(Image i, BitmapImage bt)
        {
            i.Source = bt;
        }

        private void UpdateVedioInfo(Label l, String content)
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

        public void SetVedioLabel(Label l)
        {
            this.vedio_label = l;
        }

        public void SetVedioImage(Image i)
        {
            this.vedio_image = i;
        }

        public void SetNetStatus(bool i)
        {
            this.netStatus = i;
        }
    }
}
