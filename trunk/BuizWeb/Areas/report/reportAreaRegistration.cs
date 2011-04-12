using System.Web.Mvc;

namespace BuizApp.Areas.report
{
    public class reportAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "report";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "report_default",
                "report/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
