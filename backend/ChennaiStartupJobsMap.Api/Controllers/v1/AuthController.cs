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
    /// Authentication and Identity Operations for Chennai Startup and Jobs Map.
    /// Provides JWT token issuance, refresh token rotation, user registration, and profile queries.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [Tags("Authentication and Identity")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Register a new user account.
        /// </summary>
        /// <param name="request">User registration details including email, password, and optional role (USER or RECRUITER).</param>
        /// <response code="200">Registration successful with generated JWT token and refresh token.</response>
        /// <response code="400">Validation error or email address already registered.</response>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterRequestDto request)
        {
            var result = await _authService.RegisterAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Authenticate user credentials and return JWT bearer token.
        /// </summary>
        /// <param name="request">User login credentials.</param>
        /// <response code="200">Login successful. Returns JWT access token (4-hour lifetime) and refresh token (7-day lifetime).</response>
        /// <response code="400">Invalid credentials provided.</response>
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Refresh an expired JWT access token using a valid refresh token.
        /// </summary>
        /// <param name="request">Refresh token string.</param>
        /// <response code="200">New JWT access token and rotated refresh token.</response>
        /// <response code="400">Refresh token is invalid or has expired.</response>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<AuthResponseDto>), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            if (!result.Success) return BadRequest(result);
            return Ok(result);
        }

        /// <summary>
        /// Logout user and invalidate active refresh tokens.
        /// </summary>
        /// <response code="200">Logout successful.</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<bool>>> Logout()
        {
            var userId = User.FindFirst("userId")?.Value ?? string.Empty;
            var result = await _authService.LogoutAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Get profile information of currently authenticated user.
        /// </summary>
        /// <response code="200">User profile details including name, email, verified status, and assigned role.</response>
        /// <response code="401">Unauthorized request if token is missing or expired.</response>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse<UserProfileDto>>> GetMe()
        {
            var userId = User.FindFirst("userId")?.Value ?? string.Empty;
            var result = await _authService.GetProfileAsync(userId);
            if (!result.Success) return NotFound(result);
            return Ok(result);
        }
    }
}
