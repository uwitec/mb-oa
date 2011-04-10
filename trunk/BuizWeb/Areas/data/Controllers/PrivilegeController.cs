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
    //class ExtResult
    //{
    //    public bool success;
    //    public Array data;
    //}
    ////[MyFilter]
    //public class DataController : Controller
    //{
    //    class CityData { public string city; public int temperature; } ;

    //    //
    //    // GET: /Home/
    //    //[TimerActionFilter]
    //    public JsonResult CityDatas()
    //    {
    //        var result = new[]{
    //                new CityData { city = "London", temperature = 68 }, 
    //                new CityData { city = "Hong Kong", temperature = 84 } 
    //            };

    //        return getResult(true, result);
    //    }

    //}

    public class PrivilegeController : Controller
    {

        public JsonResult index()
        {
            JsonResult jr = getResult(true, PrivilegeModel.getList());
            return jr;
        }

        public JsonResult moduleResourceTree()
        {
            return Json(PrivilegeModel.getModuleResourceTreeData(), JsonRequestBehavior.AllowGet);
        }

        #region 私有方法
        private JsonResult getResult(bool success, Array data)
        {
            object result = new { success = success, data = data };

            //JsonRequestBehavior缺省不允许get,为了防止第三方取数据
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private string getJsonString(object o)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //jss.RecursionLimit = 1;
            return serializer.Serialize(o);
        }
        #endregion
    }
}
