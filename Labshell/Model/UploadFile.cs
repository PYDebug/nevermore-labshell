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

        public static String EXTRA = "其他";

        public static String SUCCESS = "上传成功";

        public static String WAIT = "未上传";

        public static String FAIL = "上传失败";

        public static String OPENDOC = "打开文件夹";

        public static String REUPLOAD = "重新上传";

        public String FileName { get; set; }

        public String FileType { get; set; }

        public String Status { get; set; }

        public String FilePath { get; set; }

        public int Id { get; set; }

        public String Color { get; set; }

        public String Operation { get; set; }
    }
}
