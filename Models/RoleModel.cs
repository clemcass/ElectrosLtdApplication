using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Common;

namespace ElectrosLtdApplication.Models
{
    public class RoleModel
    {
        public int RoleId { get; set; }
        public string Name { get; set; }

        public virtual User User{get; set;}
        

    }
}