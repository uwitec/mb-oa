using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using EntityObjectContext;
using System.Data.Entity;

namespace BuizApp.Areas.system.Controllers
{
    //[MyFilter]
    public partial class AuthController : Controller
    {
        public ActionResult getResource()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Resource p = mydb.Resources.Find(Request.Form["ID"]);
                return Json(new
                {
                    success = true,
                    data = new { p.ID, p.resourceCode, p.resourceName, moduleID=p.module.ID, p.resourceDescription }
                }
                );
            }
        }
        /// <summary>
        /// 新建操作
        /// 请求方式: ajax请求
        /// 请求参数: JSON
        ///     [ID,moduleCode,moduleName,moduleDescription]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateResource()
        {
            //将JSON格式转换为Module类型
            //return Json(new { success = false, errors = new { clientCode = "", portOfLoading = "" } });
            
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Resource p = getResource(Request, mydb);
                p.ID = Guid.NewGuid().ToString();
                mydb.Resources.Add(p);
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateResource()
        {
            
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Resource p = getResource(Request, mydb);
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteResource()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Resource p = mydb.Resources.Find(Request.Form["ID"]);
                mydb.Resources.Remove(p);
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        private EntityObjectLib.Resource getResource(HttpRequestBase request, MyDB mydb)
        {
            EntityObjectLib.Resource p = mydb.Resources.Find(Request.Form["ID"]);
            if (p == null)
            {
                p = new EntityObjectLib.Resource();
            }
            p.ID = Request.Form["ID"];
            p.resourceCode = Request.Form["resourceCode"];
            p.resourceName = Request.Form["resourceName"];
            p.resourceDescription = Request.Form["resourceDescription"];
            p.module = mydb.Modules.Find(Request.Form["moduleID"]);
            return p;
        }
    }
}
