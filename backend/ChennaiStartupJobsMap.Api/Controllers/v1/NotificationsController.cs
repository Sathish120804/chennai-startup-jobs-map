using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.Models;
using ChennaiStartupJobsMap.Api.Services;

namespace ChennaiStartupJobsMap.Api.Controllers.v1
{
    /// <summary>
    /// User In-App Notifications and Alerts.
    /// Provides notifications for new matching jobs, company updates, and platform alerts.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Authorize]
    [Produces("application/json")]
    [Tags("User Notifications")]
    public class NotificationsController : ControllerBase
    {
        private readonly IUserService _userService;

        public NotificationsController(IUserService userService)
        {
            _userService = userService;
        }

        private string GetCurrentUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "user-demo";

        /// <summary>
        /// Retrieve notifications for the current authenticated user.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<List<Notification>>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<List<Notification>>>> GetNotifications()
        {
            var notifs = await _userService.GetNotificationsAsync(GetCurrentUserId());
            return Ok(ApiResponse<List<Notification>>.Ok(notifs));
        }

        /// <summary>
        /// Mark a notification as read.
        /// </summary>
        [HttpPut("{id}/read")]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> MarkAsRead(string id)
        {
            var success = await _userService.MarkNotificationReadAsync(GetCurrentUserId(), id);
            return Ok(ApiResponse<bool>.Ok(success, success ? "Notification marked as read." : "Notification not found."));
        }
    }
}
