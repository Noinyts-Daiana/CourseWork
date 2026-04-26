using System.Globalization;
using System.Text;
using System.Xml.Linq;
using CourseWork.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseWork.Controllers;
[ApiController]
[Route("api/reports")]
[Authorize]
public class ReportController(AppDbContext context) : ControllerBase
{
    [HttpGet("full")]
    public async Task<IActionResult> FullExport([FromQuery] string format = "json")
    {
        var report = await BuildFullReport();

        return format.ToLower() switch
        {
            "csv"  => File(Encoding.UTF8.GetBytes(ToCsv(report)),  "text/csv",              $"shelter_export_{DateTime.UtcNow:yyyyMMdd}.csv"),
            "xml"  => File(Encoding.UTF8.GetBytes(ToXml(report)),  "application/xml",       $"shelter_export_{DateTime.UtcNow:yyyyMMdd}.xml"),
            _      => File(Encoding.UTF8.GetBytes(ToJson(report)),  "application/json",      $"shelter_export_{DateTime.UtcNow:yyyyMMdd}.json")
        };
    }
    
    [HttpGet("animals")]
    public async Task<IActionResult> AnimalsReport([FromQuery] string format = "json")
    {
        var animals = await context.Animal
            .Include(a => a.Specie)
            .Include(a => a.Breed)
            .Include(a => a.AdoptAnimals)
            .OrderBy(a => a.Id)
            .ToListAsync();

        var rows = animals.Select(a =>
        {
            var lastAdopt = a.AdoptAnimals
                .OrderByDescending(aa => aa.AdoptDate ?? aa.ArrivalDate)
                .FirstOrDefault();
            return new Dictionary<string, string>
            {
                ["id"]           = a.Id.ToString(),
                ["name"]         = a.Name,
                ["species"]      = a.Specie?.Name ?? "",
                ["breed"]        = a.Breed?.Name ?? "",
                ["sex"]          = a.Sex == Sex.Male ? "Чоловіча" : "Жіноча",
                ["birthday"]     = a.Birthday?.ToString("dd.MM.yyyy") ?? "",
                ["weight_kg"]    = a.Weight.ToString(CultureInfo.InvariantCulture),
                ["height_cm"]    = a.Height.ToString(CultureInfo.InvariantCulture),
                ["sterilized"]   = a.IsSterilized ? "Так" : "Ні",
                ["status"]       = lastAdopt == null ? "Невідомо"
                                 : lastAdopt.Status == AdoptionStatus.Adopted ? "Усиновлений"
                                 : lastAdopt.Status == AdoptionStatus.Returned ? "В притулку"
                                 : "На утриманні",
                ["arrival_date"] = lastAdopt?.ArrivalDate.ToString("dd.MM.yyyy HH:mm") ?? "",
                ["adopt_date"]   = lastAdopt?.AdoptDate?.ToString("dd.MM.yyyy HH:mm") ?? "",
                ["description"]  = a.Description ?? ""
            };
        }).ToList();

        var headers = new[] { "id","name","species","breed","sex","birthday","weight_kg","height_cm","sterilized","status","arrival_date","adopt_date","description" };
        return FormatResponse(rows, headers, "animals", format, "animals_report");
    }

    [HttpGet("finance")]
    public async Task<IActionResult> FinanceReport(
        [FromQuery] string  format   = "json",
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate   = null)
    {
        var query = context.Transaction.Include(t => t.Category).Include(t => t.User).AsQueryable();

        if (fromDate.HasValue) query = query.Where(t => t.TransactionDate >= fromDate.Value);
        if (toDate.HasValue)   query = query.Where(t => t.TransactionDate <= toDate.Value);

        var transactions = await query.OrderBy(t => t.TransactionDate).ToListAsync();

        var rows = transactions.Select(t => new Dictionary<string, string>
        {
            ["id"]               = t.Id.ToString(),
            ["date"]             = t.TransactionDate.ToString("dd.MM.yyyy HH:mm"),
            ["type"]             = t.IsIncome ? "Дохід" : "Витрата",
            ["amount_uah"]       = t.Amount.ToString(CultureInfo.InvariantCulture),
            ["category"]         = t.Category?.Name ?? "",
            ["description"]      = t.Description ?? "",
            ["created_by"]       = t.User?.FullName ?? ""
        }).ToList();

        var total_income   = transactions.Where(t =>  t.IsIncome).Sum(t => t.Amount);
        var total_expense  = transactions.Where(t => !t.IsIncome).Sum(t => t.Amount);

        // Додаємо підсумковий рядок
        rows.Add(new Dictionary<string, string>
        {
            ["id"] = "ПІДСУМОК", ["date"] = "", ["type"] = "",
            ["amount_uah"] = $"Доходи: {total_income} / Витрати: {total_expense} / Баланс: {total_income - total_expense}",
            ["category"] = "", ["description"] = "", ["created_by"] = ""
        });

        var headers = new[] { "id","date","type","amount_uah","category","description","created_by" };
        return FormatResponse(rows, headers, "finance", format, "finance_report");
    }
    
