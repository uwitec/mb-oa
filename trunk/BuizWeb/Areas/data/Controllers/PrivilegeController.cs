using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using System.Web.Script.Serialization;
using BuizModel;

namespace BuizApp.Areas.data.Controllers
{
    public class PrivilegeController : Controller
    {

        public JsonResult index()
        {
            return Json(new { success = true, data = PrivilegeModel.getList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult moduleResourceTree()
        {
            return Json(PrivilegeModel.getModuleResourceTreeData(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult role()
        {
            return Json(PrivilegeModel.getRoles(), JsonRequestBehavior.AllowGet);
        }

        #region 私有方法
        private string getJsonString(object o)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //jss.RecursionLimit = 1;
            return serializer.Serialize(o);
        }
        #endregion
    }
}
