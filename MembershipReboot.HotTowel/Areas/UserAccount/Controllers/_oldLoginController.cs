﻿using BrockAllen.MembershipReboot;
using MembershipReboot.HotTowel.Areas.UserAccount.Models;
using System.Web.Mvc;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        UserAccountService userAccountService;
        ClaimsBasedAuthenticationService authSvc;

        public LoginController(
            UserAccountService userService,
            ClaimsBasedAuthenticationService authSvc)
        {
            this.userAccountService = userService;
            this.authSvc = authSvc;
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

                if (this.authSvc != null)
                {
                    this.authSvc.Dispose();
                    this.authSvc = null;
                }
            }
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return View(new LoginInputModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginInputModel model)
        {
            if (ModelState.IsValid)
            {
                BrockAllen.MembershipReboot.UserAccount account;
                if (userAccountService.AuthenticateWithUsernameOrEmail(model.Username, model.Password, out account))
                {
                    authSvc.SignIn(account);

                    //authSvc.SignIn(model.Username);

                    if (userAccountService.IsPasswordExpired(model.Username))
                    {
                        return RedirectToAction("Index", "ChangePassword");
                    }
                    else
                    {
                        if (Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }

            return View(model);
        }
    }
}
