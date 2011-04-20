using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;
using EntityObjectLib;

namespace TestProject1
{
    [TestClass]
    public class InfoDataInit
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (MyDB mydb = new MyDB())
            {
                Info info = new Info
                {
                    ID = Guid.NewGuid().ToString(),
                    Title = "这是一个测试",
                    Content = "Content",
                    Creator = mydb.Users.FirstOrDefault(u=>u.Code=="chw"),
                    CreateDate = DateTime.Now,
                    SendDate = DateTime.Now,
                    SendTypes = "SMS,Msg,Email",
                    InfoFiles = new InfoFile[]
                    {
                        new InfoFile
                        {
                             ID = Guid.NewGuid().ToString(),
                             FileName = "WWW.doc",
                             UploadDate = DateTime.Now,
                             File = new File
                             {
                                 ID = Guid.NewGuid().ToString(),
                                 Name = "XXX",
                                 Suffix = "doc",
                                 CreateDate = DateTime.Now,
                                 Creator = mydb.Users.FirstOrDefault(u=>u.Code=="chw"),
                                 UploadPath = "~/uploads"
                             }
                        },
                        new InfoFile
                        {
                             ID = Guid.NewGuid().ToString(),
                             FileName = "XXX.jpg",
                             UploadDate = DateTime.Now,
                             File = new File
                             {
                                 ID = Guid.NewGuid().ToString(),
                                 Name = "XXX",
                                 Suffix = "jpg",
                                 CreateDate = DateTime.Now,
                                 Creator = mydb.Users.FirstOrDefault(u=>u.Code=="chw"),
                                 UploadPath = "~/uploads"
                             }
                        }
                    },
                    Receivers = new MyInfo[]
                    {
                        new MyInfo
                        {
                            ID=Guid.NewGuid().ToString(),
                            Receiver = mydb.Users.FirstOrDefault(u=>u.Code=="chw"),
                            ReadDate = DateTime.Now,
                            ReceiveTypes =  "SMS,Msg,Email"
                        },
                        new MyInfo
                        {
                            ID=Guid.NewGuid().ToString(),
                            Receiver = mydb.Users.FirstOrDefault(u=>u.Code=="lilin"),
                            ReadDate = DateTime.Now,
                            ReceiveTypes =  "SMS,Msg,Email"
                        },
                    }
                };

                InfoBoard board = new InfoBoard
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "dfadsfa",
                    Administrator = mydb.Users.FirstOrDefault(u => u.Code == "lilin"),
                    CreateDate = DateTime.Now,
                    Infos = new []{info}
                };

                Subscription sub = new Subscription
                {
                    ID= Guid.NewGuid().ToString(),
                    Owner = mydb.Users.FirstOrDefault(u => u.Code == "lilin"),
                    Title = info,
                    Name = "sdsss",
                    Enable = true,
                    CreateDate = DateTime.Now
                };

                mydb.Infos.Add(info);
                mydb.InfoBoards.Add(board);
                mydb.Subscriptions.Add(sub);

                mydb.SaveChanges();
            }
        }
    }
}
