using CourseWork.DTOs;
using CourseWork.Mappers;
using CourseWork.Models;
using CourseWork.Repositories;
using CourseWork.Repositories.Interfaces;

namespace CourseWork.Services;

public class SystemAlertService(ISystemAlertRepository alertRepository, 
    IVaccinationRepository vaccinationRepository,
    ITransactionRepository transactionRepository  
) : ISystemAlertService
{
    public async Task<IEnumerable<SystemAlertDto>> GetAllAsync(bool? isDone, int pageNumber, int pageSize)
    {
        var alerts = await alertRepository.GetAllAsync(isDone, pageNumber, pageSize);
        return alerts.Select(a => a.ToDto());
    }

    public async Task<SystemAlertDto?> GetByIdAsync(int id)
    {
        var alert = await alertRepository.GetByIdAsync(id);
        if (alert is null)
            throw new ArgumentException($"Сповіщення з id {id} не знайдено.");
        return alert.ToDto();
    }

    public async Task<SystemAlertDto> CreateAsync(SystemAlertDto dto)
    {
        var entity = dto.ToEntity();
        var saved  = await alertRepository.AddAsync(entity);
        return saved.ToDto();
    }

    public async Task MarkDoneAsync(int id)
    {
        var alert = await alertRepository.GetByIdAsync(id);
        if (alert is null)
            throw new ArgumentException($"Сповіщення з id {id} не знайдено.");

        alert.IsDone = true;
        await alertRepository.UpdateAsync(alert);
    }

    public async Task DeleteAsync(int id)
    {
        await alertRepository.DeleteAsync(id);
    }

    public async Task<int> GetCountAsync(bool? isDone)
    {
        return await alertRepository.GetCountAsync(isDone);
    }
    
    public async Task GenerateAutomaticAlertsAsync()
    {
        await CheckVaccinationAlerts();
        await CheckFinancialStatus();
    }

    private async Task CheckVaccinationAlerts()
    {
        var today = DateTime.UtcNow.Date;
        var warningDate = today.AddDays(7); 

        var upcomingVaccinations = await vaccinationRepository.GetUpcomingVaccinationsAsync(warningDate);

        foreach (var v in upcomingVaccinations)
        {
            bool isExpired = v.NextDueDate < today;
            string severity = isExpired ? "danger" : "warning";
            string msg = isExpired 
                ? $"Термін вакцинації для {v.Animal.Name} МИНУВ ({v.NextDueDate:dd.MM.yyyy})!" 
                : $"Наближається вакцинація для {v.Animal.Name} ({v.NextDueDate:dd.MM.yyyy}).";

            bool alertExists = await alertRepository.CheckIfExistsAsync(msg, today);

            if (!alertExists)
            {
                await alertRepository.AddAsync(new SystemAlerts
                {
                    Message = msg,
                    Type = "medical",
                    Severity = severity,
                    IsAuto = true,
                    CreatedAt = DateTime.UtcNow,
                    IsDone = false
                });
            }
        }
    }

    private async Task CheckFinancialStatus()
    {
        decimal currentBalance = await transactionRepository.GetTotalBalanceAsync();
        decimal criticalThreshold = 5000; 

        if (currentBalance < criticalThreshold)
        {
            string msg = $"Критичний фінансовий стан! Баланс фонду впав до {currentBalance} ₴.";
            
            var today = DateTime.UtcNow.Date;
            bool alertExists = await alertRepository.CheckIfExistsAsync(msg, today);

            if (!alertExists)
            {
                await alertRepository.AddAsync(new SystemAlerts
                {
                    Message = msg,
                    Type = "system",
                    Severity = "danger",
                    IsAuto = true,
                    CreatedAt = DateTime.UtcNow,
                    IsDone = false
                });
            }
        }
    }
}