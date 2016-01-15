using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.Result
{
    class ReservationResult
    {
        public String code { get; set; }

        public String message { get; set; }

        public bool success { get; set; }

        public Reservation data { get; set; }

        public class Experiment
        {
            public int id { get; set; }

            public String name { get; set; }

            public String virtual_exp_link { get; set; }
        }

        public class Class
        {
            public int id { get; set; }

            public String number { get; set; }

            public String clazzroom { get; set; }
        }

        public class Reservation
        {
            public Experiment experiment { get; set; }

            public Class clazz { get; set; }
        }
    }
}
