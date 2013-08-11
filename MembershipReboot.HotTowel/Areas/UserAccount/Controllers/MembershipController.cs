using Breeze.WebApi;
using BrockAllen.MembershipReboot;
using MembershipReboot.HotTowel.Areas.UserAccount.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Controllers
{
    [Authorize]
    public class MembershipController : Controller
    {
        //private readonly MembershipRepository _contextProvider;

        //readonly IUserAccountRepository userAccountRepository;
        //public MembershipController(IUserAccountRepository userAccountRepository)
        //{
        //    this.userAccountRepository = userAccountRepository;
        //    _contextProvider = new MembershipRepository();
        //}

        UserAccountService userAccountService;
        ClaimsBasedAuthenticationService authSvc;

        public MembershipController(
            UserAccountService userService,
            ClaimsBasedAuthenticationService authSvc)
        {
            this.userAccountService = userService;
            this.authSvc = authSvc;
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult JsonLogin(LoginInputModel model, string returnUrl)
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
                        ModelState.AddModelError("", "The password for this account has expired.");
                    }
                    else
                    {
                        if (Url.IsLocalUrl(returnUrl))
                        {
                            return Json(new { success = true, redirect = returnUrl });
                        }
                        else
                        {
                            return Json(new { success = true, redirect = "/" });
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Username or Password");
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult JsonRegister(RegisterInputModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.userAccountService.CreateAccount(model.Username, model.Password, model.Email);
                    if (SecuritySettings.Instance.RequireAccountVerification)
                    {
                        return Json(new { success = true, confirmed = false, message = "You have been sent a verification email.  Follow the instructions to verify your account." });
                    }
                    else
                    {
                        return Json(new { success = true, confirmed = true, message = "You have been confirmed to login." });
                    }
                }
                catch (ValidationException e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult JsonResetPassword(PasswordResetInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.userAccountService.ResetPassword(model.Email);
                    return Json(new { success = true, message = "You have been sent a message to verify your password reset at "+model.Email.ToString() });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult JsonChangePasswordFromResetKey(ChangePasswordFromResetKeyInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (this.userAccountService.ChangePasswordFromResetKey(model.Key, model.Password))
                    {
                        return Json(new { success = true, message = "Your password has been reset." });
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

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult JsonSendUsername(SendUsernameReminderInputModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    this.userAccountService.SendUsernameReminder(model.Email);
                    return Json(new { success = true, email = model.Email.ToString(), message = "Sent user name to " + model.Email.ToString() });
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            // If we got this far, something failed
            return Json(new { errors = GetErrorsFromModelState() });
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

    }
}