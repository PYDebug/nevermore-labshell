using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Result
{
    class FileResult
    {
        public String code { get; set; }

        public String message { get; set; }

        public bool success { get; set; }

        public File data { get; set; }

        public class File
        {
            public int id { get; set; }

            public String url { get; set; }

            public String name { get; set; }

            public int size { get; set; }

            public AttachType attachType { get; set; }

            public MetaData metaData { get; set; }

            public int accountId { get; set; }

            public bool isDel { get; set; }
        }

        public class AttachType
        {
            public String code { get; set; }

            public String value { get; set; }
        }

        public class MetaData
        {
            public String extension { get; set; }

            public String contentType { get; set; }
        }
    }
}
