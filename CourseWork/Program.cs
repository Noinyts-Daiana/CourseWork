using CourseWork;
using CourseWork.Repositories;
using CourseWork.Repositories.Interfaces; // Перевір, чи такий namespace у твоїх інтерфейсів
using CourseWork.Services;// Перевір, чи такий namespace
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. ПІДКЛЮЧЕННЯ КОНТРОЛЕРІВ ТА SWAGGER (для тестування)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Це додасть ту саму сторінку з кнопочками

// 2. БАЗА ДАНИХ
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. РЕЄСТРАЦІЯ РЕПОЗИТОРІЇВ (Вантажники)
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IBreedRepository, BreedRepository>();
builder.Services.AddScoped<ISpecieRepository, SpecieRepository>();
builder.Services.AddScoped<ICharacteristicRepository, CharacteristicRepository>();
builder.Services.AddScoped<IAdoptAnimalRepository, AdoptAnimalRepository>();
builder.Services.AddScoped<IAnimalCharacteristicRepository, AnimalCharacteristicRepository>();

// 4. РЕЄСТРАЦІЯ СЕРВІСІВ (Мізки проекту)
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IBreedService, BreedService>();
builder.Services.AddScoped<ISpecieService, SpecieService>();
builder.Services.AddScoped<ICharacteristicService, CharacteristicService>();
builder.Services.AddScoped<IAdoptAnimalService, AdoptAnimalService>();

// ТІЛЬКИ ПІСЛЯ ВСІХ РЕЄСТРАЦІЙ ВИКЛИКАЄМО BUILD
var app = builder.Build();

// 5. НАЛАШТУВАННЯ HTTP-КОНВЕЄРА
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Це дозволить відкривати swagger у браузері
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers(); 

app.MapGet("/", () => "shelter API is running!");

app.Run();