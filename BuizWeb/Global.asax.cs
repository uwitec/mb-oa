﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Threading;

namespace BuizApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        System.Threading.Thread scheduleThread = null;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new AuthoritionFilter()); // 注册授权检查全局过滤器
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    "Default", // Route name
            //    "{controller}/{action}/{id}", // URL with parameters
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
            //    new { }, //约束
            //    new[] { "BuizApp" } //控制器类的命名空间
            //);
        }

        protected void Application_Start()
        {
            // 后台自动任务
            ThreadTimer threadTimer = new ThreadTimer();
            scheduleThread = new System.Threading.Thread(new System.Threading.ThreadStart(threadTimer.Run));
            scheduleThread.Start();

            // 注意:注册有个顺序问题???
            //AreaRegistration.RegisterAllAreas(); //调用每个AreaRegistration的RegisterArea方法
            RegisterAllAreas(); //按自定义顺序注册,AreaRegistration.RegisterAllAreas()顺序规则不能确定


            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //使用数据库存储视图
            //System.Web.Hosting.HostingEnvironment.RegisterVirtualPathProvider(new DbPathProvider());

            // 注册查询模型绑定器
            ModelBinders.Binders.Add(typeof(EfSearchModel.Model.QueryModel), new EfSearchModel.Binders.SearchModelBinder());
        }

        /// <summary>
        /// 手工注册区域,如果新建一个区域,一定要在这里注册
        /// </summary>
        private static void RegisterAllAreas()
        {
            RegisterArea<BuizApp.Areas.system.systemAreaqRegistration>(RouteTable.Routes, null);
            RegisterArea<BuizApp.Areas.test.testAreaRegistration>(RouteTable.Routes, null);
            RegisterArea<BuizApp.Areas.data.dataAreaRegistration>(RouteTable.Routes, null);
            RegisterArea<BuizApp.Areas.office.officeAreaRegistration>(RouteTable.Routes, null);
            RegisterArea<BuizApp.Areas.resource.resourceAreaRegistration>(RouteTable.Routes, null);
            RegisterArea<BuizApp.Areas.workflow.workflowAreaRegistration>(RouteTable.Routes, null);
            RegisterArea<BuizApp.Areas.report.reportAreaRegistration>(RouteTable.Routes, null);
            RegisterArea<BuizApp.Areas.msgService.msgServiceAreaRegistration>(RouteTable.Routes, null);
            RegisterArea<defaultAreaRegistration>(RouteTable.Routes, null);
        }

        /// <summary>
        /// 自定义注册区域
        /// http://stackoverflow.com/questions/1639971/mvc-2-arearegistration-routes-order
        /// 另一种方法:实现Asp.NET MVC2 的Area自定义注册顺序(http://www.lukiya.com/Blogs/2010/10/29/Post-1065.html)
        /// </summary>
        public static void RegisterArea<T>(RouteCollection routes, object state) where T : AreaRegistration
        {
            AreaRegistration registration = (AreaRegistration)Activator.CreateInstance(typeof(T));

            AreaRegistrationContext context = new AreaRegistrationContext(registration.AreaName, routes, state);

            string tNamespace = registration.GetType().Namespace;
            if (tNamespace != null)
            {
                context.Namespaces.Add(tNamespace + ".*");
            }

            registration.RegisterArea(context);
        }
    }

    public class ThreadTimer
    {
        public void Run()
        {
            Timer t = new Timer(new TimerCallback(RunTasks));
            t.Change(0, 60000);
        }

        private void RunTasks(Object obj)
        {
            try
            {
                // System.Diagnostics.Debug.WriteLine(DateTime.Now.ToLongTimeString());
                BuizModel.RemindCheck.CheckALL();
            }
            catch (Exception e)
            {
            }
        }
    }
}