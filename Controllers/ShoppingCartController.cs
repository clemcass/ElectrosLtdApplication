using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ElectrosLtdApplication.Models;
using Common;

namespace ElectrosLtdApplication.Controllers
{
    public class ShoppingCartController : Controller
    {
        //
        // GET: /ShoppingCart/

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

    }
}
