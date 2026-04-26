using CourseWork.DTOs;
using CourseWork.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/permissions")]
[Authorize]
public class PermissionController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var permissions = await context.Permission
            .OrderBy(p => p.Id)
            .Select(p => new PermissionDto { Id = p.Id, Name = p.Name })
            .ToListAsync();

        return Ok(permissions);
    }

    [HttpGet("role/{roleId:int}")]
    public async Task<IActionResult> GetByRole(int roleId)
    {
        var role = await context.Role.FindAsync(roleId);
        if (role == null) return NotFound(new { message = "Роль не знайдено" });

        var permissionIds = await context.RolePermission
            .Where(rp => rp.RoleId == roleId)
            .Select(rp => rp.PermissionId)
            .ToListAsync();

        var permissions = await context.Permission
            .Where(p => permissionIds.Contains(p.Id))
            .Select(p => new PermissionDto { Id = p.Id, Name = p.Name })
            .ToListAsync();

        return Ok(new RoleWithPermissionsDto
        {
            RoleId      = role.Id,
            RoleName    = role.Name,
            Permissions = permissions
        });
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetAllRolesWithPermissions()
    {
        var roles = await context.Role.ToListAsync();
        var allRolePermissions = await context.RolePermission.ToListAsync();
        var allPermissions = await context.Permission.ToListAsync();

        var result = roles.Select(role =>
        {
            var rolePermIds = allRolePermissions
                .Where(rp => rp.RoleId == role.Id)
                .Select(rp => rp.PermissionId)
                .ToHashSet();

            return new RoleWithPermissionsDto
            {
                RoleId      = role.Id,
                RoleName    = role.Name,
                Permissions = allPermissions
                    .Where(p => rolePermIds.Contains(p.Id))
                    .Select(p => new PermissionDto { Id = p.Id, Name = p.Name })
                    .ToList()
            };
        });

        return Ok(result);
    }
    
    [HttpPut("role/{roleId:int}")]
    public async Task<IActionResult> UpdateRolePermissions(
        int roleId,
        [FromBody] UpdateRolePermissionsDto dto)
    {
        var role = await context.Role.FindAsync(roleId);
        if (role == null) return NotFound(new { message = "Роль не знайдено" });

        var validIds = await context.Permission
            .Where(p => dto.PermissionIds.Contains(p.Id))
            .Select(p => p.Id)
            .ToListAsync();

        if (validIds.Count != dto.PermissionIds.Count)
            return BadRequest(new { message = "Деякі дозволи не знайдено" });

        var existing = context.RolePermission.Where(rp => rp.RoleId == roleId);
        context.RolePermission.RemoveRange(existing);

        var newPermissions = dto.PermissionIds.Select(pid => new RolePermission
        {
            RoleId       = roleId,
            PermissionId = pid
        });
        await context.RolePermission.AddRangeAsync(newPermissions);
        await context.SaveChangesAsync();

        return Ok(new { message = $"Дозволи для ролі «{role.Name}» оновлено" });
    }

    [HttpPost("role/{roleId:int}/add/{permissionId:int}")]
    public async Task<IActionResult> AddPermission(int roleId, int permissionId)
    {
        var exists = await context.RolePermission
            .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

        if (exists) return Conflict(new { message = "Дозвіл вже призначено цій ролі" });

        context.RolePermission.Add(new RolePermission
        {
            RoleId       = roleId,
            PermissionId = permissionId
        });
        await context.SaveChangesAsync();

        return Ok(new { message = "Дозвіл додано" });
    }

    [HttpDelete("role/{roleId:int}/remove/{permissionId:int}")]
    public async Task<IActionResult> RemovePermission(int roleId, int permissionId)
    {
        var rp = await context.RolePermission
            .FirstOrDefaultAsync(x => x.RoleId == roleId && x.PermissionId == permissionId);

        if (rp == null) return NotFound(new { message = "Дозвіл не знайдено для цієї ролі" });

        context.RolePermission.Remove(rp);
        await context.SaveChangesAsync();

        return Ok(new { message = "Дозвіл видалено" });
    }

    [HttpGet("check")]
    public async Task<IActionResult> CheckPermission(
        [FromQuery] int    userId,
        [FromQuery] string permission)
    {
        var user = await context.User.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null) return NotFound(new { message = "Користувача не знайдено" });

        var hasPermission = await context.RolePermission
            .Join(context.Permission,
                rp => rp.PermissionId,
                p  => p.Id,
                (rp, p) => new { rp.RoleId, p.Name })
            .AnyAsync(x => x.RoleId == user.RoleId && x.Name == permission);

        return Ok(new { hasPermission, userId, permission, roleId = user.RoleId });
    }
}