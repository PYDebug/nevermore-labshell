﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labshell.JsonForm
{
    class UserRecord
    {
        public int clazzId { get; set; }

        public int experimentId { get; set; }

        public List<int> stuIds { get; set; }

        public int slotId { get; set; }

        public int labId { get; set; }

        public int machineId { get; set; }
    }
}
