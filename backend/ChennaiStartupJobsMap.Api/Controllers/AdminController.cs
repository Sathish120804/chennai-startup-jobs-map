using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.Services;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ChennaiDbContext _db;
        private readonly IIngestionPipelineService _ingestionService;
        private readonly IDataQualityService _qualityService;
        private readonly ISourceRegistryService _sourceRegistry;
        private readonly ICompanyService _companyService;
        private readonly IJobService _jobService;

        public AdminController(
            ChennaiDbContext db,
            IIngestionPipelineService ingestionService,
            IDataQualityService qualityService,
            ISourceRegistryService sourceRegistry,
            ICompanyService companyService,
            IJobService jobService)
        {
            _db = db;
            _ingestionService = ingestionService;
            _qualityService = qualityService;
            _sourceRegistry = sourceRegistry;
            _companyService = companyService;
            _jobService = jobService;
        }

        [HttpGet("metrics")]
        public async Task<ActionResult> GetAdminMetrics()
        {
            var totalCompanies = await _db.Companies.CountAsync(c => c.IsActive);
            var totalJobs = await _db.Jobs.CountAsync(j => j.IsActive);
            var fresherJobs = await _db.Jobs.CountAsync(j => j.IsActive && j.IsFresher);
            var internships = await _db.Jobs.CountAsync(j => j.IsActive && j.IsInternship);
            var verifiedCompanies = await _db.Companies.CountAsync(c => c.VerificationStatus == "VERIFIED");
            var pendingSubmissions = await _db.Submissions.CountAsync(s => s.Status == "PENDING");
            var ingestionRunsCount = await _db.Set<IngestionRun>().CountAsync();

            return Ok(new
            {
                totalCompanies,
                totalJobs,
                fresherJobs,
                internships,
                verifiedCompanies,
                pendingSubmissions,
                ingestionRunsCount,
                environment = "DEVELOPMENT_ONLY",
                note = "All statistics are calculated live from EF Core Database."
            });
        }

        [HttpGet("sources")]
        public ActionResult GetSources()
        {
            return Ok(_sourceRegistry.GetRegisteredSources());
        }

        [HttpGet("ingestion/runs")]
        public async Task<ActionResult> GetIngestionRuns()
        {
            var runs = await _ingestionService.GetIngestionRunsAsync();
            return Ok(runs);
        }

        [HttpPost("ingestion/trigger")]
        public async Task<ActionResult> TriggerIngestion([FromQuery] string sourceId = "src-careers")
        {
            var run = await _ingestionService.RunMockDiscoveryIngestionAsync(sourceId);
            return Ok(run);
        }

        [HttpGet("quality/job/{id}")]
        public async Task<ActionResult> GetJobQualityScore(string id)
        {
            var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound(new { message = $"Job '{id}' not found." });

            var score = _qualityService.CalculateJobQualityScore(job);
            return Ok(score);
        }

        [HttpGet("quality/company/{id}")]
        public async Task<ActionResult> GetCompanyQualityScore(string id)
        {
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null) return NotFound(new { message = $"Company '{id}' not found." });

            var score = _qualityService.CalculateCompanyQualityScore(company);
            return Ok(score);
        }
    }
}
