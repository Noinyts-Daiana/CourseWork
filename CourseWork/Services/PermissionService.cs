using CourseWork;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Services;

public class PermissionService(AppDbContext context) : IPermissionService
{
    public async Task<bool> RoleHasPermissionAsync(int roleId, string permissionName)
    {
        Console.WriteLine($"Перевірка: Роль {roleId}, Дозвіл {permissionName}");
        return await context.RolePermission
            .Join(context.Permission,
                rp => rp.PermissionId,
                p  => p.Id,
                (rp, p) => new { rp.RoleId, p.Name })
            .AnyAsync(x => x.RoleId == roleId && x.Name == permissionName);
    }

    public async Task<List<string>> GetRolePermissionsAsync(int roleId)
    {
        return await context.RolePermission
            .Where(rp => rp.RoleId == roleId)
            .Join(context.Permission,
                rp => rp.PermissionId,
                p  => p.Id,
                (rp, p) => p.Name)
            .ToListAsync();
    }
}