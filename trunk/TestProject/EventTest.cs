using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;
using EntityObjectLib;
using System.Data.Entity;

namespace TestProject1
{
    [TestClass]
    public class EventTest
    {
        [TestMethod]
        public void Append()
        {
            using (MyDB mydb = new MyDB())
            {
                #region "创建事件"
                Event myevent = new Event
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "我的事件B",
                    Parent = null,
                    Content = "我的事件BBBBBBBBBBBBBBBBBBBBBBBBBBBBB",
                    Type = "任务",
                    StartTime = new DateTime(2011, 4, 13, 11, 0, 0),
                    FinishTime = new DateTime(2011, 5, 12, 12, 1, 1),
                    Creator = mydb.Users.First(u => u.Code.Equals("chw")),
                    CreateTime = DateTime.Now,
                    Master = mydb.Users.First(u => u.Code.Equals("lilin")),
                    Proctor = mydb.Users.First(u => u.Code.Equals("chw")),
                    EventShares = new EventShare[] {
                        new EventShare
                        {
                            ID = Guid.NewGuid().ToString(),
                            Subject = mydb.Subjects.OfType<User>().First(s=>s.Code.Equals("lilin")),
                            NeedRemind = true
                        },                        
                        new EventShare
                        {
                            ID = Guid.NewGuid().ToString(),
                            Subject = mydb.Subjects.OfType<User>().First(s=>s.Code.Equals("chw")),
                            NeedRemind = true
                        }
                    },
                    EventStates = new EventState[]{
                        new EventState
                        {
                            ID = Guid.NewGuid().ToString(),
                            StateTime = new DateTime(2011,3,12,12,1,1),
                            PlanRadio = 0.3,
                            AcutalRadio = 0.2,
                            Description = "DescriptionDescriptionDescription",
                            Creator = mydb.Users.First(u=>u.Code.Equals("chw")),
                            CreateTime = DateTime.Now
                        },
                        new EventState
                        {
                            ID = Guid.NewGuid().ToString(),
                            StateTime = new DateTime(2011,3,18,12,1,1),
                            PlanRadio = 0.5,
                            AcutalRadio = 0.4,
                            Description = "DescriptionDescriptionDescription",
                            Creator = mydb.Users.First(u=>u.Code.Equals("lilin")),
                            CreateTime = DateTime.Now
                        }
                    },
                    EventReminds = new EventRemind[]{
                        new EventRemind
                        {
                            ID = Guid.NewGuid().ToString(),
                            RemindTime = new DateTime(2011,3,18,12,1,1),
                            RemindContent = "dafsdfasdF",
                            ReceiverType = "责任人",
                            SendTime = new DateTime(2011,3,19,12,1,1)
                        }
                        ,
                        new EventRemind
                        {
                            ID = Guid.NewGuid().ToString(),
                            RemindTime = new DateTime(2011,4,18,12,1,1),
                            RemindContent = "2011,4,18,12,1,1",
                            ReceiverType = "督办人,共享人",
                            SendTime = null
                        }
                    }
                };
                #endregion

                mydb.Events.Add(myevent);
                mydb.SaveChanges();
            }
        }

        [TestMethod]
        public void clear()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.EventReminds.Load();
                mydb.EventStates.Load();
                mydb.EventShares.Load();
                mydb.Events.Load();
                mydb.AddressBookShares.Load();
                mydb.AddressBooks.Load();

                mydb.EventReminds.Local.Clear();
                mydb.EventStates.Local.Clear();
                mydb.EventShares.Local.Clear();
                mydb.Events.Local.Clear();
                mydb.AddressBookShares.Local.Clear();
                mydb.AddressBooks.Local.Clear();

                mydb.SaveChanges();
            }
        }

        class Day
        {
            public Event ev;
            public DateTime day;
        }
        [TestMethod]
        public void test()
        {
            // 当前只取当前月的,以后要改为根据用户指定年月生成
            DateTime monthBegin = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
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
                IQueryable result =
                    mydb.Events.Include("Master").Include("Proctor")
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
                    )//.ToArray()
                    ;

                //return Json(
                //        result
                //        , JsonRequestBehavior.AllowGet
                //        );
            }
        }
    }
}
