using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComputerStore.Models;

namespace ComputerStore.ViewModel
{
    public class OrdersVM
    {
        public Orders order { get; set; }

        public List<Orders> ordersList { get; set; }
    }
}