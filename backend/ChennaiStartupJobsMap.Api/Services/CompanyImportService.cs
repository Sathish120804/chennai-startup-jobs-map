using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Data;
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

    public interface ICompanyImportService
    {
        Task<CompanyImportResult> SeedVerifiedDirectoryAsync();
        Task<CompanyImportResult> ImportCompaniesAsync(List<Company> companies, string sourceAttribution);
    }

    public class CompanyImportService : ICompanyImportService
    {
        private readonly ChennaiDbContext _db;
        private readonly INormalizationService _norm;

        public CompanyImportService(ChennaiDbContext db, INormalizationService norm)
        {
            _db = db;
            _norm = norm;
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

                // 1. Validation & normalization
                if (string.IsNullOrWhiteSpace(company.Name))
                {
                    result.Skipped++;
                    result.Warnings.Add("Company with missing name was skipped.");
                    continue;
                }

                company.NormalizedName = _norm.NormalizeTitle(company.Name);
                if (string.IsNullOrWhiteSpace(company.Slug))
                {
                    company.Slug = company.NormalizedName.Replace(" ", "-").Replace("/", "-").Replace(".", "");
                }

                // Check for existing company by Slug or Normalized Name
                var existing = await _db.Companies
                    .Include(c => c.Sources)
                    .Include(c => c.CareerSources)
                    .FirstOrDefaultAsync(c => c.Slug == company.Slug || c.NormalizedName == company.NormalizedName);

                if (existing != null)
                {
                    // Update metadata
                    existing.Description = !string.IsNullOrWhiteSpace(company.Description) ? company.Description : existing.Description;
                    existing.Website = !string.IsNullOrWhiteSpace(company.Website) ? company.Website : existing.Website;
                    existing.CareersUrl = !string.IsNullOrWhiteSpace(company.CareersUrl) ? company.CareersUrl : existing.CareersUrl;
                    existing.HiringStatus = company.HiringStatus ?? existing.HiringStatus;
                    existing.Hub = !string.IsNullOrWhiteSpace(company.Hub) ? company.Hub : existing.Hub;
                    existing.Address = !string.IsNullOrWhiteSpace(company.Address) ? company.Address : existing.Address;
                    existing.UpdatedAt = DateTime.UtcNow;

                    // Ensure CompanySource exists
                    if (!existing.Sources.Any(s => s.SourceUrl == (company.SourceUrl ?? company.Website)))
                    {
                        var src = new CompanySource
                        {
                            CompanyId = existing.Id,
                            SourceName = sourceAttribution,
                            SourceUrl = company.SourceUrl ?? company.Website,
                            SourceType = "OFFICIAL_WEBSITE",
                            Confidence = 95,
                            Status = "VERIFIED",
                            DiscoveredAt = DateTime.UtcNow,
                            VerifiedAt = DateTime.UtcNow
                        };
                        _db.CompanySources.Add(src);
                    }

                    // Ensure CareerSource exists if careersUrl is present
                    if (!string.IsNullOrWhiteSpace(existing.CareersUrl) && !existing.CareerSources.Any(cs => cs.CareersUrl == existing.CareersUrl))
                    {
                        var cs = new CareerSource
                        {
                            CompanyId = existing.Id,
                            CareersUrl = existing.CareersUrl,
                            Provider = DetermineAtsProvider(existing.CareersUrl),
                            SourceType = "OFFICIAL_CAREERS",
                            IsActive = true,
                            Status = "VERIFIED",
                            LastCheckedAt = DateTime.UtcNow
                        };
                        _db.CareerSources.Add(cs);
                    }

                    result.Updated++;
                }
                else
                {
                    // New Company
                    if (string.IsNullOrWhiteSpace(company.Id))
                    {
                        company.Id = $"comp-{Guid.NewGuid():N}";
                    }

                    _db.Companies.Add(company);

                    // Add CompanySource
                    var src = new CompanySource
                    {
                        CompanyId = company.Id,
                        SourceName = sourceAttribution,
                        SourceUrl = company.SourceUrl ?? company.Website,
                        SourceType = "OFFICIAL_WEBSITE",
                        Confidence = 95,
                        Status = "VERIFIED",
                        DiscoveredAt = DateTime.UtcNow,
                        VerifiedAt = DateTime.UtcNow
                    };
                    _db.CompanySources.Add(src);

                    // Add CareerSource if available
                    if (!string.IsNullOrWhiteSpace(company.CareersUrl))
                    {
                        var cs = new CareerSource
                        {
                            CompanyId = company.Id,
                            CareersUrl = company.CareersUrl,
                            Provider = DetermineAtsProvider(company.CareersUrl),
                            SourceType = "OFFICIAL_CAREERS",
                            IsActive = true,
                            Status = "VERIFIED",
                            LastCheckedAt = DateTime.UtcNow
                        };
                        _db.CareerSources.Add(cs);
                    }

                    result.NewlyCreated++;
                }
            }

            await _db.SaveChangesAsync();
            return result;
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
    }
}
