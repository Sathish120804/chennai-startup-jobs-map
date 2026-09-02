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
    /// Company and Startup Management in Chennai.
    /// Provides search, filtering across IT corridors (OMR, Guindy, Siruseri, etc.), and company profiles.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Tags("Companies and Startups")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyService _companyService;
        private readonly ChennaiDbContext _db;

        public CompaniesController(ICompanyService companyService, ChennaiDbContext db)
        {
            _companyService = companyService;
            _db = db;
        }

        /// <summary>
        /// Retrieve a paginated list of Chennai tech companies and startups matching criteria.
        /// </summary>
        /// <param name="q">Optional search query (e.g. "SaaS", "Zoho", "Fintech").</param>
        /// <param name="hubs">Comma-delimited corridor list (e.g. "OMR (IT Corridor),Guindy and Ekkatuthangal").</param>
        /// <param name="categories">Comma-delimited sector list (e.g. "SaaS / Enterprise Software").</param>
        /// <param name="types">Comma-delimited company types (e.g. "Startup,Unicorn").</param>
        /// <param name="hiring">Filter only currently hiring companies.</param>
        /// <param name="fresher">Filter companies offering fresher roles.</param>
        /// <param name="tech">Comma-delimited tech stack keywords (e.g. "React,.NET,Python").</param>
        /// <param name="page">Page number (default: 1).</param>
        /// <param name="pageSize">Items per page (default: 20, max: 100).</param>
        /// <param name="sortBy">Sort field: "featured", "jobsCount", "foundedYear", "name".</param>
        /// <response code="200">Paginated company results with total count and metadata.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(PagedResponse<CompanyDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<CompanyDto>>> GetCompanies(
            [FromQuery] string? q,
            [FromQuery] string? hubs,
            [FromQuery] string? categories,
            [FromQuery] string? types,
            [FromQuery] bool? hiring,
            [FromQuery] bool? fresher,
            [FromQuery] string? tech,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            [FromQuery] string sortBy = "featured")
        {
            var hubList = !string.IsNullOrWhiteSpace(hubs) ? hubs.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null;
            var catList = !string.IsNullOrWhiteSpace(categories) ? categories.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null;
            var typeList = !string.IsNullOrWhiteSpace(types) ? types.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null;
            var techList = !string.IsNullOrWhiteSpace(tech) ? tech.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null;

            var paged = await _companyService.GetCompaniesAsync(q, hubList, catList, typeList, hiring, fresher, techList, sortBy, page, pageSize);
            return Ok(PagedResponse<CompanyDto>.Create(paged.Items, paged.Total, paged.Page, paged.PageSize));
        }

        /// <summary>
        /// Get company details by unique ID.
        /// </summary>
        /// <param name="id">Company unique identifier (e.g. "comp-1").</param>
        /// <response code="200">Company entity details.</response>
        /// <response code="404">Company with specified ID does not exist.</response>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CompanyDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CompanyDto>>> GetCompanyById(string id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null)
            {
                return NotFound(ApiResponse<CompanyDto>.Fail($"Company with ID '{id}' was not found."));
            }
            return Ok(ApiResponse<CompanyDto>.Ok(company));
        }

        /// <summary>
        /// Get company profile by URL slug.
        /// </summary>
        /// <param name="slug">Company URL slug (e.g. "zoho-corporation").</param>
        /// <response code="200">Company details.</response>
        /// <response code="404">Company with specified slug not found.</response>
        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<CompanyDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<CompanyDto>), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<CompanyDto>>> GetCompanyBySlug(string slug)
        {
            var company = await _companyService.GetCompanyBySlugAsync(slug);
            if (company == null)
            {
                return NotFound(ApiResponse<CompanyDto>.Fail($"Company with slug '{slug}' was not found."));
            }
            return Ok(ApiResponse<CompanyDto>.Ok(company));
        }

        /// <summary>
        /// Get semantically similar Chennai companies based on domain, tech stack, and corridor.
        /// </summary>
        /// <param name="id">Company ID.</param>
        /// <param name="recommendationService">Injected recommendation service.</param>
        /// <response code="200">List of similar company recommendations.</response>
        [HttpGet("{id}/similar")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<ChennaiStartupJobsMap.Api.Services.AI.CompanyRecommendationDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<ChennaiStartupJobsMap.Api.Services.AI.CompanyRecommendationDto>>>> GetSimilarCompanies(
            string id,
            [FromServices] ChennaiStartupJobsMap.Api.Services.AI.IJobRecommendationService recommendationService)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null) return NotFound(ApiResponse<List<ChennaiStartupJobsMap.Api.Services.AI.CompanyRecommendationDto>>.Fail("Company not found."));

            var category = company.Categories.Count > 0 ? company.Categories[0] : null;
            var similar = await recommendationService.GetCompanyRecommendationsAsync(company.Name, category, company.Hub, limit: 4);
            var filtered = similar.Where(s => s.Company.Id != id).ToList();

            return Ok(ApiResponse<List<ChennaiStartupJobsMap.Api.Services.AI.CompanyRecommendationDto>>.Ok(filtered));
        }

        /// <summary>
        /// Create a new company record (Moderator or Admin only).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Moderator}")]
        [ProducesResponseType(typeof(ApiResponse<CompanyDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<ApiResponse<CompanyDto>>> CreateCompany([FromBody] SubmitCompanyRequest request)
        {
            var newCompany = new Company
            {
                Id = $"comp-{Guid.NewGuid():N}",
                Name = request.Name,
                NormalizedName = request.Name.ToLower().Trim(),
                Slug = request.Name.ToLower().Replace(" ", "-").Replace("/", "-"),
                Website = request.Website,
                CareersUrl = request.CareersUrl ?? request.Website,
                Hub = request.Hub,
                Address = request.Address,
                Description = request.Description,
                Tagline = $"{request.Name} - Chennai Tech Organization",
                Logo = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80",
                CompanyTypes = new List<string> { "Startup" },
                Categories = new List<string> { "Enterprise Tech" },
                Tags = new List<string> { "Chennai Tech" },
                TechStack = new List<string> { "Web", "Cloud" },
                FoundedYear = DateTime.UtcNow.Year,
                EmployeeCount = "11-50",
                HiringStatus = "HIRING",
                Latitude = 12.9716,
                Longitude = 80.2435,
                MapPrecision = "EXACT_OFFICE",
                VerificationStatus = "VERIFIED",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsFeatured = false,
                IsActive = true,
                IsSeedData = false
            };

            _db.Companies.Add(newCompany);
            await _db.SaveChangesAsync();

            var dto = await _companyService.GetCompanyByIdAsync(newCompany.Id);
            return CreatedAtAction(nameof(GetCompanyById), new { id = newCompany.Id }, ApiResponse<CompanyDto>.Ok(dto!, "Company created successfully."));
        }
    }
}
