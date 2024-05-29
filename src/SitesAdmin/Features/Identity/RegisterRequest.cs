using System.ComponentModel.DataAnnotations;

namespace SitesAdmin.Features.Identity
{
    public class RegisterRequest
    {
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }

        //public Role Role { get; set; } = Role.User;
    }
}
