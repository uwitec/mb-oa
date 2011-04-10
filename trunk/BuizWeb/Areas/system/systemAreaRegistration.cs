using System.Web.Mvc;

namespace BuizApp.Areas.system
{
    public class systemAreaqRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "system";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "system_default",
                "system/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
