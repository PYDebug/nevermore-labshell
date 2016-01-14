using Labshell.Factory;
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

        private Action<ListBox, UploadFile> updateListBoxAction;

        private RecordFactory rf = new RecordFactory();

        List<FileSystemWatcher> watchers = new List<FileSystemWatcher>();

        public ListenService()
        {
            updateListBoxAction = new Action<ListBox, UploadFile>(UpdateListBox);
        }

        private void WatchCreated(object sender, FileSystemEventArgs e)
        {
            FileInfo fi = new FileInfo(e.FullPath);
            FileResult fr = rf.UploadFile(fi.FullName, CacheService.Instance.GetStuToken());
            if (fr != null)
            {
                if (fr.code == "200")
                {
                    UploadFile up = new UploadFile() { FileName = fi.Name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.SUCCESS, FilePath = fi.FullName, Id = fr.data.id };
                    this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                }
                else
                {
                    UploadFile up = new UploadFile() { FileName = fi.Name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.FAIL, FilePath = fi.FullName, Id = -1 };
                    this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
                    System.Windows.MessageBox.Show(fr.message);
                }
            }
            else
            {
                UploadFile up = new UploadFile() { FileName = fi.Name, FileType = UploadFile.EXPERIMENT, Status = UploadFile.FAIL, FilePath = fi.FullName, Id = -1 };
                this.cb.Dispatcher.BeginInvoke(updateListBoxAction, this.cb, up);
            }
        }

        public void SetPaths(List<ListenPath> lp)
        {
            foreach (ListenPath path in lp)
            {
                FileSystemWatcher watcher = new FileSystemWatcher();
                watcher.BeginInit();
                watcher.Path = path.Path;
                watcher.EnableRaisingEvents = true;
                watcher.Filter = "*.txt";
                watcher.Created += new FileSystemEventHandler(WatchCreated);
                watcher.EndInit();
                watchers.Add(watcher);
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
