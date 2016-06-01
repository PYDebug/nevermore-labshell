  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing;
using Labshell.Result;
using Labshell.JsonForm;
using Labshell.Model;
using Labshell.Factory;
using System.Windows.Controls;

namespace Labshell.Service
{
    class CaptureService
    {
        private List<int> task = new List<int>();

        private Task t;

        private VideoCaptureDevice device;

        private String save_path;

        private int device_id;

        private RecordFactory rf = new RecordFactory();

        private String fileType = UploadFile.PHOTO;

        private ListBox cb;

        private Action<ListBox, UploadFile> updateListBoxAction;

        public void Start()
        {
            updateListBoxAction = new Action<ListBox, UploadFile>(UpdateListBox);
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
                string img = this.save_path + "\\" + timepath + ".jpg";
                bitmap.Save(img);
                task.Remove(0);

                FileResult fr = rf.UploadFile(img, CacheService.Instance.GetStuToken());
                if (fr != null)
                {
                    if (fr.code == "200")
                    {
                        //关联文件与记录
                        List<Attach> attaches = new List<Attach>();
                        foreach (Student s in CacheService.Instance.GetStudentList())
                        {
                            Attach a = new Attach { subjectId = s.RecordId, ownerId = s.Id, type = "EXPERIMENT_RECORD_IMAGE" };
                            attaches.Add(a);
                        }

                        AttachResult ar = rf.AttachRecordWithFile(CacheService.Instance.ExperimentId, fr.data.id, attaches, CacheService.Instance.GetStuToken());

                        if (ar != null)
                        {
                            if (ar.code == "200")
                            {
                                UploadFile up = new UploadFile() { FileName = timepath + ".jpg", FileType = fileType, Status = UploadFile.SUCCESS, FilePath = img, Id = fr.data.id, Color = "#FF979797", Operation = UploadFile.OPENDOC };
                                this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                            }
                            else
                            {
                                UploadFile up = new UploadFile() { FileName = timepath + ".jpg", FileType = fileType, Status = UploadFile.FAIL, FilePath = img, Id = -1, Color = "Red", Operation = UploadFile.REUPLOAD };
                                this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                            }
                        }
                        else
                        {
                            UploadFile up = new UploadFile() { FileName = timepath + ".jpg", FileType = fileType, Status = UploadFile.FAIL, FilePath = img, Id = -1, Color = "Red", Operation = UploadFile.REUPLOAD };
                            this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                        }
                    }
                    else
                    {
                        UploadFile up = new UploadFile() { FileName = timepath + ".jpg", FileType = fileType, Status = UploadFile.FAIL, FilePath = img, Id = -1, Color = "Red", Operation = UploadFile.REUPLOAD };
                        this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                    }
                }
                else
                {
                    UploadFile up = new UploadFile() { FileName = timepath + ".jpg", FileType = fileType, Status = UploadFile.FAIL, FilePath = img, Id = -1, Color = "Red", Operation = UploadFile.REUPLOAD };
                    this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                }
            }
            GC.Collect();
        }

        public void AddTask()
        {
            this.task.Add(0);
        }

        private void Capture()
        {
            while (true)
            {
                Thread.Sleep(10000);
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

        public void SetListBox(ListBox cb)
        {
            this.cb = cb;
        }

        private void UpdateListBox(ListBox cb, UploadFile file)
        {
            List<UploadFile> list = (List<UploadFile>)cb.ItemsSource;
            list.Add(file);
            cb.Items.Refresh();
        }
    }
}
