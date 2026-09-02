using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Entities;
using ChennaiStartupJobsMap.Api.Models;
using ChennaiStartupJobsMap.Api.Services;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// Tech Job and Internship Vacancies across Chennai.
    /// Provides search, corridor filtering, fresher classification, and application links.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Tags("Jobs and Internships")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;
        private readonly ChennaiDbContext _db;

        public JobsController(IJobService jobService, ChennaiDbContext db)
        {
            _jobService = jobService;
            _db = db;
        }

        /// <summary>
        /// Retrieve a paginated list of Chennai jobs with advanced filters.
        /// </summary>
        /// <param name="q">Search query (e.g. "React", "Frontend", ".NET").</param>
        /// <param name="hubs">Comma-delimited Chennai tech hubs.</param>
        /// <param name="categories">Comma-delimited sector domains.</param>
        /// <param name="fresher">Filter only fresher / entry-level roles (0-1 yrs).</param>
        /// <param name="internship">Filter internship vacancies.</param>
        /// <param name="engineering">Filter engineering and tech software roles.</param>
        /// <param name="tech">Filter by specific technologies (e.g. "React,.NET").</param>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Page size (default: 20).</param>
        /// <response code="200">Paginated job vacancies.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResponse<JobDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<JobDto>>> GetJobs(
            [FromQuery] string? q,
            [FromQuery] string? hubs,
            [FromQuery] string? categories,
            [FromQuery] bool? fresher,
            [FromQuery] bool? internship,
            [FromQuery] bool? engineering,
            [FromQuery] string? tech,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var hubList = !string.IsNullOrWhiteSpace(hubs) ? hubs.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null;
            var catList = !string.IsNullOrWhiteSpace(categories) ? categories.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null;
            var techList = !string.IsNullOrWhiteSpace(tech) ? tech.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null;

            var paged = await _jobService.GetJobsAsync(q, hubList, catList, fresher, internship, engineering, techList, "recent", page, pageSize);
            return Ok(PagedResponse<JobDto>.Create(paged.Items, paged.Total, paged.Page, paged.PageSize));
        }

        /// <summary>
        /// Get job details by unique ID.
        /// </summary>
        /// <param name="id">Job identifier (e.g. "job-1").</param>
        /// <response code="200">Job vacancy details.</response>
        /// <response code="404">Job not found.</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<JobDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<JobDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<JobDto>>> GetJobById(string id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null)
            {
                return NotFound(ApiResponse<JobDto>.Fail($"Job with ID '{id}' was not found."));
            }
            return Ok(ApiResponse<JobDto>.Ok(job));
        }

        /// <summary>
        /// Create a new job vacancy (Recruiter or Admin only).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Recruiter},{UserRoles.Moderator}")]
        [ProducesResponseType(typeof(ApiResponse<JobDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<JobDto>>> CreateJob([FromBody] SubmitJobRequest request)
        {
            var job = new Job
            {
                Id = $"job-{Guid.NewGuid():N}",
                CompanyId = "comp-1",
                CompanyName = request.CompanyName,
                CompanyLogo = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80",
                CompanyHub = "OMR (IT Corridor)",
                Title = request.Title,
                NormalizedTitle = request.Title.ToLower().Trim(),
                Slug = $"{request.CompanyName}-{request.Title}".ToLower().Replace(" ", "-").Replace("/", "-"),
                DescriptionSnippet = request.DescriptionSnippet,
                PrimaryCategory = "Engineering",
                IsEngineering = true,
                EngineeringSubcategory = "Full Stack",
                Technologies = new List<string> { "React", "Node.js" },
                JobType = "Full-time",
                WorkplaceType = "On-site",
                ExperienceLevel = "Fresher / Entry (0-1 yrs)",
                IsFresher = true,
                FresherConfidence = 85,
                IsInternship = false,
                SalaryRange = request.SalaryRange ?? "Competitive Market Standard",
                Location = request.Location,
                ChennaiRelevance = "CHENNAI_CONFIRMED",
                RelevanceConfidence = 95,
                SourceName = "Direct Submission",
                OriginalUrl = request.OriginalUrl,
                ApplyUrl = request.OriginalUrl,
                FirstSeenAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow,
                LastVerifiedAt = DateTime.UtcNow,
                FreshnessStatus = "NEW",
                VerificationStatus = "VERIFIED",
                IsFeatured = false,
                IsActive = true,
                IsSeedData = false
            };

            _db.Jobs.Add(job);
            await _db.SaveChangesAsync();

            var dto = await _jobService.GetJobByIdAsync(job.Id);
            return CreatedAtAction(nameof(GetJobById), new { id = job.Id }, ApiResponse<JobDto>.Ok(dto!, "Job vacancy created successfully."));
        }
    }
}
