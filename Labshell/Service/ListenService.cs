using Labshell.Factory;
using Labshell.JsonForm;
using Labshell.Model;
using Labshell.Result;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Labshell.Service
{
    class ListenService
    {
        private ListBox cb;

        private String type;

        private List<String> filter = new List<String>();

        private String fileType;

        private Action<ListBox, UploadFile> updateListBoxAction;

        private RecordFactory rf = new RecordFactory();

        List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();

        public ListenService(String type)
        {
            updateListBoxAction = new Action<ListBox, UploadFile>(UpdateListBox);
            this.type = type;
            if (type.Equals("EXPERIMENT_RECORD_FILE"))
            {
                fileType = UploadFile.EXPERIMENT;
            }
            else if (type.Equals("EXPERIMENT_RECORD_IMAGE"))
            {
                fileType = UploadFile.PHOTO;
            }
            else
            {
                fileType = UploadFile.EXTRA;
            }
        }

        private void WatchCreated(object sender, FileSystemEventArgs e)
        {
            Thread.Sleep(3000);
            FileInfo fi = new FileInfo(e.FullPath);
            FileResult fr = rf.UploadFile(fi.FullName, CacheService.Instance.GetStuToken());
            if (fr != null)
            {
                if (fr.code == "200")
                {
                    //关联文件与记录
                    List<Attach> attaches = new List<Attach>();
                    foreach (Student s in CacheService.Instance.GetStudentList())
                    {
                        Attach a = new Attach { subjectId = s.RecordId, ownerId = s.Id, type = type};
                        attaches.Add(a);
                    }

                    AttachResult ar = rf.AttachRecordWithFile(CacheService.Instance.ExperimentId, fr.data.id, attaches, CacheService.Instance.GetStuToken());

                    if (ar != null)
                    {
                        if (ar.code == "200")
                        {
                            UploadFile up = new UploadFile() { FileName = fi.Name, FileType = fileType, Status = UploadFile.SUCCESS, FilePath = fi.FullName, Id = fr.data.id, Color = "#FF979797", Operation = UploadFile.OPENDOC };
                            this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                        }
                        else
                        {
                            UploadFile up = new UploadFile() { FileName = fi.Name, FileType = fileType, Status = UploadFile.FAIL, FilePath = fi.FullName, Id = -1, Color="Red", Operation = UploadFile.REUPLOAD };
                            this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                        }
                    }
                    else
                    {
                        UploadFile up = new UploadFile() { FileName = fi.Name, FileType = fileType, Status = UploadFile.FAIL, FilePath = fi.FullName, Id = -1, Color = "Red", Operation = UploadFile.REUPLOAD };
                        this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                    }
                }
                else
                {
                    UploadFile up = new UploadFile() { FileName = fi.Name, FileType = fileType, Status = UploadFile.FAIL, FilePath = fi.FullName, Id = -1, Color = "Red", Operation = UploadFile.REUPLOAD };
                    this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                }
            }
            else
            {
                UploadFile up = new UploadFile() { FileName = fi.Name, FileType = fileType, Status = UploadFile.FAIL, FilePath = fi.FullName, Id = -1, Color = "Red", Operation = UploadFile.REUPLOAD };
                this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
            }
        }

        public void SetPaths(List<ListenPath> lp)
        {
            foreach (ListenPath path in lp)
            {
                foreach (String f in filter)
                {
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.BeginInit();
                    watcher.Path = path.Path;
                    watcher.EnableRaisingEvents = true;
                    watcher.Filter = f;
                    watcher.Created += new FileSystemEventHandler(WatchCreated);
                    watcher.EndInit();
                    watchers.Add(watcher);
                }
            }
        }

        public void SetFilter(List<String> f)
        {
            this.filter = f;
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
