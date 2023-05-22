using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace WinProApp.Controllers
{
    public class BasedUserController : Controller
    {
        protected CultureInfo RequestCulture { get; set; }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //var langSession = HttpContext.Session.GetString("currentCulture");
            //if (!User.Identity.IsAuthenticated && (langSession == null || langSession == ""))
            //{
            //    RedirectToPage("/Login");
            //}
 
            RequestCulture = Request.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture;
        }
    }
}
