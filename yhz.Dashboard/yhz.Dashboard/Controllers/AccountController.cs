using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace yhz.Dashboard.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            //var cookie = FormsAuthentication.GetAuthCookie("yinhz", false);
            //cookie.
            //FormsAuthentication.SetAuthCookie("yinhz", false);
            return View();
        }

        [HttpPost]
        public ActionResult Login(
            string username, string password, string returnUrl) {
                bool result = FormsAuthentication.Authenticate(username, password);

                if (result)
                {
                    FormsAuthentication.SetAuthCookie(username, false);

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }

                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
        }
    }
}