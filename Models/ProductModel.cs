using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

namespace ElectrosLtdApplication.Models
{
    public class ProductModel
    {
        public ProductModel()
        {
             
           
        }


        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Features { get; set; }
        public string Image { get; set; }
        public int Stock { get; set; }
        public DateTime DateListed { get; set; } 
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

       

        public virtual Category Category { get; set; }
        public virtual Review Review { get; set; }
        public virtual Product Product { get; set; }
    }
}