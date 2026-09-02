using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.Services;
using ChennaiStartupJobsMap.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to container
builder.Services.AddControllers();

// Configure EF Core DbContext
builder.Services.AddDbContext<ChennaiDbContext>(options =>
    options.UseInMemoryDatabase("ChennaiStartupJobsMapDb"));

// Register Application Services
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddSingleton<ISourceRegistryService, SourceRegistryService>();
builder.Services.AddSingleton<INormalizationService, NormalizationService>();
builder.Services.AddScoped<ICompanyMatcher, CompanyMatcher>();
builder.Services.AddSingleton<IDataQualityService, DataQualityService>();
builder.Services.AddScoped<IIngestionPipelineService, IngestionPipelineService>();

// CORS Policy Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:4173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ensure DbContext is created and seeded for InMemory mode
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChennaiDbContext>();
    db.Database.EnsureCreated();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("CorsPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();
