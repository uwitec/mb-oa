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
        public ActionResult getEvent()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Event p = mydb.Events.Find(Request.Params["ID"]);
                return Json(new { p.ID, p.Name, p.Content, p.Type });
            }
        }

        public ActionResult saveEvent()
        {
            string ID = Request.Form["ID"];
            string Name = Request.Form["Name"];
            string Content = Request.Form["Content"];
            string Type = Request.Form["Type"];
            string Parent = Request.Form["Parent"];
            string StartDate = Request.Form["StartDate"];
            string StartTime = Request.Form["StartTime"];
            string FinishDate = Request.Form["FinishDate"];
            string FinishTime = Request.Form["FinishTime"];
            string Master = Request.Form["Master"];
            string Proctor = Request.Form["Proctor"];

            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Event p = new Event
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = Name,
                    Content = Content,
                    Type = Type,
                    Parent = mydb.Events.Find(Parent),
                    StartTime = DateTime.Parse(StartDate +" " + StartTime),
                    FinishTime = DateTime.Parse(FinishDate + " " + FinishTime),
                    Master = mydb.Users.Find(Master),
                    Proctor = mydb.Users.Find(Proctor),
                    Creator = mydb.Users.Find(this.User.Identity.Name),
                    CreateTime = DateTime.Now
                };

                mydb.Events.Add(p);

                mydb.SaveChanges();

                return Json(new
                {
                    success = true
                }
                );
            }
        }

        public ActionResult UpdateEvent()
        {
            return View();
        }
    }
}
