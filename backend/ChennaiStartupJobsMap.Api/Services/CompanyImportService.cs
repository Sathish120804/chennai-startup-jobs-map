using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Services
{
    public class CompanyImportResult
    {
        public int TotalProcessed { get; set; }
        public int NewlyCreated { get; set; }
        public int Updated { get; set; }
        public int Skipped { get; set; }
        public List<string> Warnings { get; set; } = new();
    }

    public interface IChennaiRelevanceEvaluator
    {
        (bool IsRelevant, int Score, string Reason) Evaluate(Company company);
        bool IsInChennaiGeoBounds(double lat, double lng);
    }

    public class ChennaiRelevanceEvaluator : IChennaiRelevanceEvaluator
    {
        // Greater Chennai Metropolitan Area approximate bounding box
        private const double MinLat = 12.60;
        private const double MaxLat = 13.45;
        private const double MinLng = 79.85;
        private const double MaxLng = 80.45;

        private static readonly string[] ChennaiKeywords = new[]
        {
            "chennai", "madras", "omr", "old mahabalipuram road", "tidel park", "ascendas", 
            "ramanujan it city", "guindy", "olympia tech park", "dlf cybercity", "porur", 
            "siruseri", "sipcot", "sholinganallur", "perungudi", "kandanchavadi", "taramani", 
            "tharamani", "ambattur", "sidco", "manapakkam", "ekkatuthangal", "navalur", 
            "thoraipakkam", "velachery", "nungambakkam", "anna salai", "mount road", 
            "alwarpet", "mepz", "iit madras", "iitm research park"
        };

        public bool IsInChennaiGeoBounds(double lat, double lng)
        {
            if (lat == 0 && lng == 0) return false;
            return lat >= MinLat && lat <= MaxLat && lng >= MinLng && lng <= MaxLng;
        }

        public (bool IsRelevant, int Score, string Reason) Evaluate(Company company)
        {
            int score = 0;
            var reasons = new List<string>();

            // 1. Explicit City check
            if (!string.IsNullOrWhiteSpace(company.City) && company.City.Equals("Chennai", StringComparison.OrdinalIgnoreCase))
            {
                score += 40;
                reasons.Add("Official City is Chennai (+40)");
            }

            // 2. Chennai Tech Corridor / Hub check
            if (!string.IsNullOrWhiteSpace(company.Hub) && !company.Hub.Equals("Other", StringComparison.OrdinalIgnoreCase))
            {
                score += 30;
                reasons.Add($"Assigned to Chennai tech corridor: {company.Hub} (+30)");
            }

            // 3. Geocoordinates validation
            if (IsInChennaiGeoBounds(company.Latitude, company.Longitude))
            {
                score += 25;
                reasons.Add("Geocoordinates within Chennai Metro bounds (+25)");
            }

            // 4. Address & Presence keyword audit
            var textToCheck = $"{company.Address} {company.Headquarters} {company.ChennaiPresence} {company.Description}".ToLower();
            int keywordHits = ChennaiKeywords.Count(k => textToCheck.Contains(k));
            if (keywordHits > 0)
            {
                int kwScore = Math.Min(25, keywordHits * 10);
                score += kwScore;
                reasons.Add($"Text contains Chennai ecosystem markers ({keywordHits} match) (+{kwScore})");
            }

            // Cap at 100
            score = Math.Min(100, score);

            // Minimum threshold: 50
            bool isRelevant = score >= 50;
            string summaryReason = string.Join("; ", reasons);
            if (!isRelevant)
            {
                summaryReason = "Insufficient verifiable Chennai presence markers (score below 50).";
            }

            return (isRelevant, score, summaryReason);
        }
    }

    public interface ICompanyDeduplicationService
    {
        Task<(bool IsDuplicate, Company? MatchedCompany, string MatchReason, int Confidence)> CheckDuplicateAsync(Company incoming);
        string ExtractRootDomain(string? url);
        string CleanLegalSuffixes(string companyName);
    }

    public class CompanyDeduplicationService : ICompanyDeduplicationService
    {
        private readonly ChennaiDbContext _db;

        private static readonly string[] LegalSuffixes = new[]
        {
            "pvt ltd", "private limited", "ltd", "limited", "inc", "incorporated", 
            "corp", "corporation", "llc", "llp", "technologies", "technology", 
            "solutions", "software", "services", "india", "labs"
        };

        public CompanyDeduplicationService(ChennaiDbContext db)
        {
            _db = db;
        }

        public string ExtractRootDomain(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return string.Empty;
            try
            {
                var normalized = url.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? url : "https://" + url;
                var uri = new Uri(normalized);
                var host = uri.Host.ToLower().Trim();
                if (host.StartsWith("www.")) host = host[4..];

                // Handle common ATS and subdomains (e.g. jobs.lever.co/zoho -> extract root or specific path)
                var parts = host.Split('.');
                if (parts.Length >= 2)
                {
                    return string.Join('.', parts.TakeLast(2));
                }
                return host;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string CleanLegalSuffixes(string companyName)
        {
            if (string.IsNullOrWhiteSpace(companyName)) return string.Empty;
            var clean = companyName.ToLower().Trim();

            // Replace punctuation with spaces
            clean = Regex.Replace(clean, @"[^\w\s]", " ");

            foreach (var suffix in LegalSuffixes)
            {
                clean = Regex.Replace(clean, $@"\b{Regex.Escape(suffix)}\b", " ", RegexOptions.IgnoreCase);
            }

            return Regex.Replace(clean, @"\s+", " ").Trim();
        }

        public async Task<(bool IsDuplicate, Company? MatchedCompany, string MatchReason, int Confidence)> CheckDuplicateAsync(Company incoming)
        {
            var cleanedIncoming = CleanLegalSuffixes(incoming.Name);
            var incomingDomain = ExtractRootDomain(incoming.Website);

            var existingCompanies = await _db.Companies.AsNoTracking().ToListAsync();

            foreach (var existing in existingCompanies)
            {
                // 1. Slug or ID match
                if (!string.IsNullOrWhiteSpace(incoming.Slug) && incoming.Slug.Equals(existing.Slug, StringComparison.OrdinalIgnoreCase))
                {
                    return (true, existing, "Exact Slug Match", 100);
                }

                // 2. Exact name match
                if (incoming.Name.Trim().Equals(existing.Name.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return (true, existing, "Exact Name Match", 98);
                }

                // 3. Domain match
                if (!string.IsNullOrEmpty(incomingDomain))
                {
                    var existingDomain = ExtractRootDomain(existing.Website);
                    if (!string.IsNullOrEmpty(existingDomain) && incomingDomain.Equals(existingDomain, StringComparison.OrdinalIgnoreCase))
                    {
                        return (true, existing, $"Matching Official Root Domain ({incomingDomain})", 95);
                    }
                }

                // 4. Normalized cleaned name match
                var cleanedExisting = CleanLegalSuffixes(existing.Name);
                if (!string.IsNullOrEmpty(cleanedIncoming) && !string.IsNullOrEmpty(cleanedExisting) && cleanedIncoming == cleanedExisting)
                {
                    return (true, existing, $"Normalized Corporate Alias Match ({cleanedExisting})", 90);
                }
            }

            return (false, null, "No duplicate found", 0);
        }
    }

    public interface ICompanyImportService
    {
        Task<CompanyImportResult> SeedVerifiedDirectoryAsync();
        Task<CompanyImportResult> ImportCompaniesAsync(List<Company> companies, string sourceAttribution);
        Task<CompanyImportReportDto> ImportCsvAsync(string csvContent, string sourceName, bool dryRun = false);
        Task<CompanyImportReportDto> ImportJsonAsync(string jsonContent, string sourceName, bool dryRun = false);
        Task<DataQualityDashboardDto> GetDataQualityMetricsAsync();
        Task<bool> MergeCompaniesAsync(CompanyMergeRequestDto request);
        Task<bool> VerifyCompanyAsync(string companyId, string verificationMethod = "ADMIN_MANUAL");
        Task<bool> RejectCompanyAsync(string companyId, string reason = "Unverified");
    }

    public class CompanyImportService : ICompanyImportService
    {
        private readonly ChennaiDbContext _db;
        private readonly INormalizationService _norm;
        private readonly IChennaiRelevanceEvaluator _relevanceEvaluator;
        private readonly ICompanyDeduplicationService _deduplicationService;

        public CompanyImportService(
            ChennaiDbContext db, 
            INormalizationService norm,
            IChennaiRelevanceEvaluator relevanceEvaluator,
            ICompanyDeduplicationService deduplicationService)
        {
            _db = db;
            _norm = norm;
            _relevanceEvaluator = relevanceEvaluator;
            _deduplicationService = deduplicationService;
        }

        public async Task<CompanyImportResult> SeedVerifiedDirectoryAsync()
        {
            var directory = CompanyDirectoryData.GetVerifiedChennaiCompanies();
            return await ImportCompaniesAsync(directory, "Verified Chennai Directory Seed");
        }

        public async Task<CompanyImportResult> ImportCompaniesAsync(List<Company> incoming, string sourceAttribution)
        {
            var result = new CompanyImportResult();

            foreach (var company in incoming)
            {
                result.TotalProcessed++;

                if (string.IsNullOrWhiteSpace(company.Name))
                {
                    result.Skipped++;
                    result.Warnings.Add("Company with missing name was skipped.");
                    continue;
                }

                // Normalization
                company.NormalizedName = _deduplicationService.CleanLegalSuffixes(company.Name);
                if (string.IsNullOrWhiteSpace(company.Slug))
                {
                    company.Slug = GenerateSlug(company.Name);
                }

                // Validate Chennai Relevance
                var (isRelevant, relevanceScore, relevanceReason) = _relevanceEvaluator.Evaluate(company);
                company.ChennaiRelevanceScore = relevanceScore;
                if (!isRelevant && company.VerificationStatus == "VERIFIED")
                {
                    company.VerificationStatus = "UNVERIFIED";
                    result.Warnings.Add($"Company '{company.Name}' marked UNVERIFIED: {relevanceReason}");
                }

                // Deduplication Check
                var (isDup, matched, matchReason, conf) = await _deduplicationService.CheckDuplicateAsync(company);

                if (isDup && matched != null)
                {
                    // Update existing record
                    var existing = await _db.Companies
                        .Include(c => c.Sources)
                        .Include(c => c.CareerSources)
                        .FirstOrDefaultAsync(c => c.Id == matched.Id);

                    if (existing != null)
                    {
                        if (!string.IsNullOrWhiteSpace(company.Description)) existing.Description = company.Description;
                        if (!string.IsNullOrWhiteSpace(company.Website)) existing.Website = company.Website;
                        if (!string.IsNullOrWhiteSpace(company.CareersUrl)) existing.CareersUrl = company.CareersUrl;
                        if (!string.IsNullOrWhiteSpace(company.Hub)) existing.Hub = company.Hub;
                        if (!string.IsNullOrWhiteSpace(company.Address)) existing.Address = company.Address;
                        if (company.Latitude != 0 && company.Longitude != 0)
                        {
                            existing.Latitude = company.Latitude;
                            existing.Longitude = company.Longitude;
                        }
                        if (company.TechStack.Count > 0)
                        {
                            existing.TechStack = existing.TechStack.Union(company.TechStack).Distinct().ToList();
                        }
                        if (company.CompanyTypes.Count > 0)
                        {
                            existing.CompanyTypes = existing.CompanyTypes.Union(company.CompanyTypes).Distinct().ToList();
                        }
                        existing.LastVerifiedAt = DateTime.UtcNow;
                        existing.UpdatedAt = DateTime.UtcNow;

                        // Ensure Source recorded
                        if (!existing.Sources.Any(s => s.SourceUrl == (company.SourceUrl ?? company.Website)))
                        {
                            _db.CompanySources.Add(new CompanySource
                            {
                                CompanyId = existing.Id,
                                SourceName = sourceAttribution,
                                SourceUrl = company.SourceUrl ?? company.Website ?? string.Empty,
                                SourceType = company.SourceType ?? "OFFICIAL_WEBSITE",
                                Confidence = 95,
                                Status = existing.VerificationStatus,
                                DiscoveredAt = DateTime.UtcNow,
                                VerifiedAt = DateTime.UtcNow
                            });
                        }

                        // Ensure CareerSource recorded
                        if (!string.IsNullOrWhiteSpace(existing.CareersUrl) && !existing.CareerSources.Any(cs => cs.CareersUrl == existing.CareersUrl))
                        {
                            _db.CareerSources.Add(new CareerSource
                            {
                                CompanyId = existing.Id,
                                CareersUrl = existing.CareersUrl,
                                Provider = DetermineAtsProvider(existing.CareersUrl),
                                SourceType = "OFFICIAL_CAREERS",
                                IsActive = true,
                                Status = "VERIFIED",
                                LastCheckedAt = DateTime.UtcNow
                            });
                        }

                        result.Updated++;
                    }
                }
                else
                {
                    // New Company
                    if (string.IsNullOrWhiteSpace(company.Id))
                    {
                        company.Id = $"comp-{Guid.NewGuid():N}";
                    }

                    _db.Companies.Add(company);

                    _db.CompanySources.Add(new CompanySource
                    {
                        CompanyId = company.Id,
                        SourceName = sourceAttribution,
                        SourceUrl = company.SourceUrl ?? company.Website ?? string.Empty,
                        SourceType = company.SourceType ?? "OFFICIAL_WEBSITE",
                        Confidence = 95,
                        Status = company.VerificationStatus,
                        DiscoveredAt = DateTime.UtcNow,
                        VerifiedAt = DateTime.UtcNow
                    });

                    if (!string.IsNullOrWhiteSpace(company.CareersUrl))
                    {
                        _db.CareerSources.Add(new CareerSource
                        {
                            CompanyId = company.Id,
                            CareersUrl = company.CareersUrl,
                            Provider = DetermineAtsProvider(company.CareersUrl),
                            SourceType = "OFFICIAL_CAREERS",
                            IsActive = true,
                            Status = company.VerificationStatus,
                            LastCheckedAt = DateTime.UtcNow
                        });
                    }

                    result.NewlyCreated++;
                }
            }

            await _db.SaveChangesAsync();
            return result;
        }

        public async Task<CompanyImportReportDto> ImportCsvAsync(string csvContent, string sourceName, bool dryRun = false)
        {
            var report = new CompanyImportReportDto();
            if (string.IsNullOrWhiteSpace(csvContent))
            {
                report.Errors.Add("CSV content is empty.");
                return report;
            }

            var rows = ParseCsv(csvContent);
            if (rows.Count <= 1)
            {
                report.Errors.Add("CSV contains no data rows.");
                return report;
            }

            var headers = rows[0].Select(h => h.Trim().ToLower()).ToList();
            report.TotalRows = rows.Count - 1;

            var parsedCompanies = new List<Company>();

            for (int r = 1; r < rows.Count; r++)
            {
                var row = rows[r];
                if (row.All(string.IsNullOrWhiteSpace)) continue;

                var comp = new Company();
                bool rowValid = true;

                for (int c = 0; c < row.Count && c < headers.Count; c++)
                {
                    var col = headers[c];
                    var val = row[c].Trim();

                    switch (col)
                    {
                        case "name":
                        case "company":
                        case "companyname":
                            comp.Name = val;
                            break;
                        case "description":
                        case "desc":
                            comp.Description = val;
                            break;
                        case "tagline":
                        case "shortdescription":
                            comp.Tagline = val;
                            break;
                        case "website":
                        case "officialwebsite":
                        case "url":
                            if (IsValidHttpUrl(val)) comp.Website = val;
                            else if (!string.IsNullOrEmpty(val)) report.Warnings.Add($"Row {r}: Invalid website URL '{val}'");
                            break;
                        case "careersurl":
                        case "officialcareersurl":
                        case "jobsurl":
                            if (IsValidHttpUrl(val)) comp.CareersUrl = val;
                            else comp.CareersUrl = string.Empty; // STRICT: never fabricate careers URL
                            break;
                        case "category":
                        case "sector":
                            if (!string.IsNullOrEmpty(val)) comp.Categories = val.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                            break;
                        case "subcategory":
                            comp.SubCategory = val;
                            break;
                        case "industry":
                            comp.Industry = val;
                            break;
                        case "companytype":
                        case "companytypes":
                        case "type":
                            if (!string.IsNullOrEmpty(val)) comp.CompanyTypes = val.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim().ToUpper()).ToList();
                            break;
                        case "hub":
                        case "corridor":
                            comp.Hub = val;
                            break;
                        case "address":
                            comp.Address = val;
                            break;
                        case "city":
                            comp.City = string.IsNullOrEmpty(val) ? "Chennai" : val;
                            break;
                        case "state":
                            comp.State = string.IsNullOrEmpty(val) ? "Tamil Nadu" : val;
                            break;
                        case "country":
                            comp.Country = string.IsNullOrEmpty(val) ? "India" : val;
                            break;
                        case "latitude":
                        case "lat":
                            if (double.TryParse(val, out double lat)) comp.Latitude = lat;
                            break;
                        case "longitude":
                        case "lng":
                        case "lon":
                            if (double.TryParse(val, out double lng)) comp.Longitude = lng;
                            break;
                        case "employeecount":
                        case "employeerange":
                        case "size":
                            comp.EmployeeCount = val;
                            break;
                        case "foundedyear":
                        case "founded":
                            if (int.TryParse(val, out int yr)) comp.FoundedYear = yr;
                            break;
                        case "fundingstage":
                            comp.FundingStage = val;
                            break;
                        case "hiringstatus":
                            comp.HiringStatus = val;
                            break;
                        case "tags":
                        case "technologytags":
                            if (!string.IsNullOrEmpty(val)) comp.Tags = val.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                            break;
                        case "techstack":
                        case "skills":
                            if (!string.IsNullOrEmpty(val)) comp.TechStack = val.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()).ToList();
                            break;
                        case "sourcetype":
                            comp.SourceType = val;
                            break;
                        case "sourceurl":
                            comp.SourceUrl = val;
                            break;
                    }
                }

                if (string.IsNullOrWhiteSpace(comp.Name))
                {
                    report.InvalidRows++;
                    report.Errors.Add($"Row {r}: Company name is required.");
                    rowValid = false;
                }

                if (rowValid)
                {
                    report.ValidRows++;
                    comp.NormalizedName = _deduplicationService.CleanLegalSuffixes(comp.Name);
                    comp.Slug = GenerateSlug(comp.Name);
                    comp.SourceName = sourceName;
                    comp.DiscoveredAt = DateTime.UtcNow;
                    comp.LastVerifiedAt = DateTime.UtcNow;

                    if (comp.CompanyTypes.Count == 0) comp.CompanyTypes.Add("STARTUP");
                    if (comp.Categories.Count == 0) comp.Categories.Add("Technology");

                    parsedCompanies.Add(comp);
                }
            }

            // Process parsed companies through deduplication and relevance checks
            foreach (var comp in parsedCompanies)
            {
                var (isRelevant, relevanceScore, relevanceReason) = _relevanceEvaluator.Evaluate(comp);
                comp.ChennaiRelevanceScore = relevanceScore;

                if (!isRelevant)
                {
                    report.RejectedCompanies++;
                    report.Warnings.Add($"Company '{comp.Name}' rejected: {relevanceReason}");
                    continue;
                }

                var (isDup, matched, matchReason, conf) = await _deduplicationService.CheckDuplicateAsync(comp);
                if (isDup)
                {
                    report.Duplicates++;
                    report.UpdatedCompanies++;
                }
                else
                {
                    report.NewCompanies++;
                }

                if (report.SamplePreview.Count < 5)
                {
                    report.SamplePreview.Add(new CompanyDto
                    {
                        Name = comp.Name,
                        Slug = comp.Slug,
                        Website = comp.Website,
                        CareersUrl = comp.CareersUrl,
                        Hub = comp.Hub,
                        Category = comp.Category,
                        CompanyType = comp.CompanyType,
                        VerificationStatus = comp.VerificationStatus,
                        ConfidenceScore = comp.ConfidenceScore,
                        ChennaiRelevanceScore = comp.ChennaiRelevanceScore
                    });
                }
            }

            if (!dryRun && parsedCompanies.Count > 0)
            {
                var validToImport = parsedCompanies.Where(c => c.ChennaiRelevanceScore >= 50).ToList();
                await ImportCompaniesAsync(validToImport, sourceName);
            }

            return report;
        }

        public async Task<CompanyImportReportDto> ImportJsonAsync(string jsonContent, string sourceName, bool dryRun = false)
        {
            var report = new CompanyImportReportDto();
            if (string.IsNullOrWhiteSpace(jsonContent))
            {
                report.Errors.Add("JSON content is empty.");
                return report;
            }

            List<Company>? incoming;
            try
            {
                incoming = JsonConvert.DeserializeObject<List<Company>>(jsonContent);
            }
            catch (Exception ex)
            {
                report.Errors.Add($"Invalid JSON payload: {ex.Message}");
                return report;
            }

            if (incoming == null || incoming.Count == 0)
            {
                report.Errors.Add("JSON payload contained no companies.");
                return report;
            }

            report.TotalRows = incoming.Count;
            var validCompanies = new List<Company>();

            foreach (var comp in incoming)
            {
                if (string.IsNullOrWhiteSpace(comp.Name))
                {
                    report.InvalidRows++;
                    report.Errors.Add("Company missing required 'Name' field.");
                    continue;
                }

                report.ValidRows++;
                comp.NormalizedName = _deduplicationService.CleanLegalSuffixes(comp.Name);
                if (string.IsNullOrWhiteSpace(comp.Slug)) comp.Slug = GenerateSlug(comp.Name);
                comp.SourceName = sourceName;
                comp.LastVerifiedAt = DateTime.UtcNow;

                // Validate Careers URL
                if (!IsValidHttpUrl(comp.CareersUrl))
                {
                    comp.CareersUrl = string.Empty;
                }

                var (isRelevant, relevanceScore, relevanceReason) = _relevanceEvaluator.Evaluate(comp);
                comp.ChennaiRelevanceScore = relevanceScore;

                if (!isRelevant)
                {
                    report.RejectedCompanies++;
                    report.Warnings.Add($"Company '{comp.Name}' rejected: {relevanceReason}");
                    continue;
                }

                var (isDup, matched, matchReason, conf) = await _deduplicationService.CheckDuplicateAsync(comp);
                if (isDup)
                {
                    report.Duplicates++;
                    report.UpdatedCompanies++;
                }
                else
                {
                    report.NewCompanies++;
                }

                if (report.SamplePreview.Count < 5)
                {
                    report.SamplePreview.Add(new CompanyDto
                    {
                        Name = comp.Name,
                        Slug = comp.Slug,
                        Website = comp.Website,
                        CareersUrl = comp.CareersUrl,
                        Hub = comp.Hub,
                        Category = comp.Category,
                        CompanyType = comp.CompanyType,
                        VerificationStatus = comp.VerificationStatus,
                        ConfidenceScore = comp.ConfidenceScore,
                        ChennaiRelevanceScore = comp.ChennaiRelevanceScore
                    });
                }

                validCompanies.Add(comp);
            }

            if (!dryRun && validCompanies.Count > 0)
            {
                await ImportCompaniesAsync(validCompanies, sourceName);
            }

            return report;
        }

        public async Task<DataQualityDashboardDto> GetDataQualityMetricsAsync()
        {
            var companies = await _db.Companies.AsNoTracking().ToListAsync();

            var verifiedCount = companies.Count(c => c.VerificationStatus == "VERIFIED" || c.VerificationStatus == "ADMIN_VERIFIED");
            var sourceBacked = companies.Count(c => c.VerificationStatus == "SOURCE_BACKED");
            var userSubmitted = companies.Count(c => c.VerificationStatus == "USER_SUBMITTED");
            var adminVerified = companies.Count(c => c.VerificationStatus == "ADMIN_VERIFIED");
            var unverified = companies.Count(c => c.VerificationStatus == "UNVERIFIED");
            var pendingReview = companies.Count(c => c.VerificationStatus == "PENDING_REVIEW");
            var stale = companies.Count(c => c.VerificationStatus == "STALE" || c.LastVerifiedAt < DateTime.UtcNow.AddDays(-180));

            var missingWebsite = companies.Count(c => string.IsNullOrWhiteSpace(c.Website));
            var missingCareers = companies.Count(c => string.IsNullOrWhiteSpace(c.CareersUrl));
            var missingCoords = companies.Count(c => c.Latitude == 0 && c.Longitude == 0);

            var byCategory = new Dictionary<string, int>();
            foreach (var c in companies)
            {
                var cat = c.Category;
                if (!byCategory.ContainsKey(cat)) byCategory[cat] = 0;
                byCategory[cat]++;
            }

            var byType = new Dictionary<string, int>();
            foreach (var c in companies)
            {
                foreach (var t in c.CompanyTypes)
                {
                    var typeKey = t.ToUpperInvariant();
                    if (!byType.ContainsKey(typeKey)) byType[typeKey] = 0;
                    byType[typeKey]++;
                }
            }

            var bySource = new Dictionary<string, int>();
            foreach (var c in companies)
            {
                var src = !string.IsNullOrWhiteSpace(c.SourceName) ? c.SourceName : "Unknown Source";
                if (!bySource.ContainsKey(src)) bySource[src] = 0;
                bySource[src]++;
            }

            return new DataQualityDashboardDto
            {
                TargetCompanyGoal = 700,
                CurrentVerifiedCount = verifiedCount,
                TotalCompanies = companies.Count,
                VerifiedCompanies = verifiedCount,
                SourceBackedCompanies = sourceBacked,
                UserSubmittedCompanies = userSubmitted,
                AdminVerifiedCompanies = adminVerified,
                UnverifiedCompanies = unverified,
                PendingReviewCompanies = pendingReview,
                StaleCompanies = stale,
                DuplicatesIdentified = 0,
                MissingWebsiteCount = missingWebsite,
                MissingCareersUrlCount = missingCareers,
                MissingCoordinatesCount = missingCoords,
                CompaniesByCategory = byCategory,
                CompaniesByCompanyType = byType,
                CompaniesBySource = bySource
            };
        }

        public async Task<bool> MergeCompaniesAsync(CompanyMergeRequestDto request)
        {
            var primary = await _db.Companies
                .Include(c => c.Sources)
                .Include(c => c.CareerSources)
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Id == request.PrimaryCompanyId);

            var duplicate = await _db.Companies
                .Include(c => c.Sources)
                .Include(c => c.CareerSources)
                .Include(c => c.Jobs)
                .FirstOrDefaultAsync(c => c.Id == request.DuplicateCompanyId);

            if (primary == null || duplicate == null) return false;

            // Re-point jobs to primary company
            foreach (var job in duplicate.Jobs)
            {
                job.CompanyId = primary.Id;
                job.CompanyName = primary.Name;
            }

            // Merge sources
            foreach (var src in duplicate.Sources)
            {
                src.CompanyId = primary.Id;
            }

            // Merge career sources
            foreach (var cs in duplicate.CareerSources)
            {
                cs.CompanyId = primary.Id;
            }

            // Merge tech stack and tags
            primary.TechStack = primary.TechStack.Union(duplicate.TechStack).Distinct().ToList();
            primary.Tags = primary.Tags.Union(duplicate.Tags).Distinct().ToList();
            primary.CompanyTypes = primary.CompanyTypes.Union(duplicate.CompanyTypes).Distinct().ToList();
            primary.Categories = primary.Categories.Union(duplicate.Categories).Distinct().ToList();

            // Archive or remove duplicate
            _db.Companies.Remove(duplicate);

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> VerifyCompanyAsync(string companyId, string verificationMethod = "ADMIN_MANUAL")
        {
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            if (company == null) return false;

            company.VerificationStatus = "VERIFIED";
            company.VerificationMethod = verificationMethod;
            company.ConfidenceScore = 100;
            company.LastVerifiedAt = DateTime.UtcNow;
            company.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectCompanyAsync(string companyId, string reason = "Unverified")
        {
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            if (company == null) return false;

            company.VerificationStatus = "REJECTED";
            company.IsActive = false;
            company.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        private static string GenerateSlug(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Guid.NewGuid().ToString("N");
            var clean = Regex.Replace(name.ToLower().Trim(), @"[^a-z0-9\s-]", "");
            return Regex.Replace(clean, @"[\s-]+", "-").Trim('-');
        }

        private static bool IsValidHttpUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url)) return false;
            if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && 
                !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) return false;
            return Uri.TryCreate(url, UriKind.Absolute, out _);
        }

        private static string DetermineAtsProvider(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return "Company Careers";
            var u = url.ToLower();
            if (u.Contains("greenhouse.io")) return "Greenhouse";
            if (u.Contains("lever.co")) return "Lever";
            if (u.Contains("myworkdayjobs.com") || u.Contains("workday")) return "Workday";
            if (u.Contains("smartrecruiters.com")) return "SmartRecruiters";
            if (u.Contains("ashbyhq.com")) return "Ashby";
            return "Company Careers Portal";
        }

        // RFC 4180 compliant CSV parser supporting multiline quoted cells and commas
        private static List<List<string>> ParseCsv(string text)
        {
            var records = new List<List<string>>();
            var currentRecord = new List<string>();
            var currentField = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];

                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (i + 1 < text.Length && text[i + 1] == '"')
                        {
                            currentField.Append('"');
                            i++; // skip escaped quote
                        }
                        else
                        {
                            inQuotes = false;
                        }
                    }
                    else
                    {
                        currentField.Append(c);
                    }
                }
                else
                {
                    if (c == '"')
                    {
                        inQuotes = true;
                    }
                    else if (c == ',')
                    {
                        currentRecord.Add(currentField.ToString());
                        currentField.Clear();
                    }
                    else if (c == '\r')
                    {
                        // Check for CRLF
                        if (i + 1 < text.Length && text[i + 1] == '\n') i++;
                        currentRecord.Add(currentField.ToString());
                        currentField.Clear();
                        records.Add(currentRecord);
                        currentRecord = new List<string>();
                    }
                    else if (c == '\n')
                    {
                        currentRecord.Add(currentField.ToString());
                        currentField.Clear();
                        records.Add(currentRecord);
                        currentRecord = new List<string>();
                    }
                    else
                    {
                        currentField.Append(c);
                    }
                }
            }

            if (currentField.Length > 0 || currentRecord.Count > 0)
            {
                currentRecord.Add(currentField.ToString());
                records.Add(currentRecord);
            }

            return records;
        }
    }
}

