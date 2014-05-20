using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectrosLtdApplication.Models;
using Common;
using System.Net.Mail;

namespace ElectrosLtdApplication.Controllers
{
    public class ShoppingCartController : Controller
    {
        //
        // GET: /ShoppingCart/
        [Authorize(Roles = "User")]
        public ActionResult ShoppingCart()
        {
            Order order = new Order();
            order = new OrderServ.OrderServiceClient().GetOrderByPersonId(User.Identity.Name);
            User u = new UserServ.UserServiceClient().getUserByUsername(User.Identity.Name);


            OrderModel viewModel = new OrderModel();

            viewModel.OrderItem = order.OrderItem;

            // Set up our ViewModel
            //var viewModel = new OrderModel
            //{
            //    CartItems = cart.GetCartItems(),
            //    CartTotal = cart.GetTotal()
            //};

            // Return the view
            return View(viewModel);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart

            Order order = new OrderServ.OrderServiceClient().GetOrderByPersonId(User.Identity.Name);


            OrderItem item = new OrderItemServ.OrderItemServiceClient().GetOrderItemById(id);
            //// Remove from cart
            //int itemCount = cart.RemoveFromCart(id);
            new OrderItemServ.OrderItemServiceClient().DeleteOrderItem(id);

            //// Display the confirmation message


            return Json(item.Product.Name + "has been removed");
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult UpdateFromCart(int id, string qty)
        {
            OrderItem oi = new OrderItem();
            if (id != null)
            {

                oi = new OrderItemServ.OrderItemServiceClient().GetOrderItemById(id);

                int txtQty = Convert.ToInt16(qty);
                if (txtQty != 0)
                {
                    if (oi.Product.Stock >= txtQty)
                    {
                        oi.Qty = txtQty;
                        new OrderItemServ.OrderItemServiceClient().UpdateOrderItem(oi);
                    }
                    else
                    {

                        return Json("Only " + oi.Product.Stock + " are left on stock");
                    }

                }
                else
                {
                    new OrderItemServ.OrderItemServiceClient().DeleteOrderItem(oi.OrderId);

                }

            }
            return Json(oi.Product.Name + "has been updated");
        }

        [Authorize(Roles = "User")]
        public ActionResult CheckOut()
        {
            Order order = new Order();
            order = new OrderServ.OrderServiceClient().GetOrderByPersonId(User.Identity.Name);
            User u = new UserServ.UserServiceClient().getUserByUsername(User.Identity.Name);


            OrderModel viewModel = new OrderModel();

            foreach (OrderItem oi in order.OrderItem)
            {
                viewModel.Total += oi.Qty * oi.Product.Price;
            }


            viewModel.OrderItem = order.OrderItem;

            return View(viewModel);
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public ActionResult CheckOutAndPay()
        {
             
            Order o = new OrderServ.OrderServiceClient().GetOrderByPersonId(User.Identity.Name);

            if (o != null && o.IsPending != false)
            {
                o.IsPending = false;
                o.IsShipped = true;
                o.DateTime = DateTime.Now;
                new OrderServ.OrderServiceClient().ConfirmOrder(o);
                //NAQAS QTYY

             

                //check how to refresh gridview so user cannot update order again

                User p = new UserServ.UserServiceClient().getUserByUsername(User.Identity.Name);


                System.Net.Mail.MailMessage ms = new System.Net.Mail.MailMessage();
                ms.Subject = o.OrderId.ToString();
                ms.To.Add(p.Email);
                ms.To.Add("websiteclem@gmail.com");
                ms.Body = "Hi " + p.Username + "";

                ms.Body += "<p>Order No: " + o.OrderId + " has been sucessfully ordered </p>";

                ms.Body += "Thank you from ,Electros Team";
                ms.IsBodyHtml = true;

                SmtpClient cl = new SmtpClient();
                cl.EnableSsl = true;
                cl.Send(ms);

                return Json("Order has been ordered");
            }

            return View();
        }
    }
}
