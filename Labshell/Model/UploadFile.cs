using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Model
{
    public class UploadFile
    {
        public static String EXPERIMENT = "实验产生文件";

        public static String PHOTO = "实验摄像图";

        public static String SUCCESS = "上传成功";

        public static String WAIT = "未上传";

        public String FileName { get; set; }

        public String FileType { get; set; }

        public String Status { get; set; }

    }
}
