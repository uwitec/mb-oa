using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuizApp.Areas.test.Controllers
{
    public class t1Controller : Controller
    {
        //
        // GET: /test/t1/

        public ActionResult Index(string Id)
        {
            Response.Write(string.Format("id:{0}<br/>",Id));
            return View();
        }

        public ActionResult test2()
        {
            return View();
        }

    }
}
