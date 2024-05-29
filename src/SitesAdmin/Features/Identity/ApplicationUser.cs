using Microsoft.AspNetCore.Identity;

namespace SitesAdmin.Features.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } = null!;
    }
}
