using System.ComponentModel.DataAnnotations;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Models
{
    public class ChangeUsernameInputModel
    {
        [Required]
        [Display(Name = "New Username")]
        public string NewUsername { get; set; }
    }
}