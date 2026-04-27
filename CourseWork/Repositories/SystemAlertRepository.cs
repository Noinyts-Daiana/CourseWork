using CourseWork.Hubs;
using CourseWork.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Repositories;

public class SystemAlertRepository(AppDbContext context, IHubContext<AlertHub> hubContext) : ISystemAlertRepository
{
    public async Task<IEnumerable<SystemAlerts>> GetAllAsync(bool? isDone, int pageNumber, int pageSize)
    {
        var query = context.SystemAlerts.AsQueryable();
        if (isDone.HasValue) query = query.Where(a => a.IsDone == isDone.Value);
        return await query.OrderByDescending(a => a.Id).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<SystemAlerts?> GetByIdAsync(int id) => await context.SystemAlerts.FindAsync(id);

    public async Task<SystemAlerts> AddAsync(SystemAlerts alert)
    {
        context.SystemAlerts.Add(alert);
        await context.SaveChangesAsync();
        await hubContext.Clients.All.SendAsync("ReceiveNewAlert", alert);
        return alert;
    }

    public async Task UpdateAsync(SystemAlerts alert)
    {
        context.SystemAlerts.Update(alert);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var alert = await context.SystemAlerts.FindAsync(id);
        if (alert != null) { context.SystemAlerts.Remove(alert); await context.SaveChangesAsync(); }
    }

    public async Task<int> GetCountAsync(bool? isDone)
    {
        var query = context.SystemAlerts.AsQueryable();
        if (isDone.HasValue) query = query.Where(a => a.IsDone == isDone.Value);
        return await query.CountAsync();
    }
    public async Task<bool> CheckIfExistsAsync(string message, DateTime date)
    {
        var since = date.AddDays(-7);
        return await context.SystemAlerts
            .AnyAsync(a => a.Message == message && a.CreatedAt >= since && !a.IsDone);
    }
}