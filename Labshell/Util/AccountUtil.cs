using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Labshell.Result;

namespace Labshell.Util
{
    public class AccountUtil
    {
        public static String ADMIN="ADMIN";

        public static String STUDENT="STUDENT";

        public static String TEACHER="TEACHER";

        public static bool IsRole(String code, List<LoginResult.RoleItem> items)
        {
            foreach (LoginResult.RoleItem ri in items)
            {
                if (ri.name.code.Equals(code))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
