using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Models;
using ChennaiStartupJobsMap.Api.Services;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// Authenticated User Platform and Preferences.
    /// Manages saved jobs, bookmarked companies, and personalized job alerts.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]/me")]
    [Authorize]
    [Produces("application/json")]
    [Tags("User Platform & Preferences")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        private string GetCurrentUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "user-demo";

        /// <summary>
        /// Retrieve all jobs saved/bookmarked by the current authenticated user.
        /// </summary>
        [HttpGet("saved-jobs")]
        [ProducesResponseType(typeof(ApiResponse<List<JobDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<JobDto>>>> GetSavedJobs()
        {
            var jobs = await _userService.GetSavedJobsAsync(GetCurrentUserId());
            return Ok(ApiResponse<List<JobDto>>.Ok(jobs));
        }

        /// <summary>
        /// Save/bookmark a job opportunity for later review.
        /// </summary>
        [HttpPost("saved-jobs/{jobId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse<bool>>> SaveJob(string jobId)
        {
            var success = await _userService.SaveJobAsync(GetCurrentUserId(), jobId);
            if (!success) return NotFound(ApiResponse<bool>.Fail("Job not found."));
            return Ok(ApiResponse<bool>.Ok(true, "Job saved successfully."));
        }

        /// <summary>
        /// Remove a job from the user's saved jobs list.
        /// </summary>
        [HttpDelete("saved-jobs/{jobId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> UnsaveJob(string jobId)
        {
            var success = await _userService.UnsaveJobAsync(GetCurrentUserId(), jobId);
            return Ok(ApiResponse<bool>.Ok(success, success ? "Job removed from bookmarks." : "Job was not saved."));
        }

        /// <summary>
        /// Retrieve all tech companies bookmarked by the current user.
        /// </summary>
        [HttpGet("saved-companies")]
        [ProducesResponseType(typeof(ApiResponse<List<CompanyDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<CompanyDto>>>> GetSavedCompanies()
        {
            var comps = await _userService.GetSavedCompaniesAsync(GetCurrentUserId());
            return Ok(ApiResponse<List<CompanyDto>>.Ok(comps));
        }

        /// <summary>
        /// Bookmark a company in the directory.
        /// </summary>
        [HttpPost("saved-companies/{companyId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> SaveCompany(string companyId)
        {
            var success = await _userService.SaveCompanyAsync(GetCurrentUserId(), companyId);
            if (!success) return NotFound(ApiResponse<bool>.Fail("Company not found."));
            return Ok(ApiResponse<bool>.Ok(true, "Company saved successfully."));
        }

        /// <summary>
        /// Remove a company from the user's saved list.
        /// </summary>
        [HttpDelete("saved-companies/{companyId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> UnsaveCompany(string companyId)
        {
            var success = await _userService.UnsaveCompanyAsync(GetCurrentUserId(), companyId);
            return Ok(ApiResponse<bool>.Ok(success, success ? "Company removed from bookmarks." : "Company was not saved."));
        }

        /// <summary>
        /// Retrieve configured job search alerts for the current user.
        /// </summary>
        [HttpGet("job-alerts")]
        [ProducesResponseType(typeof(ApiResponse<List<JobAlert>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<JobAlert>>>> GetJobAlerts()
        {
            var alerts = await _userService.GetJobAlertsAsync(GetCurrentUserId());
            return Ok(ApiResponse<List<JobAlert>>.Ok(alerts));
        }

        /// <summary>
        /// Create a new customized job alert (e.g. ".NET fresher Chennai").
        /// </summary>
        [HttpPost("job-alerts")]
        [ProducesResponseType(typeof(ApiResponse<JobAlert>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<JobAlert>>> CreateJobAlert([FromBody] CreateJobAlertRequest request)
        {
            var alert = await _userService.CreateJobAlertAsync(
                GetCurrentUserId(),
                request.Name,
                request.Query,
                request.FiltersJson,
                request.Frequency ?? "Daily");

            return CreatedAtAction(nameof(GetJobAlerts), ApiResponse<JobAlert>.Ok(alert, "Job alert created."));
        }

        /// <summary>
        /// Delete an active job alert.
        /// </summary>
        [HttpDelete("job-alerts/{alertId}")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteJobAlert(string alertId)
        {
            var success = await _userService.DeleteJobAlertAsync(GetCurrentUserId(), alertId);
            return Ok(ApiResponse<bool>.Ok(success, success ? "Job alert deleted." : "Alert not found."));
        }
    }

    public class CreateJobAlertRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
        public string? FiltersJson { get; set; }
        public string? Frequency { get; set; } = "Daily";
    }
}
