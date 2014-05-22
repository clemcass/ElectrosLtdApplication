using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using ElectrosLtdApplication.Models;
using System.Net.Mail;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;

namespace ElectrosLtdApplication.Controllers
{
    public class UpdateFaultController : Controller
    {
        //
        // GET: /UpdateFault/
        [Authorize(Roles = "Admin")]
        public ActionResult ListAllFaults()
        {
            List<Fault> faults = new FaultServ.FaultServiceClient().GetAllFaults().ToList();

            List<SelectListItem> ls = new List<SelectListItem>();
            List<String> types = new List<string>();

            types.Add("Date ASC");
            types.Add("Date DES");



            foreach (var type in types)
            {
                ls.Add(new SelectListItem() { Text = type, Value = type });
            }
            ViewBag.ddl = ls;


            return View(faults);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public PartialViewResult FilterFault(string from, string to, string clientId, string faultId, string sort , string category)
        {
            List<FaultTrack> faultTrack  = new FaultTrackServ.FaultTrackServiceClient().GetAllFaultTracks().ToList();

            try
            {
                int? client = null;
                int? fault = null;

                if (clientId != "Select")
                {
                    client = Int32.Parse(clientId);
                }
                if (faultId != "Select")
                {
                    fault = Int32.Parse(faultId);
                }

                DateTime startDate;
                DateTime endDate;
                if (DateTime.TryParse(from, out startDate))
                {
                    if (DateTime.TryParse(to, out endDate))
                    {
                        if (endDate > startDate)
                        {

                            faultTrack = new FaultTrackServ.FaultTrackServiceClient().Filter(client, fault, startDate, endDate).ToList();
                        }
                    }

                }

                 if (sort == "Date ASC")
                {
                    faultTrack = faultTrack.OrderBy(t => t.Date).ToList();
                }
                else if (sort == "Date DES")
                {
                    faultTrack = faultTrack.OrderByDescending(t => t.Date).ToList();
                }

                if (category != "Select" && category != "")
                            {
                                int categoryId = Int32.Parse(category);

                               faultTrack = faultTrack.Where(c => c.Fault.OrderItem.Product.CategoryId == categoryId).ToList();

                            }


               return PartialView("_FilterListAllFaultTrack", faultTrack);
              //  return View("ListAllFaultTrack", faultTrack);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Sorry there seems to be a problem"; 
                return PartialView("_FilterListAllFaultTrack", faultTrack);
            }
        }

        

        


       [Authorize(Roles = "Admin")]
       
        public ActionResult ListAllFaultTrack()
        {
            List<FaultTrack> faults = new List<FaultTrack>();
           
            try
            {
                faults = new FaultTrackServ.FaultTrackServiceClient().GetAllFaultTracks().ToList();
              

                List<SelectListItem> ls = new List<SelectListItem>();
                List<String> types = new List<string>();

                //types.Add("Price ASC");
                //types.Add("Price DES");
                types.Add("Date ASC");
                types.Add("Date DES");



                foreach (var type in types)
                {
                    ls.Add(new SelectListItem() { Text = type, Value = type });
                }
                ViewBag.ddl = ls;


                List<SelectListItem> category = new ProductServ.ProductServiceClient().GetAllCategory().Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.Name.ToString(),
                    Selected = false
                }).ToList();


                ViewBag.Category = category;

                List<SelectListItem> users = new UserServ.UserServiceClient().GetAllUsers().Select(c => new SelectListItem
            {
                Value = c.UserId.ToString(),
                Text = c.UserId.ToString(),
                Selected = false
            }).ToList();


                ViewBag.Users = users;



                List<SelectListItem> items = new FaultServ.FaultServiceClient().GetAllFaults().Select(c => new SelectListItem
                {
                    Value = c.FaultId.ToString(),
                    Text = c.FaultId.ToString()
                }).ToList();
                ViewBag.Faults = items;


                return View("ListAllFaultTrack", faults);
            }
            catch (Exception e)
            {

                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View("ListAllFaultTrack", faults);
            }
           
        }

