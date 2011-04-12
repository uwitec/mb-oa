using System.Web.Mvc;

namespace BuizApp.Areas.resource
{
    public class resourceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "resource";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "resource_default",
                "resource/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
