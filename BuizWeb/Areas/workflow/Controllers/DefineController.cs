﻿using System;
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
                            .Select(a => new
                            {
                                a.ID,
                                name = a.Name,
                                from = a.WFNode.ID,
                                to = a.NextNode.ID,
                                middlePoint = a.PositionX != null && a.PositionY != null ? new { x = a.PositionX, y = a.PositionY } : null
                            })
                            .Union(
                                template.Nodes.SelectMany(n => n.FromExpressions)
                                .Select(e => new
                                {
                                    e.ID,
                                    name = string.Format("[{0}]{1}", e.OrderNO, e.Expression),
                                    from = e.WFNode.ID,
                                    to = e.Next.ID,
                                    middlePoint = e.PositionX != null && e.PositionY != null ? new { x = e.PositionX, y = e.PositionY } : null
                                })
                            )
                            .Union(
                                template.Nodes.OfType<WFNodeStart>()
                                .Select(s => new
                                {
                                    s.ID,
                                    name = "",
                                    from = s.ID,
                                    to = s.Next.ID,
                                    middlePoint = false ? new { x = null as int?, y = null as int? } : null //注意int?与int不是同一种类型,上面的是int?,这里必须也是int?
                                })
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
                        if (Request.Params["nodeID"] != null)
                        {
                            string nodeID = Request.Params["nodeID"];
                            int x = int.Parse(Request.Params["x"]);
                            int y = int.Parse(Request.Params["y"]);

                            WFNode node = mydb.WFNodes.Find(nodeID);
                            node.PositionX = x;
                            node.PositionY = y;
                        }
                        else if (Request.Params["lineID"] != null)
                        {
                            string LineID = Request.Params["lineID"];
                            if (string.IsNullOrEmpty(Request.Params["x"]) || string.IsNullOrEmpty(Request.Params["y"]))
                            {
                                WFNodeAction action = mydb.WFNodeActions.Find(LineID);
                                if (action != null)
                                {
                                    action.PositionX = null;
                                    action.PositionY = null;
                                }

                                WFNodeExpression expression = mydb.WFNodeExpressions.Find(LineID);
                                if (expression != null)
                                {
                                    expression.PositionX = null;
                                    expression.PositionY = null;
                                }
                            }
                            else
                            {
                                int x = int.Parse(Request.Params["x"]);
                                int y = int.Parse(Request.Params["y"]);

                                WFNodeAction action = mydb.WFNodeActions.Find(LineID);
                                if (action != null)
                                {
                                    action.PositionX = x;
                                    action.PositionY = y;
                                }

                                WFNodeExpression expression = mydb.WFNodeExpressions.Find(LineID);
                                if (expression != null)
                                {
                                    expression.PositionX = x;
                                    expression.PositionY = y;
                                }
                            }
                        }
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
            string[] Handlers = Request.Form["Handlers"].Split(",".ToCharArray());

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
                    WFTemplate = template,
                    Subjects = mydb.Subjects.Where(s => Handlers.Contains(s.ID)).ToArray()
                };

                mydb.WFNodeHandles.Add(node);
                return Json(new { success = true, data = node.ID });
            }
            return Json(new { success = false });
        }

        public ActionResult addNodeXOR()
        {
            string templateId = Request.Form["templateId"];
            string id = Request.Form["id"];
            string Name = Request.Form["Name"];

            int x = int.Parse(Request.Form["x"]);
            int y = int.Parse(Request.Form["y"]);

            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;
                WFTemplate template = mydb.WFTemplates.Find(templateId);
                WFNodeXORSplit node = new WFNodeXORSplit
                {
                    ID = Guid.NewGuid().ToString(),
                    PositionX = x,
                    PositionY = y,
                    Name = Name,
                    WFTemplate = template
                };

                mydb.WFNodeXORSplits.Add(node);
                return Json(new { success = true, data = node.ID });
            }
            return Json(new { success = false });
        }
        //

        public ActionResult addNodeAction()
        {
            string templateId = Request.Form["templateId"];
            string from = Request.Form["from"];
            string to = Request.Form["to"];
            string Name = Request.Form["Name"];
            string Code = Request.Form["Code"];

            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;
                WFTemplate template = mydb.WFTemplates.Find(templateId);
                WFNodeAction action = new WFNodeAction
                {
                    ID = Guid.NewGuid().ToString(),
                    Code = Code,
                    Name = Name,
                    WFNode = mydb.WFNodeHandles.Find(from),
                    NextNode = mydb.WFNodes.Find(to)
                };

                mydb.WFNodeActions.Add(action);
                return Json(new { success = true, data = action.ID });
            }
            return Json(new { success = false });
        }

        public ActionResult addNodeExpression()
        {
            string templateId = Request.Form["templateId"];
            string from = Request.Form["from"];
            string to = Request.Form["to"];
            string Expression = Request.Form["Expression"];
            string Description = Request.Form["Description"];
            int OrderNO = int.Parse(Request.Form["OrderNO"]);

            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;
                WFTemplate template = mydb.WFTemplates.Find(templateId);
                WFNodeExpression expression = new WFNodeExpression
                {
                    ID = Guid.NewGuid().ToString(),
                    Expression = string.IsNullOrEmpty(Expression) ? "1==1" : Expression,
                    Description = Description,
                    OrderNO = OrderNO,
                    WFNode = mydb.WFNodeXORSplits.Find(from),
                    Next = mydb.WFNodes.Find(to)
                };

                mydb.WFNodeExpressions.Add(expression);
                return Json(new { success = true, data = expression.ID });
            }
            return Json(new { success = false });
        }

        public ActionResult deleteNode()
        {
            string templateId = Request.Form["templateId"];
            string nodeID = Request.Form["nodeID"];

            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;
                WFNode node = mydb.WFNodes.Find(nodeID);
                mydb.WFNodes.Remove(node);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public ActionResult deleteLine()
        {
            string templateId = Request.Form["templateId"];
            string LineID = Request.Form["LineID"];

            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;

                /// 连接线可能有三种情况:来自于活动,表达式,或分支节点的缺省
                WFNodeAction action = mydb.WFNodeActions.Find(LineID);
                if (action != null)
                {
                    mydb.WFNodeActions.Remove(action);
                    return Json(new { success = true });
                }

                WFNodeExpression expression = mydb.WFNodeExpressions.Find(LineID);
                if (expression != null)
                {
                    mydb.WFNodeExpressions.Remove(expression);
                    return Json(new { success = true });
                }

                WFNodeStart start = mydb.WFNodeStarts.Find(LineID);
                if (start != null)
                {
                    start.Next = null;
                    return Json(new { success = true });
                }

                return Json(new { success = false });
            }
            return Json(new { success = false });
        }

        public ActionResult saveWFTemplate()
        {
            string id = Request.Form["id"];
            string Name = Request.Form["Name"];
            string BuizCode = Request.Form["BuizCode"];
            string BuizName = Request.Form["BuizName"];

            using (MyDB mydb = new MyDB())
            {
                WFNodeFinish finish = new WFNodeFinish
                                  {
                                      ID = Guid.NewGuid().ToString(),
                                      Name = "结束节点",
                                      PositionX = 200,
                                      PositionY = 40
                                  };
                WFTemplate wft = new WFTemplate
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = Name,
                    BuizCode = BuizCode,
                    BuizName = BuizName,
                    Creator = mydb.Users.Find(this.User.Identity.Name),
                    CreateTime = DateTime.Now,
                    Nodes = new WFNode[]
                      {
                          new WFNodeStart
                          {
                              ID = Guid.NewGuid().ToString(),
                              PositionX = 100,
                              PositionY = 40,
                              Name = "开始节点",
                              Next = finish
                          },
                          finish
                      }
                };

                mydb.WFTemplates.Add(wft);
                mydb.SaveChanges();

                return Json(new { success = true });
            }
        }

        public ActionResult deleteTemplate()
        {
            string id = Request.Params["id"];

            using (MyDB mydb = new MyDB())
            {

                return Json(new { success = true });
            }
        }

        public ActionResult setStartNodeNext()
        {
            string templateId = Request.Params["templateId"];
            string from = Request.Params["from"];
            string to = Request.Params["to"];

            if (Session[templateId] != null)
            {
                MyDB mydb = Session[templateId] as MyDB;
                var start = mydb.WFNodeStarts.Find(from);
                start.Next = mydb.WFNodes.Find(to);

                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
