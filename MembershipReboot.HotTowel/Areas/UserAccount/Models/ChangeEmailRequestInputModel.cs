using System.ComponentModel.DataAnnotations;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Models
{
    public class ChangeEmailRequestInputModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "New Email Address")]
        public string NewEmail { get; set; }
    }
}