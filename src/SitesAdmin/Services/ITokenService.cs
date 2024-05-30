using SitesAdmin.Features.Identity.Data;

namespace SitesAdmin.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user);
    }
}
