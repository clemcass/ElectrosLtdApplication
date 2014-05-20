using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ElectrosLtdApplication.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Logo = "Electros Ltd";
           
            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
