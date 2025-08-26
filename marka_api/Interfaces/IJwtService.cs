namespace marka_api.Services
{
    public interface IJwtService
    {
        string GenerateToken(string username, IEnumerable<string> roles = null);
    }
}
