namespace CourseWork.Services;

public interface IPermissionService
{
    Task<bool> RoleHasPermissionAsync(int roleId, string permissionName);
    Task<List<string>> GetRolePermissionsAsync(int roleId);
}