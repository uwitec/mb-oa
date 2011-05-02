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

        //saveEventState

        public ActionResult saveEventState()
        {
            string ID = Request.Form["ID"];
            string eventId = Request.Form["eventId"];
            string StateDate = Request.Form["StateDate"];
            string StateTime = Request.Form["StateTime"];
            string PlanRadio = Request.Form["PlanRadio"];
            string AcutalRadio = Request.Form["AcutalRadio"];
            string Description = Request.Form["Description"];

            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.EventState p = new EventState
                {
                    ID = Guid.NewGuid().ToString(),
                    Event = mydb.Events.Find(eventId),
                    StateTime = DateTime.Parse(StateDate + " " + StateTime),
                    Description = Description,
                    Creator = mydb.Users.Find(this.User.Identity.Name),
                    CreateTime = DateTime.Now
                };

                if (!string.IsNullOrEmpty(PlanRadio))
                    p.PlanRadio = float.Parse(PlanRadio);
                if (!string.IsNullOrEmpty(AcutalRadio))
                    p.AcutalRadio = float.Parse(AcutalRadio);

                mydb.EventStates.Add(p);

                mydb.SaveChanges();

                return Json(new
                {
                    success = true
                }
                );
            }
        }

        //saveEventRemind

        public ActionResult saveEventRemind()
        {
            string ID = Request.Form["ID"];
            string eventId = Request.Form["eventId"];
            string RemindDate = Request.Form["RemindDate"];
            string RemindTime = Request.Form["RemindTime"];
            string RemindContent = Request.Form["RemindContent"];
            
            List<string> ReceiverTypes = new List<string>();
            if (Request.Form["ReceiverTypeMaster"] != null)
            {
                ReceiverTypes.Add("责任人");
            }
            if (Request.Form["ReceiverTypeProctor"] != null)
            {
                ReceiverTypes.Add("督办人");
            }
            if (Request.Form["ReceiverTypeShare"] != null)
            {
                ReceiverTypes.Add("共享人");
            }
            string ReceiverType = string.Join(",", ReceiverTypes.ToArray());

            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.EventRemind p = new EventRemind
                {
                    ID = Guid.NewGuid().ToString(),
                    Event = mydb.Events.Find(eventId),
                    RemindTime = DateTime.Parse(RemindDate + " " + RemindTime),
                    RemindContent = RemindContent,
                    ReceiverType = ReceiverType
                };

                mydb.EventReminds.Add(p);

                mydb.SaveChanges();

                return Json(new
                {
                    success = true
                }
                );
            }
        }

        //saveEventShare

        public ActionResult saveEventShare()
        {
            string ID = Request.Form["ID"];
            string eventId = Request.Form["eventId"];
            string Subject = Request.Form["Subject"];
            bool NeedRemind = Request.Form["NeedRemind"] != null;

            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.EventShare p = new EventShare
                {
                    ID = Guid.NewGuid().ToString(),
                    Event = mydb.Events.Find(eventId),
                    Subject = mydb.Subjects.Find(Subject),
                    NeedRemind = NeedRemind
                };

                mydb.EventShares.Add(p);

                mydb.SaveChanges();

                return Json(new
                {
                    success = true
                }
                );
            }
        }
    }
}
