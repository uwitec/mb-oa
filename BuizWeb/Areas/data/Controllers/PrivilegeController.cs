using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using System.Web.Script.Serialization;
using BuizModel;
using EntityObjectContext;

namespace BuizApp.Areas.data.Controllers
{
    public class PrivilegeController : Controller
    {
        public JsonResult index()
        {
            string mrId = Request.Params["mrId"];
            string depth = Request.Params["depth"];
            switch (depth)
            {
                case "1": //模块
                    return Json(new { success = true, data = PrivilegeModel.getListByModule(mrId) }, JsonRequestBehavior.AllowGet);
                case "2": //功能
                    return Json(new { success = true, data = PrivilegeModel.getListByResource(mrId) }, JsonRequestBehavior.AllowGet);
                default:
                    return Json(new { success = true, data = PrivilegeModel.getList() }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult moduleResourceTree()
        {
            return Json(PrivilegeModel.getModuleResourceTreeData(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult role()
        {
            return Json(PrivilegeModel.getRoles(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult resource()
        {
            using (MyDB mydb = new MyDB())
            {
                //return 
                //Json(
                //    new
                //    {
                //        success = true,
                //        data =
                //            mydb.Resources.Select(
                //            r => new
                //            {
                //                r.ID,
                //                r.resourceName
                //            }).ToArray()
                //    },
                //    JsonRequestBehavior.AllowGet
                //    );
                return
                Json(
                            mydb.Resources.OrderBy(r=>r.orderNO).Select(
                            r => new
                            {
                                r.ID,
                                resourceName = r.module.moduleName+":"+r.resourceName
                            }).ToArray()
                    ,
                    JsonRequestBehavior.AllowGet
                    );
            }
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
