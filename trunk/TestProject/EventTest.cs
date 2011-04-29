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
                    Name = "我的事件A",
                    Parent = null,
                    Content = "我的事件A我的事件我的事件我的事件我的事件",
                    Type = "任务",
                    StartTime = new DateTime(2011, 2, 3, 11, 0, 0),
                    FinishTime = new DateTime(2011, 4, 12, 12, 1, 1),
                    Creator = mydb.Users.First(u => u.Code.Equals("chw")),
                    CreateTime = DateTime.Now,
                    Master = mydb.Users.First(u => u.Code.Equals("lilin")),
                    Proctor = mydb.Users.First(u => u.Code.Equals("lw")),
                    EventShares = new EventShare[] {
                        new EventShare
                        {
                            ID = Guid.NewGuid().ToString(),
                            Subject = mydb.Subjects.OfType<User>().First(s=>s.Code.Equals("lxx")),
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
                            Creator = mydb.Users.First(u=>u.Code.Equals("lxx")),
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

                mydb.EventReminds.Local.Clear();
                mydb.EventStates.Local.Clear();
                mydb.EventShares.Local.Clear();
                mydb.Events.Local.Clear();

                mydb.SaveChanges();
            }
        }
    }
}
