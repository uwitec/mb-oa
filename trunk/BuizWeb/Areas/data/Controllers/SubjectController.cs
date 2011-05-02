using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;

namespace BuizApp.Areas.data.Controllers
{
    public class SubjectController : Controller
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
                    mydb.Organizations.OrderBy(org=>org.OrderNO).Select(org => new { org.ID, Name = "[组织]" + org.Name })
                    .Union(mydb.Users.Select(u => new { u.ID, Name = "[用户]" + u.Name }))
                    .ToArray()
                    ;
                return Json(
                        result//.Union(new[] { new { ID = string.Empty, Name = "(空)" } })
                        , JsonRequestBehavior.AllowGet
                        );
            }
        }

    }
}
