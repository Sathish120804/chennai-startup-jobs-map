using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Entities;
using ChennaiStartupJobsMap.Api.Models;
using ChennaiStartupJobsMap.Api.Services;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// Recruiter and Company Representative Portal.
    /// Enables verified talent acquisition teams to claim companies and submit vacancy postings for moderation.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Recruiter},{UserRoles.Moderator}")]
    [Produces("application/json")]
    [Tags("Recruiter & Company Portal")]
    public class RecruitersController : ControllerBase
    {
        private readonly IRecruiterService _recruiterService;

        public RecruitersController(IRecruiterService recruiterService)
        {
            _recruiterService = recruiterService;
        }

        private string GetCurrentUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "recruiter-demo";

        /// <summary>
        /// Submit a formal claim request to manage a Chennai company profile.
        /// </summary>
        [HttpPost("claim-company")]
        [ProducesResponseType(typeof(ApiResponse<CompanyClaim>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<CompanyClaim>>> ClaimCompany([FromBody] ClaimCompanyRequest request)
        {
            var claim = await _recruiterService.ClaimCompanyAsync(
                GetCurrentUserId(),
                request.CompanyId,
                request.CorporateEmail,
                request.ProofNotes);

            return Ok(ApiResponse<CompanyClaim>.Ok(claim, "Company claim request submitted for admin review."));
        }

        /// <summary>
        /// Retrieve all company claim requests submitted by the current recruiter.
        /// </summary>
        [HttpGet("my-claims")]
        [ProducesResponseType(typeof(ApiResponse<List<CompanyClaim>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<CompanyClaim>>>> GetMyClaims()
        {
            var claims = await _recruiterService.GetMyClaimsAsync(GetCurrentUserId());
            return Ok(ApiResponse<List<CompanyClaim>>.Ok(claims));
        }

        /// <summary>
        /// Submit a direct job vacancy for moderation. Starts in PENDING_REVIEW status.
        /// </summary>
        [HttpPost("jobs")]
        [ProducesResponseType(typeof(ApiResponse<Job>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<Job>>> PostJob([FromBody] SubmitJobRequest request)
        {
            var job = await _recruiterService.SubmitRecruiterJobAsync(GetCurrentUserId(), request);
            return Ok(ApiResponse<Job>.Ok(job, "Job vacancy submitted. It will appear live once approved by moderators."));
        }

        /// <summary>
        /// Retrieve jobs submitted by recruiter.
        /// </summary>
        [HttpGet("my-jobs")]
        [ProducesResponseType(typeof(ApiResponse<List<JobDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<JobDto>>>> GetMyJobs()
        {
            var jobs = await _recruiterService.GetRecruiterJobsAsync(GetCurrentUserId());
            return Ok(ApiResponse<List<JobDto>>.Ok(jobs));
        }
    }

    public class ClaimCompanyRequest
    {
        public string CompanyId { get; set; } = string.Empty;
        public string CorporateEmail { get; set; } = string.Empty;
        public string ProofNotes { get; set; } = string.Empty;
    }
}
