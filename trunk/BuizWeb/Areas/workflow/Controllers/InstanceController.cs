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

namespace BuizApp.Areas.workflow.Controllers
{
    public class InstanceController : Controller
    {
        //
        // GET: /workflow/Instance/ 

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

                System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom(Server.MapPath("~/bin/EntityObjectLib.dll"));
                Type t = ass.GetType("EntityObjectLib." + template.BuizCode);
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
                #region 如果实例节点ID不在数据库中,则需要创建流程实例
                if (instNode == null) 
                {
                    WFTemplate template = mydb.WFTemplates.Find(TemplateID);
                    System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom(Server.MapPath("~/bin/EntityObjectLib.dll"));
                    Type t = ass.GetType("EntityObjectLib." + template.BuizCode);
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

                    inst.WFInstNodes = new List<WFInstNode>(){
                        new WFInstNode
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
                        }
                    };

                    mydb.WFInsts.Add(inst);
                    mydb.SaveChanges();

                    instNode = mydb.WFInstNodes.FirstOrDefault(n => n.ID.Equals(instNodeID));
                }
                #endregion

                // 此时 instNode 已经不为空

                WFNodeAction action = mydb.WFNodeActions.Find(actionID);
                WFNode next = action.NextNode;
                if (next != null)
                {
                    // 当前节点处理完成,准备转入下一节点
                    instNode.State = "已处理";
                    instNode.LeaveTime = DateTime.Now;

                    // 节点处理人
                    WFInstNodeHandler handler = instNode.WFInstNodeHandlers.First(h=>h.Handler.ID.Equals(UserID));
                    handler.HandleTime = DateTime.Now;
                    handler.Opinion = action.Name;
                    handler.State = "已处理";
                    foreach(WFInstNodeHandler other in instNode.WFInstNodeHandlers.Where(h=>!h.Handler.ID.Equals(UserID)))
                    {
                        handler.State = "已失效";
                    }

                    if (next is WFNodeHandle)
                    {
                        WFInstNode inode = new WFInstNode
                        {
                            WFNode = next,
                            WFInst = instNode.WFInst,
                            WFInstNodeHandlers = ((WFNodeHandle)next).Subjects.OfType<User>().Select(a => new WFInstNodeHandler { Handler= a}).ToArray()
                        };

                        mydb.WFInstNodes.Add(inode);
                    }
                    else if (next is WFNodeXORSplit)
                    {

                    }
                    else if (next is WFNodeFinish)
                    {

                    }
                    else
                    {

                    }
                }

                mydb.SaveChanges();
            }
            return View("handleSuccess");
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
                return View("tohandle", node);
            }
        }
    }
}
