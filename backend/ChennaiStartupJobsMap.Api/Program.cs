using System;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Hangfire;
using Hangfire.MemoryStorage;
using ChennaiStartupJobsMap.Api.Authentication;
using ChennaiStartupJobsMap.Api.BackgroundJobs;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.Entities;
using ChennaiStartupJobsMap.Api.Middleware;
using ChennaiStartupJobsMap.Api.Repositories;
using ChennaiStartupJobsMap.Api.Services;
using ChennaiStartupJobsMap.Api.Services.AI;

var builder = WebApplication.CreateBuilder(args);

// 1. Controller & Routing configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 2. EF Core In-Memory Database (PostgreSQL-ready abstraction)
builder.Services.AddDbContext<ChennaiDbContext>(options =>
    options.UseInMemoryDatabase("ChennaiStartupJobsMapDb"));

// 3. JWT Authentication & Token Validation
var jwtSecret = builder.Configuration["Jwt:SecretKey"] ?? "ChennaiStartupJobsMapSuperSecretEnterpriseSigningKey2026";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "ChennaiStartupJobsMap";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "ChennaiStartupJobsMapAudience";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ClockSkew = TimeSpan.Zero
    };
});

// 4. Role-based Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole(UserRoles.Admin));
    options.AddPolicy("ModeratorOnly", policy => policy.RequireRole(UserRoles.Admin, UserRoles.Moderator));
    options.AddPolicy("RecruiterAccess", policy => policy.RequireRole(UserRoles.Admin, UserRoles.Recruiter));
});

// 5. Hangfire Background Job Processing
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseMemoryStorage());
builder.Services.AddHangfireServer();

// 6. Application Services & Repositories Registration
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddSingleton<ISourceRegistryService, SourceRegistryService>();
builder.Services.AddSingleton<INormalizationService, NormalizationService>();
builder.Services.AddScoped<ICompanyMatcher, CompanyMatcher>();
builder.Services.AddSingleton<IDataQualityService, DataQualityService>();
builder.Services.AddScoped<IIngestionPipelineService, IngestionPipelineService>();
builder.Services.AddScoped<IBackgroundJobManager, BackgroundJobManager>();
builder.Services.AddSingleton<IEmbeddingProvider, DeterministicEmbeddingProvider>();
builder.Services.AddScoped<IJobRecommendationService, JobRecommendationService>();

// 7. Swagger / OpenAPI Configuration with JWT Bearer Security & XML Comments
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Chennai Startup & Jobs Map - Enterprise API Portal",
        Version = "v1",
        Description = "Production-grade RESTful API platform for discovering Chennai startups, tech companies, job vacancies, and internships with automated data ingestion, deduplication, and quality scoring.",
        Contact = new OpenApiContact
        {
            Name = "Chennai Startup & Jobs Map Engineering",
            Email = "support@chennaistartups.in",
            Url = new Uri("https://github.com/Sathish120804/chennai-startup-jobs-map")
        },
        License = new OpenApiLicense
        {
            Name = "MIT License"
        }
    });

    // JWT Bearer Security Definition
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT Bearer token. Example: 'Bearer eyJhbGciOiJIUzI1NiIsIn...' ",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            Array.Empty<string>()
        }
    });

    // Enable XML Documentation in Swagger
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// 8. CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://localhost:3000", "http://localhost:4173")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Ensure Database Seeded
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ChennaiDbContext>();
    db.Database.EnsureCreated();
}

// Global Exception Handling Middleware
app.UseMiddleware<ExceptionMiddleware>();

// Enable Swagger UI at /swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Chennai Startup & Jobs Map API v1");
    c.RoutePrefix = "swagger";
    c.DocumentTitle = "Chennai Startup & Jobs Map - Enterprise API Portal";
});

// Hangfire Dashboard at /hangfire
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    DashboardTitle = "Chennai Jobs Map - Hangfire Job Scheduler"
});

// Register Recurring Background Jobs
RecurringJob.AddOrUpdate<IBackgroundJobManager>("job-discovery-job", x => x.ExecuteJobDiscoveryAsync(), Cron.Hourly);
RecurringJob.AddOrUpdate<IBackgroundJobManager>("verification-job", x => x.ExecuteVerificationAsync(), Cron.Daily);
RecurringJob.AddOrUpdate<IBackgroundJobManager>("expire-stale-jobs-job", x => x.ExecuteExpireStaleJobsAsync(), Cron.Daily);

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
