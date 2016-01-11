using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Result
{
    class LabResult
    {
        public String code { get; set; }

        public String message { get; set; }

        public List<Lab> data { get; set; }

        public bool success { get; set; }

        public class Lab
        {
            public int id { get; set; }

            public String number { get; set; }

            public String name { get; set; }

            public int capacity { get; set; }

            public bool active { get; set; }
        }
    }
}
