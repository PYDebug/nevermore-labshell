using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Util
{
    public class MachineUtil
    {
        public static String GetMac()
        {
            String mac;
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
