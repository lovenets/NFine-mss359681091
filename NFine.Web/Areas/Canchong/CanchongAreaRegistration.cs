using System.Web.Mvc;

namespace NFine.Web.Areas.Canchong
{
    public class CanchongAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Canchong";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
               this.AreaName + "_Default",
               this.AreaName + "/{controller}/{action}/{id}",
               new { area = this.AreaName, controller = "PicHourse", action = "Index", id = UrlParameter.Optional },
               new string[] { "NFine.Web.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
