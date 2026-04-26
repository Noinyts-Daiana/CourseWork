using Microsoft.AspNetCore.SignalR;

namespace CourseWork.Hubs;

// Це наша радіостанція. Angular буде підключатися до неї.
public class AlertHub : Hub
{
    // Ми можемо додавати сюди методи, якщо Angular захоче щось відправити на бекенд напряму,
    // але для нашої задачі (бекенд -> фронтенд) достатньо просто порожнього класу.
}