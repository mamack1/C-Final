using System.ComponentModel.DataAnnotations;

namespace C_DiscApp.ViewModels
{
    public class ProfileViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Username")]
        public string Username { get; set; }
    }
}