using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.Entities;
using ChennaiStartupJobsMap.Api.Services;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// Platform Analytics and Observability.
    /// Tracks aggregate interaction events (search, job view, company view, apply click) with zero intrusive tracking.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Tags("Platform Analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IAnalyticsService _analytics;

        public AnalyticsController(IAnalyticsService analytics)
        {
            _analytics = analytics;
        }

        /// <summary>
        /// Log an aggregate platform event (e.g. search, job view, apply click).
        /// </summary>
        [HttpPost("event")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> TrackEvent([FromBody] TrackEventRequest request)
        {
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            await _analytics.TrackEventAsync(request.EventType, request.EntityId, request.MetadataJson, clientIp);
            return Ok(ApiResponse<bool>.Ok(true, "Event logged."));
        }

        /// <summary>
        /// Retrieve high-level aggregate platform metrics and interaction activity.
        /// </summary>
        [HttpGet("overview")]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Moderator}")]
        [ProducesResponseType(typeof(ApiResponse<AnalyticsOverviewDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<AnalyticsOverviewDto>>> GetOverview()
        {
            var overview = await _analytics.GetAnalyticsOverviewAsync();
            return Ok(ApiResponse<AnalyticsOverviewDto>.Ok(overview));
        }
    }

    public class TrackEventRequest
    {
        public string EventType { get; set; } = "SEARCH";
        public string? EntityId { get; set; }
        public string? MetadataJson { get; set; }
    }
}