    [HttpGet("medical")]
    public async Task<IActionResult> MedicalReport([FromQuery] string format = "json")
    {
        var vaccinations = await context.Vaccination
            .Include(v => v.Animal)
            .OrderBy(v => v.NextDueDate)
            .ToListAsync();

        var exams = await context.MedicalExam
            .Include(e => e.Animal)
            .OrderByDescending(e => e.ExamDate)
            .ToListAsync();

        var vacRows = vaccinations.Select(v => new Dictionary<string, string>
        {
            ["record_type"]    = "Вакцинація",
            ["animal_name"]    = v.Animal?.Name ?? "",
            ["vaccine_name"]   = v.VaccineName ?? "",
            ["date_given"]     = v.DateAdministered.ToString("dd.MM.yyyy"),
            ["next_due_date"]  = v.NextDueDate.ToString("dd.MM.yyyy"),
            ["is_overdue"]     = v.NextDueDate < DateTime.UtcNow ? "Так" : "Ні",
            ["exam_temp"]      = "",
            ["exam_weight"]    = "",
            ["notes"]          = ""
        });

        var examRows = exams.Select(e => new Dictionary<string, string>
        {
            ["record_type"]    = "Медогляд",
            ["animal_name"]    = e.Animal?.Name ?? "",
            ["vaccine_name"]   = "",
            ["date_given"]     = e.ExamDate.ToString("dd.MM.yyyy"),
            ["next_due_date"]  = "",
            ["is_overdue"]     = "",
            ["exam_temp"]      = e.Temperature.ToString(CultureInfo.InvariantCulture),
            ["exam_weight"]    = e.Weight.ToString(CultureInfo.InvariantCulture),
            ["notes"]          = e.Notes ?? ""
        });

        var rows = vacRows.Concat(examRows).ToList();
        var headers = new[] { "record_type","animal_name","vaccine_name","date_given","next_due_date","is_overdue","exam_temp","exam_weight","notes" };
        return FormatResponse(rows, headers, "medical", format, "medical_report");
    }

    [HttpGet("inventory")]
    public async Task<IActionResult> InventoryReport([FromQuery] string format = "json")
    {
        var foods = await context.FoodType.OrderBy(f => f.Name).ToListAsync();

        var rows = foods.Select(f => new Dictionary<string, string>
        {
            ["id"]            = f.Id.ToString(),
            ["name"]          = f.Name ?? "",
            ["brand"]         = f.Brand ?? "",
            ["unit"]          = f.Unit ?? "",
            ["stock"]         = f.StockQuantity.ToString(CultureInfo.InvariantCulture),
            ["min_threshold"] = f.MinThreshold.ToString(CultureInfo.InvariantCulture),
            ["is_low_stock"]  = f.StockQuantity <= f.MinThreshold ? "Так" : "Ні"
        }).ToList();

        var headers = new[] { "id","name","brand","unit","stock","min_threshold","is_low_stock" };
        return FormatResponse(rows, headers, "inventory", format, "inventory_report");
    }


    private IActionResult FormatResponse(
        List<Dictionary<string, string>> rows,
        string[] headers,
        string section,
        string format,
        string fileName)
    {
        return format.ToLower() switch
        {
            "csv" => File(
                Encoding.UTF8.GetBytes(RowsToCsv(rows, headers)),
                "text/csv",
                $"{fileName}_{DateTime.UtcNow:yyyyMMdd}.csv"),
            "xml" => File(
                Encoding.UTF8.GetBytes(RowsToXml(rows, section)),
                "application/xml",
                $"{fileName}_{DateTime.UtcNow:yyyyMMdd}.xml"),
            _ => Ok(new
            {
                generatedAt = DateTime.UtcNow,
                section,
                totalRows = rows.Count,
                data = rows
            })
        };
    }

