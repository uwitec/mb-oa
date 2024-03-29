﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib;
using System.IO;
using System.Web.Configuration;

namespace BuizApp.Areas.office.Controllers
{
    public partial class myOfficeController : Controller
    {

        //
        // GET: /office/myOffice/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult myCarlendar()
        {


            return View();
            //return this.Redirect("/content/extensible-1.0/examples/calendar/test-app.html");
        }

        public ActionResult myRemind()
        {
            return View();
        }

        public ActionResult myMessage()
        {
            return View();
        }

        /// <summary>
        /// 显示消息
        /// Request.QueryString["id"]： 消息id
        /// </summary>
        /// <returns></returns>
        public ActionResult MsgShow()
        {
            string id = Request.QueryString["id"];
            string UserID = HttpContext.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                Info info = mydb.Infos.Find(id);
                EntityObjectLib.InfoInbox mi = info.Receivers.FirstOrDefault(r => r.Receiver.ID.Equals(UserID));
                if (mi != null)
                {
                    info.Receivers.FirstOrDefault(r => r.Receiver.ID.Equals(UserID)).ReadDate = DateTime.Now;
                    mydb.SaveChanges();
                }

                ViewBag.IsSender = UserID.Equals(info.Creator.ID);
                ViewBag.info = info;
                ViewBag.Creator = info.Creator.Name;
                ViewBag.Receivers = string.Join(",", info.Receivers.Select(r => r.Receiver.Name).ToArray());
                ViewBag.files = info.InfoFiles.Select(f => string.Format("<a href='/uploads/{1}' target='_blank'>{0}</a>", f.File.Name, f.File.ID + f.File.Suffix)).ToArray();
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
                    case "reply": // 消息回复
                        ViewBag.Receiver = string.Format("{0}({1})", info.Creator.Name, info.Creator.Code);
                        ViewBag.ParentID = id;
                        ViewBag.Title = string.Format("回复:{0}", info.Title);
                        ViewBag.Content = string.Format("<br/><br/><hr>{0}", info.Content);
                        ViewBag.hideReceiver = "false";
                        break;
                    case "transmit": // 消息转发
                        ViewBag.parentID = id;
                        ViewBag.Title = string.Format("转发:{0}", info.Title);
                        ViewBag.Content = string.Format("<br/><br/><hr/>{0}", info.Content);
                        ViewBag.hideReceiver = "false";
                        break;
                    case "board": // 新建公告板主题
                        ViewBag.hideReceiver = "true";
                        break;
                    case "boardReply": // 新建公告板回复
                        //找到订阅此内容所有主题的用户,将其置为收件人
                        Info root = info;
                        while (root.Parent != null)
                        {
                            root = root.Parent;
                        }

                        ViewBag.Receiver = string.Join(",", root.Subscriptions.Select(s => s.Owner).Select(u => string.Format("{0}({1})", u.Name, u.Code)).ToArray());
                        //////////
                        
                        ViewBag.parentID = id;
                        ViewBag.Title = string.Format("回复:{0}", info.Title);
                        ViewBag.Content = string.Format("<br/><br/><hr/>{0}", info.Content);
                        ViewBag.hideReceiver = "true";
                        break;
                    default: // 新建消息
                        ViewBag.hideReceiver = "false";
                        break;
                }
            }
            return View();
        }

        public ActionResult imgSend()
        {
            using (MyDB mydb = new MyDB())
            {
                HttpPostedFileBase file = Request.Files[0];
                FileInfo fi = new FileInfo(file.FileName);
                string fileName = string.Format("{0}{1}", Guid.NewGuid().ToString(), fi.Extension);
                file.SaveAs(Server.MapPath("~/" + fileName));

                Response.Clear();
                return Json(new { success = true, fileName }, "text/html");
            }
        }

        public string fileUpload()
        {
            // 保存传来的文件
            HttpPostedFileBase file = Request.Files["Filedata"]; // 在FileData里
            FileInfo fi = new FileInfo(file.FileName);
            string fileID = Guid.NewGuid().ToString();
            string uploadDir = WebConfigurationManager.AppSettings["uploadDir"];
            file.SaveAs(Server.MapPath(uploadDir + fileID + fi.Extension));

            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.File f = new EntityObjectLib.File
                {
                    ID = fileID,
                    CreateDate = DateTime.Now,
                    Name = file.FileName,
                    Creator = mydb.Users.Find(HttpContext.User.Identity.Name),
                    UploadPath = uploadDir,
                    Suffix = fi.Extension
                };
                mydb.Files.Add(f);
                mydb.SaveChanges();
            }
            
            //System.Threading.Thread.Sleep(3000);

            //// 回传
            Response.StatusCode = 200; //成功
            return fileID;

        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MsgSend()
        {
            // 如果字符串含双引号，则将截断双引号及后面的字符，但在其他表单里是没有的，可能与
            // 该表单含文件上传有关
            string Receivers = Request.Form["Receivers"];
            string Title = Request.Form["Title"];
            string Content = Request.Form["Content"].Replace("\r\n",""); //去掉回车
            string ParentID = Server.HtmlEncode(Request.Form["ParentID"]);
            string hiddenFileIDs = Request.Form["hiddenFileIDs"];

            string infoID = Guid.NewGuid().ToString();
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.File[] files = mydb.Files.Where(f => hiddenFileIDs.Contains(f.ID)).ToArray();
                EntityObjectLib.Info info = new Info
                {
                    ID = infoID,
                    Title = Title,
                    Content = Content,
                    CreateDate = DateTime.Now,
                    SendDate = DateTime.Now,
                    Creator = mydb.Users.Find(HttpContext.User.Identity.Name),
                    SendTypes = "",
                    Receivers = string.IsNullOrEmpty(Receivers) ? null : Receivers.Split(",".ToCharArray()).Select(r => new EntityObjectLib.InfoInbox
                    {
                        ID = Guid.NewGuid().ToString(),
                        Receiver = mydb.Users.FirstOrDefault(u => r.Equals(u.Name + "(" + u.Code + ")")),
                        ReceiveTypes = ""
                    }).ToArray(),
                    InfoFiles = files.Select(f =>
                            new InfoFile
                            {
                                ID = Guid.NewGuid().ToString(),
                                File = f,
                                FileName = f.Name,
                                UploadDate = DateTime.Now
                            }
                        ).ToArray(),
                    Parent = mydb.Infos.Find(ParentID)/*,
                    Board = mydb.InfoBoards.First()*/
                };
                /*
                List<EntityObjectLib.File> files = new List<EntityObjectLib.File>();
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
                */

                mydb.Infos.Add(info);

                mydb.SaveChanges();
            }

            Response.Clear();
            return Json(new { success = true, ID = infoID }, "text/html");
        }

        public ActionResult mySubscription()
        {
            return View();
        }

        public ActionResult updateMySubscriptions()
        {
            IEnumerable<string> Ids = Request.Params["IDs"].Split(",".ToArray()).AsEnumerable(); //新的角色ID串
            string userID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                IQueryable<InfoSubscription> subscriptions = mydb.InfoSubscriptions.Where(s => s.Owner.ID.Equals(userID));

                foreach (InfoSubscription s in subscriptions)
                {
                    if(!Ids.Contains(s.Title.ID))
                        mydb.InfoSubscriptions.Remove(s);
                }

                string[] appendIDS = Ids.Except(subscriptions.Select(s=>s.Title.ID)).ToArray();
                foreach (string s in appendIDS)
                {
                    InfoSubscription sub = new InfoSubscription();
                    sub.ID = Guid.NewGuid().ToString();
                    sub.Owner = mydb.Users.Find(userID);
                    sub.Title = mydb.Infos.Find(s);

                    mydb.InfoSubscriptions.Add(sub);
                }
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        public ActionResult boards()
        {
            return View();
        }

        public ActionResult loadReceiverType()
        {
            string userID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                User user = mydb.Users.Find(userID);
                string[] ss = user.InfoReceiveTypes == null ? new string[] { } : user.InfoReceiveTypes.Split(",".ToCharArray());
                return Json(new
                {
                    success = true,
                    data = new { sms = ss.Contains("sms") ? "on" : "", email = ss.Contains("email") ? "on" : "" }
                });
            }
        }

        public ActionResult saveReceiverType()
        {
            string userID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                User user = mydb.Users.Find(userID);
                List<string> ss = new List<string>();
                if (Request.Form["sms"] != null && Request.Form["sms"].Equals("on"))
                    ss.Add("sms");
                if (Request.Form["email"] != null && Request.Form["email"].Equals("on"))
                    ss.Add("email");
                user.InfoReceiveTypes = string.Join(",",ss.ToArray());
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// 我的通讯录
        /// </summary>
        /// <returns></returns>
        public ActionResult myAddressBook()
        {
            return View();
        }

        /// <summary>
        /// 更新通讯录
        /// </summary>
        /// <returns></returns>
        public ActionResult updateAddress()
        {
            string id = Request.Params["id"];
            string field = Request.Params["field"];
            string value = Request.Params["value"];

            string userID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                AddressBook ab = mydb.AddressBooks.Find(id);
                Type type =ab.GetType().GetProperty(field).PropertyType;
                mydb.Entry<AddressBook>(ab).Property(field).CurrentValue = Convert.ChangeType(value, type); //需要类型转换
                ab.LastUpdateTime = DateTime.Now;
                mydb.SaveChanges();
                return Json(new { success = true });
            }
        }

        /// <summary>
        /// 创建联系人
        /// </summary>
        /// <returns></returns>
        public ActionResult createAddress()
        {
            string Name = Request.Params["Name"];

            string userID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                AddressBook ab = new AddressBook
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = Name,
                    Creator = mydb.Users.Find(this.User.Identity.Name),
                    Owner = mydb.Users.Find(this.User.Identity.Name),
                    CreateTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now
                };
                mydb.AddressBooks.Add(ab);
                mydb.SaveChanges();
                return Json(new { success = true });
            }
        }
    }
}
