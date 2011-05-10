using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib.WF;
using System.Data.Entity;

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
            WFTemplate template = null;
            using (MyDB mydb = new MyDB())
            {
                template = mydb.WFTemplates.Find(id);
            }

            System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom(Server.MapPath("~/bin/EntityObjectLib.dll"));
            Type t = ass.GetType("EntityObjectLib." + template.BuizCode);
            //Type t = Type.GetType("EntityObjectLib.ApplyExpense");
            EntityObjectLib.WFInst inst = Activator.CreateInstance(t) as EntityObjectLib.WFInst;
            inst.ID = Guid.NewGuid().ToString();
            inst.State = "state";
            return View(inst);
        }

        /// <summary>
        /// URL: /workflow/Instance/create/id
        /// id: 流程实例节点ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult handle(string id)
        {
            return View();
        }
    }
}
