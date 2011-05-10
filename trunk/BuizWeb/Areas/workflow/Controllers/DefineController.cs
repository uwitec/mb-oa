using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuizApp.Areas.workflow.Controllers
{
    public class DefineController : Controller
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
            return this.Redirect("~/canvas/canvas.htm");
        }
    }
}
