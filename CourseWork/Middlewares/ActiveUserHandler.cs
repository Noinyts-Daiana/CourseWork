using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

public class ActiveUserRequirement : IAuthorizationRequirement { }

public class ActiveUserHandler : AuthorizationHandler<ActiveUserRequirement>
{
    private readonly AppDbContext _context;
    public ActiveUserHandler(AppDbContext context) => _context = context;

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ActiveUserRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null) return;

        var userId = int.Parse(userIdClaim.Value);
        var isActive = await _context.User
            .Where(u => u.Id == userId)
            .Select(u => u.IsActive)
            .FirstOrDefaultAsync();

        if (isActive) context.Succeed(requirement);
    }
}