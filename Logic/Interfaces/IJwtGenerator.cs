using TheatreApi.Entities;

namespace TheatreApi.Logic.Interfaces
{
    public interface IJwtGenerator
    {
         string CreateToken(AppUser user);
    }
}