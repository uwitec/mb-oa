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
    }
}
