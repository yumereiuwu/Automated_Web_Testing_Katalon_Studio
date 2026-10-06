using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace A3_MVC_ASP.Filters
{
    /// <summary>Chỉ cho VaiTro QuanTri trong Session.</summary>
    public class QuanTriAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.Session != null
                && httpContext.Session["VaiTro"] as string == "QuanTri";
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { controller = "Account", action = "Login", returnUrl = filterContext.HttpContext.Request.RawUrl }));
        }
    }
}
