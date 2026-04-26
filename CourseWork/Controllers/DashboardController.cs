using System.Security.Claims;
using CourseWork.Models;
using CourseWork.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController(AppDbContext context) : ControllerBase
{
    [HttpGet("stats")]
    [RequirePermission("ViewDashboard")]
    public async Task<IActionResult> GetStats()
    {
        var now = DateTime.UtcNow;
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var totalAnimals = await context.Animal.CountAsync();

        var adoptedAnimalIds = await context.AdoptAnimal
            .GroupBy(aa => aa.AnimalId)
            .Select(g => new
            {
                AnimalId = g.Key,
                LastStatus = g.OrderByDescending(x => x.AdoptDate ?? x.ArrivalDate).First().Status
            })
            .Where(x => x.LastStatus == AdoptionStatus.Adopted)
            .Select(x => x.AnimalId)
            .ToListAsync();

        var totalAdopted = adoptedAnimalIds.Count;
        var animalsInShelter = totalAnimals - totalAdopted;

        var transactions = await context.Transaction.ToListAsync();

        var fundBalance = transactions.Sum(t => t.IsIncome ? t.Amount : -t.Amount);
        var incomeThisMonth = transactions
            .Where(t => t.IsIncome && t.TransactionDate >= startOfMonth)
            .Sum(t => t.Amount);
        var expensesThisMonth = transactions
            .Where(t => !t.IsIncome && t.TransactionDate >= startOfMonth)
            .Sum(t => t.Amount);

        var vaccinations = await context.Vaccination.ToListAsync();

        var upcomingVaccinations = vaccinations
            .Count(v => v.NextDueDate >= now && v.NextDueDate <= now.AddDays(7));
        var overdueVaccinations = vaccinations
            .Count(v => v.NextDueDate < now);

        var lowStockCount = await context.FoodType
            .CountAsync(f => f.StockQuantity <= f.MinThreshold);

        var totalUsers = await context.User.CountAsync();
        var activeUsers = await context.User.CountAsync(u => u.IsActive);

        return Ok(new
        {
            animals = new
            {
                total = totalAnimals,
                inShelter = animalsInShelter,
                adopted = totalAdopted
            },
            finance = new
            {
                balance = fundBalance,
                incomeThisMonth,
                expensesThisMonth
            },
            medical = new
            {
                upcomingVaccinations,
                overdueVaccinations
            },
            inventory = new
            {
                lowStockCount
            },
            users = new
            {
                total = totalUsers,
                active = activeUsers
            }
        });
    }

    [HttpGet("alerts")]
    [RequirePermission("ViewDashboard")]
    public async Task<IActionResult> GetAlerts()
    {
        var now = DateTime.UtcNow;
        var autoAlerts = new List<object>();

        var overdueVaccinations = await context.Vaccination
            .Include(v => v.Animal)
            .Where(v => v.NextDueDate < now)
            .OrderByDescending(v => v.NextDueDate)
            .Take(10)
            .ToListAsync();

        foreach (var v in overdueVaccinations)
        {
            var daysOverdue = (int)(now - v.NextDueDate).TotalDays;
            autoAlerts.Add(new
            {
                id = (int?)null,
                type = "vaccination",
                severity = "danger",
                message = $"Прострочена вакцинація у {v.Animal?.Name ?? "Невідома тварина"}: {v.VaccineName} ({daysOverdue} дн. тому)",
                relatedEntityId = v.AnimalId,
                createdAt = v.NextDueDate,
                isDone = false,
                isAuto = true
            });
        }

        var upcomingVaccinations = await context.Vaccination
            .Include(v => v.Animal)
            .Where(v => v.NextDueDate >= now && v.NextDueDate <= now.AddDays(7))
            .OrderBy(v => v.NextDueDate)
            .Take(10)
            .ToListAsync();

        foreach (var v in upcomingVaccinations)
        {
            var daysLeft = (int)(v.NextDueDate - now).TotalDays;
            autoAlerts.Add(new
            {
                id = (int?)null,
                type = "vaccination",
                severity = "info",
                message = $"Планова вакцинація через {daysLeft} дн: {v.Animal?.Name} — {v.VaccineName}",
                relatedEntityId = v.AnimalId,
                createdAt = now,
                isDone = false,
                isAuto = true
            });
        }

        var lowStockFoods = await context.FoodType
            .Where(f => f.StockQuantity <= f.MinThreshold)
            .ToListAsync();

        foreach (var f in lowStockFoods)
        {
            autoAlerts.Add(new
            {
                id = (int?)null,
                type = "inventory",
                severity = "warning",
                message = $"Низький запас: {f.Name} ({f.Brand}) — залишок: {f.StockQuantity} {f.Unit}",
                relatedEntityId = f.Id,
                createdAt = now,
                isDone = false,
                isAuto = true
            });
        }

        var adoptedIds = await context.AdoptAnimal
            .GroupBy(aa => aa.AnimalId)
            .Select(g => new
            {
                AnimalId = g.Key,
                LastStatus = g.OrderByDescending(x => x.AdoptDate ?? x.ArrivalDate).First().Status
            })
            .Where(x => x.LastStatus == AdoptionStatus.Adopted)
            .Select(x => x.AnimalId)
            .ToListAsync();

        var threshold24h = now.AddHours(-24);

        var recentFeedings = await context.FeedingLog
            .GroupBy(fl => fl.AnimalId)
            .Select(g => new { AnimalId = g.Key, LastFedAt = g.Max(fl => fl.FedAt) })
            .ToListAsync();

        var fedAnimalIds = recentFeedings
            .Where(f => f.LastFedAt >= threshold24h)
            .Select(f => f.AnimalId)
            .ToHashSet();

        var unfedAnimals = await context.Animal
            .Where(a => !adoptedIds.Contains(a.Id) && !fedAnimalIds.Contains(a.Id))
            .Take(10)
            .ToListAsync();

        foreach (var a in unfedAnimals)
        {
            var lastFeeding = recentFeedings.FirstOrDefault(f => f.AnimalId == a.Id);
            var hoursAgo = lastFeeding != null
                ? (int)(now - lastFeeding.LastFedAt).TotalHours
                : -1;

            var message = hoursAgo < 0
                ? $"Тварина ніколи не отримувала запис годування: {a.Name}"
                : $"Тварина не годована більше 24 год: {a.Name} (остання — {hoursAgo} год тому)";

            autoAlerts.Add(new
            {
                id = (int?)null,
                type = "feeding",
                severity = "warning",
                message,
                relatedEntityId = a.Id,
                createdAt = now,
                isDone = false,
                isAuto = true
            });
        }

        List<object> manualAlerts = new();
        try
        {
            var dbAlerts = await context.SystemAlerts
                .Where(a => !a.IsDone)
                .OrderByDescending(a => a.Id)
                .Take(10)
                .ToListAsync();

            manualAlerts = dbAlerts.Select(a => (object)new
            {
                id = (int?)a.Id,
                type = a.Type ?? "info",
                severity = "info",
                message = a.Message ?? "",
                relatedEntityId = (int?)null,
                createdAt = DateTime.UtcNow,
                isDone = a.IsDone,
                isAuto = false
            }).ToList();
        }
        catch { }

        return Ok(new
        {
            auto = autoAlerts,
            manual = manualAlerts,
            total = autoAlerts.Count + manualAlerts.Count
        });
    }

    [HttpPatch("alerts/{id:int}/done")]
    [RequirePermission("CloseAlerts")]
    public async Task<IActionResult> MarkAlertDone(int id)
    {
        var alert = await context.SystemAlerts.FindAsync(id);
        if (alert == null)
            return NotFound(new { message = "Сповіщення не знайдено" });

        alert.IsDone = true;
        await context.SaveChangesAsync();

        return Ok(new { message = "Сповіщення закрито" });
    }

    [HttpGet("activity")]
    [RequirePermission("ViewDashboard")]
    public async Task<IActionResult> GetActivity()
    {
        var arrivals = await context.AdoptAnimal
            .Include(aa => aa.Animal)
            .Where(aa => aa.Status == AdoptionStatus.Returned || aa.OwnerId == null)
            .Select(aa => new
            {
                date = (DateTime?)aa.ArrivalDate,
                description = $"{aa.Animal.Name} прибула до притулку",
                type = "arrival",
                icon = "paw"
            })
            .Take(20)
            .ToListAsync();

        var adoptions = await context.AdoptAnimal
            .Include(aa => aa.Animal)
            .Where(aa => aa.Status == AdoptionStatus.Adopted && aa.AdoptDate != null)
            .Select(aa => new
            {
                date = (DateTime?)aa.AdoptDate,
                description = $"{aa.Animal.Name} знайшла нову родину",
                type = "adoption",
                icon = "heart"
            })
            .Take(20)
            .ToListAsync();

        var transactions = await context.Transaction
            .Include(t => t.Category)
            .OrderByDescending(t => t.TransactionDate)
            .Take(10)
            .Select(t => new
            {
                date = (DateTime?)t.TransactionDate,
                description = $"{(t.IsIncome ? "Надходження" : "Витрата")}: {t.Amount:N0} ₴ — {t.Category!.Name}",
                type = t.IsIncome ? "income" : "expense",
                icon = "wallet"
            })
            .ToListAsync();

        var recentFeedings = await context.FeedingLog
            .Include(fl => fl.Animal)
            .Include(fl => fl.FoodType)
            .Where(fl => fl.FedAt >= DateTime.UtcNow.AddHours(-24))
            .OrderByDescending(fl => fl.FedAt)
            .Take(10)
            .Select(fl => new
            {
                date = (DateTime?)fl.FedAt,
                description = $"{fl.Animal!.Name} нагодована ({fl.Amount} {fl.FoodType!.Unit} {fl.FoodType.Name})",
                type = "feeding",
                icon = "utensils"
            })
            .ToListAsync();

        var result = arrivals
            .Concat<object>(adoptions)
            .Concat(transactions)
            .Concat(recentFeedings)
            .Where(x => ((dynamic)x).date != null)
            .OrderByDescending(x => ((dynamic)x).date)
            .Take(30)
            .ToList();

        return Ok(result);
    }
}