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
        public List<string> CompanyTypes { get; set; } = new(); // MNC, GCC, STARTUP, SCALEUP, PRODUCT, IT_SERVICES, SAAS, etc.
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
        public string VerificationStatus { get; set; } = "VERIFIED"; // VERIFIED, SOURCE_BACKED, USER_SUBMITTED, ADMIN_VERIFIED, UNVERIFIED, STALE, REJECTED
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSeedData { get; set; } = true;
        public string SourceType { get; set; } = "OFFICIAL_WEBSITE";
        public string SourceName { get; set; } = "Official Careers / Company Website";
        public string? SourceUrl { get; set; }
        public string? SourceRecordId { get; set; }
        public string VerificationMethod { get; set; } = "OFFICIAL_DOMAIN_AUDIT";
        public int ConfidenceScore { get; set; } = 95;
        public int ChennaiRelevanceScore { get; set; } = 100;
        public string Industry { get; set; } = "Technology";
        public string? SubCategory { get; set; }
        public string Headquarters { get; set; } = "Chennai, Tamil Nadu";
        public string ChennaiPresence { get; set; } = "Headquarters / Technology Center";
        public List<string> ChennaiLocations { get; set; } = new();
        public string City { get; set; } = "Chennai";
        public string State { get; set; } = "Tamil Nadu";
        public string Country { get; set; } = "India";
        public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
        public DateTime LastVerifiedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Aliases & convenience accessors for Milestone 11
        public string ShortDescription { get => !string.IsNullOrEmpty(Tagline) ? Tagline : (Description.Length > 150 ? Description.Substring(0, 147) + "..." : Description); set => Tagline = value; }
        public string LogoUrl { get => Logo; set => Logo = value; }
        public string OfficialWebsite { get => Website; set => Website = value; }
        public string OfficialCareersUrl { get => CareersUrl; set => CareersUrl = value; }
        public string Category { get => Categories.Count > 0 ? Categories[0] : "SaaS / Enterprise Software"; set { if (!Categories.Contains(value) && !string.IsNullOrWhiteSpace(value)) Categories.Insert(0, value); } }
        public string CompanyType { get => CompanyTypes.Count > 0 ? CompanyTypes[0] : "STARTUP"; set { if (!CompanyTypes.Contains(value) && !string.IsNullOrWhiteSpace(value)) CompanyTypes.Insert(0, value); } }
        public string EmployeeRange { get => EmployeeCount; set => EmployeeCount = value; }
        public bool IsStartup => CompanyTypes.Any(t => t.Equals("STARTUP", StringComparison.OrdinalIgnoreCase));
        public bool IsMNC => CompanyTypes.Any(t => t.Equals("MNC", StringComparison.OrdinalIgnoreCase) || t.Equals("ENTERPRISE", StringComparison.OrdinalIgnoreCase));
        public bool IsGCC => CompanyTypes.Any(t => t.Equals("GCC", StringComparison.OrdinalIgnoreCase) || t.Equals("GLOBAL CAPABILITY CENTER", StringComparison.OrdinalIgnoreCase));
        public bool IsProductCompany => CompanyTypes.Any(t => t.Equals("PRODUCT COMPANY", StringComparison.OrdinalIgnoreCase) || t.Equals("PRODUCT", StringComparison.OrdinalIgnoreCase) || t.Equals("SAAS", StringComparison.OrdinalIgnoreCase));
        public bool IsITServices => CompanyTypes.Any(t => t.Equals("IT SERVICES", StringComparison.OrdinalIgnoreCase) || t.Equals("CONSULTING", StringComparison.OrdinalIgnoreCase));
        public bool IsHiring => HiringStatus.Equals("Active", StringComparison.OrdinalIgnoreCase) || HiringStatus.Equals("Hiring Surge", StringComparison.OrdinalIgnoreCase);
        public List<string> TechnologyTags { get => Tags; set => Tags = value; }
        public List<string> Skills { get => TechStack; set => TechStack = value; }

        public List<Job> Jobs { get; set; } = new();
        public List<CompanySource> Sources { get; set; } = new();
        public List<CareerSource> CareerSources { get; set; } = new();
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
        public string FreshnessStatus { get; set; } = "NEW"; // NEW, ACTIVE, RECENTLY_VERIFIED, STALE, EXPIRED, CLOSED
        public string VerificationStatus { get; set; } = "VERIFIED"; // VERIFIED, PENDING_REVIEW, UNVERIFIED, REJECTED
        public string? DuplicateGroupId { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsSeedData { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }
    }

    public class CompanySource
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CompanyId { get; set; } = string.Empty;
        public string SourceType { get; set; } = "OFFICIAL_WEBSITE"; // OFFICIAL_WEBSITE, OFFICIAL_CAREERS, PUBLIC_DIRECTORY, MANUAL_ADMIN_IMPORT, USER_SUBMISSION
        public string SourceName { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
        public string? SourceRecordId { get; set; }
        public int Confidence { get; set; } = 90;
        public string Status { get; set; } = "VERIFIED";
        public DateTime DiscoveredAt { get; set; } = DateTime.UtcNow;
        public DateTime? VerifiedAt { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }
    }

    public class CareerSource
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CompanyId { get; set; } = string.Empty;
        public string Provider { get; set; } = "Company Careers"; // Greenhouse, Lever, Workday, SmartRecruiters, Ashby, Company Careers
        public string CareersUrl { get; set; } = string.Empty;
        public string? JobsApiUrl { get; set; }
        public string SourceType { get; set; } = "OFFICIAL_CAREERS";
        public bool IsActive { get; set; } = true;
        public DateTime LastCheckedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "VERIFIED";

        public Company? Company { get; set; }
    }

    public class SavedJob
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public string JobId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Job? Job { get; set; }
    }

    public class SavedCompany
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }
    }

    public class JobAlert
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Query { get; set; } = string.Empty;
        public string? FiltersJson { get; set; }
        public string Frequency { get; set; } = "Daily"; // Daily, Weekly
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastSentAt { get; set; }
    }

    public class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public string Type { get; set; } = "SYSTEM"; // NEW_JOB, JOB_ALERT, COMPANY_UPDATE, SYSTEM
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Link { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CompanyClaim
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string CompanyId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string CorporateEmail { get; set; } = string.Empty;
        public string ProofNotes { get; set; } = string.Empty;
        public string Status { get; set; } = "PENDING"; // PENDING, APPROVED, REJECTED
        public string? ReviewedBy { get; set; }
        public DateTime? ReviewedAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Company? Company { get; set; }
    }

    public class AnalyticsEvent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EventType { get; set; } = "SEARCH"; // SEARCH, JOB_VIEW, COMPANY_VIEW, APPLY_CLICK, SAVED_JOB, SAVED_COMPANY
        public string? EntityId { get; set; }
        public string? MetadataJson { get; set; }
        public string? UserIdentifierHash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
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
