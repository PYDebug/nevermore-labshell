using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Result
{
    public class RecordResult
    {
        public String code { get; set; }

        public String message { get; set; }

        public List<Record> data { get; set; }

        public bool success { get; set; }

        public class Record
        {
            public int id { get; set; }

            public Status status { get; set; }

            public int classId { get; set; }

            public int experimentId { get; set; }

            public int studentId { get; set; }

            public int labId { get; set; }

            public int machineId { get; set; }

            public int slotId { get; set; }

            public class Status
            {
                public String code { get; set; }

                public String value { get; set; }
            }
        }
    }
}
