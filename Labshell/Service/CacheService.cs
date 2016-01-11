using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labshell.Model;
using System.Management;

namespace Labshell.Service
{
    class CacheService
    {
        private static String admin_token;

        private static Dictionary<String, Student> stuList = new Dictionary<String, Student>();

        private static string mac;

        private static int labId = -1;

        private static String launchPath;

        private static List<ListenPath> listenPath = new List<ListenPath>();

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

        public static void SetLab(int id)
        {
            labId = id;
        }

        public static int GetLab()
        {
            return labId;
        }

        public static void SetMac(String m)
        {
            mac = m;
        }

        public static string GetMac()
        {
            if (mac != null)
            {
                return mac;
            }
            else
            {
                string MoAddress = string.Empty;
                try
                {
                    ManagementClass networkAdapter = new ManagementClass("Win32_NetworkAdapterConfiguration");
                    ManagementObjectCollection adapterC = networkAdapter.GetInstances();
                    foreach (ManagementObject m in adapterC)
                    {
                        if ((bool)m["IPEnabled"] == true)
                        {
                            MoAddress = m["MacAddress"].ToString().Trim();
                            m.Dispose();
                        }
                    }
                    mac = MoAddress;
                    return mac;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
