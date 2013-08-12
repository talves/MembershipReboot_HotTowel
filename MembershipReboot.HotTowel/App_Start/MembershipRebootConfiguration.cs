using BrockAllen.MembershipReboot;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace MembershipReboot.HotTowel.App_Start
{
    public class PasswordValidator : IValidator
    {
        public ValidationResult Validate(UserAccountService service, UserAccount account, string value)
        {
            if (value.Length < 4)
            {
                return new ValidationResult("Password must be at least 4 characters long");
            }
            
            return null;
        }
    }

    public class MembershipRebootConfig
    {
        public static MembershipRebootConfiguration Create()
        {
            var settings = SecuritySettings.Instance;
            settings.MultiTenant = false;
            
            var config = new MembershipRebootConfiguration(settings, new DelegateFactory(()=>new DefaultUserAccountRepository(settings.ConnectionStringName)));
            config.RegisterPasswordValidator(new PasswordValidator());
            var delivery = new SmtpMessageDelivery();
            var formatter = new CustomEmailMessageFormatter(new Lazy<ApplicationInformation>(() =>
            {
                // build URL
                var baseUrl = HttpContext.Current.GetApplicationUrl();
                // area name
                baseUrl += "UserAccount/";

                return new ApplicationInformation
                {
                    ApplicationName = "Mvc4 Bootstrap Example App",
                    LoginUrl = baseUrl + "Login",
                    VerifyAccountUrl = baseUrl + "Login/RegisterConfirm/",
                    CancelNewAccountUrl = baseUrl + "Login/RegisterCancel/",
                    ConfirmPasswordResetUrl = baseUrl + "Login/ConfirmPassword/",
                    ConfirmChangeEmailUrl = baseUrl + "Login/ConfirmEmail/"

                };
            }));
            if (settings.RequireAccountVerification)
            {
                config.AddEventHandler(new EmailAccountCreatedEventHandler(formatter, delivery));
            }
            config.AddEventHandler(new EmailAccountEventsHandler(formatter, delivery));
            // Add the event to email someone of hacking attempts or failed login attempts
            config.AddEventHandler(new NotifyAccountOwnerWhenTooManyFailedLoginAttempts());
            
            return config;
        }
    }
}