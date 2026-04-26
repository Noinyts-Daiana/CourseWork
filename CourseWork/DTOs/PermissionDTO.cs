
namespace CourseWork.DTOs;

public class PermissionDto
{
    public int    Id   { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class RoleWithPermissionsDto
{
    public int                   RoleId      { get; set; }
    public string                RoleName    { get; set; } = string.Empty;
    public List<PermissionDto>   Permissions { get; set; } = new();
}

public class UpdateRolePermissionsDto
{
    public List<int> PermissionIds { get; set; } = new();
}