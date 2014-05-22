using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using ElectrosLtdApplication.BarCodeServ;
using ElectrosLtdApplication.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Security;

namespace ElectrosLtdApplication.Controllers
{
    public class ManageReturnsFaultsController : Controller
    {
        //
        // GET: /Manage ReturnsFaults/

        [Authorize(Roles = "User")]
        public ActionResult AllItemsPurchased()
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            List<OrderItem> allOrderItems = new List<OrderItem>();
            try
            {

                List<Order> orders = new OrderServ.OrderServiceClient().GetShippedOrdersByPersonId(User.Identity.Name).ToList();


                foreach (Order o in orders)
                {
                    orderItems = new OrderItemServ.OrderItemServiceClient().GetOrderItemByOrder(o.OrderId).ToList();

                    foreach (OrderItem oi in orderItems)
                    {
                        allOrderItems.Add(oi);
                    }

                }

                return View(allOrderItems);
            }
            catch (Exception e)
            {

                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View(allOrderItems);
            }
        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult GenerateFault(int id, FaultModel f)
        {
            try
            {
                Fault fault = new Fault();

                fault.Barcode = f.Barcode;
                fault.TicketNo = f.TicketNo;
                fault.Details = f.Details;
                fault.OrderItemId = id;
                fault.Date = DateTime.Now;


                MemoryStream memStream = new MemoryStream(f.Barcode);
                Bitmap bm = new Bitmap(memStream);
                bm.Save(HttpContext.Response.OutputStream, ImageFormat.Jpeg);
                memStream.Position = 0;

                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Image.Jpeg;
                contentType.Name = "screen";


                string user = User.Identity.Name;

                User u = new UserServ.UserServiceClient().getUserByUsername(user);

              

                new FaultServ.FaultServiceClient().AddFault(fault);

                FaultTrack faultTrack = new FaultTrack();
                faultTrack.Type = "Reported";
                faultTrack.Description = fault.Details;
                faultTrack.Date = DateTime.Now;

                
                Fault lastfault = new FaultServ.FaultServiceClient().GetLastFault();
                faultTrack.FaultId = lastfault.FaultId;

                MailMessage mail = new MailMessage();



                mail.Subject = id.ToString();
                mail.To.Add(u.Email);
                mail.To.Add("websiteclem@gmail.com");
                mail.IsBodyHtml = true;
                mail.Body = "Hi " + u.Username + "";
                mail.Body += "<p>Order Item No: " + f.OrderItemId + "</p>";
                mail.Body += "<p>Problem: " + f.Details + "</p>";
                mail.Body += "<p>Problem: " + faultTrack.Type + "</p>";
                mail.Body += "<p>Thank you from ,Electros Team</p>";
                mail.Attachments.Add(new Attachment(memStream, contentType));

                SmtpClient cl = new SmtpClient();
                cl.EnableSsl = true;
                cl.Send(mail);


                new FaultTrackServ.FaultTrackServiceClient().AddFaultTrack(faultTrack);


               
                //Need to get last fault added

         



                //NOT WORKINGGGGGGGGG
                


                //Image image = Image.FromStream(new System.IO.MemoryStream(f.Barcode));

                //tblBarcode tb = new tblBarcode();
                //tb.BarcodeImage = imgBarcode;

                // new UserServ.UserServiceClient().addImage(tb);
                // new FaultServ.FaultServiceClient().AddFault(fault);

                // string user = User.Identity.Name;

                // User u = new UserServ.UserServiceClient().getUserByUsername(user);


                // System.Net.Mail.MailMessage ms = new System.Net.Mail.MailMessage();
                //// ms.Subject = o.OrderNo.ToString();
                // ms.To.Add(u.Email);
                // ms.To.Add("websiteclem@gmail.com");
                // ms.Body = "Hi " + u.Username + "";
                // ms.Body += "<p>Order Item No: " + f.OrderItemId + "</p>";
                // ms.Body += "<p>Problem: " + f.Details + "</p>";


                //  LinkedResource r = new LinkedResource(memStream, MediaTypeNames.Image.Jpeg);

                //      r.ContentId = "myImage";
                //      //add linked resource to message and send email here


                //  ms.IsBodyHtml = true;

                //  Response.Clear();
                //  Response.ContentType = "image/png";
                //  Response.AddHeader("content-disposition", "attachment;filename=barcode.jpg");
                //  Response.Buffer = true;
                //  memStream.WriteTo(Response.OutputStream);
                //  Response.End();



                //  AlternateView htmlView = AlternateView.CreateAlternateViewFromString(ms.Body, null, "text/html");
                //  LinkedResource imagelink = new LinkedResource(Server.MapPath(".") + @"\codedigest.jpg", "image/jpg");
                //  imagelink.ContentId = "imageId";
                //  imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
                //  htmlView.LinkedResources.Add(imagelink);
                //  ms.AlternateViews.Add(htmlView);



                //  string attachmentPath = Environment.CurrentDirectory + @"\image.jpg";
                //  Attachment inline = new Attachment(attachmentPath);
                //  inline.ContentDisposition.Inline = true;
                //  inline.ContentDisposition.DispositionType = DispositionTypeNames.Inline;
                //  inline.ContentId = "myImage";
                //  inline.ContentType.MediaType = "image/jpg";
                //  inline.ContentType.Name = Path.GetFileName(attachmentPath);
                //  ms.Attachments.Add(inline);
                //  ms.Body = ms.Body.Replace("@@IMAGE@@", "cid:" + "myImage");

                //  //Add the Image to the Alternate view
                // // htmlView.LinkedResources.Add(inline);

                //  //Add view to the Email Message
                ////  ms.AlternateViews.Add(htmlView);

                //  //ms.Body += "<p>Thank you from ,Electros Team</p>";


                //  SmtpClient cl = new SmtpClient();
                //  cl.EnableSsl = true;
                //  cl.Send(ms);

                //  FaultTrack faultTrack = new FaultTrack();
                //  faultTrack.Type = "Reported";
                //  faultTrack.Description = "Item has been reported but not yet picked up for service";

                //  new FaultTrackServ.FaultTrackServiceClient().AddFaultTrack(faultTrack);


                return Redirect(Url.Action("AllItemsPurchased"));

            }


            catch (Exception e)
            {

                ViewBag.Message = "Sorry there seems to be a problem"; 
                return Redirect(Url.Action("AllItemsPurchased"));
            }
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult GenerateFault(int id)
        {
            FaultModel f = new FaultModel();

            try
            {

                //Random r = new Random();
                //int num = r.Next(100000, 199999);

                int num = new GenNumServ.GenerateNumbersClient().GenerateTicketNo();

                BarCodeData barCodeData = new BarCodeData();
                barCodeData.Height = 225;
                barCodeData.Width = 225;
                barCodeData.Angle = 0;
                barCodeData.Ratio = 5;
                barCodeData.Module = 0;
                barCodeData.Left = 25;
                barCodeData.Top = 0;
                barCodeData.CheckSum = false;
                barCodeData.FontName = "Arial";
                barCodeData.BarColor = "Black";
                barCodeData.BGColor = "White";
                barCodeData.FontSize = 10.0f;
                barCodeData.barcodeOption = BarcodeOption.Both;
                barCodeData.barcodeType = BarcodeType.Code_2_5_interleaved;
                barCodeData.checkSumMethod = CheckSumMethod.None;
                barCodeData.showTextPosition = ShowTextPosition.BottomCenter;
                barCodeData.BarCodeImageFormat = ImageFormats.JPEG;

                BarCodeServ.BarCode barcode = new BarCodeServ.BarCode();


                //Byte[] imgBarcode = new BarCodeServ.BarCodeSoapClient().GenerateBarCode(barCodeData, (num + num).ToString());

                Byte[] imgBarcode = barcode.GenerateBarCode(barCodeData, (num).ToString());

                // MemoryStream memStream = new MemoryStream(imgBarcode);
                // Bitmap bm = new Bitmap(memStream);
                // bm.Save(HttpContext.Response.OutputStream, ImageFormat.Png);

                //Image image = Image.FromStream(new System.IO.MemoryStream(imgBarcode));

                // tblBarcode tb = new tblBarcode();
                // tb.BarcodeImage = imgBarcode;

                // new UserServ.UserServiceClient().addImage(tb);



                f.Barcode = imgBarcode;
                f.TicketNo = num;
                f.OrderItemId = id;
                f.Date = DateTime.Now;
                return View(f);
            }


            catch (Exception e)
            {

                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View(f);
            }

        }

    }
}
