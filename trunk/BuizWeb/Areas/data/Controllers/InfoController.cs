using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using System.Data.Entity;

namespace BuizApp.Areas.data.Controllers
{
    public class InfoController : Controller
    {
        /// <summary>
        /// /data/info,data/info/index
        /// 取我的消息(收件箱)
        /// </summary>
        /// <returns>JSON格式</returns>
        public ActionResult Index()
        {
            // unread:未读消息; all:收件箱全部消息; outbox:发件箱
            string type = Request.Params["type"];
            string UserID = HttpContext.User.Identity.Name;

            switch (type)
            {
                case "all":
                    using (MyDB mydb = new MyDB())
                    {
                        List<EntityObjectLib.Info> infoes =
                            mydb.InfoInboxs.Where(info => info.Receiver.ID.Equals(UserID))
                            .OrderByDescending(info=>info.Info.SendDate)
                            .Select(info => info.Info).ToList();//ToList不能少,要本地化,才能执行后面的string.Join
                        object[] data = infoes.AsQueryable().Select(info => new
                            {
                                info.ID,
                                info.Title,
                                Sender = info.Creator.Name,
                                SendDate = info.SendDate.ToString("yyyy年M月d日"),
                                SendDateTime = info.SendDate.ToString("yyyy年M月d日 HH:mm"),
                                Receivers = string.Join(",", info.Receivers.Select(r => r.Receiver.Name).ToArray())
                            }).ToArray();

                        return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
                    }
                case "unread":
                    using (MyDB mydb = new MyDB())
                    {
                        List<EntityObjectLib.Info> infoes =
                            mydb.InfoInboxs.Where(info => info.Receiver.ID.Equals(UserID) && info.ReadDate==null)
                            .OrderByDescending(info => info.Info.SendDate)
                            .Select(info => info.Info).ToList();//ToList不能少,要本地化,才能执行后面的string.Join
                        object[] data = infoes.AsQueryable().Select(info => new
                        {
                            info.ID,
                            info.Title,
                            Sender = info.Creator.Name,
                            SendDate = info.SendDate.ToString("yyyy年M月d日"),
                            SendDateTime = info.SendDate.ToString("yyyy年M月d日 HH:mm"),
                            Receivers = string.Join(",", info.Receivers.Select(r => r.Receiver.Name).ToArray())
                        }).ToArray();

                        return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
                    }
                case "outbox":
                    using (MyDB mydb = new MyDB())
                    {
                        List<EntityObjectLib.Info> infoes =
                            mydb.Infos.Where(info => info.Creator.ID.Equals(UserID))
                            .OrderByDescending(info => info.SendDate)
                            .ToList();//ToList不能少,要本地化,才能执行后面的string.Join
                        object[] data = infoes.AsQueryable().Select(info => new
                        {
                            info.ID,
                            info.Title,
                            Sender = info.Creator.Name,
                            SendDate = info.SendDate.ToString("yyyy年M月d日"),
                            SendDateTime = info.SendDate.ToString("yyyy年M月d日 HH:mm"),
                            Receivers = string.Join(",", info.Receivers.Select(r => r.Receiver.Name).ToArray())
                        }).ToArray();

                        return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        /// data/info/unReadCount
        /// 取当前用户的未读消息数
        /// </summary>
        /// <returns></returns>
        public string unReadCount()
        {
            string UserID = HttpContext.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                int count =
                    mydb.InfoInboxs.Where(info => info.Receiver.ID.Equals(UserID) && info.ReadDate == null)
                    .Count();

                return count.ToString();
            }
        }

        public ActionResult mySubscription()
        {
            string UserID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                var mySubscriptions =
                    mydb.Infos.Where(i => i.Board != null && i.Parent==null)
                    .GroupJoin(mydb.InfoSubscriptions.Where(s => s.Owner.ID.Equals(UserID)),
                    i => i.ID,
                    s => s.Title.ID,
                    (i, s) => new { i.ID, i.Title, Creator = i.Creator.Name, CreateDate = i.CreateDate, @checked = s.Count() > 0 })
                    .ToArray()
                    ;


                return Json(
                    mySubscriptions.Select(x => new { x.ID, x.Creator, x.CreateDate, x.Title, x.@checked })
                    , JsonRequestBehavior.AllowGet
                    );
            }
        }

        public ActionResult Subscriptions()
        {
            string UserID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                object[] result = mydb.Infos
                    .Where(i => i.Board != null && i.Parent==null)
                    .OrderBy(i => i.SendDate)
                    .Select(x => new { x.ID, x.Title, x.SendDate, Sender = x.Creator.Name, x.Content })
                    .ToArray();
                return Json(
                    result
                    , JsonRequestBehavior.AllowGet
                    );
            }
        }

        public ActionResult selfSubscription()
        {
            string UserID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                object[] mySubscriptions = mydb.InfoSubscriptions
                    .Where(s => s.Owner.ID.Equals(this.User.Identity.Name))
                    .Select(s => new { s.Title.ID, s.Title.Title }).ToArray();

                return Json(
                    mySubscriptions
                    , JsonRequestBehavior.AllowGet
                    );
            }
        }

        public ActionResult replyInfos()
        {
            string titleID = Request.Params["titleID"];
            using (MyDB mydb = new MyDB())
            {
                object[] infos = mydb.Infos
                    .Where(s => s.Parent.ID.Equals(titleID))
                    .OrderBy(i => i.SendDate)
                    .Select(x => new { x.ID, x.Title, Sender=x.Creator.Name, x.SendDate, x.Content })
                    .ToArray();

                return Json(
                    infos
                    , JsonRequestBehavior.AllowGet
                    );
            }
        }

        public JsonResult replyInfosTree()
        {
            string titleID = Request.Params["titleID"];
            using (MyDB mydb = new MyDB())
            {
                mydb.Infos.Load();
                object[] result = mydb.Infos.Local.OrderBy(i => i.SendDate)
                    .Where(o => o.Parent!=null&&o.Parent.ID.Equals(titleID)).Select(o => getOrg(o.ID, mydb)).ToArray();
                return Json(new { text = ",", children = result }, JsonRequestBehavior.AllowGet);
            }
        }

        private object getOrg(string infoID, MyDB mydb)
        {
            //EntityObjectLib.Organization org = mydb.Organizations.Local.FirstOrDefault(o => o.ID.Equals(OrgID));
            EntityObjectLib.Info info = mydb.Infos.Local.FirstOrDefault(o => o.ID.Equals(infoID));
            return new
            {
                ID = info.ID,
                info.Title,
                Sender = info.Creator.Name,
                info.SendDate,
                info.Content,
                expanded = true,
                leaf = info.Children.Count == 0/*org.Children.Count() == 0*/,
                //@checked = false,
                //iconCls = "icon-org",
                children = info.Children.Select(o => getOrg(o.ID, mydb)).ToArray()
            };
        }
    }
}
