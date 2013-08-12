using BrockAllen.MembershipReboot;
using MembershipReboot.HotTowel.Areas.UserAccount.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public ActionResult ConfirmPassword(string id)
        {
            var vm = new ChangePasswordFromResetKeyInputModel()
            {
                Key = id
            };
            return View("ConfirmPassword", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmPassword(ChangePasswordFromResetKeyInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var result = this.userAccountService.ChangePasswordFromResetKey(model.Key, model.Password);
                    if (result)
                    {
                        return View("RegisterConfirm", new ResponseModel() { success = result, action = "PasswordReset" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Error changing password. The key might be invalid.");
                    }
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }

        public ActionResult ConfirmEmail(string id)
        {
            var vm = new ChangeEmailFromKeyInputModel()
            {
                Key = id
            };
            return View("ConfirmEmail", vm);
        }

    }
}
