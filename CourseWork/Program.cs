using System.Text;
using CourseWork;
using CourseWork.Repositories;
using CourseWork.Repositories.Interfaces;
using CourseWork.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;



var builder = WebApplication.CreateBuilder(args);



var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddCors(options =>
{
    options.AddPolicy("ShelterSysPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// 1. ПІДКЛЮЧЕННЯ КОНТРОЛЕРІВ
builder.Services.AddControllers();

// 2. SWAGGER (Тільки Swashbuckle!)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

// 3. БАЗА ДАНИХ
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// 4. РЕЄСТРАЦІЯ РЕПОЗИТОРІЇВ
builder.Services.AddScoped<IAnimalRepository, AnimalRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBreedRepository, BreedRepository>();
builder.Services.AddScoped<ISpecieRepository, SpecieRepository>();
builder.Services.AddScoped<ICharacteristicRepository, CharacteristicRepository>();
builder.Services.AddScoped<IAdoptAnimalRepository, AdoptAnimalRepository>();
builder.Services.AddScoped<IAnimalCharacteristicRepository, AnimalCharacteristicRepository>();

// 5. РЕЄСТРАЦІЯ СЕРВІСІВ
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IBreedService, BreedService>();
builder.Services.AddScoped<ISpecieService, SpecieService>();
builder.Services.AddScoped<ICharacteristicService, CharacteristicService>();
builder.Services.AddScoped<IAdoptAnimalService, AdoptAnimalService>();
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Shelter API V1");
    });
}

app.UseHttpsRedirection();

app.UseCors("ShelterSysPolicy"); 

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.MapGet("/", () => "Shelter API is running!");

app.Run();