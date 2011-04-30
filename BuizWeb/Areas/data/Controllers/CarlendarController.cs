using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;

namespace BuizApp.Areas.data.Controllers
{
    public class CarlendarController : Controller
    {
        class Day
        {
            public EntityObjectLib.Event ev;
            public DateTime day;
        }

        public JsonResult Index()
        {
            int year = Convert.ToInt16(Request.Params["year"]);
            int month = Convert.ToInt16(Request.Params["month"]);
            // 当前只取当前月的,以后要改为根据用户指定年月生成
            DateTime monthBegin = new DateTime(year, month, 1);
            DateTime monthEnd = monthBegin.AddMonths(1).AddDays(-1);

            List<Day> monthDays = new List<Day>();
            DateTime i = monthBegin;
            for (; i <= monthEnd; )
            {
                monthDays.Add(new Day { day = i });
                i = i.AddDays(1);
            }
            using (MyDB mydb = new MyDB())
            {
                object[] result =
                    mydb.Events
                    .Include("Master")
                    .Include("Proctor")
                    .Where(ev => (ev.FinishTime > monthEnd ? monthEnd : ev.FinishTime) >= (ev.StartTime > monthBegin ? ev.StartTime : monthBegin))
                    .ToList()
                    .AsQueryable()
                    .Join( //笛卡尔积
                        monthDays,
                        r => 1,
                        e => 1,
                        (r, e) => new Day { ev = r, day = e.day }
                    )
                    .Where(d => d.day >= d.ev.StartTime && d.day <= d.ev.FinishTime)
                    .Select(d =>
                        new
                        {
                            d.day,
                            d.ev.ID,
                            d.ev.Name,
                            d.ev.Content,
                            d.ev.StartTime,
                            d.ev.FinishTime,
                            d.ev.Type,
                            Master = d.ev.Master.Name,
                            Proctor = d.ev.Proctor.Name
                        }
                    ).ToArray();

                return Json(
                        result
                        , JsonRequestBehavior.AllowGet
                        );
            }
        }

    }
}
