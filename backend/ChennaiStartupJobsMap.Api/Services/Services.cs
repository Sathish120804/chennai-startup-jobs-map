using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Services
{
    public interface ICompanyService
    {
        Task<PagedResultDto<CompanyDto>> GetCompaniesAsync(
            string? searchQuery = null,
            List<string>? hubs = null,
            List<string>? categories = null,
            List<string>? types = null,
            bool? isHiringOnly = null,
            bool? isFresherOnly = null,
            List<string>? technologies = null,
            string sortBy = "featured",
            int page = 1,
            int pageSize = 20,
            bool? isMnc = null,
            bool? isGcc = null,
            bool? isStartup = null,
            bool? isProductCompany = null,
            string? industry = null
        );

        Task<CompanyDto?> GetCompanyByIdAsync(string id);
        Task<CompanyDto?> GetCompanyBySlugAsync(string slug);
    }

    public class CompanyService : ICompanyService
    {
        private readonly ChennaiDbContext _db;

        public CompanyService(ChennaiDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResultDto<CompanyDto>> GetCompaniesAsync(
            string? searchQuery = null,
            List<string>? hubs = null,
            List<string>? categories = null,
            List<string>? types = null,
            bool? isHiringOnly = null,
            bool? isFresherOnly = null,
            List<string>? technologies = null,
            string sortBy = "featured",
            int page = 1,
            int pageSize = 20,
            bool? isMnc = null,
            bool? isGcc = null,
            bool? isStartup = null,
            bool? isProductCompany = null,
            string? industry = null)
        {
            var query = _db.Companies.AsNoTracking().Where(c => c.IsActive);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var q = searchQuery.Trim().ToLower();
                query = query.Where(c => 
                    c.Name.ToLower().Contains(q) ||
                    c.NormalizedName.Contains(q) ||
                    c.Tagline.ToLower().Contains(q) ||
                    c.Description.ToLower().Contains(q) ||
                    c.Hub.ToLower().Contains(q)
                );
            }

            if (hubs != null && hubs.Count > 0)
            {
                query = query.Where(c => hubs.Contains(c.Hub));
            }

            var companies = await query.ToListAsync();

            // Calculate Company DTOs with Stats
            var companyDtos = new List<CompanyDto>();
            foreach (var c in companies)
            {
                var companyJobs = await _db.Jobs.AsNoTracking().Where(j => j.CompanyId == c.Id && j.IsActive).ToListAsync();
                var activeJobsCount = companyJobs.Count;
                var fresherJobsCount = companyJobs.Count(j => j.IsFresher);
                var engJobsCount = companyJobs.Count(j => j.IsEngineering);
                var internCount = companyJobs.Count(j => j.IsInternship);

                // Category filter
                if (categories != null && categories.Count > 0 && !c.Categories.Any(cat => categories.Contains(cat)))
                {
                    continue;
                }

                // Type filter
                if (types != null && types.Count > 0 && !c.CompanyTypes.Any(t => types.Contains(t)))
                {
                    continue;
                }

                // Classification filters
                if (isMnc == true && !c.IsMNC) continue;
                if (isGcc == true && !c.IsGCC) continue;
                if (isStartup == true && !c.IsStartup) continue;
                if (isProductCompany == true && !c.IsProductCompany) continue;
                if (!string.IsNullOrWhiteSpace(industry) && !c.Industry.Equals(industry, StringComparison.OrdinalIgnoreCase)) continue;

                // Hiring filter
                if (isHiringOnly == true && activeJobsCount == 0 && !c.IsHiring)
                {
                    continue;
                }

                // Fresher filter
                if (isFresherOnly == true && fresherJobsCount == 0)
                {
                    continue;
                }

                // Technology filter
                if (technologies != null && technologies.Count > 0 && !technologies.Any(t => c.TechStack.Contains(t) || companyJobs.Any(j => j.Technologies.Contains(t))))
                {
                    continue;
                }

                companyDtos.Add(MapToDto(c, activeJobsCount, engJobsCount, fresherJobsCount, internCount));
            }

            // Sorting
            companyDtos = sortBy switch
            {
                "name" => companyDtos.OrderBy(c => c.Name).ToList(),
                "foundedYear" => companyDtos.OrderByDescending(c => c.FoundedYear).ToList(),
                "jobsCount" => companyDtos.OrderByDescending(c => c.Stats.ActiveJobsCount).ToList(),
                "recent" => companyDtos.OrderByDescending(c => c.LastVerifiedAt).ToList(),
                _ => companyDtos.OrderByDescending(c => c.IsFeatured).ThenByDescending(c => c.Stats.ActiveJobsCount).ToList(),
            };

            var total = companyDtos.Count;
            var items = companyDtos
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResultDto<CompanyDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                Total = total
            };
        }

        public async Task<CompanyDto?> GetCompanyByIdAsync(string id)
        {
            var company = await _db.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (company == null) return null;
            var jobs = await _db.Jobs.AsNoTracking().Where(j => j.CompanyId == id).ToListAsync();
            return MapToDto(company, jobs.Count, jobs.Count(j => j.IsEngineering), jobs.Count(j => j.IsFresher), jobs.Count(j => j.IsInternship));
        }

        public async Task<CompanyDto?> GetCompanyBySlugAsync(string slug)
        {
            var company = await _db.Companies.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug);
            if (company == null) return null;
            var jobs = await _db.Jobs.AsNoTracking().Where(j => j.CompanyId == company.Id).ToListAsync();
            return MapToDto(company, jobs.Count, jobs.Count(j => j.IsEngineering), jobs.Count(j => j.IsFresher), jobs.Count(j => j.IsInternship));
        }

        private static CompanyDto MapToDto(Company c, int activeJobs, int engJobs, int fresherJobs, int internJobs)
        {
            return new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                Slug = c.Slug,
                Tagline = c.Tagline,
                Description = c.Description,
                ShortDescription = c.ShortDescription,
                Logo = c.Logo,
                LogoUrl = c.LogoUrl,
                Website = c.Website,
                OfficialWebsite = c.OfficialWebsite,
                CareersUrl = c.CareersUrl,
                OfficialCareersUrl = c.OfficialCareersUrl,
                CompanyTypes = c.CompanyTypes,
                Categories = c.Categories,
                Category = c.Category,
                SubCategory = c.SubCategory,
                Industry = c.Industry,
                CompanyType = c.CompanyType,
                Hub = c.Hub,
                Address = c.Address,
                City = c.City,
                State = c.State,
                Country = c.Country,
                Headquarters = c.Headquarters,
                ChennaiPresence = c.ChennaiPresence,
                ChennaiLocations = c.ChennaiLocations,
                Coordinates = new CoordinatesDto { Lat = c.Latitude, Lng = c.Longitude },
                MapPrecision = c.MapPrecision,
                FoundedYear = c.FoundedYear,
                EmployeeCount = c.EmployeeCount,
                EmployeeRange = c.EmployeeRange,
                FundingStage = c.FundingStage,
                TotalFundingRaised = c.TotalFundingRaised,
                HiringStatus = c.HiringStatus,
                IsHiring = c.IsHiring || activeJobs > 0,
                IsStartup = c.IsStartup,
                IsMNC = c.IsMNC,
                IsGCC = c.IsGCC,
                IsProductCompany = c.IsProductCompany,
                IsITServices = c.IsITServices,
                Tags = c.Tags,
                TechnologyTags = c.TechnologyTags,
                TechStack = c.TechStack,
                Skills = c.Skills,
                VerificationStatus = c.VerificationStatus,
                VerificationMethod = c.VerificationMethod,
                ConfidenceScore = c.ConfidenceScore,
                ChennaiRelevanceScore = c.ChennaiRelevanceScore,
                IsFeatured = c.IsFeatured,
                IsSeedData = c.IsSeedData,
                SourceType = c.SourceType,
                SourceName = c.SourceName,
                SourceUrl = c.SourceUrl,
                DiscoveredAt = c.DiscoveredAt,
                LastVerifiedAt = c.LastVerifiedAt,
                Stats = new CompanyJobStatsDto
                {
                    ActiveJobsCount = activeJobs,
                    EngineeringJobsCount = engJobs,
                    FresherJobsCount = fresherJobs,
                    InternshipsCount = internJobs
                }
            };
        }
    }

    public interface IJobService
    {
        Task<PagedResultDto<JobDto>> GetJobsAsync(
            string? searchQuery = null,
            List<string>? hubs = null,
            List<string>? categories = null,
            bool? isFresherOnly = null,
            bool? isInternshipOnly = null,
            bool? isEngineeringOnly = null,
            List<string>? technologies = null,
            string sortBy = "recent",
            int page = 1,
            int pageSize = 20
        );

        Task<JobDto?> GetJobByIdAsync(string id);
        Task<JobDto?> GetJobBySlugAsync(string slug);
    }

    public class JobService : IJobService
    {
        private readonly ChennaiDbContext _db;

        public JobService(ChennaiDbContext db)
        {
            _db = db;
        }

        public async Task<PagedResultDto<JobDto>> GetJobsAsync(
            string? searchQuery = null,
            List<string>? hubs = null,
            List<string>? categories = null,
            bool? isFresherOnly = null,
            bool? isInternshipOnly = null,
            bool? isEngineeringOnly = null,
            List<string>? technologies = null,
            string sortBy = "recent",
            int page = 1,
            int pageSize = 20)
        {
            var query = _db.Jobs.AsNoTracking().Where(j => j.IsActive);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var q = searchQuery.Trim().ToLower();
                query = query.Where(j => 
                    j.Title.ToLower().Contains(q) ||
                    j.NormalizedTitle.Contains(q) ||
                    j.CompanyName.ToLower().Contains(q) ||
                    j.DescriptionSnippet.ToLower().Contains(q) ||
                    j.CompanyHub.ToLower().Contains(q)
                );
            }

            if (hubs != null && hubs.Count > 0)
            {
                query = query.Where(j => hubs.Contains(j.CompanyHub));
            }

            if (categories != null && categories.Count > 0)
            {
                query = query.Where(j => categories.Contains(j.PrimaryCategory));
            }

            if (isFresherOnly == true)
            {
                query = query.Where(j => j.IsFresher);
            }

            if (isInternshipOnly == true)
            {
                query = query.Where(j => j.IsInternship);
            }

            if (isEngineeringOnly == true)
            {
                query = query.Where(j => j.IsEngineering);
            }

            var jobs = await query.ToListAsync();

            if (technologies != null && technologies.Count > 0)
            {
                jobs = jobs.Where(j => technologies.Any(t => j.Technologies.Contains(t))).ToList();
            }

            // Sorting
            jobs = sortBy switch
            {
                "recent" => jobs.OrderByDescending(j => j.FirstSeenAt).ToList(),
                _ => jobs.OrderByDescending(j => j.IsFeatured).ThenByDescending(j => j.FirstSeenAt).ToList(),
            };

            var total = jobs.Count;
            var items = jobs.Skip((page - 1) * pageSize).Take(pageSize).Select(MapToDto).ToList();

            return new PagedResultDto<JobDto>
            {
                Items = items,
                Page = page,
                PageSize = pageSize,
                Total = total
            };
        }

        public async Task<JobDto?> GetJobByIdAsync(string id)
        {
            var job = await _db.Jobs.AsNoTracking().FirstOrDefaultAsync(j => j.Id == id);
            return job != null ? MapToDto(job) : null;
        }

        public async Task<JobDto?> GetJobBySlugAsync(string slug)
        {
            var job = await _db.Jobs.AsNoTracking().FirstOrDefaultAsync(j => j.Slug == slug);
            return job != null ? MapToDto(job) : null;
        }

        private static JobDto MapToDto(Job j)
        {
            return new JobDto
            {
                Id = j.Id,
                CompanyId = j.CompanyId,
                CompanyName = j.CompanyName,
                CompanyLogo = j.CompanyLogo,
                CompanyHub = j.CompanyHub,
                Title = j.Title,
                Slug = j.Slug,
                DescriptionSnippet = j.DescriptionSnippet,
                PrimaryCategory = j.PrimaryCategory,
                IsEngineering = j.IsEngineering,
                EngineeringSubcategory = j.EngineeringSubcategory,
                Technologies = j.Technologies,
                JobType = j.JobType,
                WorkplaceType = j.WorkplaceType,
                ExperienceLevel = j.ExperienceLevel,
                IsFresher = j.IsFresher,
                FresherConfidence = j.FresherConfidence,
                IsInternship = j.IsInternship,
                SalaryRange = j.SalaryRange,
                Location = j.Location,
                ChennaiRelevance = j.ChennaiRelevance,
                RelevanceConfidence = j.RelevanceConfidence,
                SourceName = j.SourceName,
                OriginalUrl = j.OriginalUrl,
                ApplyUrl = j.ApplyUrl ?? j.OriginalUrl,
                FirstSeenAt = j.FirstSeenAt,
                LastSeenAt = j.LastSeenAt,
                LastVerifiedAt = j.LastVerifiedAt,
                FreshnessStatus = j.FreshnessStatus,
                VerificationStatus = j.VerificationStatus,
                IsFeatured = j.IsFeatured,
                IsSeedData = j.IsSeedData
            };
        }
    }

    public interface ISearchService
    {
        Task<SearchResponseDto> SearchAsync(string query, int page = 1, int pageSize = 20);
    }

    public class SearchService : ISearchService
    {
        private readonly ICompanyService _companyService;
        private readonly IJobService _jobService;

        public SearchService(ICompanyService companyService, IJobService jobService)
        {
            _companyService = companyService;
            _jobService = jobService;
        }

        public async Task<SearchResponseDto> SearchAsync(string query, int page = 1, int pageSize = 20)
        {
            var intent = ParseSearchIntent(query);

            var companies = await _companyService.GetCompaniesAsync(
                searchQuery: query,
                hubs: intent.Hub != null ? new List<string> { intent.Hub } : null,
                types: intent.CompanyType != null ? new List<string> { intent.CompanyType } : null,
                categories: intent.Category != null ? new List<string> { intent.Category } : null,
                technologies: intent.Technology != null ? new List<string> { intent.Technology } : null,
                isFresherOnly: intent.IsFresher,
                page: page,
                pageSize: pageSize
            );

            var jobs = await _jobService.GetJobsAsync(
                searchQuery: query,
                hubs: intent.Hub != null ? new List<string> { intent.Hub } : null,
                categories: intent.Category != null ? new List<string> { intent.Category } : null,
                technologies: intent.Technology != null ? new List<string> { intent.Technology } : null,
                isFresherOnly: intent.IsFresher,
                isInternshipOnly: intent.IsInternship,
                page: page,
                pageSize: pageSize
            );

            return new SearchResponseDto
            {
                Intent = intent,
                Companies = companies,
                Jobs = jobs
            };
        }

        private static ParsedSearchIntentDto ParseSearchIntent(string rawQuery)
        {
            var q = (rawQuery ?? string.Empty).Trim().ToLower();
            var matchedSynonyms = new List<string>();

            string? tech = null;
            if (q.Contains("dotnet") || q.Contains(".net") || q.Contains("c#")) { tech = ".NET"; matchedSynonyms.Add("Tech: .NET"); }
            else if (q.Contains("react")) { tech = "React"; matchedSynonyms.Add("Tech: React"); }
            else if (q.Contains("python")) { tech = "Python"; matchedSynonyms.Add("Tech: Python"); }
            else if (q.Contains("java")) { tech = "Java"; matchedSynonyms.Add("Tech: Java"); }

            bool? isFresher = q.Contains("fresher") || q.Contains("0-1") || q.Contains("graduate") ? true : null;
            if (isFresher == true) matchedSynonyms.Add("Intent: Fresher");

            bool? isIntern = q.Contains("intern") || q.Contains("stipend") ? true : null;
            if (isIntern == true) matchedSynonyms.Add("Intent: Internship");

            string? hub = null;
            if (q.Contains("omr")) { hub = "OMR (IT Corridor)"; matchedSynonyms.Add("Hub: OMR"); }
            else if (q.Contains("guindy")) { hub = "Guindy (SIDCO / Olympia)"; matchedSynonyms.Add("Hub: Guindy"); }
            else if (q.Contains("perungudi")) { hub = "Perungudi & Kandanchavadi"; matchedSynonyms.Add("Hub: Perungudi"); }

            string? companyType = q.Contains("startup") ? "STARTUP" : q.Contains("product") ? "PRODUCT COMPANY" : null;

            return new ParsedSearchIntentDto
            {
                RawQuery = rawQuery ?? string.Empty,
                Technology = tech,
                IsFresher = isFresher,
                IsInternship = isIntern,
                Hub = hub,
                CompanyType = companyType,
                HasLocationIntent = hub != null || q.Contains("chennai"),
                MatchedSynonyms = matchedSynonyms
            };
        }
    }
}
