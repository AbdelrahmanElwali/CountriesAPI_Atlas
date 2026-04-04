using CountriesAPI.Application.Interfaces;
using CountriesAPI.Application.Services;
using CountriesAPI.Infrastructure.Data;
using CountriesAPI.Infrastructure.ExternalApi;
using CountriesAPI.Infrastructure.FileStorage;
using CountriesAPI.Infrastructure.Repositories;
using CountriesAPI.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<RestCountriesClient>();

// Services
builder.Services.AddScoped<ISyncService, SyncService>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<CountryService>();

builder.Services.AddSingleton<ITranslationStore>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var store = new TranslationStore(config);
    return store;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseCors("AllowAngular");

// force الـ TranslationStore يتبني فوراً
var translationStore = app.Services.GetRequiredService<ITranslationStore>();

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.MapRazorPages();

app.Run();