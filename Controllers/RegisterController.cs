using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Common;
using ElectrosLtdApplication.Models;

namespace ElectrosLtdApplication.Controllers
{
    public class RegisterController : Controller
    {
        //


        public ActionResult Register()
        {
            try
            {
                ViewBag.Roles = new UserServ.UserServiceClient().getAllRoles();
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View();
            }

        }
        [HttpPost]
        public ActionResult Register(UserModel u, FormCollection c)
        {
            try
            {
                //first check for all validations like if username exist password exist, matching confirm password etcc...


                TempData["PasswordE"] = new UserServ.UserServiceClient().Encrypt(u.Password, u.Pin.ToString());


                var roles = new UserServ.UserServiceClient().getAllRoles();
                bool flag = false;
                foreach (Role r in roles)
                {
                    string val = c["Role" + r.RoleId];

                    if (val.Contains("true"))
                    {
                        flag = true;
                        //at least one checkbox either user or admin is selected
                    }

                }
                if (flag == false)
                {
                    ModelState.AddModelError("", "No role is selected");
                }
                else
                {
                    User user = new User();
                    //Add user according to user model
                    user.Name = u.Name;
                    user.Surname = u.Surname;
                    user.Address1 = u.Address1;
                    user.Address2 = u.Address2;
                    user.PostCode = u.PostCode;
                    user.Mobile = u.Mobile;
                    user.Email = u.Email;
                    user.DOB = u.DOB;
                    user.TownId = u.Town.TownId;
                    user.Username = u.Username;
                    user.Password = u.Password;
                    user.Pin = u.Pin;




                    //loop again for roles and for each role that is true add a user role
                    List<int> roleList = new List<int>();
                    foreach (Role r in roles)
                    {
                        string val = c["Role" + r.RoleId];

                        if (val.Contains("true"))
                        {
                            roleList.Add(r.RoleId);
                        }


                    }
                    int[] listOfRoles = roleList.ToArray();

                    new UserServ.UserServiceClient().AddUser(user, listOfRoles);
                    return RedirectToAction("Login", "UserAuthentication");

                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }

            ViewBag.Roles = new UserServ.UserServiceClient().getAllRoles();
            return View();
        }

        public static List<SelectListItem> GetDropDown()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            IEnumerable<Town> towns = new TownServ.TownServiceClient().getAllTowns().AsEnumerable();
            foreach (var town in towns)
            {
                ls.Add(new SelectListItem() { Text = town.Name, Value = town.TownId.ToString() });
            }
            return ls;
        }

        public static List<SelectListItem> GetCountry()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            IEnumerable<Country> countries = new CountryServ.CountryServiceClient().GetAllCountries().AsEnumerable();
            foreach (var country in countries)
            {
                ls.Add(new SelectListItem() { Text = country.Name, Value = country.CountryId.ToString() });
            }
            return ls;

        }


        [HttpPost]
        public JsonResult doesUserNameExist(string Username)
        {

            bool username = new UserServ.UserServiceClient().GetUsername(Username);

            return Json(username == false);
        }

        [HttpPost]
        public JsonResult doesEmailExist(string Email)
        {

            bool email = new UserServ.UserServiceClient().GetEmail(Email);

            return Json(email == false);
        }



    }
}
