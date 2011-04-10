using System.Web.Mvc;

namespace BuizApp
{
    /// <summary>
    /// 统一区域管理,没有区域设定的default路由在这里定义
    /// default区域的路由链接无区域标识
    /// </summary>
    public class defaultAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "default";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }, // Parameter defaults
                new { }, //约束
                new[] { "BuizApp" } //控制器类的命名空间
            );
            // 以上数据是否可以写在数据库的模块表里,自动加载???
        }
    }
}
