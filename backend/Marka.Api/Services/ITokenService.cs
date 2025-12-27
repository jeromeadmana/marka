using Marka.Api.Models;

namespace Marka.Api.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}
