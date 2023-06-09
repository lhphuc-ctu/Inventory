﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Core.Enums
{
    public enum TicketStatus
    {
        Pending = 1,
        Processing = 2,
        Done = 3, 
        Reject = 4
    }

    public enum TicketPMStatus
    {
        Pending = 1,
        Approve = 2,
        Reject = 3
    }
}
