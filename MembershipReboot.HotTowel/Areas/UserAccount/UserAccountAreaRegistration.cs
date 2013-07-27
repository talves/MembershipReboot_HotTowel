using System.Web.Mvc;

namespace MembershipReboot.HotTowel.Areas.UserAccount
{
    public class UserAccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "UserAccount";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //Used for the api Calls to the Membership Controller
            context.MapRoute(
                "UserAccount_api",
                "UserAccount/api/Membership/{action}/{id}",
                new { controller = "Membership", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "MembershipReboot.HotTowel.Areas.UserAccount.Controllers" }
            );
            //Used to access the Standard MVC Forms for testing, Etc
            context.MapRoute(
                "UserAccount_default",
                "UserAccount/{controller}/{action}/{id}",
                new {controller="Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "MembershipReboot.HotTowel.Areas.UserAccount.Controllers" }
            );
        }
    }
}
