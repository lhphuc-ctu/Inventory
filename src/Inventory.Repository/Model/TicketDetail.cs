﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Repository.Model
{
    public class TicketDetail
    {
        public Guid TicketId { get; set; }
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
    }
}