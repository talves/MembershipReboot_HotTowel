using BrockAllen.MembershipReboot;
using System;

namespace MembershipReboot.HotTowel
{

    //***********************************************************************************************************
    //**** Custom Email Messages
    //***********************************************************************************************************

    public class CustomEmailMessageFormatter : EmailMessageFormatter
    {
        public CustomEmailMessageFormatter(Lazy<ApplicationInformation> info)
            : base(info)
        {
        }

        protected override string GetBody(UserAccountEvent evt)
        {
            if (evt is AccountVerifiedEvent)
            {
                return "Your account was verified with " + this.ApplicationInformation.ApplicationName + ". Welcome!";
            }

            if (evt is AccountClosedEvent)
            {
                return FormatValue(evt, "your account was closed with {applicationName}. good riddance.");
            }

            return base.GetBody(evt);
        }
    }

    //***********************************************************************************************************
    //**** Custom Event Handlers
    //***********************************************************************************************************

    // Add the event to email someone of hacking attempts or failed login attempts
    public class NotifyAccountOwnerWhenTooManyFailedLoginAttempts
    : IEventHandler<TooManyRecentPasswordFailuresEvent>
    {
        public void Handle(TooManyRecentPasswordFailuresEvent evt)
        {
            var smtp = new SmtpMessageDelivery();
            var msg = new Message
            {
                To = evt.Account.Email,
                Subject = "Your Account",
                Body = "It seems someone has tried to login too many times to your account. It's currently locked. \nIt is recommended you Reset your password (required="+evt.Account.RequiresPasswordReset.ToString()+")"
            };
            smtp.Send(msg);
        }
    }

}