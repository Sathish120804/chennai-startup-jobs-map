using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using ChennaiStartupJobsMap.Api.Authentication;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Entities;
using ChennaiStartupJobsMap.Api.Services;

namespace ChennaiStartupJobsMap.Tests
{
    public class ApiTests
    {
        private ChennaiDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ChennaiDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{System.Guid.NewGuid()}")
                .Options;

            var db = new ChennaiDbContext(options);
            db.Database.EnsureCreated();
            return db;
        }

        private IConfiguration GetMockConfiguration()
        {
            var inMemorySettings = new System.Collections.Generic.Dictionary<string, string?>
            {
                { "Jwt:SecretKey", "ChennaiStartupJobsMapSuperSecretEnterpriseSigningKey2026" },
                { "Jwt:Issuer", "ChennaiStartupJobsMap" },
                { "Jwt:Audience", "ChennaiStartupJobsMapAudience" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public async Task AuthService_RegistersUserAndReturnsTokens()
        {
            using var db = GetInMemoryDbContext();
            var config = GetMockConfiguration();
            var jwt = new JwtTokenService(config);
            var auth = new AuthService(db, jwt);

            var registerDto = new RegisterRequestDto
            {
                Name = "Test Developer",
                Email = "dev@chennaistartup.test",
                Password = "Password@123",
                Role = UserRoles.User
            };

            var response = await auth.RegisterAsync(registerDto);

            Assert.True(response.Success);
            Assert.NotNull(response.Data);
            Assert.NotEmpty(response.Data.Token);
            Assert.NotEmpty(response.Data.RefreshToken);
            Assert.Equal("dev@chennaistartup.test", response.Data.User.Email);
        }

        [Fact]
        public async Task AuthService_AuthenticatesValidCredentials()
        {
            using var db = GetInMemoryDbContext();
            var config = GetMockConfiguration();
            var jwt = new JwtTokenService(config);
            var auth = new AuthService(db, jwt);

            // Register user
            await auth.RegisterAsync(new RegisterRequestDto
            {
                Name = "Login User",
                Email = "login@chennaistartup.test",
                Password = "SecretPassword123",
                Role = UserRoles.User
            });

            // Login
            var loginResponse = await auth.LoginAsync(new LoginRequestDto
            {
                Email = "login@chennaistartup.test",
                Password = "SecretPassword123"
            });

            Assert.True(loginResponse.Success);
            Assert.NotNull(loginResponse.Data);
            Assert.NotEmpty(loginResponse.Data.Token);
        }

        [Fact]
        public async Task AuthService_RejectsInvalidCredentials()
        {
            using var db = GetInMemoryDbContext();
            var config = GetMockConfiguration();
            var jwt = new JwtTokenService(config);
            var auth = new AuthService(db, jwt);

            var response = await auth.LoginAsync(new LoginRequestDto
            {
                Email = "nonexistent@test.com",
                Password = "WrongPassword"
            });

            Assert.False(response.Success);
            Assert.Null(response.Data);
        }

        [Fact]
        public async Task CompanyService_ReturnsSeededCompanies()
        {
            using var db = GetInMemoryDbContext();
            var companyService = new CompanyService(db);

            var result = await companyService.GetCompaniesAsync();

            Assert.NotNull(result);
            Assert.True(result.Total >= 2);
            Assert.Contains(result.Items, c => c.Slug == "zoho");
        }

        [Fact]
        public async Task JobService_FiltersFresherJobsCorrectly()
        {
            using var db = GetInMemoryDbContext();
            var jobService = new JobService(db);

            var result = await jobService.GetJobsAsync(isFresherOnly: true);

            Assert.NotNull(result);
            Assert.True(result.Total >= 1);
            Assert.All(result.Items, j => Assert.True(j.IsFresher));
        }

        [Fact]
        public async Task SearchService_ParsesDotnetFresherIntent()
        {
            using var db = GetInMemoryDbContext();
            var companyService = new CompanyService(db);
            var jobService = new JobService(db);
            var searchService = new SearchService(companyService, jobService);

            var response = await searchService.SearchAsync("dotnet fresher Chennai");

            Assert.NotNull(response);
            Assert.NotNull(response.Intent);
            Assert.Equal(".NET", response.Intent.Technology);
            Assert.True(response.Intent.IsFresher);
        }

        [Fact]
        public void NormalizationService_NormalizesTitlesAndExtractsTech()
        {
            var norm = new NormalizationService();
            var title = norm.NormalizeTitle("Associate Software Engineer (.NET & React)");
            var techs = norm.ExtractTechnologies("Looking for React, Python, and C# developer.");

            Assert.Equal("associate software engineer net react", title);
            Assert.Contains("React", techs);
            Assert.Contains("Python", techs);
            Assert.Contains("C#", techs);
        }

        [Fact]
        public async Task IngestionPipeline_RunsIdempotentDiscoveryWithoutDuplicates()
        {
            using var db = GetInMemoryDbContext();
            var norm = new NormalizationService();
            var matcher = new CompanyMatcher(db, norm);
            var pipeline = new IngestionPipelineService(db, norm, matcher);

            var run1 = await pipeline.RunMockDiscoveryIngestionAsync();
            var initialJobsCount = await db.Jobs.CountAsync();

            // Re-run same ingestion
            var run2 = await pipeline.RunMockDiscoveryIngestionAsync();
            var finalJobsCount = await db.Jobs.CountAsync();

            Assert.Equal("COMPLETED", run1.Status);
            Assert.Equal("COMPLETED", run2.Status);
            Assert.Equal(initialJobsCount, finalJobsCount); // Idempotency check: no duplicates created!
            Assert.True(run2.DuplicatesFound > 0);
        }

        [Fact]
        public void EmbeddingProvider_ComputesHigherSimilarityForRelatedConcepts()
        {
            var provider = new ChennaiStartupJobsMap.Api.Services.AI.DeterministicEmbeddingProvider();

            var vBackend = provider.GenerateEmbedding("backend developer .NET C# api");
            var vDotNet = provider.GenerateEmbedding("ASP.NET Core Software Engineer");
            var vFrontend = provider.GenerateEmbedding("React UI UX Designer Frontend");

            var simRelated = provider.CosineSimilarity(vBackend, vDotNet);
            var simUnrelated = provider.CosineSimilarity(vBackend, vFrontend);

            Assert.True(simRelated > simUnrelated, $"Expected related similarity ({simRelated}) > unrelated ({simUnrelated})");
        }

        [Fact]
        public async Task RecommendationService_ReturnsRankedMatchesWithExplanations()
        {
            using var db = GetInMemoryDbContext();
            var embedding = new ChennaiStartupJobsMap.Api.Services.AI.DeterministicEmbeddingProvider();
            var companyService = new CompanyService(db);
            var jobService = new JobService(db);
            var recService = new ChennaiStartupJobsMap.Api.Services.AI.JobRecommendationService(db, embedding, jobService, companyService);

            var recs = await recService.GetJobRecommendationsAsync(
                query: "React frontend fresher",
                technologies: new System.Collections.Generic.List<string> { "React" },
                isFresher: true,
                limit: 5);

            Assert.NotEmpty(recs);
            var topMatch = recs.First();
            Assert.True(topMatch.MatchScore >= 70);
            Assert.NotEmpty(topMatch.MatchReasons);
            Assert.Contains(topMatch.MatchReasons, r => r.Contains("React") || r.Contains("Fresher") || r.Contains("Entry-level"));
        }
    }
}