        [Authorize(Roles = "Admin")]
        public ActionResult UpdateFault(int id)
        {
            FaultTrackModel faultTrack = new FaultTrackModel();
            try
            {


                faultTrack.FaultTrackList = new FaultTrackServ.FaultTrackServiceClient().GetFaultTracksByFaultId(id).ToList();
                faultTrack.FaultId = id;


                ViewBag.Status = new UserServ.UserServiceClient().getAllRoles();


                return View(faultTrack);
            }
            catch (Exception e)
            {

                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View(faultTrack);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult UpdateFault(FaultTrackModel faultTrack)
        {
            FaultTrackModel ft = faultTrack;
            FaultTrack faultT = new FaultTrack();
            try
            {

                ft.FaultTrackList = new FaultTrackServ.FaultTrackServiceClient().GetFaultTracksByFaultId(faultTrack.FaultId).ToList();
              

                if (faultTrack.Description != null && faultTrack.Type != null) 
                {
                    faultT.Type = faultTrack.Type;
                    faultT.Date = DateTime.Now;
                    faultT.Description = faultTrack.Description;

                    faultT.FaultId = faultTrack.FaultId;



                    new FaultTrackServ.FaultTrackServiceClient().AddFaultTrack(faultT);

                    //User u = new UserServ.UserServiceClient().GetUserById(faultTrack.Fault.OrderItem.Order.UserId);
                    Fault fault = new FaultServ.FaultServiceClient().GetFaultById(faultTrack.FaultId);
                    OrderItem oi = new OrderItemServ.OrderItemServiceClient().GetOrderItemById(fault.OrderItemId);
                    Order o = new OrderServ.OrderServiceClient().GetOrderByOrderId(oi.OrderId);
                    User u = new UserServ.UserServiceClient().GetUserById(o.UserId);

                    MailMessage mail = new MailMessage();


                    mail.Subject = oi.OrderItemId.ToString();
                    mail.To.Add(u.Email);
                    mail.To.Add("websiteclem@gmail.com");
                    mail.IsBodyHtml = true;
                    mail.Body = "Hi " + u.Username + "";
                    mail.Body += "<p>Order Item No: " + oi.OrderItemId + "</p>";
                    mail.Body += "<p>Status: " + faultTrack.Type + "</p>";
                    mail.Body += "<p>Thank you from ,Electros Team</p>";

                    SmtpClient cl = new SmtpClient();
                    cl.EnableSsl = true;
                    cl.Send(mail);

                    // SmsServ.ClickatellSms sms = new SmsServ.ClickatellSms();
                    //amlu li ma ikunx null u zid 356
                    // String[] numTo = {u.Mobile.ToString()};
                    // String[] numTo = { "35679009492" };

                    //c92454769eb50ca96ae716332a30c52

                    //commented
                    // sms.sendmsg("", 3473164, true, "clementina", "OcPOfKPODFWTMg", "bla", numTo, "35679009492", 1, false, 0, false, 7, false, 7, true, 1, false, 1, false, 3, true, 0, true, 0, false,"3473164", 0, false, "SMS_TEXT", "Fault", "String", 1440, true);

                    //sms.sendmsg("", 3480167, true, "clemcass", "PFcKPRaLVCDWPZ", "bla", numTo, "35679009492", 1, false, 0, false, 7, false, 7, true, 1, false, 1, false, 3, true, 0, true, 0, false, "3473164", 0, false, "SMS_TEXT", "Fault", "String", 1440, true);

                    SmsServ.ClickatellSms sms = new SmsServ.ClickatellSms();
                    //amlu li ma ikunx null u zid 356
                    // String[] numTo = {u.Mobile.ToString()};
                    string num = "356" + u.Mobile.ToString();
                    String[] numTo = { num };

                    //c92454769eb50ca96ae716332a30c52
                    string message = "Hi just to inform you that your order item no :" + oi.OrderItemId + " is  " + faultTrack.Type;

                    //commented
                    //sms.sendmsg("", 3480178, true, "clementina", "OcPOfKPODFWTMg", message, numTo, "35679009492", 1, false, 0, false, 7, false, 7, true, 1, false, 1, false, 3, true, 0, true, 0, false, "3480178", 0, false, "SMS_TEXT", "Fault", "String", 1440, true);

                    WebClient clientSMS = new WebClient();
                    clientSMS.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
                    clientSMS.QueryString.Add("user", "clementina");
                    clientSMS.QueryString.Add("password", "OcPOfKPODFWTMg");
                    clientSMS.QueryString.Add("api_id", "3473164");
                    clientSMS.QueryString.Add("to", "35679009492");

                    clientSMS.QueryString.Add("text", message);
                    string baseurl = "http://api.clickatell.com/http/sendmsg";
                    Stream data = clientSMS.OpenRead(baseurl);
                    StreamReader reader = new StreamReader(data);
                    string sms1 = reader.ReadToEnd();
                    data.Close();
                    reader.Close();
                    // Response.Redirect(sms1);
                    ViewBag.Message = "Email and SMS was sent to the user";
                }
                else {

                    ViewBag.Message = "Please fill all required information";
                }


                return View(ft);
            }
            catch (Exception e)
            {

                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View();
            }
        }

        public static List<SelectListItem> GetDropDown()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            List<String> types = new List<string>();

            types.Add("Reported");
            types.Add("Picked up Transit to main office");
            types.Add("Service in progress");
            types.Add("Service completed  Ready for delivery");
            types.Add("Picked up  Transit to customer");
            types.Add("Fault Completed");

            foreach (var type in types)
            {
                ls.Add(new SelectListItem() { Text = type, Value = type });
            }
            return ls;
        }
    }


    


}
