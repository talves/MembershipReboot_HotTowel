using System.ComponentModel.DataAnnotations;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Models
{
    public class PasswordResetInputModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}