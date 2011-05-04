using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BuizApp.Models;
using System.Web.Security;
using System.Web.Script.Serialization;
using System.Web.Routing;
using EntityObjectContext;

namespace BuizApp
{
    public class MainController : Controller
    {
        public ActionResult Index()
        {
            TempData["MenuJson"] = MenuToJson();
            return View();
        }

        public ActionResult mydesk()
        {
            string userID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User user = mydb.Users.Find(userID);
                TempData["layoutData"] = string.IsNullOrEmpty(user.LayoutData) ? "[[],[],[]]" : user.LayoutData;
            }
            
            return View();
        }

        public ActionResult Ext_Extend()
        {
            return View();
        }

        /// <summary>
        /// 获取某个菜单的链接
        /// </summary>
        /// <param name="menuItem">菜单数据｛操作名称，操作代码，资源代码，模块代码｝</param>
        /// <returns>
        /// 返回链接的HTML
        /// 如:<a href="/模块代码/资源代码/操作代码">操作名称</a>
        /// </returns>
        private string[] getLinks(List<string[]> menuItem)
        {
            return menuItem
                .Select(m => string.Format("<a href='javascript:addTab(\"{0}\",\"{1}\")'>{1}</a>", Url.Action(m[1], m[2], new { area = m[3] }), m[0]))
                .ToArray();
                // 如果test区域不存在，返回的url为空
        }

        /// <summary>
        /// 生成菜单JSON
        /// </summary>
        /// <returns>
        /// 返回菜单的JSON
        /// 如:
        /// [{
        ///     title: 'Office Space',
        ///     html: '<ul><li>111111</li><li>2222</li><li>33333333</li><li>444444444</li></ul>'
        /// }, {
        ///     title: 'Super Troopers',
        ///     html: '<ul><li>111111</li><li>2222</li><li>33333333</li><li>444444444</li></ul>'
        /// }, {
        ///     title: 'American Beauty',
        ///     //autoLoad: 'html/4.txt'
        ///     html: '<ul><li>111111</li><li>2222</li><li>33333333</li><li>444444444</li></ul>'
        /// }]
        /// </returns>
        private string MenuToJson()
        {
            string userID = HttpContext.User.Identity.Name;

            object[] ms = LoginModel.getMenusData(userID).Select(m => new
            {
                title = m.Key,
                html = string.Format(
                    "<ul class='MBmenu'>{0}</ul>"
                    , string.Join("", this.getLinks(m.Value).Select(mm => string.Format("<li>{0}</li>", mm)))
                    )
            }).ToArray();

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            //jss.RecursionLimit = 1;
            string result = serializer.Serialize(ms);

            return result;
        }

        public ActionResult save()
        {
            string userID = this.User.Identity.Name;
            using (MyDB mydb = new MyDB())
            {
                EntityObjectLib.User user = mydb.Users.Find(userID);
                user.LayoutData = Request.Params["LayoutData"];

                mydb.SaveChanges();
            }

            return Json(new { success = true });
        }
    }
}
