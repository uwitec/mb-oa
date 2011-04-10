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
    public class TimerActionFilter : ActionFilterAttribute
    {
        #region IActionFilter Members

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write("<p>TimerActionFilter.OnActionExecuting</p>");
            var controller = filterContext.Controller;
            if (controller != null)
            {
                var stopwatch = new Stopwatch();
                controller.ViewData["StopWatch"] = stopwatch;
                stopwatch.Start();
            }
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var controller = filterContext.Controller;
            if (controller != null)
            {
                var stopwatch = (Stopwatch)controller.ViewData["StopWatch"];
                stopwatch.Stop();
                controller.ViewData["Duration"] = stopwatch.Elapsed.TotalMilliseconds;
            }
        }

        #endregion

        #region IResultFilter Members

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Write(filterContext.Controller.ViewData["Duration"]);
            //if (filterContext.Result is ViewResult)
            //{
            //    filterContext.HttpContext.Response.Write(filterContext.Controller.ViewData["Duration"]);
            //}
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }

        #endregion
    }
}
