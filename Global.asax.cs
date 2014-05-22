using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using Common;
using BusinessLayer;
using System.Security.Principal;

namespace ElectrosLtdApplication
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "HttpError404";
                        break;
                    case 500:
                        // server error
                        action = "HttpError500";
                        break;
                    default:
                        action = "General";
                        break;
                }

                // clear error on server
                Server.ClearError();

                Response.Redirect(String.Format("~/Error/{0}/?message={1}", action, exception.Message));
            }
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

       

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (Context.User != null)
            {
                if (!string.IsNullOrEmpty(Context.User.Identity.Name))
                {
                    // Code to load role from database
                    User c = new UserServ.UserServiceClient().getUserByUsername(Context.User.Identity.Name);
                    string[] userRoles = new string[c.Role.Count];
                    int i = 0;
                    foreach (Role r in c.Role)
                    {
                        userRoles[i] = r.Name;
                        i++;
                    }

                    GenericPrincipal gp = new GenericPrincipal(Context.User.Identity, userRoles);
                    Context.User = gp;

                }
            }
        }
    }
}