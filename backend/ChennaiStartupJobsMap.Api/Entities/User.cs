using System;

namespace ChennaiStartupJobsMap.Api.Entities
{
    public static class UserRoles
    {
        public const string Admin = "ADMIN";
        public const string Moderator = "MODERATOR";
        public const string Recruiter = "RECRUITER";
        public const string User = "USER";
    }

    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = UserRoles.User;
        public string? CompanyId { get; set; }
        public bool IsVerified { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiry { get; set; }
    }
}
