using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;

namespace BuizApp.Areas.system.Controllers
{
    //[MyFilter]
    public partial class AuthController : Controller
    {
        //
        // GET: /Home/
        [TimerActionFilter]
        public string Index()
        {
            return "m.Title";
        }

        public ActionResult selfAuth()
        {
            return View();
        }

        public ActionResult role()
        {
            return View();
        }

        public ActionResult user()
        {
            return View();
        }

        public ActionResult Organization()
        {
            return View();
        }

        public ActionResult query()
        {
            return View();
        }
    }
}
