using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labshell.Model;
using System.Management;
using Labshell.Result;

namespace Labshell.Service
{
    class CacheService
    {
        //登录信息
        private static String admin_token;

        private static Dictionary<String, Student> stuList = new Dictionary<String, Student>();

        private static DateTime loginTime;

        //机器配置信息
        private static int machineId = -1;

        private static string mac;

        private static MachineResult.Lab lab;

        private static String labName;

        private static String launchPath;

        private static List<ListenPath> listenPath = new List<ListenPath>();

        public static void SetMachineConf(MachineResult mr)
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
        }

        public static void SetMachineId(int id)
        {
            machineId = id;
        }

        public static int GetMachineId()
        {
            return machineId;
        }

        public static void SetAdminToken(String token)
        {
            admin_token = token;
        }

        public static String GetAdminToken()
        {
            return admin_token;
        }

        public static void AddStuList(Student stu)
        {
            stuList.Add(stu.Number,stu);
        }

        public static void DeleteStuList(Student stu)
        {
            stuList.Remove(stu.Number);
        }

        public static List<Student> GetStudentList()
        {
            List<Student> students = new List<Student>();
            foreach (Student s in stuList.Values)
            {
                students.Add(s);
            }
            return students;
        }

        public static void ClearStudentList()
        {
            stuList.Clear();
        }

        public static void AddListenPath(ListenPath lp)
        {
            listenPath.Add(lp);
        }

        public static void RemoveListenPath(String path)
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

        public static List<ListenPath> GetListenPath()
        {
            return listenPath;
        }

        public static void SetLaunchPath(String lp)
        {
            launchPath = lp;
        }

        public static String GetLaunchPath()
        {
            return launchPath;
        }

        public static void SetLab(MachineResult.Lab l)
        {
            lab = l;
        }

        public static MachineResult.Lab GetLab()
        {
            return lab;
        }

        public static void SetMac(String m)
        {
            mac = m;
        }

        public static string GetMac()
        {
            return mac;
        }

        public static void SetLoginTime(DateTime dt)
        {
            loginTime = dt;
        }

        public static DateTime GetLoginTime()
        {
            return loginTime;
        }

        public static String GetStuToken()
        {
            foreach (Student s in stuList.Values)
            {
                return s.Token;
            }
            return null;
        }

        public static String GetLabName()
        {
            return labName;
        }

        public static void SetLabName(String name)
        {
            labName = name;
        }
    }
}
