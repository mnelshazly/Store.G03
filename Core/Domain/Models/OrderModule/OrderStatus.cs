﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrderModule
{
    public enum OrderStatus
    {
        Pending  = 0,
        PaymentRecieved = 1,
        PaymentFailed = 2
    }
}
