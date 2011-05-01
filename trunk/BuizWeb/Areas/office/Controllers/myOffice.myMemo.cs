using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EntityObjectContext;
using EntityObjectLib;

namespace BuizApp.Areas.office.Controllers
{
    public partial class myOfficeController : Controller
    {
        public ActionResult myMemo()
        {
            return View();
        }
    }
}
