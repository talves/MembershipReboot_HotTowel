using BrockAllen.MembershipReboot;
using Breeze.WebApi;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;

//using Ninject;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Models
{
    public class MembershipRebootDbContext : DbContext
    {
        public BrockAllen.MembershipReboot.UserAccount User { get; set; }
        public DbSet<BrockAllen.MembershipReboot.UserAccount> Users { get; set; }

        public LoginInputModel LoginInputModel { get; set; }
        public ChangeEmailFromKeyInputModel ChangeEmailFromKeyInputModel { get; set; }
        public ChangeEmailRequestInputModel ChangeEmailRequestInputModel { get; set; }
        public ChangePasswordFromResetKeyInputModel ChangePasswordFromResetKeyInputModel { get; set; }
        public ChangeUsernameInputModel ChangeUsernameInputModel { get; set; }
        public PasswordResetInputModel PasswordResetInputModel { get; set; }
        public RegisterInputModel RegisterInputModel { get; set; }
        public SendUsernameReminderInputModel SendUsernameReminderInputModel { get; set; }
    }
}