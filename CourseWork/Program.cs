using CourseWork;
using CourseWork.Repositories;
using CourseWork.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Це "мозок" для твоїх контролерів
builder.Services.AddControllers();

// Підключення до бази
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

builder.Services.AddScoped<IBreedRepository, BreedRepository>();
builder.Services.AddScoped<IBreedService, BreedService>();

// Це "дорожня карта" для запитів
app.MapControllers(); 

// Твій "Hello World" для швидкої перевірки, чи живий сервер
app.MapGet("/", () => "Hello World!");

app.Run();