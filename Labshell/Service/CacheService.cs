using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labshell.Model;
using System.Management;
using Labshell.Result;
using System.ComponentModel;

namespace Labshell.Service
{
    class CacheService : INotifyPropertyChanged
    {
        private static readonly CacheService instance = new CacheService();
        private CacheService() { }

        public static CacheService Instance
        {
            get
            {
                return instance;
            }
        }

        //登录信息
        private String admin_token;

        private Dictionary<int, Student> stuList = new Dictionary<int, Student>();

        private DateTime loginTime;

        private int experimentId = 0;

        //机器配置信息
        private int machineId = -1;

        private string mac;

        private MachineResult.Lab lab;

        private String launchPath;

        private List<ListenPath> listenPath = new List<ListenPath>();

        public void SetMachineConf(MachineResult mr)
        {
            machineId = mr.data.id;
            mac = mr.data.macAddress;
            lab = mr.data.lab;
            launchPath = mr.data.launchPath;
            listenPath.Clear();
            foreach (String p in mr.data.listenPath)
            {
                listenPath.Add(new ListenPath { Path = p });
            }
            NotifyPropertyChanged("Lab");
        }

        public int MachineId 
        {
            get { return machineId; }
            set { machineId = value; }
        }

        public String AdminToken
        {
            get { return admin_token; }
            set { admin_token = value; }
        }

        public void AddStuList(Student stu)
        {
            stuList.Add(stu.Id,stu);
        }

        public void DeleteStuList(Student stu)
        {
            stuList.Remove(stu.Id);
        }

        public List<Student> GetStudentList()
        {
            List<Student> students = new List<Student>();
            foreach (Student s in stuList.Values)
            {
                students.Add(s);
            }
            return students;
        }

        public Student GetStudent(int id)
        {
            return stuList[id];
        }

        public void ClearStudentList()
        {
            stuList.Clear();
        }

        public void AddListenPath(ListenPath lp)
        {
            listenPath.Add(lp);
        }

        public void RemoveListenPath(String path)
        {
            foreach (ListenPath lp in listenPath)
            {
                if (lp.Path.Equals(path))
                {
                    listenPath.Remove(lp);
                    break;
                }
            }
        }

        public List<ListenPath> GetListenPath()
        {
            return listenPath;
        }

        public String LaunchPath
        {
            get { return launchPath; }
            set { launchPath = value; }
        }

        public MachineResult.Lab Lab
        {
            get { return lab; }
            set 
            { 
                lab = value;
                NotifyPropertyChanged("Lab");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public String Mac
        {
            get { return mac; }
            set { mac = value; }
        }

        public DateTime LoginTime
        {
            get { return loginTime; }
            set { loginTime = value; }
        }

        public String GetStuToken()
        {
            foreach (Student s in stuList.Values)
            {
                return s.Token;
            }
            return null;
        }

        public int ExperimentId 
        {
            get { return experimentId; }
            set { experimentId = value; }
        }
    }
}
