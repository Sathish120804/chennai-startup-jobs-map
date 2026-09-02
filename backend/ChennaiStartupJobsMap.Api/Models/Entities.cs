using System;
using System.Collections.Generic;

namespace ChennaiStartupJobsMap.Api.Models
{
    public class Company
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Tagline { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string CareersUrl { get; set; } = string.Empty;
        public List<string> CompanyTypes { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public string Hub { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string MapPrecision { get; set; } = "exact"; // exact, approximate, area, city
        public int FoundedYear { get; set; }
        public string EmployeeCount { get; set; } = "1-10";
        public string FundingStage { get; set; } = "Bootstrapped";
        public string? TotalFundingRaised { get; set; }
        public string HiringStatus { get; set; } = "Active"; // Active, Hiring Surge, Selective, Not Hiring
        public List<string> Tags { get; set; } = new();
        public List<string> TechStack { get; set; } = new();
        public string VerificationStatus { get; set; } = "VERIFIED";
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSeedData { get; set; } = true;
        public string SourceName { get; set; } = "Company Careers";
        public string? SourceUrl { get; set; }
        public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
        public DateTime LastVerifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<Job> Jobs { get; set; } = new();
    }

    public class Job
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyLogo { get; set; } = string.Empty;
        public string CompanyHub { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string NormalizedTitle { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string DescriptionSnippet { get; set; } = string.Empty;
        public string PrimaryCategory { get; set; } = "SaaS / Enterprise Software";
        public bool IsEngineering { get; set; } = true;
        public string? EngineeringSubcategory { get; set; }
        public List<string> Technologies { get; set; } = new();
        public string JobType { get; set; } = "Full-time";
        public string WorkplaceType { get; set; } = "On-site";
        public string ExperienceLevel { get; set; } = "Mid (3-5 yrs)";
        public double? ExperienceMin { get; set; }
        public double? ExperienceMax { get; set; }
        public bool IsFresher { get; set; }
        public int FresherConfidence { get; set; } = 80;
        public bool IsInternship { get; set; }
        public string? SalaryRange { get; set; }
        public double? SalaryMin { get; set; }
        public double? SalaryMax { get; set; }
        public string SalaryCurrency { get; set; } = "INR";
        public string Location { get; set; } = "Chennai, Tamil Nadu";
        public string ChennaiRelevance { get; set; } = "CHENNAI_CONFIRMED";
        public int RelevanceConfidence { get; set; } = 95;
        public string SourceName { get; set; } = "Company Careers";
        public string OriginalUrl { get; set; } = string.Empty;
        public string? ApplyUrl { get; set; }
        public string? SourceRecordId { get; set; }
        public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;
        public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
        public DateTime LastVerifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ExpiresAt { get; set; }
        public string FreshnessStatus { get; set; } = "NEW"; // NEW, ACTIVE, RECENTLY_VERIFIED, STALE, EXPIRED
        public string VerificationStatus { get; set; } = "VERIFIED";
        public string? DuplicateGroupId { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSeedData { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }
    }

    public class Technology
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> Synonyms { get; set; } = new();
    }

    public class Location
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Hub { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Precision { get; set; } = "exact";
    }

    public class UserSubmission
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Type { get; set; } = "company"; // company or job
        public string SubmittedBy { get; set; } = "Community Member";
        public string? Email { get; set; }
        public string TitleOrName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string? Hub { get; set; }
        public string? Notes { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "PENDING"; // PENDING, APPROVED, REJECTED
    }

    public class RawIngestionRecord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SourceName { get; set; } = string.Empty;
        public string ExternalId { get; set; } = string.Empty;
        public string RawTitle { get; set; } = string.Empty;
        public string RawCompany { get; set; } = string.Empty;
        public string RawLocation { get; set; } = string.Empty;
        public string RawUrl { get; set; } = string.Empty;
        public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "PROCESSED"; // PENDING, PROCESSED, FAILED
        public string? ErrorMessage { get; set; }
    }
}
