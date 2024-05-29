using SitesAdmin.Features.Identity;

namespace SitesAdmin.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
