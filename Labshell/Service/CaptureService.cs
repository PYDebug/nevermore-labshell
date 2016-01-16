using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;

namespace Labshell.Service
{
    class CaptureService
    {
        private List<int> task = new List<int>();

        private Task t;

        private VideoCaptureDevice device;

        private String save_path;

        private int device_id;

        public void Start()
        {
            device.NewFrame += new NewFrameEventHandler(videoSourcePlayer_NewFrame);
            t = Task.Factory.StartNew(Capture);
        }

        public void SetDevice(VideoCaptureDevice d)
        {
            this.device = d;
        }

        public void SetSavePath(String sp)
        {
            this.save_path = sp;
        }

        public void SetDeviceId(int id)
        {
            this.device_id = id;
        }

        private void videoSourcePlayer_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();

            if (task.Contains(0))
            {
                String timepath = DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString() + "_" + device_id;
                string img = this.save_path + "/" + timepath + ".jpg";
                bitmap.Save(img);
                task.Remove(0);
            }
        }

        public void AddTask()
        {
            this.task.Add(0);
        }

        private void Capture()
        {
            while (true)
            {
                Thread.Sleep(5000);
                try
                {
                    task.Add(0);
                }
                catch (Exception)
                { 

                }

                Thread.Sleep(600000);
            }
        }
    }
}
