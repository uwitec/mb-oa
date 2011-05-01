using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;

namespace BuizApp.Areas.data.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /data/User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult list()
        {
            using (MyDB mydb = new MyDB())
            {
                object[] result =
                    mydb.Users
                    .Select(u => new { u.ID, u.Name })
                    .ToArray()
                    ;
                return Json(
                        result.Union(new[] { new { ID = string.Empty, Name = "(空)" } })
                        , JsonRequestBehavior.AllowGet
                        );
            }
        }

    }
}
