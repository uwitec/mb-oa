using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib.WF;

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

        public ActionResult getTemplate()
        {
            string id = Request.Params["templateId"];
            using (MyDB mydb = new MyDB())
            {
                WFTemplate template = mydb.WFTemplates.Find(id);

            }
            return null;
        }
    }
}
