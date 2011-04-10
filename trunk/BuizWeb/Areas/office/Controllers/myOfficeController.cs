using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuizApp.Areas.office.Controllers
{
    public class myOfficeController : Controller
    {
        //
        // GET: /office/myOffice/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult myCarlendar()
        {
            return this.Redirect("/extensible-1.0/examples/calendar/test-app.html");
        }

        public ActionResult myRemind()
        {
            return View();
        }
    }
}
