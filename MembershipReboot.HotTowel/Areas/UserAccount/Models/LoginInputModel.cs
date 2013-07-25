using System.ComponentModel.DataAnnotations;

namespace MembershipReboot.HotTowel.Areas.UserAccount.Models
{
    public class LoginInputModel
    {
        [Required]
        [Display(Name = "User name")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [ScaffoldColumn(false)]
        public string ReturnUrl { get; set; }
        
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}