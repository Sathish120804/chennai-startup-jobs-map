using System;
using System.Collections.Generic;

namespace ChennaiStartupJobsMap.Api.Models
{
    public enum SourceType
    {
        COMPANY_CAREERS,
        GREENHOUSE_ATS,
        LEVER_ATS,
        WORKDAY_ATS,
        AUTHORIZED_SEARCH_API,
        USER_SUBMISSION,
        MANUAL_IMPORT,
        PUBLIC_DIRECTORY
    }

    public class SourceInfo
    {
        public string SourceId { get; set; } = string.Empty;
        public string SourceName { get; set; } = string.Empty;
        public SourceType Type { get; set; } = SourceType.COMPANY_CAREERS;
        public string BaseUrl { get; set; } = string.Empty;
        public bool Enabled { get; set; } = true;
        public bool IsEnabled { get => Enabled; set => Enabled = value; }
        public int Priority { get; set; } = 1; // 1 = highest
        public int TrustLevel { get; set; } = 100;
        public int ReliabilityScore { get => TrustLevel; set => TrustLevel = value; }
        public DateTime? LastSuccessfulRun { get; set; }
        public DateTime? LastFailedRun { get; set; }
        public DateTime? LastRun { get => LastSuccessfulRun ?? LastFailedRun; set => LastSuccessfulRun = value; }
        public DateTime? NextRun { get; set; }
        public int RequestsPerMinute { get; set; } = 30;
        public int RateLimit { get => RequestsPerMinute; set => RequestsPerMinute = value; }
        public string TermsNotes { get; set; } = "Public official metadata / authorized ingest";
        public string RobotsPolicy { get; set; } = "Respect robots.txt / official pages only";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class SourceDefinition
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "OFFICIAL_COMPANY";
        public string BaseUrl { get; set; } = string.Empty;
        public bool IsEnabled { get; set; } = true;
        public int Priority { get; set; } = 1; // 1 = Highest
        public DateTime? LastRun { get; set; }
        public DateTime? NextRun { get; set; }
        public int RateLimit { get; set; } = 30; // req/min
        public string TermsNotes { get; set; } = "Official verified public source";
        public string RobotsPolicy { get; set; } = "Compliant with robots.txt; no rate abuse";
        public int ReliabilityScore { get; set; } = 95; // 0-100
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class IngestionRun
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SourceId { get; set; } = string.Empty;
        public string EntityType { get; set; } = "job"; // job or company
        public DateTime StartedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CompletedAt { get; set; }
        public string Status { get; set; } = "RUNNING"; // RUNNING, COMPLETED, FAILED
        public int RecordsDiscovered { get; set; }
        public int RecordsCreated { get; set; }
        public int RecordsUpdated { get; set; }
        public int RecordsSkipped { get; set; }
        public int DuplicatesFound { get; set; }
        public int ErrorsCount { get; set; }
        public string? ErrorSummary { get; set; }
    }

    public class JobSourceRecord
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string JobId { get; set; } = string.Empty;
        public string SourceId { get; set; } = string.Empty;
        public string ExternalId { get; set; } = string.Empty;
        public string SourceUrl { get; set; } = string.Empty;
        public string ApplyUrl { get; set; } = string.Empty;
        public DateTime FirstSeenAt { get; set; } = DateTime.UtcNow;
        public DateTime LastSeenAt { get; set; } = DateTime.UtcNow;
        public DateTime LastVerifiedAt { get; set; } = DateTime.UtcNow;
        public string RawTitle { get; set; } = string.Empty;
        public string RawCompany { get; set; } = string.Empty;
        public string RawLocation { get; set; } = string.Empty;
    }

    public class VerificationLog
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string EntityType { get; set; } = "job";
        public string EntityId { get; set; } = string.Empty;
        public string SourceId { get; set; } = string.Empty;
        public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
        public string Result { get; set; } = "VERIFIED";
        public string? Reason { get; set; }
        public int ResponseStatusCode { get; set; } = 200;
    }

    public class DataQualityScore
    {
        public int OverallScore { get; set; } // 0 to 100
        public string Rating { get; set; } = "High Quality";
        public List<string> PassedSignals { get; set; } = new();
        public List<string> Warnings { get; set; } = new();
    }
}
