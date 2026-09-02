using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// Community and Recruiter Submissions.
    /// Allows users to submit new Chennai startups or job vacancies for moderation.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Tags("Community Submissions")]
    public class SubmissionsController : ControllerBase
    {
        private readonly ChennaiDbContext _db;

        public SubmissionsController(ChennaiDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Submit a Chennai company for indexing.
        /// </summary>
        [HttpPost("company")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<UserSubmission>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<UserSubmission>>> SubmitCompany([FromBody] SubmitCompanyRequest request)
        {
            var submission = new UserSubmission
            {
                Type = "company",
                TitleOrName = request.Name,
                Url = request.Website,
                Notes = $"{request.Hub} • {request.Description}",
                Status = "PENDING",
                SubmittedBy = request.SubmittedBy
            };

            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubmissionStatus), new { id = submission.Id }, ApiResponse<UserSubmission>.Ok(submission, "Company submission received for review."));
        }

        /// <summary>
        /// Submit a job vacancy for indexing.
        /// </summary>
        [HttpPost("job")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<UserSubmission>), StatusCodes.Status201Created)]
        public async Task<ActionResult<ApiResponse<UserSubmission>>> SubmitJob([FromBody] SubmitJobRequest request)
        {
            var submission = new UserSubmission
            {
                Type = "job",
                TitleOrName = $"{request.Title} @ {request.CompanyName}",
                Url = request.OriginalUrl,
                Notes = $"Location: {request.Location} • Description: {request.DescriptionSnippet}",
                Status = "PENDING",
                SubmittedBy = request.SubmittedBy
            };

            _db.Submissions.Add(submission);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubmissionStatus), new { id = submission.Id }, ApiResponse<UserSubmission>.Ok(submission, "Job submission received for review."));
        }

        /// <summary>
        /// Check moderation status of a submission.
        /// </summary>
        [HttpGet("{id}/status")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<UserSubmission>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<UserSubmission>>> GetSubmissionStatus(string id)
        {
            var submission = await _db.Submissions.FirstOrDefaultAsync(s => s.Id == id);
            if (submission == null) return NotFound(ApiResponse<UserSubmission>.Fail("Submission not found."));

            return Ok(ApiResponse<UserSubmission>.Ok(submission));
        }
    }
}
