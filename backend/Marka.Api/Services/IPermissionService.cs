namespace Marka.Api.Services;

public interface IPermissionService
{
    Task<bool> UserHasPermissionAsync(Guid userId, string permissionCode);
    Task<List<string>> GetUserPermissionsAsync(Guid userId);
}
