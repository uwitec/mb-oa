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
        public ActionResult Privilege()
        {
            return View();
        }

        public ActionResult getPrivilege()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Privilege p = mydb.Privileges.Find(Request.Form["ID"]);
                return Json(new
                {
                    success = true,
                    data = new 
                    { 
                        ID = p.ID, 
                        privilegeCode = p.privilegeCode, 
                        privilegeName = p.privilegeName, 
                        p.isMenuEntry,
                        p.needAuth,
                        privilegeDescription = p.privilegeDescription, 
                        resourceID=p.resource.ID 
                    }
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
        public ActionResult CreatePrivilege()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Privilege p = getPrivilege(Request, mydb);
                p.ID = Guid.NewGuid().ToString();
                mydb.Privileges.Add(p);
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// 更新操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePrivilege()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Privilege p = getPrivilege(Request, mydb);
                //mydb.Modules.Attach(p);
                //mydb.Entry<EntityObjectLib.Privilege>(p).State = System.Data.EntityState.Modified;
                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeletePrivilege()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Privilege p = mydb.Privileges.Find(Request.Form["ID"]);
                mydb.Privileges.Remove(p);
                mydb.SaveChanges();
            }
            return Json(new { success = true });
        }

        /// <summary>
        /// 对操作重新排序
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReOrderPrivileges()
        {
            return View();
        }


        private EntityObjectLib.Privilege getPrivilege(HttpRequestBase request,MyDB mydb)
        {
            EntityObjectLib.Privilege p = mydb.Privileges.Find(Request.Form["ID"]);
            if (p == null)
            {
                p = new EntityObjectLib.Privilege();
            }
            p.privilegeCode = Request.Form["privilegeCode"];
            p.privilegeName = Request.Form["privilegeName"];
            p.needAuth = Request.Form["needAuth"] != null;
            p.isMenuEntry = Request.Form["isMenuEntry"] != null;
            p.privilegeDescription = Request.Form["privilegeDescription"];
            p.resource = mydb.Resources.Find(Request.Form["resourceID"]);
            return p;
        }
    }
}
