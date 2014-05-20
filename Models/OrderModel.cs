using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace ElectrosLtdApplication.Models
{
    public class OrderModel
    {

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public DateTime Datetime { get; set; }
        public bool IsPending { get; set; }
        public bool IsShipped { get; set; }


        public decimal Total { get; set; }

        public virtual User Person { get; set; }
        public virtual ICollection<OrderItem> OrderItem { get; set; }
    }
}