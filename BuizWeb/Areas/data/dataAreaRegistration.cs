using System.Web.Mvc;

namespace BuizApp.Areas.data
{
    public class dataAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "data";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "data_default",
                "data/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
