using BrockAllen.MembershipReboot;
using MembershipReboot.HotTowel.Areas.UserAccount.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        UserAccountService userAccountService;

        public LoginController(UserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.userAccountService != null)
                {
                    this.userAccountService.Dispose();
                    this.userAccountService = null;
                }
            }
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterConfirm(string id)
        {
            var result = this.userAccountService.VerifyAccount(id);
            return View("RegisterConfirm", new ResponseModel() { success = result, action = "RegisterConfirm" });
        }

        public ActionResult RegisterCancel(string id)
        {
            var result = this.userAccountService.CancelNewAccount(id);
            return View("RegisterConfirm", new ResponseModel() { success = result, action = "RegisterCancel" });
        }

    }
}
