using System.Web.Mvc;

namespace MembershipReboot.HotTowel.Controllers
{
    public class HotTowelController : Controller
    {
        //
        // GET: /HotTowel/
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

    }
}
