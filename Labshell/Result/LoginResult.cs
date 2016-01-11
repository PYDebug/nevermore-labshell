﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Result
{
    public class LoginResult
    {
        public String code { get; set; }

        public String message { get; set;}

        public Account data { get; set; }

        public String token { get; set; }

        public class Account
        {
            public String account { get; set; }

            public String name { get; set; }
        }
    }
}