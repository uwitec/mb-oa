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
        public ActionResult getModule()
        {
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.Module p = mydb.Modules.Find(Request.Form["ID"]);
                return Json(new
                {
                    success = true,
                    data = new { ID = p.ID, moduleCode = p.moduleCode, moduleName = p.moduleName, moduleDescription = p.moduleDescription }
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
        /// 使用本地实体集，参考：http://blogs.msdn.com/b/adonet/archive/2011/02/01/using-dbcontext-in-ef-feature-ctp5-part-7-local-data.aspx
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ReOrderModules()
        {
            string[] Ids = Request.Form["data"].Split(",".ToCharArray());
            using (MyDB mydb = new MyDB())
            {
                mydb.Modules.Load();
                mydb.Resources.Load();

                EntityObjectLib.Module last = null;
                int order = 0;
                foreach (string id in Ids)
                {
                    order = order + 10;
                    EntityObjectLib.Module p = mydb.Modules.Local.FirstOrDefault(m => m.ID.Equals(id));
                    if (p == null)
                    {
                        EntityObjectLib.Resource r = mydb.Resources.Local.FirstOrDefault(m => m.ID.Equals(id));
                        if (r == null)
                        {
                            continue;
                        }
                        else
                        {
                            r.orderNO = order;
                            r.module = last;
                        }
                    }
                    else
                    {
                        last = p;
                        p.orderNO = order;
                    }
                }
                mydb.SaveChanges();
            }
            return Json(new { success = true });
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
