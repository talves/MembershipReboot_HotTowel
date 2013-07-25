using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Models
{
    public class ChangeEmailFromKeyInputModel
    {
        [Required]
        [DataType(DataType.Password)]
        public virtual string Password { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "New Email Address")]
        public virtual string NewEmail { get; set; }

        [HiddenInput]
        public virtual string Key { get; set; }
    }
}