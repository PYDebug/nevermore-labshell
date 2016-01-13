using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Result
{
    class MachineResult
    {
        public String code { get; set; }

        public String message { get; set; }

        public bool success { get; set; }

        public Machine data { get; set; }

        public class Machine
        {
            public int id { get; set; }

            public Lab lab { get; set; }

            public String launchPath { get; set; }

            public String macAddress { get; set; }

            public List<String> listenPath { get; set; }
        }

        public class Lab
        {
            public int id { get; set; }

            public String name { get; set; }
        }
    }
}
