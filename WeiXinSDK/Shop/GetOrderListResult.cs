﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WeiXinSDK.Shop
{
    public class GetOrderListResult:ReturnCode
    {
        public List<Order> order_list { get; set; }
    }
}
