using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib.WF;
using System.Data.Entity;
using EntityObjectLib;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Linq.Dynamic;

namespace BuizApp.Areas.workflow.Controllers
{
    public class InstanceController : Controller
    {
        //
        // GET: /workflow/Instance/ 

        public Type getType(WFTemplate template)
        {
            //System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom(Server.MapPath("~/bin/EntityObjectLib.dll"));
            System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom(Server.MapPath("~/bin/RTBuizModel.dll"));
            return ass.GetType("EntityObjectLib." + template.BuizCode);
        }

        public ActionResult Index()
        {
            using (MyDB mydb = new MyDB())
            {
                mydb.WFTemplates.Load();
                WFTemplate[] templates = mydb.WFTemplates.Local.ToArray();
                return View(templates);
            }
        }

        /// <summary>
        /// URL: /workflow/Instance/create/id
        /// id: 模板ID
        /// 发起流程
        /// </summary>
        /// <returns></returns>
        public ActionResult create(string id)
        {
            using (MyDB mydb = new MyDB())
            {
                WFTemplate template = mydb.WFTemplates.Find(id);
                WFNodeStart start = template.Nodes.OfType<WFNodeStart>().First();
                WFNodeHandle next = start.Next as WFNodeHandle;
                if (next is WFNodeHandle)
                {
                    ((WFNodeHandle)next).Actions.Select(a=>a);
                }

                Type t = getType(template);
                EntityObjectLib.WFInst inst = Activator.CreateInstance(t) as EntityObjectLib.WFInst;
                inst.WFTemplate = template;
                inst.State = "处理中";
                inst.WFInstNodes = new WFInstNode[]{
                    new WFInstNode
                    {
                        WFNode = next,
                        State = "处理中",
                        EntryTime = DateTime.Now,
                        WFInst = inst
                    }
                };
                // 这里有一个设计约束:开始节点后一定是个处理节点
                return View("toHandle", inst.CurrentNode);
            }
        }

        /// <summary>
        /// URL: /workflow/Instance/handle
        /// id: 流程实例节点ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult handle()
        {
            string UserID = this.User.Identity.Name;

            string TemplateID = Request.Form["TemplateID"];
            string instNodeID = Request.Form["instNodeID"];
            string nodeID = Request.Form["nodeID"];
            string actionID = Request.Form["actionID"];

            using (MyDB mydb = new MyDB())
            {
                WFInstNode instNode = mydb.WFInstNodes.FirstOrDefault(n => n.ID.Equals(instNodeID));
                WFTemplate template = mydb.WFTemplates.Find(TemplateID);

                Type t = getType(template);

                if (instNode == null)
                {
                    EntityObjectLib.WFInst inst = Activator.CreateInstance(t) as EntityObjectLib.WFInst;
                    //Object inst = Activator.CreateInstance(t);
                    foreach (string key in Request.Form.Keys)
                    {
                        PropertyInfo pi = t.GetProperty(key);
                        if (pi != null)
                        {
                            pi.SetValue(
                                inst,
                                //Request.Form[key],
                                Convert.ChangeType(Request.Form[key], pi.PropertyType.UnderlyingSystemType),
                                null);
                        }
                    }

                    inst.WFTemplate = template;
                    inst.State = "处理中";
                    inst.Creator = mydb.Users.Find(UserID);
                    inst.CreateTime = DateTime.Now;

                    instNode = new WFInstNode
                    {
                        ID = instNodeID,
                        WFNode = mydb.WFNodes.Find(nodeID),
                        State = "处理中",
                        EntryTime = DateTime.Now,
                        WFInst = inst,
                        WFInstNodeHandlers = new List<WFInstNodeHandler>()
                            {
                                new WFInstNodeHandler
                                {
                                    Handler = mydb.Users.Find(UserID),
                                    State = "待处理" //如果是"处理中",即表明一个人已经接收,准备处理,其他人不能处理
                                }
                            }
                    };

                    mydb.WFInstNodes.Add(instNode);
                    mydb.SaveChanges();
                }
            }
                        

            this.process(instNodeID, this.HttpContext);


            return View("handleSuccess");
        }

