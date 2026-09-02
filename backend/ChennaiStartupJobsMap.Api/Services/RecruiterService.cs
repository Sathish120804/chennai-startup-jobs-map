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
    public interface IRecruiterService
    {
        Task<CompanyClaim> ClaimCompanyAsync(string userId, string companyId, string corporateEmail, string proofNotes);
        Task<List<CompanyClaim>> GetMyClaimsAsync(string userId);
        Task<Job> SubmitRecruiterJobAsync(string userId, SubmitJobRequest request);
        Task<List<JobDto>> GetRecruiterJobsAsync(string userId);
    }

    public class RecruiterService : IRecruiterService
    {
        private readonly ChennaiDbContext _db;
        private readonly INormalizationService _norm;
        private readonly IJobService _jobService;

        public RecruiterService(ChennaiDbContext db, INormalizationService norm, IJobService jobService)
        {
            _db = db;
            _norm = norm;
            _jobService = jobService;
        }

        public async Task<CompanyClaim> ClaimCompanyAsync(string userId, string companyId, string corporateEmail, string proofNotes)
        {
            var company = await _db.Companies.FirstOrDefaultAsync(c => c.Id == companyId);
            if (company == null) throw new ArgumentException("Company not found.");

            var claim = new CompanyClaim
            {
                UserId = userId,
                CompanyId = companyId,
                CorporateEmail = corporateEmail,
                ProofNotes = proofNotes,
                Status = "PENDING",
                CreatedAt = DateTime.UtcNow
            };

            _db.CompanyClaims.Add(claim);
            await _db.SaveChangesAsync();
            return claim;
        }

        public async Task<List<CompanyClaim>> GetMyClaimsAsync(string userId)
        {
            return await _db.CompanyClaims
                .Include(c => c.Company)
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<Job> SubmitRecruiterJobAsync(string userId, SubmitJobRequest request)
        {
            var job = new Job
            {
                Id = $"job-{Guid.NewGuid():N}",
                CompanyId = "comp-1", // default fallback or mapped to claimed company
                CompanyName = request.CompanyName,
                CompanyLogo = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80",
                CompanyHub = "OMR (IT Corridor)",
                Title = request.Title,
                NormalizedTitle = _norm.NormalizeTitle(request.Title),
                Slug = $"{request.CompanyName}-{request.Title}".ToLower().Replace(" ", "-").Replace("/", "-"),
                DescriptionSnippet = request.DescriptionSnippet,
                PrimaryCategory = "Engineering",
                IsEngineering = true,
                EngineeringSubcategory = "Software Engineering",
                Technologies = _norm.ExtractTechnologies(request.DescriptionSnippet + " " + request.Title),
                JobType = "Full-time",
                WorkplaceType = "On-site",
                ExperienceLevel = "Fresher / Entry (0-1 yrs)",
                IsFresher = request.Title.Contains("fresher", StringComparison.OrdinalIgnoreCase) || request.Title.Contains("junior", StringComparison.OrdinalIgnoreCase),
                FresherConfidence = 85,
                IsInternship = request.Title.Contains("intern", StringComparison.OrdinalIgnoreCase),
                SalaryRange = request.SalaryRange ?? "Market Standard",
                Location = request.Location,
                ChennaiRelevance = "CHENNAI_CONFIRMED",
                RelevanceConfidence = 95,
                SourceName = $"Recruiter Direct ({request.SubmittedBy})",
                OriginalUrl = request.OriginalUrl,
                ApplyUrl = request.OriginalUrl,
                FirstSeenAt = DateTime.UtcNow,
                LastSeenAt = DateTime.UtcNow,
                LastVerifiedAt = DateTime.UtcNow,
                FreshnessStatus = "NEW",
                VerificationStatus = "PENDING_REVIEW", // Recruiter postings start as PENDING_REVIEW!
                IsFeatured = false,
                IsActive = false, // Becomes active when approved by admin/moderator
                IsSeedData = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _db.Jobs.Add(job);
            await _db.SaveChangesAsync();
            return job;
        }

        public async Task<List<JobDto>> GetRecruiterJobsAsync(string userId)
        {
            var jobs = await _db.Jobs
                .Where(j => j.SourceName.Contains("Recruiter Direct"))
                .OrderByDescending(j => j.CreatedAt)
                .ToListAsync();

            var list = new List<JobDto>();
            foreach (var j in jobs)
            {
                var dto = await _jobService.GetJobByIdAsync(j.Id);
                if (dto != null) list.Add(dto);
            }
            return list;
        }
    }
}
