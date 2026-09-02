using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Authentication;
using ChennaiStartupJobsMap.Api.Common;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Entities;

namespace ChennaiStartupJobsMap.Api.Services
{
    public interface IAuthService
    {
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);
        Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request);
        Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<ApiResponse<bool>> LogoutAsync(string userId);
        Task<ApiResponse<UserProfileDto>> GetProfileAsync(string userId);
    }

    public class AuthService : IAuthService
    {
        private readonly ChennaiDbContext _db;
        private readonly IJwtTokenService _jwt;

        public AuthService(ChennaiDbContext db, IJwtTokenService jwt)
        {
            _db = db;
            _jwt = jwt;
        }

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _db.Set<User>().FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
            if (existingUser != null)
            {
                return ApiResponse<AuthResponseDto>.Fail("A user with this email already exists.");
            }

            var validRole = request.Role.ToUpper() switch
            {
                UserRoles.Recruiter => UserRoles.Recruiter,
                _ => UserRoles.User
            };

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = request.Name.Trim(),
                Email = request.Email.Trim().ToLower(),
                PasswordHash = _jwt.HashPassword(request.Password),
                Role = validRole,
                CompanyId = request.CompanyId,
                IsVerified = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                LastLogin = DateTime.UtcNow
            };

            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            _db.Set<User>().Add(user);
            await _db.SaveChangesAsync();

            return ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(4),
                User = MapToProfile(user)
            }, "User registered successfully.");
        }

        public async Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request)
        {
            var user = await _db.Set<User>().FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower());
            if (user == null || !_jwt.VerifyPassword(request.Password, user.PasswordHash))
            {
                return ApiResponse<AuthResponseDto>.Fail("Invalid email or password.");
            }

            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
            user.LastLogin = DateTime.UtcNow;

            await _db.SaveChangesAsync();

            return ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(4),
                User = MapToProfile(user)
            }, "Login successful.");
        }

        public async Task<ApiResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await _db.Set<User>().FirstOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
            if (user == null || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return ApiResponse<AuthResponseDto>.Fail("Invalid or expired refresh token.");
            }

            var newAccessToken = _jwt.GenerateAccessToken(user);
            var newRefreshToken = _jwt.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);

            await _db.SaveChangesAsync();

            return ApiResponse<AuthResponseDto>.Ok(new AuthResponseDto
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(4),
                User = MapToProfile(user)
            }, "Token refreshed successfully.");
        }

        public async Task<ApiResponse<bool>> LogoutAsync(string userId)
        {
            var user = await _db.Set<User>().FirstOrDefaultAsync(u => u.Id == userId);
            if (user != null)
            {
                user.RefreshToken = null;
                user.RefreshTokenExpiry = null;
                await _db.SaveChangesAsync();
            }
            return ApiResponse<bool>.Ok(true, "Logged out successfully.");
        }

        public async Task<ApiResponse<UserProfileDto>> GetProfileAsync(string userId)
        {
            var user = await _db.Set<User>().AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return ApiResponse<UserProfileDto>.Fail("User profile not found.");
            }

            return ApiResponse<UserProfileDto>.Ok(MapToProfile(user));
        }

        private static UserProfileDto MapToProfile(User user) => new()
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            CompanyId = user.CompanyId,
            IsVerified = user.IsVerified,
            CreatedAt = user.CreatedAt,
            LastLogin = user.LastLogin
        };
    }
}
