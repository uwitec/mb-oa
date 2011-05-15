using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib.WF;
using System.Web.Script.Serialization;

namespace BuizApp.Areas.workflow.Controllers
{
    public class ManageController : Controller
    {
        //
        // GET: /workflow/workflowManage/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult define()
        {
            return View();
        }

        /// <summary>
        /// 取模板，准备编辑，上下文在于session中
        /// </summary>
        /// <returns></returns>
        public ActionResult getTemplate()
        {
            string id = Request.Params["templateId"];
            //if (Session[id] == null)
                Session[id] = new MyDB();
            MyDB mydb = Session[id] as MyDB;

            WFTemplate template = mydb.WFTemplates.Find(id);
            return Json(
                new
                {
                    success = true,
                    data = new
                    {
                        template.ID,
                        template.Name,
                        template.BuizCode,
                        template.BuizName,
                        template.OffsetX,
                        template.OffsetY,
                        nodes = template.Nodes.Select(n => new { n.ID, name = n.Name, type = n.ExtType, position = new { x = n.PositionX, y = n.PositionY } }).ToArray(),
                        lines =
                            template.Nodes.SelectMany(n => n.FromActions)
                            .Where(a => a.NextNode != null)
                            .Select(a => new { a.ID, name = a.Name, from = a.WFNode.ID, to = a.NextNode.ID })
                            .Union(
                                template.Nodes.SelectMany(n => n.FromExpressions)
                                .Select(e => new { e.ID, name = e.Expression, from = e.WFNode.ID, to = e.Next.ID })
                            )
                            .Union(
                                template.Nodes.OfType<WFNodeStart>().Select(s => new { s.ID, name = "", from = s.ID, to = s.Next.ID })
                            )
                            .ToArray()
                    }
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public ActionResult getAllTemplates()
        {
            string id = Request.Params["templateId"];
            using (MyDB mydb = new MyDB())
            {
                return Json(mydb.WFTemplates.Select(t => new { t.ID, t.Name, t.BuizName }).ToArray(), JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult saveGraph()
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            //Dictionary<string, object> data = (Dictionary<string, object>)jss.DeserializeObject(Request.Params["data"]);
            //return Json(new { success = true });

            string id = Request.Params["templateId"];
            if (Session[id] != null)
            {
                (Session[id] as MyDB).SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public ActionResult modifyTemplate()
        {
            string id = Request.Params["templateId"];
            string mode = Request.Params["mode"];

            if (Session[id] == null)
                Session[id] = new MyDB();
            MyDB mydb = Session[id] as MyDB;

            switch (mode)
            {
                case "select":
                    {
                        string nodeID = Request.Params["nodeID"];
                        int x = int.Parse(Request.Params["x"]);
                        int y = int.Parse(Request.Params["y"]);

                        WFNode node = mydb.WFNodes.Find(nodeID);
                        node.PositionX = x;
                        node.PositionY = y;
                    }
                    break;
                case "move":
                    {
                        int x = int.Parse(Request.Params["x"]);
                        int y = int.Parse(Request.Params["y"]);

                        WFTemplate t = mydb.WFTemplates.Find(id);
                        t.OffsetX = x;
                        t.OffsetY = y;
                    }
                    break;
                default:
                    break;
            }

            return Json(new { success = true });
        }

        public ActionResult getNodehandlers()
        {
            string id = Request.Params["id"];
            string templateId = Request.Params["templateId"];
            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;
                if (!string.IsNullOrEmpty(id))
                {
                    object[] objs = mydb.WFNodeHandles.Find(id).Subjects.Select(s => new { s.ID, s.Code, s.Name, s.Category }).ToArray();
                    return Json(new { success = true, data = objs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getNodeACLs()
        {
            string id = Request.Params["id"];
            string templateId = Request.Params["templateId"];
            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;
                if (!string.IsNullOrEmpty(id))
                {
                    object[] objs = mydb.WFNodeHandles.Find(id).NodeACLs.Select(s => new { s.ID, s.Code, s.Name, s.ACL }).ToArray();
                    return Json(new { success = true, data = objs }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult addNodeHandle()
        {
            string templateId = Request.Form["templateId"];
            string id = Request.Form["id"];
            string Name = Request.Form["Name"];
            string ViewCode = Request.Form["ViewCode"];
            bool IsCountersign = Request.Form["IsCountersign"] != null;

            int x = int.Parse(Request.Form["x"]);
            int y = int.Parse(Request.Form["y"]);

            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;
                WFTemplate template = mydb.WFTemplates.Find(templateId);
                WFNodeHandle node = new WFNodeHandle
                {
                    ID = Guid.NewGuid().ToString(),
                    PositionX = x,
                    PositionY = y,
                    Name = Name,
                    ViewCode = ViewCode,
                    IsCountersign = IsCountersign,
                    WFTemplate = template
                };

                mydb.WFNodeHandles.Add(node);
                return Json(new { success = true ,data= node.ID});
            }
            return Json(new { success = false });
        }
        
    }
}
