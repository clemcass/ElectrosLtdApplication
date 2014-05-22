using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElectrosLtdApplication.Controllers
{
    public class ErrorController : Controller
    {
        //
        // GET: /Error/

        public ActionResult HttpError404(string message)
        {
            ViewBag.Error = "Page Not Found";
            ViewBag.Msg = message;
            return View("Error");
        }

        public ActionResult HttpError500(string message)
        {
            ViewBag.Error = "Server Error";
            ViewBag.Msg = message;
            return View("Error");
        }
        public ActionResult General(string message)
        {
            ViewBag.Error = "Error";
            ViewBag.Msg = message;
            return View("Error");
        }

    }
}
