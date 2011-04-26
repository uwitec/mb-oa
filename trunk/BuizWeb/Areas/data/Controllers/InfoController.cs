using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;

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
                            mydb.MyInfos.Where(info => info.Receiver.ID.Equals(UserID))
                            .Select(info => info.Info).OrderBy(info => info.SendDate).ToList();//ToList不能少,要本地化,才能执行后面的string.Join
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
                            mydb.MyInfos.Where(info => info.Receiver.ID.Equals(UserID) && info.ReadDate==null)
                            .Select(info => info.Info).OrderBy(info => info.SendDate).ToList();//ToList不能少,要本地化,才能执行后面的string.Join
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
                            .OrderBy(info => info.SendDate).ToList();//ToList不能少,要本地化,才能执行后面的string.Join
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
                    mydb.MyInfos.Where(info => info.Receiver.ID.Equals(UserID) && info.ReadDate == null)
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
                    mydb.Infos.Where(i => i.Board != null)
                    .GroupJoin(mydb.Subscriptions.Where(s => s.Owner.ID.Equals(UserID)),
                    i => i.ID,
                    s => s.Title.ID,
                    (i, s) => new { i.ID, i.Title, Creator = i.Creator.Name, CreateDate = i.CreateDate, @checked = s.Count() > 0 })
                    .ToArray()
                    ;


                return Json(
                    mySubscriptions.Select(x => new { x.ID, x.Creator, CreateDate = x.CreateDate.ToString("yyyy-M-d H:m:s"), x.Title, x.@checked })
                    , JsonRequestBehavior.AllowGet
                    );
            }
        }

        public ActionResult Subscriptions()
        {
            string UserID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                object[] result = mydb.Infos.Where(i => i.Board != null).Select(x => new { x.ID, x.Title,x.SendDate,x.Creator.Name,x.Content }).ToArray();
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
                object[] mySubscriptions = mydb.Subscriptions.Where(s => s.Owner.ID.Equals(this.User.Identity.Name))
                    .Select(s => new { s.Title.ID, s.Title.Title }).ToArray();

                return Json(
                    mySubscriptions
                    , JsonRequestBehavior.AllowGet
                    );
            }
        }
    }
}
