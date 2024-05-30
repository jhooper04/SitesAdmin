using Microsoft.AspNetCore.Identity;

namespace SitesAdmin.Features.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; } = null!;
    }
}
