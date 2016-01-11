using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.JsonForm
{
    class Machine
    {
        public String macAddress { get; set; }

        public int labId { get; set; }

        public String launchPath { get; set; }

        public List<String> listenPath { get; set; }
    }
}
