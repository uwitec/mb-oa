using System.Web.Mvc;

namespace BuizApp.Areas.msgService
{
    public class msgServiceAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "msgService";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "msgService_default",
                "msgService/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
