using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics;
using BuizApp.Models;

namespace BuizApp
{
    public class AuthoritionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            //{
            //    //filterContext.Result = new RedirectToRouteResult("Default", new RouteValueDictionary(new { controller = "Manage", action = "AdminLogin" }));
            //    filterContext.HttpContext.Response.Write("ValidateLogin");
            //}
            base.OnActionExecuting(filterContext);

            return;
            //filterContext.HttpContext.Response.Write("<p>授权检查...</p>");

            // 取出区域名、控制器名、行为名，看是否包含在当前用户的权限集里
            string areaName = filterContext.RouteData.DataTokens.ContainsKey("area") ? filterContext.RouteData.DataTokens["area"].ToString() : string.Empty; //怎么读区域名？？？
            string controllerName = filterContext.RouteData.Values["controller"].ToString();//filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.RouteData.Values["action"].ToString();//filterContext.ActionDescriptor.ActionName;
            string verb = filterContext.HttpContext.Request.HttpMethod;

            string userID = filterContext.HttpContext.User.Identity.Name;

            if (LoginModel.UserHasPrivilege(userID, areaName, controllerName, actionName))
            {
                // 授权通过执行以下代码
            }
            else
            {
                // 授权不通过执行以下代码
                filterContext.HttpContext.Response.Redirect("/login");
            }
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    base.OnActionExecuted(filterContext);
        //    filterContext.HttpContext.Response.Write("<p>OnActionExecuted</p>");
        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    base.OnResultExecuting(filterContext);
        //    filterContext.HttpContext.Response.Write("<p>OnResultExecuting</p>");
        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    base.OnResultExecuted(filterContext);
        //    filterContext.HttpContext.Response.Write("<p>OnResultExecuted</p>");
        //}
    }
}
