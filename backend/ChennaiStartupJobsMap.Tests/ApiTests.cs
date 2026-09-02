using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ChennaiStartupJobsMap.Api.Data;
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
    }
}
