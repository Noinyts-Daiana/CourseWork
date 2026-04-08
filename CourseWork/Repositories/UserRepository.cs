using CourseWork.Exceptions;
using CourseWork.Models;
using CourseWork.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class UserRepository(AppDbContext context): IUserRepository
{

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await context.User
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower().Trim());
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await context.User.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == userId);

    }

    public async Task<IEnumerable<User>> GetUsersAsync(int pageNumber, int pageSize, string? searchTerm = null)
    {
        var query = context.User.AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower(); 

            query = query.Include(u => u.Role).Where(u => 
                u.FullName.ToLower().Contains(term) || 
                u.Email.ToLower().Contains(term)
                    
            );

            return query;
        }
        
        int skip = (pageNumber - 1) * pageSize;
        return await context.User.Include(u => u.Role)
            .OrderBy(a => a.Id) 
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<User> AddUserAsync(User user)
    {
        context.User.Add(user);
        await context.SaveChangesAsync();
      
        return user;
    }
    
    public async Task UpdateUserAsync(int userId, User user)
    {
        var userInDb = await context.User.FindAsync(userId);
        if (userInDb == null) throw new NotFoundException("Користувача не знайдено");

        var newEmail = user.Email?.Trim().ToLower();
        var oldEmail = userInDb.Email?.Trim().ToLower();

        if (newEmail != oldEmail)
        {
            var emailTaken = await context.User
                .AsNoTracking()
                .AnyAsync(u => u.Email.ToLower() == newEmail && u.Id != userId);

            if (emailTaken) 
            {
                throw new InvalidOperationException("Цей емейл вже використовується іншим акаунтом");
            }
            userInDb.Email = user.Email; 
        }

        userInDb.FullName = user.FullName;

        try 
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex.InnerException is Npgsql.PostgresException { SqlState: "23505" })
        {
            throw new InvalidOperationException("Конфлікт даних: цей емейл щойно був зайнятий.");
        }
    }
    
    public async Task<User?> DeleteUserAsync(int userId) 
    {
        var user = await context.User.FirstOrDefaultAsync(u => u.Id == userId);
    
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
        return await context.User
            .Include(u => u.Role) 
            .Where(u => u.RoleId == roleId)
            .ToListAsync();
    }

    public async Task<int> GetTotalUsersCountAsync()
    {
        return await context.User.CountAsync(); 
    }
}