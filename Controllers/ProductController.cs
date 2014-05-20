using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Common;
using ElectrosLtdApplication.Models;
using System.Net;
using System.IO;
using System.Web.Services.Protocols;

namespace ElectrosLtdApplication.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Products()
        {

            //get list of all products
            List<Product> products = new ProductServ.ProductServiceClient().GetAllProducts().ToList();

            

            // index ax hem view ta index 
            List<SelectListItem> ls = new List<SelectListItem>();
            List<String> types = new List<string>();

            types.Add("Price ASC");
            types.Add("Price DES");
            types.Add("Date ASC");
            types.Add("Date DES");



            foreach (var type in types)
            {
                ls.Add(new SelectListItem() { Text = type, Value = type });
            }
             ViewBag.ddl= ls;


            //view (tadijomlu)
            return View("Products", products);

        }
        [Authorize(Roles = "User")]
        public ActionResult FilterProducts(string search, string from, string to,string date,string sort)
        {
            List<Product> products = new ProductServ.ProductServiceClient().GetAllProducts().ToList();
            if (search != "")
            {

                products = new ProductServ.ProductServiceClient().GetByText(search).ToList();

            }

            else if (to != "0" || from != "0")
            {
                decimal tO = Decimal.Parse(to);
                decimal fRom = Decimal.Parse(from);

                products = new ProductServ.ProductServiceClient().GetByPriceRange(tO, fRom).ToList();


            }

            else if (date != "")
            {
                DateTime datel;
                if (DateTime.TryParse(date, out datel))
                {
                    products = new ProductServ.ProductServiceClient().GetByDateListed(datel).ToList();
                }
            }

            if (sort == "Price ASC")
            {
                

                products = products.OrderBy(t => t.Price).ToList();
            }
            else if (sort == "Price DES")
            {
                products = products.OrderByDescending(t => t.Price).ToList();
            }

            else if (sort == "Date ASC")
            {
                products = products.OrderBy(t => t.DateListed).ToList();
            }
            else if (sort == "Date DES")
            {
                products = products.OrderByDescending(t => t.DateListed).ToList();
            }


           
            return PartialView("_Products", products);
        }

        [Authorize(Roles = "User")] 
        public ActionResult ProductDetails(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                User u = new UserServ.UserServiceClient().getUserByUsername(User.Identity.Name);
                ViewBag.Bought = new OrderItemServ.OrderItemServiceClient().GetOrderItemIfBought(id, u.UserId);
            }
            else
            {

                ViewBag.Bought = false;
            }

            HttpContext.User.IsInRole("Admin");

            ViewBag.ProductId = id;
            List<Rate> ratings = new ProductServ.ProductServiceClient().GetAllRateOfProduct(id).ToList();
            int avg = 0;
            if (ratings.Count > 0)
            {
                int num = 0;
                foreach (Rate r in ratings)
                {
                    num += r.RateNo;
                }

                avg = num / ratings.Count();
            }


            ViewBag.Avg = avg;
            //set stars as avg first, then if user add stars set avg to his stars
            Product product = new ProductServ.ProductServiceClient().GetProductById(id);

            List<Review> reviews = new ReviewServ.ReviewServiceClient().GetReviewByProductId(product.ProductId).ToList();


            ViewBag.Review = new ReviewServ.ReviewServiceClient().GetReviewByProductId(product.ProductId).ToList();

            //        model.Product = product;




            return View(product);


        }
        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult AddToShoppingCart(int id, string qty)
        {

            //        function OnButtonClick(product, quantity) {

            //    $.ajax({
            //        url: '/Product/AddToShoppingCart/',
            //        data: { id: product, qty:quantity },
            //        type: "POST",
            //        dataType: 'Json',
            //        cache: false,
            //        success: function (data) {


            //        }
            //    });

            //}

            int productId = id;



            if (qty != null)
            {
                try
                {

                    int qtyOrdered = 0;
                    if (int.TryParse(qty, out qtyOrdered))
                    {
                        qtyOrdered = Convert.ToInt32(qty);


                        if (qtyOrdered > 0)
                        {

                            return Json(new ProductServ.ProductServiceClient().AddProductForOrder(productId, qtyOrdered, User.Identity.Name));

                        }
                        else
                        {
                            throw new Exception("Wrong Qty");

                        }
                    }
                    else
                    {
                        throw new Exception("Qty entered is not valid");
                    }
                }
                catch (Exception ex)
                {

                    return Json(ex.Message);

                }
            }

            return Json("bla bla");
           // return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult AddStar(int productId, int star)
        {
            Rate rm = new Rate();
            rm.ProductId = productId;
            rm.RateNo = star;
            User u = new UserServ.UserServiceClient().getUserByUsername(User.Identity.Name);
            rm.UserId = u.UserId;

            new ProductServ.ProductServiceClient().AddRate(rm);
            //Add star as product id
            return Json(true);
        }

        [Authorize(Roles = "User")]
        public ActionResult AddReview()
        {

            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult AddReview(int id, ReviewModel r)
        {

            Review review = new Review();

            review.ReviewComment = r.ReviewComment;
            review.ProductId = id;
            string user = User.Identity.Name;

            User u = new UserServ.UserServiceClient().getUserByUsername(user);
            review.UserId = u.UserId;

            new ReviewServ.ReviewServiceClient().AddReview(review);


            return Redirect(Url.Action("ProductDetails") + "/" + id);
            //return View();


        }


       
   

    }
}
