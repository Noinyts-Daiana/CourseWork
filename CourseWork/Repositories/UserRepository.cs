using CourseWork.Models;
using CourseWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class UserRepository(AppDbContext context): IUserRepository
{

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.Users
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower().Trim());
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await context.Users.FindAsync(userId);
    }

    public async Task<IEnumerable<User>> GetUsersAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<User> AddUserAsync(User user)
    {
        context.Users.Add(user);
         await context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }
    public async Task<User?> DeleteUserAsync(int userId) 
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    
        if (user == null)
        {
            return null; 
        }

        user.IsActive = false;
        user.UpdatedAt = DateTime.UtcNow; 

        await context.SaveChangesAsync();

        return user;
    }

    public async Task<IEnumerable<User>> GetUserByRole(int roleId)
    {
        var users = await context.Users.Where(u=>u.RoleId==roleId).ToListAsync();
        return users;
    }
}