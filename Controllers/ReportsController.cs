using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace ElectrosLtdApplication.Controllers
{
    public class ReportsController : Controller
    {
        //
        // GET: /Reports/
        [Authorize(Roles = "Admin")]
        public ActionResult Reports()
        {

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult HighlyRatedItem()
        {
            List<Rate> rates = new ReportServ.ReportsServiceClient().HighlyRatedItem().ToList();
            Rate r = rates.First();
            Product p = new ProductServ.ProductServiceClient().GetProductById(r.ProductId);


            // Create a Document object
            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a new PdfWriter object, specifying the output stream
            var output = new System.IO.MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            // Open the Document for writing
            document.Open();


            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
            var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            document.Add(new Paragraph("Highly Rated Item", titleFont));



            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));

            Image logo = Image.GetInstance(Server.MapPath("~" + p.Image.ToString()));
            logo.ScaleToFit(200,200);
            logo.HasBorder(200);
            logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            document.Add(logo);

            var orderInfoTable = new PdfPTable(6);

            orderInfoTable.HorizontalAlignment = 0;
            orderInfoTable.SpacingBefore = 10;
            orderInfoTable.SpacingAfter = 10;
            orderInfoTable.DefaultCell.Border = 0;
            

            
            orderInfoTable.AddCell(new Phrase("Name:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Features:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Stock:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Price", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Date Listed", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Category", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Image", boldTableFont));

            orderInfoTable.AddCell(p.Name);
            orderInfoTable.AddCell(p.Features);
            orderInfoTable.AddCell(p.Stock.ToString());
            decimal price = p.Price;
            orderInfoTable.AddCell(price.ToString());
            orderInfoTable.AddCell(p.DateListed.ToString());
           
            orderInfoTable.AddCell(p.Category.Name);
           //razorpdf.pdfresult
            //.Value.ToShortDateString()
          
            document.Add(orderInfoTable);
   // @Server.MapPath("~/images/" +  @Model.Image )"
            
           // logo.SetAbsolutePosition(800, 800);
            

            document.Close();

            Response.ContentType = "application/pdf";
            //  Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", fm.OrderItemId.ToString()));
            Response.BinaryWrite(output.ToArray());


            return View("Reports");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult MostPurchasedItem()
        {
            List<OrderItem> orderItem = new ReportServ.ReportsServiceClient().MostPurchasedItem().ToList();
            OrderItem o = orderItem.First();
            Product p = new ProductServ.ProductServiceClient().GetProductById(o.ProductId);


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

            document.Add(new Paragraph("Most Item Purchased", titleFont));



            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));

            Image logo = Image.GetInstance(Server.MapPath("~" + p.Image.ToString()));
            logo.ScaleToFit(200, 200);
            logo.HasBorder(200);
            logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            document.Add(logo);

            var orderInfoTable = new PdfPTable(6);

            orderInfoTable.HorizontalAlignment = 0;
            orderInfoTable.SpacingBefore = 10;
            orderInfoTable.SpacingAfter = 10;
            orderInfoTable.DefaultCell.Border = 0;



            orderInfoTable.AddCell(new Phrase("Name:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Features:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Stock:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Price", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Date Listed", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Category", boldTableFont));

            orderInfoTable.AddCell(p.Name);
            orderInfoTable.AddCell(p.Features);
            orderInfoTable.AddCell(p.Stock.ToString());
            decimal price = p.Price;
            orderInfoTable.AddCell(price.ToString());
            orderInfoTable.AddCell(p.DateListed.ToString());

            orderInfoTable.AddCell(p.Category.Name);
            //razorpdf.pdfresult
            //.Value.ToShortDateString()

            document.Add(orderInfoTable);
            // @Server.MapPath("~/images/" +  @Model.Image )"

            // logo.SetAbsolutePosition(800, 800);


            document.Close();

            Response.ContentType = "application/pdf";
            //  Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", fm.OrderItemId.ToString()));
            Response.BinaryWrite(memoryStream.ToArray());


            return View("Reports");

           
        }

        [Authorize(Roles = "Admin")]
        public ActionResult HighestNumberOfFaults()
        {

            //List<Fault> fault = new ReportServ.ReportsServiceClient().HighestNumberFaults().ToList();
            //Fault f = fault.First();
            //Product p = new ProductServ.ProductServiceClient().GetProductById(f.OrderItem.ProductId);

            List<ProductDTO> highestFault = new ReportServ.ReportsServiceClient().HighestNumberFaults().ToList();
            //Product p = new ProductServ.ProductServiceClient().GetProductById(f.OrderItem.ProductId);

         

            // Create a Document object
            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a new PdfWriter object, specifying the output stream
            var output = new System.IO.MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            // Open the Document for writing
            document.Open();


            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
            var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            document.Add(new Paragraph("Highest Number Of Faults", titleFont));



            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));

            //foreach (ProductDTO p in highestFault)
            //{
            //    Image logo = Image.GetInstance(Server.MapPath("~" + p.Image.ToString()));
            //    logo.ScaleToFit(200, 200);
            //    logo.HasBorder(200);
            //    logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            //    document.Add(logo);
            //}
            

            var orderInfoTable = new PdfPTable(6);

            orderInfoTable.HorizontalAlignment = 0;
            orderInfoTable.SpacingBefore = 10;
            orderInfoTable.SpacingAfter = 10;
            orderInfoTable.DefaultCell.Border = 0;



            orderInfoTable.AddCell(new Phrase("Name:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Features:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Stock:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Price", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Date Listed", boldTableFont));
            
            orderInfoTable.AddCell(new Phrase("Image", boldTableFont));

           
                foreach (ProductDTO p in highestFault)
                {
                    orderInfoTable.AddCell(p.Name);

                    orderInfoTable.AddCell(p.Features);

                    orderInfoTable.AddCell(p.Stock.ToString());

                    decimal price = p.Price;
                    orderInfoTable.AddCell(price.ToString());

                    orderInfoTable.AddCell(p.DateListed.ToString());

                    Image logo = Image.GetInstance(Server.MapPath("~" + p.Image.ToString()));
                    orderInfoTable.AddCell(logo);

                }
           

                
                //
               
                //

               // orderInfoTable.AddCell(p.Category.Name);
          
            //razorpdf.pdfresult
            //.Value.ToShortDateString()

            document.Add(orderInfoTable);
            // @Server.MapPath("~/images/" +  @Model.Image )"

            // logo.SetAbsolutePosition(800, 800);


            document.Close();

            Response.ContentType = "application/pdf";
            //  Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", fm.OrderItemId.ToString()));
            Response.BinaryWrite(output.ToArray());


            return View("Reports");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult LeastNumberOfFaults()
        {

            List<ProductDTO> lowestFault = new ReportServ.ReportsServiceClient().LowestNumberFaults().ToList();
            //Product p = new ProductServ.ProductServiceClient().GetProductById(f.OrderItem.ProductId);



            // Create a Document object
            var document = new Document(PageSize.A4, 50, 50, 25, 25);

            // Create a new PdfWriter object, specifying the output stream
            var output = new System.IO.MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            // Open the Document for writing
            document.Open();


            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            var boldTableFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var endingMessageFont = FontFactory.GetFont("Arial", 10, Font.ITALIC);
            var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            document.Add(new Paragraph("Lowest Number Of Faults", titleFont));



            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));
            document.Add(new Paragraph(""));

            //foreach (ProductDTO p in highestFault)
            //{
            //    Image logo = Image.GetInstance(Server.MapPath("~" + p.Image.ToString()));
            //    logo.ScaleToFit(200, 200);
            //    logo.HasBorder(200);
            //    logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
            //    document.Add(logo);
            //}


            var orderInfoTable = new PdfPTable(6);

            orderInfoTable.HorizontalAlignment = 0;
            orderInfoTable.SpacingBefore = 10;
            orderInfoTable.SpacingAfter = 10;
            orderInfoTable.DefaultCell.Border = 0;



            orderInfoTable.AddCell(new Phrase("Name:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Features:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Stock:", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Price", boldTableFont));
            orderInfoTable.AddCell(new Phrase("Date Listed", boldTableFont));

            orderInfoTable.AddCell(new Phrase("Image", boldTableFont));


            foreach (ProductDTO p in lowestFault)
            {
                orderInfoTable.AddCell(p.Name);

                orderInfoTable.AddCell(p.Features);

                orderInfoTable.AddCell(p.Stock.ToString());

                decimal price = p.Price;
                orderInfoTable.AddCell(price.ToString());

                orderInfoTable.AddCell(p.DateListed.ToString());

                Image logo = Image.GetInstance(Server.MapPath("~" + p.Image.ToString()));
                orderInfoTable.AddCell(logo);

            }



            //

            //

            // orderInfoTable.AddCell(p.Category.Name);

            //razorpdf.pdfresult
            //.Value.ToShortDateString()

            document.Add(orderInfoTable);
            // @Server.MapPath("~/images/" +  @Model.Image )"

            // logo.SetAbsolutePosition(800, 800);


            document.Close();

            Response.ContentType = "application/pdf";
            //  Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", fm.OrderItemId.ToString()));
            Response.BinaryWrite(output.ToArray());


            return View("Reports");
        }

    }
}
