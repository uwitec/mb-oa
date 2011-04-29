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
    public class InfoTest
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
                    Receivers = new InfoInbox[]
                    {
                        new InfoInbox
                        {
                            ID=Guid.NewGuid().ToString(),
                            Receiver = mydb.Users.FirstOrDefault(u=>u.Code=="chw"),
                            ReadDate = DateTime.Now,
                            ReceiveTypes =  "SMS,Msg,Email"
                        },
                        new InfoInbox
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

                InfoSubscription sub = new InfoSubscription
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
                mydb.InfoSubscriptions.Add(sub);

                mydb.SaveChanges();
            }
        }

        [TestMethod]
        public void clear()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.Files.Load(); mydb.Files.Local.Clear();
                mydb.InfoFiles.Load(); mydb.InfoFiles.Local.Clear();
                mydb.InfoInboxs.Load(); mydb.InfoInboxs.Local.Clear();
                mydb.InfoSubscriptions.Load(); mydb.InfoSubscriptions.Local.Clear();
                mydb.Infos.Load(); mydb.Infos.Local.Clear();
                mydb.InfoBoards.Load(); mydb.InfoBoards.Local.Clear();             

                mydb.SaveChanges();
            }
        }
    }
}
