using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.DTOs;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Services.AI
{
    public class JobRecommendationDto
    {
        public JobDto Job { get; set; } = new();
        public int MatchScore { get; set; } // 0 to 100
        public string MatchStrength { get; set; } = "Good match"; // Strong match, Good match, Relevant match
        public List<string> MatchReasons { get; set; } = new();
    }

    public class CompanyRecommendationDto
    {
        public CompanyDto Company { get; set; } = new();
        public int MatchScore { get; set; }
        public string MatchStrength { get; set; } = "Good match";
        public List<string> MatchReasons { get; set; } = new();
    }

    public interface IJobRecommendationService
    {
        Task<List<JobRecommendationDto>> GetJobRecommendationsAsync(
            string? query = null,
            List<string>? technologies = null,
            string? locationHub = null,
            bool? isFresher = null,
            bool? isInternship = null,
            int limit = 10);

        Task<List<CompanyRecommendationDto>> GetCompanyRecommendationsAsync(
            string? query = null,
            string? category = null,
            string? locationHub = null,
            int limit = 10);
    }

    public class JobRecommendationService : IJobRecommendationService
    {
        private readonly ChennaiDbContext _db;
        private readonly IEmbeddingProvider _embedding;
        private readonly IJobService _jobService;
        private readonly ICompanyService _companyService;

        public JobRecommendationService(
            ChennaiDbContext db,
            IEmbeddingProvider embedding,
            IJobService jobService,
            ICompanyService companyService)
        {
            _db = db;
            _embedding = embedding;
            _jobService = jobService;
            _companyService = companyService;
        }

        public async Task<List<JobRecommendationDto>> GetJobRecommendationsAsync(
            string? query = null,
            List<string>? technologies = null,
            string? locationHub = null,
            bool? isFresher = null,
            bool? isInternship = null,
            int limit = 10)
        {
            var pagedJobs = await _jobService.GetJobsAsync(pageSize: 100);
            var jobs = pagedJobs.Items;
            if (jobs.Count == 0) return new List<JobRecommendationDto>();

            var queryString = $"{query} {string.Join(' ', technologies ?? new())} {locationHub}".Trim();
            var queryVector = _embedding.GenerateEmbedding(string.IsNullOrWhiteSpace(queryString) ? "software engineer chennai" : queryString);

            var ranked = new List<JobRecommendationDto>();

            foreach (var job in jobs)
            {
                var reasons = new List<string>();
                int score = 50; // Baseline relevance

                // 1. Semantic embedding similarity
                var jobRepresentation = $"{job.Title} {string.Join(' ', job.Technologies)} {job.PrimaryCategory} {job.Location} {job.CompanyHub}";
                var jobVector = _embedding.GenerateEmbedding(jobRepresentation);
                float semanticSim = _embedding.CosineSimilarity(queryVector, jobVector);

                int semanticPoints = (int)(semanticSim * 30);
                score += semanticPoints;

                // 2. Technology alignment
                if (technologies != null && technologies.Count > 0)
                {
                    var matchedTechs = job.Technologies.Intersect(technologies, StringComparer.OrdinalIgnoreCase).ToList();
                    if (matchedTechs.Count > 0)
                    {
                        score += 20;
                        reasons.Add($"Matches your {string.Join(", ", matchedTechs)} skill");
                    }
                }
                else if (!string.IsNullOrWhiteSpace(query))
                {
                    var qLower = query.ToLower();
                    var matchedTechs = job.Technologies.Where(t => qLower.Contains(t.ToLower())).ToList();
                    if (matchedTechs.Count > 0)
                    {
                        score += 15;
                        reasons.Add($"Matches requested tech: {string.Join(", ", matchedTechs)}");
                    }
                }

                // 3. Fresher / Internship intent
                if (isFresher == true && job.IsFresher)
                {
                    score += 15;
                    reasons.Add("Entry-level / Fresher opportunity");
                }
                else if (job.IsFresher)
                {
                    reasons.Add("Fresher friendly");
                }

                if (isInternship == true && job.IsInternship)
                {
                    score += 15;
                    reasons.Add("Internship vacancy with practical training");
                }

                // 4. Location proximity
                if (!string.IsNullOrWhiteSpace(locationHub) && (job.Location.Contains(locationHub, StringComparison.OrdinalIgnoreCase) || job.CompanyHub.Contains(locationHub, StringComparison.OrdinalIgnoreCase)))
                {
                    score += 15;
                    reasons.Add($"Located in {job.CompanyHub}");
                }
                else
                {
                    reasons.Add($"Office in {job.CompanyHub}");
                }

                // 5. Verification boost
                if (job.VerificationStatus == "VERIFIED")
                {
                    score += 5;
                    reasons.Add("Directly verified application link");
                }

                score = Math.Clamp(score, 40, 99);
                string strength = score >= 80 ? "Strong match" : score >= 65 ? "Good match" : "Relevant match";

                ranked.Add(new JobRecommendationDto
                {
                    Job = job,
                    MatchScore = score,
                    MatchStrength = strength,
                    MatchReasons = reasons.Distinct().Take(3).ToList()
                });
            }

            return ranked.OrderByDescending(r => r.MatchScore).Take(limit).ToList();
        }

        public async Task<List<CompanyRecommendationDto>> GetCompanyRecommendationsAsync(
            string? query = null,
            string? category = null,
            string? locationHub = null,
            int limit = 10)
        {
            var paged = await _companyService.GetCompaniesAsync(pageSize: 100);
            var companies = paged.Items;

            var queryString = $"{query} {category} {locationHub}".Trim();
            var queryVector = _embedding.GenerateEmbedding(string.IsNullOrWhiteSpace(queryString) ? "saas tech startup chennai" : queryString);

            var ranked = new List<CompanyRecommendationDto>();

            foreach (var company in companies)
            {
                var reasons = new List<string>();
                int score = 50;

                var compText = $"{company.Name} {company.Description} {string.Join(' ', company.Categories)} {string.Join(' ', company.TechStack)} {company.Hub}";
                var compVector = _embedding.GenerateEmbedding(compText);
                float semanticSim = _embedding.CosineSimilarity(queryVector, compVector);

                score += (int)(semanticSim * 30);

                if (company.HiringStatus == "HIRING")
                {
                    score += 15;
                    reasons.Add("Actively hiring tech talent in Chennai");
                }

                if (!string.IsNullOrWhiteSpace(locationHub) && company.Hub.Contains(locationHub, StringComparison.OrdinalIgnoreCase))
                {
                    score += 15;
                    reasons.Add($"Headquarters in {company.Hub}");
                }
                else
                {
                    reasons.Add($"Tech Hub: {company.Hub}");
                }

                if (company.Categories.Count > 0)
                {
                    reasons.Add($"Domain: {company.Categories[0]}");
                }

                score = Math.Clamp(score, 40, 99);
                string strength = score >= 80 ? "Strong match" : score >= 65 ? "Good match" : "Relevant match";

                ranked.Add(new CompanyRecommendationDto
                {
                    Company = company,
                    MatchScore = score,
                    MatchStrength = strength,
                    MatchReasons = reasons.Distinct().Take(3).ToList()
                });
            }

            return ranked.OrderByDescending(r => r.MatchScore).Take(limit).ToList();
        }
    }
}
