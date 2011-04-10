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
    public class AuthController : Controller
    {
        //
        // GET: /Home/
        [TimerActionFilter]
        public string Index()
        {
            return "m.Title";
        }
        public ActionResult Privilege()
        {
            ViewData["dataUrl"] = "/data/Privilege";
            ViewData["cwid"] = Request.Form["cwid"];
            return View();
        }

        public ActionResult selfAuth()
        {
            return View();
        }

        public ActionResult role()
        {
            return View();
        }
    }
}
