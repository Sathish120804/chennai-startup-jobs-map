using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.Entities;
using ChennaiStartupJobsMap.Api.Models;
using ChennaiStartupJobsMap.Api.Services;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// Admin Dashboard and Moderation Controls.
    /// Provides metrics, ingestion management, data quality diagnostics, and submission moderation.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Tags("Admin and Moderation")]
    public class AdminController : ControllerBase
    {
        private readonly ChennaiDbContext _db;
        private readonly IIngestionPipelineService _ingestionService;
        private readonly IDataQualityService _qualityService;
        private readonly ISourceRegistryService _sourceRegistry;

        public AdminController(
            ChennaiDbContext db,
            IIngestionPipelineService ingestionService,
            IDataQualityService qualityService,
            ISourceRegistryService sourceRegistry)
        {
            _db = db;
            _ingestionService = ingestionService;
            _qualityService = qualityService;
            _sourceRegistry = sourceRegistry;
        }

        /// <summary>
        /// Get ecosystem metrics and database counts.
        /// </summary>
        [HttpGet("metrics")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<object>>> GetAdminMetrics()
        {
            var totalCompanies = await _db.Companies.CountAsync(c => c.IsActive);
            var totalJobs = await _db.Jobs.CountAsync(j => j.IsActive);
            var fresherJobs = await _db.Jobs.CountAsync(j => j.IsActive && j.IsFresher);
            var internships = await _db.Jobs.CountAsync(j => j.IsActive && j.IsInternship);
            var verifiedCompanies = await _db.Companies.CountAsync(c => c.VerificationStatus == "VERIFIED");
            var pendingSubmissions = await _db.Submissions.CountAsync(s => s.Status == "PENDING");
            var ingestionRunsCount = await _db.IngestionRuns.CountAsync();

            var metrics = new
            {
                totalCompanies,
                totalJobs,
                fresherJobs,
                internships,
                verifiedCompanies,
                pendingSubmissions,
                ingestionRunsCount,
                environment = "ENTERPRISE_API"
            };

            return Ok(ApiResponse<object>.Ok(metrics));
        }

        /// <summary>
        /// Get registered data discovery sources and trust levels.
        /// </summary>
        [HttpGet("sources")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public ActionResult<ApiResponse<object>> GetSources()
        {
            return Ok(ApiResponse<object>.Ok(_sourceRegistry.GetRegisteredSources()));
        }

        /// <summary>
        /// Retrieve ingestion execution history runs.
        /// </summary>
        [HttpGet("ingestion/runs")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<object>>> GetIngestionRuns()
        {
            var runs = await _ingestionService.GetIngestionRunsAsync();
            return Ok(ApiResponse<object>.Ok(runs));
        }

        /// <summary>
        /// Trigger an automated data discovery ingestion run.
        /// </summary>
        [HttpPost("ingestion/trigger")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Moderator}")]
        [ProducesResponseType(typeof(ApiResponse<IngestionRun>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<IngestionRun>>> TriggerIngestion([FromQuery] string sourceId = "src-careers")
        {
            var run = await _ingestionService.RunMockDiscoveryIngestionAsync(sourceId);
            return Ok(ApiResponse<IngestionRun>.Ok(run, "Discovery run completed."));
        }

        /// <summary>
        /// Calculate data quality score for a company entity.
        /// </summary>
        [HttpGet("quality/company/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<DataQualityScore>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<DataQualityScore>>> GetCompanyQualityScore(string id)
        {
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == id);
            if (company == null) return NotFound(ApiResponse<DataQualityScore>.Fail("Company not found."));

            var score = _qualityService.CalculateCompanyQualityScore(company);
            return Ok(ApiResponse<DataQualityScore>.Ok(score));
        }

        /// <summary>
        /// Calculate data quality score for a job posting.
        /// </summary>
        [HttpGet("quality/job/{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<DataQualityScore>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<DataQualityScore>>> GetJobQualityScore(string id)
        {
            var job = await _db.Jobs.FirstOrDefaultAsync(j => j.Id == id);
            if (job == null) return NotFound(ApiResponse<DataQualityScore>.Fail("Job not found."));

            var score = _qualityService.CalculateJobQualityScore(job);
            return Ok(ApiResponse<DataQualityScore>.Ok(score));
        }
    }
}
