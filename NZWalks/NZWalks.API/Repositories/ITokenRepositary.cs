using Microsoft.AspNetCore.Identity;

namespace NZWalks.API.Repositories
{
    public interface ITokenRepositary
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
