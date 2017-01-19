using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace yhz.Dashboard.Controllers
{
    public class User
    {
        public string Name { get; set; }
        public string Age { get; set; }
    }

    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index(User u, string str)
        {
            return View();
        }

        public ActionResult Canvas() { return View(); }
    }
}