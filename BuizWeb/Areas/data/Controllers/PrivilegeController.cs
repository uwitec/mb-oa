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

        public JsonResult rolePrivilege()
        {
            return Json(PrivilegeModel.rolePrivilege(Request.Params["roleID"]), JsonRequestBehavior.AllowGet);
        }

        public JsonResult moduleResourceTree()
        {
            return Json(new { text = "ALL", children = PrivilegeModel.getModuleResourceTreeData() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult moduleResourceTree2()
        {
            return Json(new { text = "ALL", children = PrivilegeModel.getModuleResourceTreeData2() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult role()
        {
            using (MyDB mydb = new MyDB())
            {
                return Json(
                    mydb.Roles.Select(
                        r => new
                        {
                            r.ID,
                            r.roleCode,
                            r.roleName,
                            r.roleDescription
                        }).ToArray(),
                    JsonRequestBehavior.AllowGet); ;
            }
        }

        public JsonResult resource()
        {
            using (MyDB mydb = new MyDB())
            {
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

        public JsonResult module()
        {
            using (MyDB mydb = new MyDB())
            {
                return
                Json(mydb.Modules.OrderBy(r => r.orderNO).Select(r => new { r.ID, r.moduleName }).ToArray(), JsonRequestBehavior.AllowGet);
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
