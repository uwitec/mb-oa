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

                return View("Handle", inst.CurrentNode);
            }
        }

        /// <summary>
        /// URL: /workflow/Instance/create/id
        /// id: 流程实例节点ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult handle(EntityObjectLib.WFInst instance)
        {
            string TemplateID = Request.Form["TemplateID"];
            string instNodeID = Request.Form["instNodeID"];
            string nodeID = Request.Form["nodeID"];
            string actionID = Request.Form["actionID"];

            using (MyDB mydb = new MyDB())
            {
                WFInstNode instNode = mydb.WFInstNodes.FirstOrDefault(n => n.ID.Equals(instNodeID));
                #region 创建实例
                if (instNode == null) //如果实例节点ID不在数据库中,则需要创建流程实例
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
                    inst.Creator = mydb.Users.Find(this.User.Identity.Name);
                    inst.CreateTime = DateTime.Now;

                    inst.WFInstNodes = new WFInstNode[]{
                        new WFInstNode
                        {
                            ID = instNodeID,
                            WFNode = mydb.WFNodes.Find(nodeID),
                            State = "处理中",
                            EntryTime = DateTime.Now,
                            WFInst = inst
                        }
                    };

                    mydb.WFInsts.Add(inst);
                    mydb.SaveChanges();

                    instNode = mydb.WFInstNodes.FirstOrDefault(n => n.ID.Equals(instNodeID));
                }
                #endregion

                // 此时 instNode 已经不为空



                mydb.SaveChanges();
            }
            return View();
        }
    }
}
