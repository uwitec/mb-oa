using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using EntityObjectContext;

namespace BuizApp.Areas.system.Controllers
{
    //[MyFilter]
    public partial class AuthController : Controller
    {
        public ActionResult getModule()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Module p = mydb.Modules.Find(Request.Form["ID"]);
                return Json(new { success = true, data = new { ID = p.ID, moduleCode = p.moduleCode, moduleName = p.moduleName, moduleDescription = p.moduleDescription } });
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
        public ActionResult CreateModule()
        {
            //将JSON格式转换为Module类型
            //return Json(new { success = false, errors = new { clientCode = "", portOfLoading = "" } });
            EntityObjectLib.Module p = getModule(Request);
            using (MyDB mydb = new MyDB())
            {
                p.ID = Guid.NewGuid().ToString();
                mydb.Modules.Add(p);
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateModule()
        {
            EntityObjectLib.Module p = getModule(Request);
            using (MyDB mydb = new MyDB())
            {
                //mydb.Modules.Attach(p);
                mydb.Entry<EntityObjectLib.Module>(p).State = System.Data.EntityState.Modified;
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteModule()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Module p = mydb.Modules.Find(Request.Form["ID"]);
                mydb.Modules.Remove(p);
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// 对操作重新排序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReOrderModules()
        {
            return View();
        }

        private EntityObjectLib.Module getModule(HttpRequestBase request)
        {
            EntityObjectLib.Module p = new EntityObjectLib.Module();
            p.ID = Request.Form["ID"];
            p.moduleCode = Request.Form["moduleCode"];
            p.moduleName = Request.Form["moduleName"];
            p.moduleDescription = Request.Form["moduleDescription"];
            return p;
        }
    }
}