        /// <summary>
        /// 已完成当前节点工作,将流程实例推入下一个节点
        /// 1,判断下一个节点的类型
        /// 2.如果是处理节点,则创建实例节点,分配处理人
        /// 3.如果是分支节点,则创建实例节点,调用处理该节点
        /// 4.如果是结束节点,创建结束节点,调用处理该节点
        /// </summary>
        /// <param name="instID">流程实例ID</param>
        /// <param name="nextNodeID">下一个节点ID</param>
        /// <param name="ctx">HTTP上下文</param>
        private void entry(string instID, string nextNodeID, HttpContextBase ctx)
        {
            using (MyDB mydb = new MyDB())
            {
                WFNode next = mydb.WFNodes.Find(nextNodeID);
                WFInst inst = mydb.WFInsts.Find(instID);

                if (next is WFNodeHandle)
                {
                    WFInstNode inode = new WFInstNode
                    {
                        EntryTime = DateTime.Now,
                        State = "处理中",
                        WFNode = next,
                        WFInst = inst,
                        WFInstNodeHandlers = ((WFNodeHandle)next).Subjects.OfType<User>().Select(a => new WFInstNodeHandler { Handler = a }).ToArray()
                    };
                    mydb.WFInstNodes.Add(inode);
                    mydb.SaveChanges();
                }
                else if (next is WFNodeXORSplit)
                {
                    //WFNodeXORSplit Split = next as WFNodeXORSplit;
                    WFInstNode inode = new WFInstNode
                    {
                        EntryTime = DateTime.Now,
                        State = "处理中",
                        WFNode = next,
                        WFInst = inst
                    };
                    mydb.WFInstNodes.Add(inode);
                    mydb.SaveChanges();
                    this.process(inode.ID, this.HttpContext);
                }
                else if (next is WFNodeFinish)
                {
                    //WFNodeXORSplit Split = next as WFNodeXORSplit;
                    WFInstNode inode = new WFInstNode
                    {
                        EntryTime = DateTime.Now,
                        State = "处理中",
                        WFNode = next,
                        WFInst = inst
                    };
                    mydb.WFInstNodes.Add(inode);
                    mydb.SaveChanges();
                    this.process(inode.ID, this.HttpContext);
                }
                else
                {
                }
            }
        }

