using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib;

namespace BuizApp.Areas.office.Controllers
{
    public partial class myOfficeController : Controller
    {
        public ActionResult myMemo()
        {
            return View();
        }

        public ActionResult getMemo()
        {
            string id = Request.Params["id"];

            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Event p = mydb.Events.Find(id);
                return Json(new
                {
                    success = true,
                    data = new { p.ID, p.Name, p.Content, p.Type, p.CreateTime }
                });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult saveMemo()
        {
            string id = Request.Params["id"];

            using (MyDB mydb = new MyDB())
            {
                User user = mydb.Users.Find(this.User.Identity.Name);
                if (string.IsNullOrEmpty(id))
                {
                    Event ev = new Event
                    {
                        ID = Guid.NewGuid().ToString(),
                        Name = Request.Params["Name"],
                        Content = Request.Params["Content"],
                        Type = "便笺",
                        Master = user,
                        Proctor = user,
                        Creator = user,
                        CreateTime = DateTime.Now
                    };
                    mydb.Events.Add(ev);
                }
                else
                {
                    Event ev = mydb.Events.Find(id);
                    ev.Name = Request.Params["Name"];
                    ev.Content = Request.Params["Content"];
                }

                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }
    }
}
