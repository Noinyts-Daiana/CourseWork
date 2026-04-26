using CourseWork.DTOs;
using CourseWork.Models;

namespace CourseWork.Mappers;

public static class SystemAlertMappers
{
    public static SystemAlertDto ToDto(this SystemAlerts alert)
    {
        return new SystemAlertDto
        {
            Id      = alert.Id,
            Type    = alert.Type,
            Message = alert.Message,
            IsDone  = alert.IsDone
        };
    }

    public static SystemAlerts ToEntity(this SystemAlertDto dto)
    {
        return new SystemAlerts
        {
            Type    = dto.Type,
            Message = dto.Message,
            IsDone  = false
        };
    }
}