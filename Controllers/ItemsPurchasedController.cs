using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using ElectrosLtdApplication.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Net.Mail;

namespace ElectrosLtdApplication.Controllers
{
    public class ItemsPurchasedController : Controller
    {
        //
        // GET: /ItemsPurchased/

        [Authorize(Roles = "User")]
        public ActionResult ItemsPurchased()
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            List<OrderItem> allOrderItems = new List<OrderItem>();

            try
            {
                //gib orders kollha ta dak l persuna li huma shipped GetShippedOrdersByPersonId
                List<Order> orders = new OrderServ.OrderServiceClient().GetShippedOrdersByPersonId(User.Identity.Name).ToList();


                foreach (Order o in orders)
                {
                    orderItems = new OrderItemServ.OrderItemServiceClient().GetOrderItemByOrder(o.OrderId).ToList();

                    foreach (OrderItem oi in orderItems)
                    {
                        allOrderItems.Add(oi);
                    }


                }

                return View("ItemsPurchased", allOrderItems);
            }
            catch (Exception e)
            {

                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View("ItemsPurchased", allOrderItems);
            }


        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult FilterItemsPurchased(string start, string end, string search, string from, string to)
        {

            List<OrderItem> orderItems = new List<OrderItem>();

            DateTime startDate;
            DateTime endDate;

            try
            {
                if (DateTime.TryParse(start, out startDate))
                {
                    if (DateTime.TryParse(end, out endDate))
                    {
                        if (endDate > startDate)
                        {
                            List<Order> orders = new OrderServ.OrderServiceClient().FilterWithDate(User.Identity.Name, startDate, endDate).ToList();


                            foreach (Order o in orders)
                            {

                                var items = new OrderItemServ.OrderItemServiceClient().GetOrderItemByOrder(o.OrderId).ToList();
                                foreach (OrderItem i in items)
                                {
                                    orderItems.Add(i);
                                }
                            }


                        }

                    }



                }
                return PartialView("_FilterItemsPurchased", orderItems);
            }
            catch (Exception e)
            {


                ViewBag.Message = "Sorry there seems to be a problem"; 
                return PartialView("_FilterItemsPurchased", orderItems);
            }
            //gib orders kollha ta dak l persuna li huma shipped GetShippedOrdersByPersonId


            //amel for each li al kull order tara x order items andek u add ma lista tal itemspurchased




        }
        [Authorize(Roles = "User")]
        public ActionResult Details(int id)
        {
            FaultModel fm = new FaultModel();

            try
            {
                OrderItem orderItem = new OrderItemServ.OrderItemServiceClient().GetOrderItemById(id);
                //irid inzid tal fault jekk ikolhom 
                //get last status of the item purchased jekk hemm
                Fault f = new FaultServ.FaultServiceClient().OrderItemHasFault(orderItem.OrderItemId);
                FaultTrack ft = new FaultTrack();

                if (f != null)
                {
                   List<FaultTrack> fList = new FaultTrackServ.FaultTrackServiceClient().GetFaultTracksByFaultId(f.FaultId).ToList();

                   if (fList.Count != 0)
                   {
                       fm.FaultTrackList = fList;
                       ViewBag.Found = true;
                   }
                   else {

                       ViewBag.Found = false;
                   }
                }



                fm.OrderItem = orderItem;

                fm.LastFaultTrack = ft;

                return View(fm);
            }
            catch (Exception e)
            {

                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View(fm);
            }
        }
        [Authorize(Roles = "User")]
        [HttpGet]
        public ActionResult Print(int id)
        {
            List<OrderItem> orderItems = new List<OrderItem>();
            try
            {
                OrderItem orderItem = new OrderItemServ.OrderItemServiceClient().GetOrderItemById(id);
                //irid inzid tal fault jekk ikolhom 
                //get last status of the item purchased jekk hemm
                Fault f = new FaultServ.FaultServiceClient().OrderItemHasFault(orderItem.OrderItemId);
                FaultTrack ft = new FaultTrack();
                if (f != null)
                {
                    ft = new FaultTrackServ.FaultTrackServiceClient().GetLastFaultTrackByFaultId(f.FaultId);
                }


                FaultModel fm = new FaultModel();
                fm.OrderItem = orderItem;
                fm.LastFaultTrack = ft;


                // Create a Document object
                var document = new Document(PageSize.A4, 50, 50, 25, 25);

                // Create a new PdfWriter object, specifying the output stream
                MemoryStream memoryStream = new MemoryStream();
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

                // Open the Document for writing
                document.Open();


                var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
                var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
                var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

                document.Add(new Paragraph("Electros Ltd Receipt", titleFont));

                document.Add(new Paragraph(""));
                document.Add(new Paragraph(""));
                document.Add(new Paragraph(""));

                var orderInfoTable = new PdfPTable(5);

                orderInfoTable.HorizontalAlignment = 0;
                orderInfoTable.SpacingBefore = 10;
                orderInfoTable.SpacingAfter = 10;
                orderInfoTable.DefaultCell.Border = 0;


                orderInfoTable.AddCell(new Phrase("Order:", boldTableFont));
                orderInfoTable.AddCell(new Phrase("Name:", boldTableFont));
                orderInfoTable.AddCell(new Phrase("Ammount Purchased:", boldTableFont));
                orderInfoTable.AddCell(new Phrase("Date Purchased:", boldTableFont));
                orderInfoTable.AddCell(new Phrase("Warranty Expiry Date:", boldTableFont));

                orderInfoTable.AddCell(orderItem.OrderItemId.ToString());
                orderInfoTable.AddCell(fm.OrderItem.Product.Name);
                decimal price = fm.OrderItem.Product.Price * fm.OrderItem.Qty;
                orderInfoTable.AddCell(price.ToString());
                orderInfoTable.AddCell(fm.OrderItem.Order.DateTime.ToString());
                //.Value.ToShortDateString()
                orderInfoTable.AddCell(fm.OrderItem.Warranty.ToString());
                document.Add(orderInfoTable);


                var faultInfoTable = new PdfPTable(3);

                faultInfoTable.HorizontalAlignment = 0;
                faultInfoTable.SpacingBefore = 10;
                faultInfoTable.SpacingAfter = 10;
                faultInfoTable.DefaultCell.Border = 0;


                faultInfoTable.AddCell(new Phrase("Date:", boldTableFont));
                faultInfoTable.AddCell(new Phrase("Fault Details:", boldTableFont));
                faultInfoTable.AddCell(new Phrase("Status:", boldTableFont));


                Fault faults = new FaultServ.FaultServiceClient().OrderItemHasFault(id);
                List<FaultTrack> faultTrackList = new List<FaultTrack>();

                if (faults != null)
                {
                    faultTrackList = new FaultTrackServ.FaultTrackServiceClient().GetFaultTracksByFaultId(faults.FaultId).ToList();


                    foreach (FaultTrack fat in faultTrackList)
                    {
                        faultInfoTable.AddCell(fat.Date.ToString());
                        faultInfoTable.AddCell(fat.Description);
                        faultInfoTable.AddCell(fat.Type);
                    }

                    document.Add(faultInfoTable);
                }

                var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/logo.jpg"));
                logo.SetAbsolutePosition(440, 800);
                document.Add(logo);

                writer.CloseStream = false;
                document.Close();


                string user = User.Identity.Name;

                //User u = new UserServ.UserServiceClient().getUserByUsername(user);

                //MailMessage mail = new MailMessage();



                //mail.Subject = id.ToString();
                //mail.To.Add(u.Email);
                //mail.To.Add("websiteclem@gmail.com");
                //mail.IsBodyHtml = true;
                //mail.Body = "Hi " + u.Username + "";
                //mail.Body += "<p>This is a pdf with the update of the fault: " + f.TicketNo + "</p>";
                //mail.Body += "<p>Thank you from ,Electros Team</p>";
                //memoryStream.Position = 0;
                //mail.Attachments.Add(new Attachment(memoryStream, "filename.pdf"));

                //SmtpClient cl = new SmtpClient();
                //cl.EnableSsl = true;
                //cl.Send(mail);

                Response.ContentType = "application/pdf";
                //  Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", fm.OrderItemId.ToString()));
                Response.BinaryWrite(memoryStream.ToArray());



                List<Order> orders = new OrderServ.OrderServiceClient().GetShippedOrdersByPersonId(User.Identity.Name).ToList();


                foreach (Order o in orders)
                {

                    orderItems = new OrderItemServ.OrderItemServiceClient().GetOrderItemByOrder(o.OrderId).ToList();
                }



                return View("ItemsPurchased", orderItems);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View("ItemsPurchased", orderItems);
            }

        }


        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult SendEmail(string id, string start, string end)
        {
            try
            {
                SendEmailPDF(id, start, end);
                return View();
            }
            catch (Exception e)
            {
                ViewBag.Message = "Sorry there seems to be a problem"; 
                return View();
            }
        }