    private async Task<object> BuildFullReport()
    {
        var animals = await context.Animal.Include(a => a.Specie).Include(a => a.Breed)
            .Include(a => a.AdoptAnimals).ToListAsync();
        var vaccinations    = await context.Vaccination.Include(v => v.Animal).ToListAsync();
        var exams           = await context.MedicalExam.Include(e => e.Animal).ToListAsync();
        var transactions    = await context.Transaction.Include(t => t.Category).ToListAsync();
        var foods           = await context.FoodType.ToListAsync();
        var users           = await context.User.Include(u => u.Role).ToListAsync();
        var alerts          = await context.SystemAlerts.OrderByDescending(a => a.CreatedAt).Take(50).ToListAsync();

        return new
        {
            meta = new
            {
                exportedAt    = DateTime.UtcNow,
                exportedBy    = "ShelterSystem",
                formatVersion = "1.0",
                schema = new
                {
                    animals      = "id, name, species, breed, sex, birthday, weight, height, sterilized, status",
                    vaccinations = "id, animal, vaccine, dateGiven, nextDue, isOverdue",
                    exams        = "id, animal, date, temperature, weight, notes",
                    transactions = "id, date, type, amount, category, description",
                    inventory    = "id, name, brand, unit, stock, minThreshold, isLowStock",
                    users        = "id, fullName, email, role, isActive, createdAt",
                    alerts       = "id, type, severity, message, isAuto, isDone, createdAt"
                }
            },
            summary = new
            {
                totalAnimals      = animals.Count,
                adoptedAnimals    = animals.Count(a => a.AdoptAnimals.Any(aa => aa.Status == AdoptionStatus.Adopted)),
                totalTransactions = transactions.Count,
                fundBalance       = transactions.Sum(t => t.IsIncome ? t.Amount : -t.Amount),
                lowStockItems     = foods.Count(f => f.StockQuantity <= f.MinThreshold),
                overdueVaccines   = vaccinations.Count(v => v.NextDueDate < DateTime.UtcNow),
                activeUsers       = users.Count(u => u.IsActive)
            },
            animals = animals.Select(a =>
            {
                var last = a.AdoptAnimals.OrderByDescending(aa => aa.AdoptDate ?? aa.ArrivalDate).FirstOrDefault();
                return new
                {
                    a.Id, a.Name,
                    species     = a.Specie?.Name,
                    breed       = a.Breed?.Name,
                    sex         = a.Sex.ToString(),
                    birthday    = a.Birthday?.ToString("yyyy-MM-dd"),
                    a.Weight, a.Height, a.IsSterilized,
                    status      = last?.Status.ToString(),
                    arrivalDate = last?.ArrivalDate,
                    adoptDate   = last?.AdoptDate
                };
            }),
            vaccinations = vaccinations.Select(v => new
            {
                v.Id,
                animal      = v.Animal?.Name,
                v.VaccineName,
                dateGiven   = v.DateAdministered,
                nextDue     = v.NextDueDate,
                isOverdue   = v.NextDueDate < DateTime.UtcNow
            }),
            medicalExams = exams.Select(e => new
            {
                e.Id,
                animal  = e.Animal?.Name,
                e.ExamDate, e.Temperature, e.Weight, e.Notes
            }),
            transactions = transactions.Select(t => new
            {
                t.Id,
                date        = t.TransactionDate,
                type        = t.IsIncome ? "income" : "expense",
                t.Amount,
                category    = t.Category?.Name,
                t.Description
            }),
            inventory = foods.Select(f => new
            {
                f.Id, f.Name, f.Brand, f.Unit,
                stock        = f.StockQuantity,
                minThreshold = f.MinThreshold,
                isLowStock   = f.StockQuantity <= f.MinThreshold
            }),
            users = users.Select(u => new
            {
                u.Id, u.FullName, u.Email,
                role      = u.Role?.Name,
                u.IsActive,
                createdAt = u.CreatedAt
            }),
            systemAlerts = alerts.Select(a => new
            {
                a.Id, a.Type, a.Severity, a.Message, a.IsAuto, a.IsDone, a.CreatedAt
            })
        };
    }

    private static string ToJson(object data)
    {
        return System.Text.Json.JsonSerializer.Serialize(data, new System.Text.Json.JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        });
    }

    private static string ToCsv(object report)
    {
        return "Full export is best used in JSON or XML format. Use /api/reports/animals?format=csv for flat CSV.";
    }

    private static string ToXml(object report)
    {
        var json   = ToJson(report);
        var doc    = new XDocument(new XElement("ShelterReport",
            new XAttribute("exportedAt", DateTime.UtcNow.ToString("o")),
            new XElement("RawJson", new XCData(json))));
        return doc.ToString();
    }

    private static string RowsToCsv(List<Dictionary<string, string>> rows, string[] headers)
    {
        var sb = new StringBuilder();
        sb.AppendLine(string.Join(";", headers));

        foreach (var row in rows)
        {
            var values = headers.Select(h =>
            {
                var val = row.GetValueOrDefault(h, "");
                // Екранування для CSV
                if (val.Contains(';') || val.Contains('"') || val.Contains('\n'))
                    val = $"\"{val.Replace("\"", "\"\"")}\"";
                return val;
            });
            sb.AppendLine(string.Join(";", values));
        }

        return sb.ToString();
    }

    private static string RowsToXml(List<Dictionary<string, string>> rows, string section)
    {
        var doc = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement("Report",
                new XAttribute("section", section),
                new XAttribute("exportedAt", DateTime.UtcNow.ToString("o")),
                new XAttribute("totalRows", rows.Count),
                rows.Select(row =>
                    new XElement("Row",
                        row.Select(kv => new XElement(kv.Key, kv.Value))
                    )
                )
            )
        );
        return doc.ToString();
    }
}