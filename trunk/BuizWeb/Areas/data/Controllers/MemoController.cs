using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;

namespace BuizApp.Areas.data.Controllers
{
    public class MemoController : Controller
    {
        public ActionResult Index()
        {
            using (MyDB mydb = new MyDB())
            {
                object[] result =
                    mydb.Events
                    .Where(ev => ev.Type.Equals("便笺"))
                    .Where(ev => ev.Creator.ID.Equals(this.User.Identity.Name))
                    .OrderByDescending(ev => ev.CreateTime)
                    .Select(ev => new { ev.ID, ev.Name, ev.Content, ev.CreateTime })
                    .ToArray();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