        /// <summary>
        /// 处理当前流程实例节点
        /// 1.判断节点类型
        /// 2.如果是处理节点,则取活动CODE,找到下一个节点,调用推入
        /// 3.如果是分支节点,则计算表达式,找到下一个节点,调用推入
        /// 4.如果是结束节点,完成结束必须的工作
        /// </summary>
        /// <param name="instNodeID">当前流程实例节点ID</param>
        /// <param name="ctx">HTTP上下文</param>
        private void process(string instNodeID, HttpContextBase ctx)
        {
            using (MyDB mydb = new MyDB())
            {
                WFInstNode current = mydb.WFInstNodes.Find(instNodeID);
                WFInst inst = current.WFInst;

                if (current.WFNode is WFNodeHandle)
                {
                    string actionID = Request.Form["actionID"];
                    string UserID = ctx.User.Identity.Name;
                    WFNodeAction action = mydb.WFNodeActions.Find(actionID);
                    WFNode next = action.NextNode;
                    if (next != null)
                    {
                        Type t = getType(inst.WFTemplate);

                        PropertyInfo[] PropertyInfos = t.GetProperties().ToArray();

                        foreach (string key in Request.Form.Keys)
                        {
                            PropertyInfo pi = t.GetProperty(key);
                            
                            if (pi != null)
                            {
                                DisplayAttribute[] attrs = pi.GetCustomAttributes(typeof(DisplayAttribute), true) as DisplayAttribute[];
                                if (attrs.Length > 0)
                                {
                                    pi.SetValue(
                                        inst,
                                        //Request.Form[key],
                                        Convert.ChangeType(Request.Form[key], pi.PropertyType.UnderlyingSystemType),
                                        null);
                                }

                            }
                        }
                        // 当前节点处理完成,准备转入下一节点
                        current.State = "已处理";
                        current.LeaveTime = DateTime.Now;
                        current.Summary = string.Format("节点名称:{0},处理人:{1},动作:{2}", current.WFNode.Name, mydb.Subjects.Find(UserID).Name, action.Name);

                        // 节点处理人
                        WFInstNodeHandler handler = current.WFInstNodeHandlers.First(h => h.Handler.ID.Equals(UserID));
                        handler.HandleTime = DateTime.Now;
                        handler.Opinion = action.Name;
                        handler.State = "已处理";
                        // 其他的处理人置为失效
                        foreach (WFInstNodeHandler other in current.WFInstNodeHandlers.Where(h => !h.Handler.ID.Equals(UserID)))
                        {
                            handler.State = "已失效";
                        }
                        mydb.SaveChanges();
                        this.entry(inst.ID,next.ID,ctx);
                        return;
                    }
                }
                else if (current.WFNode is WFNodeXORSplit)
                {
                    WFNodeXORSplit Split = current.WFNode as WFNodeXORSplit;

                    Type t = getType(inst.WFTemplate);

                    PropertyInfo[] PropertyInfos = t.GetProperties().ToArray();

                    IEnumerable<WFNodeExpression> exps = Split.WFNodeExpressions.OrderBy(exp => exp.OrderNO).Select(exp => exp);
                    foreach (WFNodeExpression exp in exps)
                    {
                        string expString = exp.Expression;
                        foreach (PropertyInfo pi in PropertyInfos)
                        {
                            DisplayAttribute[] attrs = pi.GetCustomAttributes(typeof(DisplayAttribute), true) as DisplayAttribute[];
                            if (attrs.Length > 0)
                            {
                                string Name = attrs[0].Name;

                                expString = expString.Replace(
                                    string.Format("{{{0}}}", Name),
                                    pi.Name
                                    );
                            }
                        }

                        expString = string.Format("select count(*) as v from {2}s where ID=\"{0}\" and {1}", current.WFInst.ID, expString, inst.WFTemplate.BuizCode);
                        //int count = mydb.WFInsts.Where(expString).Count();
                        int count = mydb.Database.SqlQuery<int>(expString, new object[] { }).ToArray()[0];
                        if (count > 0)// 条件满足,短路,找下一节点
                        {
                            current.Summary = string.Format("节点名称:{0},满足条件:{1}", current.WFNode.Name, exp.Expression);
                            current.LeaveTime = DateTime.Now;
                            current.State = "已处理";
                            mydb.SaveChanges();
                            this.entry(inst.ID, exp.Next.ID, ctx);
                            return;
                        }
                    }
                    
                }
                else if (current.WFNode is WFNodeFinish)
                {
                    current.Summary = "流程结束";
                    current.LeaveTime = DateTime.Now;
                    current.State = "已处理";
                    inst.State = "已处理";
                    mydb.SaveChanges();
                    return;
                }
                else
                {
                }
            }
        }


        public ActionResult workList()
        {
            string UserID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                WFInstNodeHandler[] hs = mydb.WFInstNodeHandlers
                    .Include(h => h.WFInstNode)
                    .Include(h => h.WFInstNode.WFNode)
                    .Include(h => h.WFInstNode.WFInst)
                    .Include(h => h.WFInstNode.WFInst.WFTemplate)
                    .Where(h => h.Handler.ID.Equals(UserID) && h.State.Equals("待处理"))
                    .ToArray();
                return View(hs);
            }
        }

        public ActionResult preHandle(string id)
        {
            using (MyDB mydb = new MyDB())
            {
                WFInstNode node = mydb.WFInstNodes.Find(id);
                mydb.Entry(node).Reference<WFInst>(n => n.WFInst).Load();
                mydb.Entry(node).Reference<WFNode>(n => n.WFNode).Load();
                mydb.Entry(node.WFInst).Reference<WFTemplate>(n => n.WFTemplate).Load();
                mydb.Entry((WFNodeHandle)node.WFNode).Collection<WFNodeAction>(n=>n.Actions).Load();
                mydb.Entry(node.WFInst).Collection<WFInstNode>(inst => inst.WFInstNodes).Load();
                return View("tohandle", node);
            }
        }
    }
}
