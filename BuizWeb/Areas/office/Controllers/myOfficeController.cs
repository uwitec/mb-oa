using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib;

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
            return this.Redirect("/content/extensible-1.0/examples/calendar/test-app.html");
        }

        public ActionResult myRemind()
        {
            return View();
        }

        public ActionResult myMessage()
        {
            return View();
        }

        public ActionResult MsgShow()
        {
            string id = Request.QueryString["id"];
            string UserID = HttpContext.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                Info info = mydb.Infos.Find(id);
                if (!UserID.Equals(info.Creator.ID))
                {
                    info.Receivers.FirstOrDefault(r => r.Receiver.ID.Equals(UserID)).ReadDate = DateTime.Now;
                    mydb.SaveChanges();
                }
                ViewData["Title"] = info.Title;
            }
            return View();
        }

        public ActionResult MsgNew()
        {
            return View();
        }
    }
}
