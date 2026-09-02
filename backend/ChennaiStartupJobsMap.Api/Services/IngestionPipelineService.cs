using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Services
{
    public interface ISourceRegistryService
    {
        List<SourceInfo> GetRegisteredSources();
        SourceInfo? GetSource(string sourceId);
    }

    public class SourceRegistryService : ISourceRegistryService
    {
        private readonly List<SourceInfo> _sources = new()
        {
            new SourceInfo { SourceId = "src-careers", SourceName = "Company Careers Portals", Type = SourceType.COMPANY_CAREERS, Priority = 1, TrustLevel = 100 },
            new SourceInfo { SourceId = "src-greenhouse", SourceName = "Greenhouse ATS", Type = SourceType.GREENHOUSE_ATS, Priority = 1, TrustLevel = 95 },
            new SourceInfo { SourceId = "src-lever", SourceName = "Lever ATS", Type = SourceType.LEVER_ATS, Priority = 1, TrustLevel = 95 },
            new SourceInfo { SourceId = "src-workday", SourceName = "Workday ATS", Type = SourceType.WORKDAY_ATS, Priority = 2, TrustLevel = 90 },
            new SourceInfo { SourceId = "src-search", SourceName = "Authorized Search API", Type = SourceType.AUTHORIZED_SEARCH_API, Priority = 3, TrustLevel = 85 },
            new SourceInfo { SourceId = "src-user", SourceName = "Community Submission", Type = SourceType.USER_SUBMISSION, Priority = 4, TrustLevel = 80 }
        };

        public List<SourceInfo> GetRegisteredSources() => _sources;

        public SourceInfo? GetSource(string sourceId) => _sources.FirstOrDefault(s => s.SourceId == sourceId);
    }

    public interface INormalizationService
    {
        string NormalizeTitle(string rawTitle);
        string NormalizeCompanyName(string rawCompanyName);
        string NormalizeLocation(string rawLocation);
        List<string> ExtractTechnologies(string text);
        bool DetectFresher(string title, string description);
        bool DetectInternship(string title, string description);
    }

    public class NormalizationService : INormalizationService
    {
        private static readonly Dictionary<string, string> TechAliases = new(StringComparer.OrdinalIgnoreCase)
        {
            { "dotnet", ".NET" },
            { ".net", ".NET" },
            { "c#", "C#" },
            { "asp.net", "ASP.NET Core" },
            { "react", "React" },
            { "reactjs", "React" },
            { "node", "Node.js" },
            { "nodejs", "Node.js" },
            { "python", "Python" },
            { "java", "Java" },
            { "spring boot", "Spring Boot" },
            { "postgresql", "PostgreSQL" },
            { "aws", "AWS" },
            { "azure", "Azure" },
            { "docker", "Docker" },
            { "kubernetes", "Kubernetes" },
            { "flutter", "Flutter" },
            { "golang", "Go" },
            { "ai", "AI / ML" },
            { "machine learning", "Machine Learning" }
        };

        public string NormalizeTitle(string rawTitle)
        {
            if (string.IsNullOrWhiteSpace(rawTitle)) return string.Empty;
            var cleaned = Regex.Replace(rawTitle.ToLower(), @"[^a-z0-9\s]", " ");
            return Regex.Replace(cleaned, @"\s+", " ").Trim();
        }

        public string NormalizeCompanyName(string rawCompanyName)
        {
            if (string.IsNullOrWhiteSpace(rawCompanyName)) return string.Empty;
            var cleaned = Regex.Replace(rawCompanyName.ToLower(), @"\b(pvt|ltd|inc|corp|corporation|private|limited|technologies|solutions|labs)\b", "", RegexOptions.IgnoreCase);
            cleaned = Regex.Replace(cleaned, @"[^a-z0-9\s]", " ");
            return Regex.Replace(cleaned, @"\s+", " ").Trim();
        }

        public string NormalizeLocation(string rawLocation)
        {
            if (string.IsNullOrWhiteSpace(rawLocation)) return "Chennai, Tamil Nadu";
            return rawLocation.Trim();
        }

        public List<string> ExtractTechnologies(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new List<string>();
            var found = new HashSet<string>();
            var lower = text.ToLower();

            foreach (var kvp in TechAliases)
            {
                var key = kvp.Key.ToLower();
                if (lower.Contains(key))
                {
                    found.Add(kvp.Value);
                }
            }

            return found.ToList();
        }

        public bool DetectFresher(string title, string description)
        {
            var combined = $"{title} {description}".ToLower();

            // Negative check for senior roles
            if (Regex.IsMatch(combined, @"\b(senior|lead|architect|staff|principal|head|director|manager)\b"))
                return false;

            if (Regex.IsMatch(combined, @"\b(3|4|5|6|7|8|9|10)\+?\s*years?\b"))
                return false;

            return Regex.IsMatch(combined, @"\b(fresher|freshers|entry level|0-1 years?|0 years?|graduate trainee|college graduate|associate developer|2025|2026)\b");
        }

        public bool DetectInternship(string title, string description)
        {
            var combined = $"{title} {description}".ToLower();
            return Regex.IsMatch(title.ToLower(), @"\bintern\b") || Regex.IsMatch(combined, @"\binternship\b|\bstipend\b");
        }
    }

    public interface ICompanyMatcher
    {
        Task<(Company? company, string confidence)> MatchCompanyAsync(string rawCompanyName, string? website = null);
    }

    public class CompanyMatcher : ICompanyMatcher
    {
        private readonly ChennaiDbContext _db;
        private readonly INormalizationService _norm;

        public CompanyMatcher(ChennaiDbContext db, INormalizationService norm)
        {
            _db = db;
            _norm = norm;
        }

        public async Task<(Company? company, string confidence)> MatchCompanyAsync(string rawCompanyName, string? website = null)
        {
            if (!string.IsNullOrWhiteSpace(website))
            {
                var domain = ExtractDomain(website);
                if (!string.IsNullOrEmpty(domain))
                {
                    var byDomain = await _db.Companies.FirstOrDefaultAsync(c => c.Website.Contains(domain) || c.CareersUrl.Contains(domain));
                    if (byDomain != null) return (byDomain, "HIGH");
                }
            }

            var normName = _norm.NormalizeCompanyName(rawCompanyName);
            if (string.IsNullOrEmpty(normName)) return (null, "LOW");

            var exactMatch = await _db.Companies.FirstOrDefaultAsync(c => c.NormalizedName == normName || c.Name.ToLower() == rawCompanyName.ToLower());
            if (exactMatch != null) return (exactMatch, "HIGH");

            var partialMatch = await _db.Companies.FirstOrDefaultAsync(c => c.NormalizedName.Contains(normName) || normName.Contains(c.NormalizedName));
            if (partialMatch != null) return (partialMatch, "MEDIUM");

            return (null, "LOW");
        }

        private static string ExtractDomain(string url)
        {
            try
            {
                var uri = new Uri(url.StartsWith("http") ? url : "https://" + url);
                return uri.Host.Replace("www.", "");
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    public interface IDataQualityService
    {
        DataQualityScore CalculateJobQualityScore(Job job);
        DataQualityScore CalculateCompanyQualityScore(Company company);
    }

    public class DataQualityService : IDataQualityService
    {
        public DataQualityScore CalculateJobQualityScore(Job job)
        {
            int score = 0;
            var passed = new List<string>();
            var warnings = new List<string>();

            if (!string.IsNullOrWhiteSpace(job.CompanyId)) { score += 25; passed.Add("Associated with verified company entity (+25)"); }
            else { warnings.Add("Missing company association"); }

            if (!string.IsNullOrWhiteSpace(job.Title)) { score += 20; passed.Add("Valid job title (+20)"); }

            if (!string.IsNullOrWhiteSpace(job.OriginalUrl) && Uri.IsWellFormedUriString(job.OriginalUrl, UriKind.Absolute)) { score += 20; passed.Add("Valid external apply URL (+20)"); }
            else { warnings.Add("Invalid application URL"); }

            if (job.ChennaiRelevance == "CHENNAI_CONFIRMED") { score += 15; passed.Add("Confirmed Chennai location (+15)"); }
            else { warnings.Add("Unconfirmed Chennai relevance"); }

            if (job.Technologies.Count > 0) { score += 10; passed.Add($"Technologies extracted ({job.Technologies.Count}) (+10)"); }
            else { warnings.Add("No technologies extracted"); }

            if (job.VerificationStatus == "VERIFIED") { score += 10; passed.Add("Verified job posting (+10)"); }

            string rating = score >= 90 ? "High Quality" : score >= 70 ? "Good" : score >= 50 ? "Needs Review" : "Poor";

            return new DataQualityScore
            {
                OverallScore = score,
                Rating = rating,
                PassedSignals = passed,
                Warnings = warnings
            };
        }

        public DataQualityScore CalculateCompanyQualityScore(Company company)
        {
            int score = 0;
            var passed = new List<string>();
            var warnings = new List<string>();

            if (!string.IsNullOrWhiteSpace(company.Name)) { score += 25; passed.Add("Valid company name (+25)"); }
            if (!string.IsNullOrWhiteSpace(company.Website) && Uri.IsWellFormedUriString(company.Website, UriKind.Absolute)) { score += 25; passed.Add("Valid website URL (+25)"); }
            else { warnings.Add("Missing/invalid website URL"); }

            if (company.Latitude != 0 && company.Longitude != 0) { score += 20; passed.Add("Geocoded map coordinates (+20)"); }
            else { warnings.Add("Missing coordinates"); }

            if (company.TechStack.Count > 0) { score += 15; passed.Add("Tech stack documented (+15)"); }
            if (company.VerificationStatus == "VERIFIED") { score += 15; passed.Add("Verified entity (+15)"); }

            string rating = score >= 90 ? "High Quality" : score >= 70 ? "Good" : score >= 50 ? "Needs Review" : "Poor";

            return new DataQualityScore
            {
                OverallScore = score,
                Rating = rating,
                PassedSignals = passed,
                Warnings = warnings
            };
        }
    }

    public interface IIngestionPipelineService
    {
        Task<IngestionRun> RunMockDiscoveryIngestionAsync(string sourceId = "src-careers");
        Task<List<IngestionRun>> GetIngestionRunsAsync();
    }

    public class IngestionPipelineService : IIngestionPipelineService
    {
        private readonly ChennaiDbContext _db;
        private readonly INormalizationService _norm;
        private readonly ICompanyMatcher _matcher;

        public IngestionPipelineService(ChennaiDbContext db, INormalizationService norm, ICompanyMatcher matcher)
        {
            _db = db;
            _norm = norm;
            _matcher = matcher;
        }

        public async Task<IngestionRun> RunMockDiscoveryIngestionAsync(string sourceId = "src-careers")
        {
            var run = new IngestionRun
            {
                SourceId = sourceId,
                EntityType = "job",
                StartedAt = DateTime.UtcNow,
                Status = "RUNNING"
            };

            _db.IngestionRuns.Add(run);
            await _db.SaveChangesAsync();

            try
            {
                // Simulated Discovered Job Items
                var discoveredItems = new[]
                {
                    new { Title = "Full Stack .NET Developer (0-1 yrs)", Company = "Zoho Corporation", Url = "https://www.zoho.com/careers/job-dotnet.html", Location = "Estancia IT Park, OMR, Chennai" },
                    new { Title = "React & Node.js Engineer Intern", Company = "Freshworks", Url = "https://www.freshworks.com/careers/intern-react", Location = "Global Infocity, Perungudi, Chennai" },
                    new { Title = "Python AI / ML Engineer", Company = "Zoho Corporation", Url = "https://www.zoho.com/careers/job-ai-python.html", Location = "Estancia IT Park, OMR, Chennai" }
                };

                run.RecordsDiscovered = discoveredItems.Length;

                foreach (var item in discoveredItems)
                {
                    // 1. Raw Ingestion Record
                    var raw = new RawIngestionRecord
                    {
                        SourceName = sourceId,
                        ExternalId = $"ext-{item.Title.GetHashCode()}",
                        RawTitle = item.Title,
                        RawCompany = item.Company,
                        RawLocation = item.Location,
                        RawUrl = item.Url,
                        DiscoveredAt = DateTime.UtcNow,
                        Status = "PROCESSED"
                    };
                    _db.RawIngestionRecords.Add(raw);

                    // 2. Normalize
                    var normTitle = _norm.NormalizeTitle(item.Title);
                    var (matchedCompany, confidence) = await _matcher.MatchCompanyAsync(item.Company);
                    var companyId = matchedCompany?.Id ?? "comp-1";
                    var companyName = matchedCompany?.Name ?? item.Company;
                    var companyLogo = matchedCompany?.Logo ?? "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80";
                    var companyHub = matchedCompany?.Hub ?? "OMR (IT Corridor)";

                    // 3. Deduplication Check (Idempotency)
                    var existingJob = await _db.Jobs.FirstOrDefaultAsync(j => j.CompanyId == companyId && j.NormalizedTitle == normTitle);

                    if (existingJob != null)
                    {
                        existingJob.LastSeenAt = DateTime.UtcNow;
                        existingJob.LastVerifiedAt = DateTime.UtcNow;
                        existingJob.FreshnessStatus = "RECENTLY_VERIFIED";
                        run.RecordsUpdated++;
                        run.DuplicatesFound++;
                    }
                    else
                    {
                        var isFresher = _norm.DetectFresher(item.Title, item.Title);
                        var isIntern = _norm.DetectInternship(item.Title, item.Title);
                        var techs = _norm.ExtractTechnologies(item.Title);

                        var newJob = new Job
                        {
                            Id = $"job-ingest-{Guid.NewGuid():N}",
                            CompanyId = companyId,
                            CompanyName = companyName,
                            CompanyLogo = companyLogo,
                            CompanyHub = companyHub,
                            Title = item.Title,
                            NormalizedTitle = normTitle,
                            Slug = $"{companyName}-{item.Title}".ToLower().Replace(" ", "-").Replace("/", "-"),
                            DescriptionSnippet = $"Discovered open opportunity via {sourceId}. Minimum requirements include experience in {string.Join(", ", techs)}.",
                            PrimaryCategory = "SaaS / Enterprise Software",
                            IsEngineering = true,
                            EngineeringSubcategory = "Software Engineering",
                            Technologies = techs,
                            JobType = isIntern ? "Internship" : "Full-time",
                            WorkplaceType = "On-site",
                            ExperienceLevel = isFresher ? "Fresher / Entry (0-1 yrs)" : "Mid (3-5 yrs)",
                            IsFresher = isFresher,
                            FresherConfidence = isFresher ? 90 : 20,
                            IsInternship = isIntern,
                            SalaryRange = "Competitive Market Std",
                            Location = item.Location,
                            ChennaiRelevance = "CHENNAI_CONFIRMED",
                            RelevanceConfidence = 95,
                            SourceName = "Company Careers",
                            OriginalUrl = item.Url,
                            ApplyUrl = item.Url,
                            FirstSeenAt = DateTime.UtcNow,
                            LastSeenAt = DateTime.UtcNow,
                            LastVerifiedAt = DateTime.UtcNow,
                            FreshnessStatus = "NEW",
                            VerificationStatus = "VERIFIED",
                            IsFeatured = false,
                            IsActive = true,
                            IsSeedData = false
                        };

                        _db.Jobs.Add(newJob);
                        run.RecordsCreated++;
                    }
                }

                run.Status = "COMPLETED";
                run.CompletedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                run.Status = "FAILED";
                run.ErrorSummary = ex.Message;
                run.ErrorsCount++;
            }

            await _db.SaveChangesAsync();
            return run;
        }

        public async Task<List<IngestionRun>> GetIngestionRunsAsync()
        {
            return await _db.Set<IngestionRun>().AsNoTracking().OrderByDescending(r => r.StartedAt).Take(50).ToListAsync();
        }
    }
}
