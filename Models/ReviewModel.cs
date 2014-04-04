using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Common;

namespace ElectrosLtdApplication.Models
{
    public class ReviewModel
    {
        public int ReviewId { get; set; }
        public string ReviewComment { get; set; }
        public int ProuctId { get; set; }
        public int UserId { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }


    }
}