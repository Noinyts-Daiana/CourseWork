namespace CourseWork.DTOs;

public record DashboardStatsDto(
    PipelineStatsDto Pipeline,
    IEnumerable<InventoryForecastDto> Inventory,
    OccupancyStatsDto Occupancy,
    FinancialDto Finances,
    IEnumerable<MonthlyAdoptionDto> Analytics
);

public record PipelineStatsDto(int Quarantine, int Treatment, int Ready, int Total);
public record InventoryForecastDto(string Name, int DaysLeft, int Percent, string Status);
public record OccupancyStatsDto(int Dogs, int MaxDogs, int Cats, int MaxCats, int Quarantine, int MaxQuarantine);
public record FinancialDto(decimal Balance, int DaysRemaining);
public record MonthlyAdoptionDto(string Month, int Arrived, int Adopted);