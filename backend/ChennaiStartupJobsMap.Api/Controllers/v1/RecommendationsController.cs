using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.Services.AI;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// AI-Powered Recommendations Engine.
    /// Recommends Chennai jobs and tech companies using semantic similarity, skill matching, and location proximity.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Tags("AI Recommendations")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IJobRecommendationService _recommendations;

        public RecommendationsController(IJobRecommendationService recommendations)
        {
            _recommendations = recommendations;
        }

        /// <summary>
        /// Get smart job recommendations with match scores and explainable "Why this matches" reasoning.
        /// </summary>
        /// <param name="q">Natural language profile / search query (e.g. "React developer looking for entry level roles in OMR").</param>
        /// <param name="tech">Comma-delimited candidate skills / technologies (e.g. ".NET,React,Python").</param>
        /// <param name="hub">Preferred Chennai corridor (e.g. "OMR", "Guindy", "Siruseri").</param>
        /// <param name="fresher">Filter for fresher / entry-level opportunities.</param>
        /// <param name="internship">Filter for internship vacancies.</param>
        /// <param name="limit">Maximum number of recommendations to return (default: 6).</param>
        /// <response code="200">Ranked job recommendations with match scores and match reasons.</response>
        [HttpGet("jobs")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<JobRecommendationDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<JobRecommendationDto>>>> GetJobRecommendations(
            [FromQuery] string? q,
            [FromQuery] string? tech,
            [FromQuery] string? hub,
            [FromQuery] bool? fresher,
            [FromQuery] bool? internship,
            [FromQuery] int limit = 6)
        {
            var techList = !string.IsNullOrWhiteSpace(tech) ? tech.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() : null;
            var results = await _recommendations.GetJobRecommendationsAsync(q, techList, hub, fresher, internship, limit);
            return Ok(ApiResponse<List<JobRecommendationDto>>.Ok(results, "Recommendations retrieved successfully."));
        }

        /// <summary>
        /// Get smart company recommendations matching user preferences or industry interests.
        /// </summary>
        /// <param name="q">Search query or domain interest.</param>
        /// <param name="category">Industry sector (e.g. "SaaS / Enterprise Software").</param>
        /// <param name="hub">Preferred corridor.</param>
        /// <param name="limit">Maximum recommendations (default: 6).</param>
        /// <response code="200">Ranked company recommendations.</response>
        [HttpGet("companies")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<List<CompanyRecommendationDto>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<CompanyRecommendationDto>>>> GetCompanyRecommendations(
            [FromQuery] string? q,
            [FromQuery] string? category,
            [FromQuery] string? hub,
            [FromQuery] int limit = 6)
        {
            var results = await _recommendations.GetCompanyRecommendationsAsync(q, category, hub, limit);
            return Ok(ApiResponse<List<CompanyRecommendationDto>>.Ok(results, "Company recommendations retrieved successfully."));
        }
    }
}
