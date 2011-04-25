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
                EntityObjectLib.MyInfo mi = info.Receivers.FirstOrDefault(r => r.Receiver.ID.Equals(UserID));
                if (mi != null)
                {
                    info.Receivers.FirstOrDefault(r => r.Receiver.ID.Equals(UserID)).ReadDate = DateTime.Now;
                    mydb.SaveChanges();
                }

                ViewBag.IsSender = UserID.Equals(info.Creator.ID);
                ViewBag.info = info;
                ViewBag.Creator = info.Creator.Name;
                ViewBag.Receivers = string.Join(",", info.Receivers.Select(r => r.Receiver.Name).ToArray());
                ViewBag.files = info.InfoFiles.Select(f => string.Format("<a href='/{1}' target='_blank'>{0}</a>", f.File.Name, f.File.Name)).ToArray();
                ViewBag.children = info.Children.Select(inf => string.Format("<a href=javascript:this.parent.addTab('/office/myOffice/MsgShow?ID={0}','消息:{3}') target='_blank'>[{1}]{2}:{3}</a>", inf.ID, inf.CreateDate, inf.Creator.Name, inf.Title)).ToArray();
            }
            return View();
        }

        public ActionResult MsgNew()
        {
            string type = Request.QueryString["type"];
            string id = Request.QueryString["id"];

            Info info;
            using (MyDB mydb = new MyDB())
            {
                info = mydb.Infos.Find(id);
                switch (type)
                {
                    case "reply":
                        ViewBag.Receiver = string.Format("{0}({1})", info.Creator.Name, info.Creator.Code);
                        ViewBag.ParentID = id;
                        ViewBag.Title = string.Format("回复:{0}", info.Title);
                        ViewBag.Content = string.Format("<br/><br/><hr/>{0}", info.Content);
                        break;
                    case "transmit":
                        ViewBag.parentID = id;
                        ViewBag.Title = string.Format("转发:{0}", info.Title);
                        ViewBag.Content = string.Format("<br/><br/><hr/>{0}", info.Content);
                        break;
                    default:
                        break;
                }
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult MsgSend()
        {
            string Receivers = Request.Form["Receivers"];
            string Title = Server.HtmlEncode(Request.Form["Title"]);
            string Content = Server.HtmlEncode(Request.Form["Content"]);
            string ParentID = Request.Form["ParentID"];


            using (MyDB mydb = new MyDB())
            {
                List<EntityObjectLib.File> files = new List<File>();
                for (int i = 0; i < Request.Files.Count; i++)
                {

                    if (Request.Files[i].ContentLength > 0)
                    {
                        Request.Files[i].SaveAs(Server.MapPath("~/" + Request.Files[i].FileName));

                        EntityObjectLib.File file = new EntityObjectLib.File
                        {
                            ID = Guid.NewGuid().ToString(),
                            CreateDate = DateTime.Now,
                            Name = Request.Files[i].FileName,
                            Creator = mydb.Users.Find(HttpContext.User.Identity.Name),
                            UploadPath = "~/",
                            Suffix = ""
                        };
                        files.Add(file);
                    }
                }

                EntityObjectLib.Info info = new Info
                {
                    ID = Guid.NewGuid().ToString(),
                    Title = Title,
                    Content = Content,
                    CreateDate = DateTime.Now,
                    SendDate = DateTime.Now,
                    Creator = mydb.Users.Find(HttpContext.User.Identity.Name),
                    SendTypes = "",
                    Receivers = Receivers.Split(",".ToCharArray()).Select(r => new EntityObjectLib.MyInfo
                    {
                        ID = Guid.NewGuid().ToString(),
                        Receiver = mydb.Users.FirstOrDefault(u => r.Equals(u.Name + "(" + u.Code + ")")),
                        ReceiveTypes = ""
                    }).ToArray(),
                    InfoFiles = files.Select(f => new EntityObjectLib.InfoFile
                    {
                        ID = Guid.NewGuid().ToString(),
                        FileName = f.Name,
                        UploadDate = DateTime.Now,
                        File = f
                    }).ToArray(),
                    Parent = mydb.Infos.Find(ParentID)
                };

                mydb.Infos.Add(info);

                mydb.SaveChanges();
            }

            Response.Clear();
            return Json(new { success = true }, "text/html");
        }
    }
}
