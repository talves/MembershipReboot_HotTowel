using Breeze.WebApi;
using BrockAllen.MembershipReboot;
using MembershipReboot.HotTowel.Areas.UserAccount.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
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

        [AllowAnonymous]
        [HttpPost]
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
                            return Json(new { success = true, redirect = "" });
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

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }

    }
}