        public void SendEmailPDF(string id, string start, string end)
        {
            //try
            //{
            int idO = Convert.ToInt32(id);
            OrderItem orderItem = new OrderItemServ.OrderItemServiceClient().GetOrderItemById(idO);
            //irid inzid tal fault jekk ikolhom 
            //get last status of the item purchased jekk hemm
            Fault f = new FaultServ.FaultServiceClient().OrderItemHasFault(orderItem.OrderItemId);
            FaultTrack ft = new FaultTrack();
            if (f != null)
            {
                ft = new FaultTrackServ.FaultTrackServiceClient().GetLastFaultTrackByFaultId(f.FaultId);
            }


            FaultModel fm = new FaultModel();
            fm.OrderItem = orderItem;
            fm.LastFaultTrack = ft;


            // Create a Document object
            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a new PdfWriter object, specifying the output stream
            MemoryStream memoryStream = new MemoryStream();
            PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);

            // Open the Document for writing
            document.Open();


            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
            var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            document.Add(new Paragraph("Electros Ltd Receipt", titleFont));

            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            if (start != null && end != null)
            {
                document.Add(new Paragraph(start + " to " + end));
            }
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));

            var orderInfoTable = new PdfPTable(5);

            orderInfoTable.HorizontalAlignment = 0;
            orderInfoTable.SpacingBefore = 10;
            orderInfoTable.SpacingAfter = 10;
            orderInfoTable.DefaultCell.Border = 0;


            orderInfoTable.AddCell(new Phrase("Order:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Name:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Ammount Purchased:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Date Purchased:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Warranty Expiry Date:", boldTableFont));

            orderInfoTable.AddCell(orderItem.OrderItemId.ToString());
            orderInfoTable.AddCell(fm.OrderItem.Product.Name);
            decimal price = fm.OrderItem.Product.Price * fm.OrderItem.Qty;
            orderInfoTable.AddCell(price.ToString());
            orderInfoTable.AddCell(fm.OrderItem.Order.DateTime.Value.ToShortDateString());

            orderInfoTable.AddCell(fm.OrderItem.Warranty.ToString());
            document.Add(orderInfoTable);


            var faultInfoTable = new PdfPTable(4);

            faultInfoTable.HorizontalAlignment = 0;
            faultInfoTable.SpacingBefore = 10;
            faultInfoTable.SpacingAfter = 10;
            faultInfoTable.DefaultCell.Border = 0;

            faultInfoTable.AddCell(new Phrase("Fault Id:", boldTableFont));
            faultInfoTable.AddCell(new Phrase("Date:", boldTableFont));
            faultInfoTable.AddCell(new Phrase("Fault Details:", boldTableFont));
            faultInfoTable.AddCell(new Phrase("Status:", boldTableFont));


            Fault faults = new FaultServ.FaultServiceClient().OrderItemHasFault(idO);
            List<FaultTrack> faultTrackList = new List<FaultTrack>();

            if (faults != null)
            {
                faultTrackList = new FaultTrackServ.FaultTrackServiceClient().GetFaultTracksByFaultId(faults.FaultId).ToList();


                foreach (FaultTrack fat in faultTrackList)
                {
                    faultInfoTable.AddCell(faults.FaultId.ToString());
                    faultInfoTable.AddCell(fat.Date.ToString());
                    faultInfoTable.AddCell(fat.Description);
                    faultInfoTable.AddCell(fat.Type);
                }

                document.Add(faultInfoTable);
            }

            var logo = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/logo.jpg"));
            logo.SetAbsolutePosition(440, 800);
            document.Add(logo);

            writer.CloseStream = false;
            document.Close();


            string user = User.Identity.Name;

            User u = new UserServ.UserServiceClient().getUserByUsername(user);

            MailMessage mail = new MailMessage();



            mail.Subject = id.ToString();
            mail.To.Add(u.Email);
            mail.To.Add("websiteclem@gmail.com");
            mail.IsBodyHtml = true;
            mail.Body = "Hi " + u.Username + "";
            mail.Body += "<p>This is a pdf with the update of the fault: " + f.TicketNo + "</p>";
            mail.Body += "<p>Thank you from ,Electros Team</p>";
            memoryStream.Position = 0;
            mail.Attachments.Add(new Attachment(memoryStream, "filename.pdf"));

            SmtpClient cl = new SmtpClient();
            cl.EnableSsl = true;
            cl.Send(mail);

            Response.ContentType = "application/pdf";
            //  Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", fm.OrderItemId.ToString()));
            Response.BinaryWrite(memoryStream.ToArray());



        }
        //catch (Exception e)
        //{

        //   // ViewBag.Message = "There seems to be a problem. Error :" + e.Message;
        //}

    }
}

