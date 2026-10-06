using System.Data.Entity;
using System.Text;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using A3_MVC_ASP.Models;

namespace A3_MVC_ASP
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest()
        {
            Response.ContentEncoding = Encoding.UTF8;
            Response.HeaderEncoding = Encoding.UTF8;
        }

        protected void Application_Start()
        {
            Database.SetInitializer<DoAnVatA3Context>(null);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
