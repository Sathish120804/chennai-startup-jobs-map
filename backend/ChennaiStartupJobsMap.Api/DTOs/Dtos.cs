using System;
using System.Collections.Generic;

namespace ChennaiStartupJobsMap.Api.DTOs
{
    public class PagedResultDto<T>
    {
        public List<T> Items { get; set; } = new();
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public int Total { get; set; }
        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)Total / PageSize) : 0;
    }

    public class CompanyDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Tagline { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public string LogoUrl { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string OfficialWebsite { get; set; } = string.Empty;
        public string CareersUrl { get; set; } = string.Empty;
        public string OfficialCareersUrl { get; set; } = string.Empty;
        public List<string> CompanyTypes { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public string Category { get; set; } = string.Empty;
        public string? SubCategory { get; set; }
        public string Industry { get; set; } = "Technology";
        public string CompanyType { get; set; } = "STARTUP";
        public string Hub { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = "Chennai";
        public string State { get; set; } = "Tamil Nadu";
        public string Country { get; set; } = "India";
        public string Headquarters { get; set; } = "Chennai, Tamil Nadu";
        public string ChennaiPresence { get; set; } = "Headquarters / Tech Hub";
        public List<string> ChennaiLocations { get; set; } = new();
        public CoordinatesDto Coordinates { get; set; } = new();
        public string MapPrecision { get; set; } = "exact";
        public int FoundedYear { get; set; }
        public string EmployeeCount { get; set; } = string.Empty;
        public string EmployeeRange { get; set; } = string.Empty;
        public string FundingStage { get; set; } = string.Empty;
        public string? TotalFundingRaised { get; set; }
        public string HiringStatus { get; set; } = "Active";
        public bool IsHiring { get; set; }
        public bool IsStartup { get; set; }
        public bool IsMNC { get; set; }
        public bool IsGCC { get; set; }
        public bool IsProductCompany { get; set; }
        public bool IsITServices { get; set; }
        public List<string> Tags { get; set; } = new();
        public List<string> TechnologyTags { get; set; } = new();
        public List<string> TechStack { get; set; } = new();
        public List<string> Skills { get; set; } = new();
        public string VerificationStatus { get; set; } = "VERIFIED";
        public string VerificationMethod { get; set; } = "OFFICIAL_DOMAIN_AUDIT";
        public int ConfidenceScore { get; set; } = 95;
        public int ChennaiRelevanceScore { get; set; } = 100;
        public bool IsFeatured { get; set; }
        public bool IsSeedData { get; set; }
        public string SourceType { get; set; } = "OFFICIAL_WEBSITE";
        public string SourceName { get; set; } = string.Empty;
        public string? SourceUrl { get; set; }
        public DateTime DiscoveredAt { get; set; }
        public DateTime LastVerifiedAt { get; set; }
        public CompanyJobStatsDto Stats { get; set; } = new();
    }

    public class CompanyImportReportDto
    {
        public int TotalRows { get; set; }
        public int ValidRows { get; set; }
        public int InvalidRows { get; set; }
        public int Duplicates { get; set; }
        public int NewCompanies { get; set; }
        public int UpdatedCompanies { get; set; }
        public int RejectedCompanies { get; set; }
        public List<string> Warnings { get; set; } = new();
        public List<string> Errors { get; set; } = new();
        public List<CompanyDto> SamplePreview { get; set; } = new();
    }

    public class CompanyMergeRequestDto
    {
        public string PrimaryCompanyId { get; set; } = string.Empty;
        public string DuplicateCompanyId { get; set; } = string.Empty;
    }

    public class DataQualityDashboardDto
    {
        public int TargetCompanyGoal { get; set; } = 700;
        public int CurrentVerifiedCount { get; set; }
        public int TotalCompanies { get; set; }
        public int VerifiedCompanies { get; set; }
        public int SourceBackedCompanies { get; set; }
        public int UserSubmittedCompanies { get; set; }
        public int AdminVerifiedCompanies { get; set; }
        public int UnverifiedCompanies { get; set; }
        public int PendingReviewCompanies { get; set; }
        public int StaleCompanies { get; set; }
        public int DuplicatesIdentified { get; set; }
        public int MissingWebsiteCount { get; set; }
        public int MissingCareersUrlCount { get; set; }
        public int MissingCoordinatesCount { get; set; }
        public Dictionary<string, int> CompaniesByCategory { get; set; } = new();
        public Dictionary<string, int> CompaniesByCompanyType { get; set; } = new();
        public Dictionary<string, int> CompaniesBySource { get; set; } = new();
        public double TargetProgressPercentage => TargetCompanyGoal > 0 ? Math.Min(100.0, Math.Round((double)CurrentVerifiedCount / TargetCompanyGoal * 100.0, 1)) : 0.0;
    }

    public class CoordinatesDto
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class CompanyJobStatsDto
    {
        public int ActiveJobsCount { get; set; }
        public int EngineeringJobsCount { get; set; }
        public int FresherJobsCount { get; set; }
        public int InternshipsCount { get; set; }
        public string? LastJobDiscoveredAt { get; set; }
    }

    public class JobDto
    {
        public string Id { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string CompanyLogo { get; set; } = string.Empty;
        public string CompanyHub { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string DescriptionSnippet { get; set; } = string.Empty;
        public string PrimaryCategory { get; set; } = string.Empty;
        public bool IsEngineering { get; set; }
        public string? EngineeringSubcategory { get; set; }
        public List<string> Technologies { get; set; } = new();
        public string JobType { get; set; } = string.Empty;
        public string WorkplaceType { get; set; } = string.Empty;
        public string ExperienceLevel { get; set; } = string.Empty;
        public bool IsFresher { get; set; }
        public int FresherConfidence { get; set; }
        public bool IsInternship { get; set; }
        public string? SalaryRange { get; set; }
        public string Location { get; set; } = string.Empty;
        public string ChennaiRelevance { get; set; } = string.Empty;
        public int RelevanceConfidence { get; set; }
        public string SourceName { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
        public string? ApplyUrl { get; set; }
        public DateTime FirstSeenAt { get; set; }
        public DateTime LastSeenAt { get; set; }
        public DateTime LastVerifiedAt { get; set; }
        public string FreshnessStatus { get; set; } = string.Empty;
        public string VerificationStatus { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public bool IsSeedData { get; set; }
    }

    public class ParsedSearchIntentDto
    {
        public string RawQuery { get; set; } = string.Empty;
        public string? Technology { get; set; }
        public bool? IsFresher { get; set; }
        public bool? IsInternship { get; set; }
        public string? Hub { get; set; }
        public string? CompanyType { get; set; }
        public string? Category { get; set; }
        public bool HasLocationIntent { get; set; }
        public List<string> MatchedSynonyms { get; set; } = new();
    }

    public class SearchResponseDto
    {
        public ParsedSearchIntentDto Intent { get; set; } = new();
        public PagedResultDto<CompanyDto> Companies { get; set; } = new();
        public PagedResultDto<JobDto> Jobs { get; set; } = new();
    }

    public class SubmitCompanyRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string? CareersUrl { get; set; }
        public string Hub { get; set; } = "OMR (IT Corridor)";
        public string Address { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SubmittedBy { get; set; } = "Community Member";
        public string? Email { get; set; }
    }

    public class SubmitJobRequest
    {
        public string CompanyName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string OriginalUrl { get; set; } = string.Empty;
        public string Location { get; set; } = "Chennai, Tamil Nadu";
        public string DescriptionSnippet { get; set; } = string.Empty;
        public string? SalaryRange { get; set; }
        public string SubmittedBy { get; set; } = "Community Member";
        public string? Email { get; set; }
    }
}
