using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace ElectrosLtdApplication.Models
{
    public class OrderItemModel
    {

        public int OrderItemId { get; set; }
        public Nullable<int> Qty { get; set; }
        public int OrderId { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual Order Order { get; set; }


        public string Message { get; set; }

    }
}