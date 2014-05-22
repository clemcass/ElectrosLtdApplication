using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Common;
//models ref
using ElectrosLtdApplication.Models;
//ref to service
using ElectrosLtdApplication.UserServ;
//reererence to web security to access form authentication
using System.Web.Security;



namespace ElectrosLtdApplication.Controllers
{
    public class UserAuthenticationController : Controller
    {
        //
        // GET: /UserAuthentication/

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(UserModel data)
        {
            try
            {

                string pin = data.Pin.ToString();
                // string enteredPass = ;

                User user = new UserServ.UserServiceClient().getUserByUsername(data.Username);
                string passwordDec = new UserServ.UserServiceClient().Decrypt(data.Password, pin);

                if (passwordDec == user.Password && user != null)
                {
                    FormsAuthentication.RedirectFromLoginPage(data.Username, true);
                    return RedirectToAction("Index", "Home");


                }

                else
                {

                    return View();
                }
            }
            catch (Exception e)
            {


                return View();
            }
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }



    }
}
