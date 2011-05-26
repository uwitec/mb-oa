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
    public class FWTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (MyDB mydb = new MyDB())
            {
                FWType type1 = new FWType
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "发文类别一",
                    FWPrefixes = "1A发,1B发,1C发",
                    FWTemplates = new[]{
                            new FWTemplate
                            {
                                ID = Guid.NewGuid().ToString(),
                                 Name="发文模板1A",
                                  TemplateFile = mydb.Files.First()
                            },
                            new FWTemplate
                            {
                                ID = Guid.NewGuid().ToString(),
                                Name="发文模板1B",
                                TemplateFile = mydb.Files.First()
                            }
                    }
                };
                FWType type2 = new FWType
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "发文类别二",
                    FWPrefixes = "2A发,2B发,2C发",
                    FWTemplates = new[]{
                            new FWTemplate
                            {
                                ID = Guid.NewGuid().ToString(),
                                 Name="发文模板2A",
                                 TemplateFile = mydb.Files.First()
                            },
                            new FWTemplate
                            {
                                ID = Guid.NewGuid().ToString(),
                                Name="发文模板2B",
                                TemplateFile = mydb.Files.First()
                            }
                    }
                };

                FWInfo fw = new FWInfo
                {
                    ID = Guid.NewGuid().ToString(),
                    FWType = type1,
                    FWTemplate = type1.FWTemplates.First(),
                    FWPrefix = "1B发",
                    FWYear = DateTime.Now.Year,
                    FWNO = 10,
                    Urgency = "普通",
                    Secrecy = "保密",
                    Title = "关于****项目实施的通知",
                    Keywords = "项目,实施,通知",
                    Summary = @"****项目已于某年某月某日测试完成,现已进入实施阶段,请各部门在信息部领导下积极参与实施.",
                    Recipient = "各部门",
                    Participator = "信息部",
                    Drafter = mydb.Users.First(u => u.Code == "chw"),
                    DraftTime = DateTime.Now,
                    CopyAmount = 10,
                    FWInfoFiles = new []{
                        new FWInfoFile
                        {
                            ID = Guid.NewGuid().ToString(),
                            FileName="test.ddd",
                            UploadDate = DateTime.Now,
                            File = mydb.Files.First()
                        }
                    },
                    WFTemplate=mydb.WFTemplates.First(), // 参与流程后,创建业务实例一定要有流程模板
                    Creator = mydb.Users.First()
                };

                mydb.FWTypes.Add(type1);
                mydb.FWTypes.Add(type2);
                //mydb.SaveChanges();
                mydb.FWInfos.Add(fw);
                mydb.SaveChanges();
            }

        }

        [TestMethod]
        public void clear()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.FWInfoFiles.Load();
                mydb.FWInfoFiles.Local.Clear();

                mydb.FWInfos.Load();
                mydb.FWInfos.Local.Clear();

                mydb.FWTemplates.Load();
                mydb.FWTemplates.Local.Clear();

                mydb.FWTypes.Load();
                mydb.FWTypes.Local.Clear();

                mydb.SaveChanges();
            }
        }
    }
}
