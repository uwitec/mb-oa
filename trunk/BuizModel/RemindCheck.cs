using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EntityObjectLib;
using EntityObjectContext;

namespace BuizModel
{
    public class RemindCheck
    {
        public static void CheckALL()
        {
            using (MyDB mydb = new MyDB())
            {
                // 检出需要提醒的事件
                IQueryable<EventRemind> eventReminds =
                    mydb.EventReminds
                    .Where(er => er.SendTime == null && er.RemindTime <= DateTime.Now)
                    .Select(er => er);

                foreach (EventRemind eventRemind in eventReminds)
                {
                    string[] sendTypes = string.IsNullOrEmpty(eventRemind.ReceiverType)
                            ? new string[]{}
                            :eventRemind.ReceiverType.Split(",".ToArray());
                    foreach (string sendType in sendTypes)
                    {
                        switch (sendType)
                        {
                            case "责任人":
                                if (eventRemind.Event.Master != null)
                                {
                                    SendRemind(eventRemind.Event.Master, eventRemind.Event.Name, eventRemind.Event.Content, mydb);
                                }
                                break;
                            case "督办人":
                                if (eventRemind.Event.Master != null)
                                {
                                    SendRemind(eventRemind.Event.Proctor, eventRemind.Event.Name, eventRemind.Event.Content, mydb);
                                }
                                break;
                            case "共享人":
                                foreach (Subject subject in eventRemind.Event.EventShares.Select(es => es.Subject).Distinct())
                                {
                                    if (subject is User)
                                    {
                                        SendRemind(subject as User, eventRemind.Event.Name, eventRemind.Event.Content, mydb);
                                    }
                                    if (subject is Organization)
                                    {
                                        foreach (User u in (subject as Organization).Users)
                                        {
                                            SendRemind(u, eventRemind.Event.Name, eventRemind.Event.Content, mydb);
                                        }
                                    }
                                }
                                break;
                            default: break;
                        }
                    }

                    eventRemind.SendTime = DateTime.Now;
                }

                mydb.SaveChanges();
                return;
            }
        }

        private static void SendRemind(User user, string title, string Content, MyDB db)
        {
            User sender = db.Users.First(u => u.Code.Equals("remindRobot")); //后台模拟用户
            Info info = new Info()
            {
                ID = Guid.NewGuid().ToString(),
                Title = "[提醒]"+title,
                Content = Content,
                CreateDate = DateTime.Now,
                SendDate = DateTime.Now,
                Creator = sender,
                SendTypes = "",
                Type = "提醒",
                Receivers = new InfoInbox[]
                    {
                        new InfoInbox {
                        ID = Guid.NewGuid().ToString(),
                        Receiver = user,
                        ReceiveTypes = ""
                        }
                    }
            };

            db.Infos.Add(info);
        }
    }
}
