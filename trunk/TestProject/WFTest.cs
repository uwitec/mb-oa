using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EntityObjectContext;
using EntityObjectLib;
using System.Data.Entity;
using EntityObjectLib.WF;
using System.Linq.Expressions;

namespace TestProject1
{
    [TestClass]
    public class WFTest
    {
        [TestMethod]
        public void InitData()
        {
            clear();

            using (MyDB mydb = new MyDB())
            {
                WFTemplate wft = new WFTemplate
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = "费用申请流程",
                    BuizCode = "ApplyExpense",
                    BuizName = "费用申请",
                    Creator = mydb.Users.First(u => u.Code.Equals("chw")),
                    CreateTime = DateTime.Now,
                    Nodes = new WFNode[]
                    {
                        new WFNodeStart
                        {
                             Name="开始",
                             Next=null
                        },
                        new WFNodeHandle
                        {
                            Name="创建申请",
                            ViewCode="createForm",
                            ViewName="创建表单",
                            Actions = new []
                            {
                                new WFNodeAction
                                {
                                    Code="save",
                                    Name="保存",
                                    NextNode=null //为空表示仍在停留在此节点中
                                },
                                new WFNodeAction
                                {
                                    Code="submit",
                                    Name="提交",
                                    NextNode=null //这个不应该为NULL,但所需的节点还未创建
                                },
                            },
                            Subjects = new []
                            {
                                mydb.Users.First(u => u.Code.Equals("chw")),
                                mydb.Users.First(u => u.Code.Equals("lilin"))
                            }
                        },
                        new WFNodeHandle
                        {
                            Name="部门确认",
                            ViewCode="auditForm",
                            ViewName="审核表单",
                            Actions = new []
                            {
                                new WFNodeAction
                                {
                                    Code="pass",
                                    Name="确认",
                                    NextNode=null //为空表示仍在停留在此节点中
                                },
                                new WFNodeAction
                                {
                                    Code="deny",
                                    Name="否决",
                                    NextNode=null //这个不应该为NULL,但所需的节点还未创建
                                },
                            },
                            Subjects = new []
                            {
                                mydb.Users.First(u => u.Code.Equals("chw"))
                            }
                        },
                        new WFNodeHandle
                        {
                            Name="副总审核",
                            ViewCode="auditForm",
                            ViewName="审核表单",
                            Actions = new []
                            {
                                new WFNodeAction
                                {
                                    Code="pass",
                                    Name="确认",
                                    NextNode=null //为空表示仍在停留在此节点中
                                },
                                new WFNodeAction
                                {
                                    Code="deny",
                                    Name="否决",
                                    NextNode=null //这个不应该为NULL,但所需的节点还未创建
                                },
                            },
                            Subjects = new []
                            {
                                mydb.Users.First(u => u.Code.Equals("chw"))
                            }
                        },
                        new WFNodeXORSplit
                        {
                             Name="费用金额分支",
                             Next = null, //缺省节点
                             WFNodeExpressions = new []
                             {
                                 new WFNodeExpression
                                 {
                                     Expression = "{费用金额}>10000",
                                     Description = "费用金额大于10000元时,需要总经理审核",
                                     Next = null,
                                     OrderNO = 1
                                 },
                                 new WFNodeExpression
                                 {
                                     Expression = "{费用金额}>5000",
                                     Description = "费用金额大于5000元时,需要通知监察科",
                                     Next = null,
                                     OrderNO = 3
                                 }
                             }
                        },
                        new WFNodeHandle
                        {
                            Name="总经理审核",
                            ViewCode="auditForm",
                            ViewName="审核表单",
                            Actions = new []
                            {
                                new WFNodeAction
                                {
                                    Code="pass",
                                    Name="同意",
                                    NextNode=null //为空表示仍在停留在此节点中
                                },
                                new WFNodeAction
                                {
                                    Code="deny",
                                    Name="不同意",
                                    NextNode=null //这个不应该为NULL,但所需的节点还未创建
                                },
                            },
                            Subjects = new []
                            {
                                mydb.Users.First(u => u.Code.Equals("lxx"))
                            }
                        },
                        new WFNodeFinish
                        {
                            ID = Guid.NewGuid().ToString(),
                            Name="结束"
                        }
                    }
                };

                mydb.WFTemplates.Add(wft);

                wft.Nodes.OfType<WFNodeStart>().First().Next = wft.Nodes.First(n => n.Name.Equals("创建申请"));
                wft.Nodes.OfType<WFNodeHandle>().First(n => n.Name.Equals("创建申请")).Actions.First(a => a.Code.Equals("submit")).NextNode = wft.Nodes.First(n => n.Name.Equals("部门确认"));
                wft.Nodes.OfType<WFNodeHandle>().First(n => n.Name.Equals("部门确认")).Actions.First(a => a.Code.Equals("pass")).NextNode = wft.Nodes.First(n => n.Name.Equals("副总审核"));
                wft.Nodes.OfType<WFNodeHandle>().First(n => n.Name.Equals("部门确认")).Actions.First(a => a.Code.Equals("deny")).NextNode = wft.Nodes.OfType<WFNodeFinish>().First();

                wft.Nodes.OfType<WFNodeHandle>().First(n => n.Name.Equals("副总审核")).Actions.First(a => a.Code.Equals("pass")).NextNode = wft.Nodes.First(n => n.Name.Equals("费用金额分支"));
                wft.Nodes.OfType<WFNodeHandle>().First(n => n.Name.Equals("副总审核")).Actions.First(a => a.Code.Equals("deny")).NextNode = wft.Nodes.OfType<WFNodeFinish>().First();

                wft.Nodes.OfType<WFNodeXORSplit>().First(n => n.Name.Equals("费用金额分支")).Next = wft.Nodes.OfType<WFNodeFinish>().First();
                wft.Nodes.OfType<WFNodeXORSplit>().First(n => n.Name.Equals("费用金额分支")).WFNodeExpressions.First(exp => exp.Expression.Equals("{费用金额}>10000")).Next = wft.Nodes.First(n => n.Name.Equals("总经理审核"));
                wft.Nodes.OfType<WFNodeXORSplit>().First(n => n.Name.Equals("费用金额分支")).WFNodeExpressions.First(exp => exp.Expression.Equals("{费用金额}>5000")).Next = wft.Nodes.First(n => n.Name.Equals("总经理审核"));

                wft.Nodes.OfType<WFNodeHandle>().First(n => n.Name.Equals("总经理审核")).Actions.First(a => a.Code.Equals("pass")).NextNode = wft.Nodes.OfType<WFNodeFinish>().First();
                wft.Nodes.OfType<WFNodeHandle>().First(n => n.Name.Equals("总经理审核")).Actions.First(a => a.Code.Equals("deny")).NextNode = wft.Nodes.OfType<WFNodeFinish>().First();
                try
                {
                    mydb.SaveChanges();
                }
                catch (Exception e)
                {

                }
            }
        }

