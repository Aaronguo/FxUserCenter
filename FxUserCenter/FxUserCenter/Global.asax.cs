using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FxUserCenter
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
                new { controller = "UserCenter", action = "About", id = UrlParameter.Optional } // Parameter defaults
            );

            routes.MapRoute(
                "Admin",
                "{controller}/{action}/{page}",
                new object[] { 
                 new { controller = "Admin", action = "GoodsBuy", page = UrlParameter.Optional }, 
                 new { controller = "Admin", action = "GoodsTransfer", page = UrlParameter.Optional }, 
                 new { controller = "Admin", action = "CarBuy", page = UrlParameter.Optional }, 
                 new { controller = "Admin", action = "CarTransfer", page = UrlParameter.Optional }, 
                 new { controller = "Admin", action = "HouseBuy", page = UrlParameter.Optional }, 
                 new { controller = "Admin", action = "HouseTransfer", page = UrlParameter.Optional }, 
                 new { controller = "Admin", action = "CarTransferTopShow", page = UrlParameter.Optional },
                 new { controller = "Admin", action = "CarBuyTopShow", page = UrlParameter.Optional },
                 new { controller = "Admin", action = "HouseTransferTopShow", page = UrlParameter.Optional },
                 new { controller = "Admin", action = "HouseBuyTopShow", page = UrlParameter.Optional },
                 new { controller = "Admin", action = "GoodsTransferTopShow", page = UrlParameter.Optional },
                 new { controller = "Admin", action = "GoodsBuyTopShow", page = UrlParameter.Optional },
                 new { controller = "Admin", action = "CancleTopShow", page = UrlParameter.Optional },
                 
                }
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            // Use LocalDB for Entity Framework by default
            Database.DefaultConnectionFactory = new SqlConnectionFactory(@"Data Source=(localdb)\v11.0; Integrated Security=True; MultipleActiveResultSets=True");

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}