using System.Web.Mvc;

namespace BuizApp.Areas.office
{
    public class officeAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "office";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "office_default",
                "office/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
