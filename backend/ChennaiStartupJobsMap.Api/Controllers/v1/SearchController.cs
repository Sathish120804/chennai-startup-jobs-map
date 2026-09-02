using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Services;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// Intelligent Multi-Entity Search API.
    /// Analyzes natural language intent ("dotnet fresher OMR", "React internship Chennai") and returns unified results.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Tags("Unified Search")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Perform an intent-aware unified search across Chennai companies and active jobs.
        /// </summary>
        /// <param name="q">Natural language search query (e.g. "dotnet fresher chennai", "ai startups guindy").</param>
        /// <response code="200">Unified search response with extracted intent, matching companies, and matching jobs.</response>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<SearchResponseDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<SearchResponseDto>>> Search([FromQuery] string? q)
        {
            var result = await _searchService.SearchAsync(q ?? string.Empty);
            return Ok(ApiResponse<SearchResponseDto>.Ok(result, "Search executed successfully."));
        }
    }
}
