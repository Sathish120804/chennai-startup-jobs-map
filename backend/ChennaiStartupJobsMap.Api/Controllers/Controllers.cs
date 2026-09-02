using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Models;
using ChennaiStartupJobsMap.Api.Services;
using ChennaiStartupJobsMap.Api.Data;

namespace ChennaiStartupJobsMap.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;

        public CompaniesController(ICompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<CompanyDto>>> GetCompanies(
            [FromQuery] string? q = null,
            [FromQuery] string? hubs = null,
            [FromQuery] string? categories = null,
            [FromQuery] string? types = null,
            [FromQuery] bool? hiring = null,
            [FromQuery] bool? fresher = null,
            [FromQuery] string? tech = null,
            [FromQuery] string sortBy = "featured",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var hubList = !string.IsNullOrWhiteSpace(hubs) ? new List<string>(hubs.Split(',')) : null;
            var catList = !string.IsNullOrWhiteSpace(categories) ? new List<string>(categories.Split(',')) : null;
            var typeList = !string.IsNullOrWhiteSpace(types) ? new List<string>(types.Split(',')) : null;
            var techList = !string.IsNullOrWhiteSpace(tech) ? new List<string>(tech.Split(',')) : null;

            var result = await _companyService.GetCompaniesAsync(
                searchQuery: q,
                hubs: hubList,
                categories: catList,
                types: typeList,
                isHiringOnly: hiring,
                isFresherOnly: fresher,
                technologies: techList,
                sortBy: sortBy,
                page: page,
                pageSize: Math.Clamp(pageSize, 1, 100)
            );

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyById(string id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null) return NotFound(new { message = $"Company with ID '{id}' not found." });
            return Ok(company);
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<CompanyDto>> GetCompanyBySlug(string slug)
        {
            var company = await _companyService.GetCompanyBySlugAsync(slug);
            if (company == null) return NotFound(new { message = $"Company with slug '{slug}' not found." });
            return Ok(company);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class JobsController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobsController(IJobService jobService)
        {
            _jobService = jobService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResultDto<JobDto>>> GetJobs(
            [FromQuery] string? q = null,
            [FromQuery] string? hubs = null,
            [FromQuery] string? categories = null,
            [FromQuery] bool? fresher = null,
            [FromQuery] bool? internship = null,
            [FromQuery] bool? engineering = null,
            [FromQuery] string? tech = null,
            [FromQuery] string sortBy = "recent",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var hubList = !string.IsNullOrWhiteSpace(hubs) ? new List<string>(hubs.Split(',')) : null;
            var catList = !string.IsNullOrWhiteSpace(categories) ? new List<string>(categories.Split(',')) : null;
            var techList = !string.IsNullOrWhiteSpace(tech) ? new List<string>(tech.Split(',')) : null;

            var result = await _jobService.GetJobsAsync(
                searchQuery: q,
                hubs: hubList,
                categories: catList,
                isFresherOnly: fresher,
                isInternshipOnly: internship,
                isEngineeringOnly: engineering,
                technologies: techList,
                sortBy: sortBy,
                page: page,
                pageSize: Math.Clamp(pageSize, 1, 100)
            );

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JobDto>> GetJobById(string id)
        {
            var job = await _jobService.GetJobByIdAsync(id);
            if (job == null) return NotFound(new { message = $"Job with ID '{id}' not found." });
            return Ok(job);
        }

        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<JobDto>> GetJobBySlug(string slug)
        {
            var job = await _jobService.GetJobBySlugAsync(slug);
            if (job == null) return NotFound(new { message = $"Job with slug '{slug}' not found." });
            return Ok(job);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpGet]
        public async Task<ActionResult<SearchResponseDto>> Search(
            [FromQuery] string q,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return BadRequest(new { message = "Search query 'q' parameter is required." });
            }

            var result = await _searchService.SearchAsync(q, page, Math.Clamp(pageSize, 1, 100));
            return Ok(result);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SubmissionsController : ControllerBase
    {
        private readonly ChennaiDbContext _db;

        public SubmissionsController(ChennaiDbContext db)
        {
            _db = db;
        }

        [HttpPost("company")]
        public async Task<ActionResult> SubmitCompany([FromBody] SubmitCompanyRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name) || string.IsNullOrWhiteSpace(req.Website))
            {
                return BadRequest(new { message = "Company name and website are required." });
            }

            var submission = new UserSubmission
            {
                Id = $"sub-comp-{DateTime.UtcNow.Ticks}",
                Type = "company",
                SubmittedBy = req.SubmittedBy,
                Email = req.Email,
                TitleOrName = req.Name,
                Url = req.Website,
                Hub = req.Hub,
                Notes = req.Description,
                SubmittedAt = DateTime.UtcNow,
                Status = "PENDING"
            };

            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Company submission received for verification.", submissionId = submission.Id });
        }

        [HttpPost("job")]
        public async Task<ActionResult> SubmitJob([FromBody] SubmitJobRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.CompanyName) || string.IsNullOrWhiteSpace(req.Title) || string.IsNullOrWhiteSpace(req.OriginalUrl))
            {
                return BadRequest(new { message = "Company name, job title, and application URL are required." });
            }

            var submission = new UserSubmission
            {
                Id = $"sub-job-{DateTime.UtcNow.Ticks}",
                Type = "job",
                SubmittedBy = req.SubmittedBy,
                Email = req.Email,
                TitleOrName = $"{req.Title} @ {req.CompanyName}",
                Url = req.OriginalUrl,
                Notes = req.DescriptionSnippet,
                SubmittedAt = DateTime.UtcNow,
                Status = "PENDING"
            };

            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Job submission received for verification.", submissionId = submission.Id });
        }
    }
}
