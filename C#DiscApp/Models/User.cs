using Microsoft.AspNetCore.Identity;


namespace C_DiscApp.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateJoined { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}