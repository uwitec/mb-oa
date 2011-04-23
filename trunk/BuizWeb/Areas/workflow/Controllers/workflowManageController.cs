using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BuizApp.Areas.workflow.Controllers
{
    public class workflowManageController : Controller
    {
        //
        // GET: /workflow/workflowManage/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult define()
        {
            return this.Redirect("~/canvas/canvas.htm");
        }
    }
}
