using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ComputerStore.Models;

namespace ComputerStore.ViewModel
{
    public class inventoryVM
    {
            
        public Inventory item { get; set; }

        public List<Inventory> itemsList { get; set; }
    }
}