        [TestMethod]
        public void clear()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.WFInstNodeHandlers.Load();
                mydb.WFInstNodeHandlers.Local.Clear();

                mydb.WFInstNodes.Load();
                mydb.WFInstNodes.Local.Clear();

                mydb.WFInsts.Load();
                mydb.WFInsts.Local.Clear();

                mydb.WFInstNodeHandlers.Load();
                mydb.WFInstNodeHandlers.Local.Clear();

                mydb.WFNodeActions.Load();
                mydb.WFNodeActions.Local.Clear();

                mydb.WFNodeACLs.Load();
                mydb.WFNodeACLs.Local.Clear();

                mydb.WFNodeHandles.Load();
                mydb.WFNodeHandles.Local.Clear();

                mydb.WFNodeExpressions.Load();
                mydb.WFNodeExpressions.Local.Clear();

                mydb.WFNodeXORSplits.Load();
                mydb.WFNodeXORSplits.Local.Clear();

                mydb.WFNodeStarts.Load();
                mydb.WFNodeStarts.Local.Clear();

                mydb.WFNodeFinishs.Load();
                mydb.WFNodeFinishs.Local.Clear();

                mydb.WFNodes.Load();
                mydb.WFNodes.Local.Clear();

                mydb.WFTemplates.Load();
                mydb.WFTemplates.Local.Clear();

                mydb.SaveChanges();
            }
        }


    }
}
