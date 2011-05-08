using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuizApp.Areas.workflow.Controllers
{
    public class InstanceController : Controller
    {
        //
        // GET: /workflow/Instance/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// /workflow/Instance/create/id
        /// id: 模板ID
        /// </summary>
        /// <returns></returns>
        public ActionResult create(string id)
        {
            string TypeName = "ApplyExpense";
            System.Reflection.Assembly ass = System.Reflection.Assembly.LoadFrom(Server.MapPath("~/bin/EntityObjectLib.dll"));
            Type t = ass.GetType("EntityObjectLib." + TypeName);
            //Type t = Type.GetType("EntityObjectLib.ApplyExpense");
            EntityObjectLib.WFInst inst = Activator.CreateInstance(t) as EntityObjectLib.WFInst;
            inst.State = "state";
            return View(inst);
        }

    }
}
