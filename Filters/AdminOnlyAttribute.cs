using System.Web.Mvc;

namespace AutoServisWeb.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var session = filterContext.HttpContext.Session;

            if (session["KorisnikID"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "Account", action = "Login" }));
                return;
            }

            if ((int)session["UlogaID"] != 1)
            {
                filterContext.Result = new HttpStatusCodeResult(403, "Pristup zabranjen");
            }
        }
    }
}