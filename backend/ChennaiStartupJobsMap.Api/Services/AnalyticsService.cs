using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Data;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Services
{
    public class AnalyticsOverviewDto
    {
        public int TotalSearches { get; set; }
        public int TotalJobViews { get; set; }
        public int TotalCompanyViews { get; set; }
        public int TotalApplyClicks { get; set; }
        public int TotalSavedJobs { get; set; }
        public int TotalSavedCompanies { get; set; }
        public List<KeyValuePair<string, int>> TopSearches { get; set; } = new();
        public List<KeyValuePair<string, int>> TopViewedJobs { get; set; } = new();
        public List<KeyValuePair<string, int>> TopViewedCompanies { get; set; } = new();
    }

    public interface IAnalyticsService
    {
        Task TrackEventAsync(string eventType, string? entityId = null, string? metadataJson = null, string? clientIp = null);
        Task<AnalyticsOverviewDto> GetAnalyticsOverviewAsync();
    }

    public class AnalyticsService : IAnalyticsService
    {
        private readonly ChennaiDbContext _db;

        public AnalyticsService(ChennaiDbContext db)
        {
            _db = db;
        }

        public async Task TrackEventAsync(string eventType, string? entityId = null, string? metadataJson = null, string? clientIp = null)
        {
            string? ipHash = null;
            if (!string.IsNullOrWhiteSpace(clientIp))
            {
                using var sha = SHA256.Create();
                var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(clientIp + "ChennaiSalt"));
                ipHash = Convert.ToHexString(bytes)[..16];
            }

            var evt = new AnalyticsEvent
            {
                EventType = eventType.ToUpperInvariant(),
                EntityId = entityId,
                MetadataJson = metadataJson,
                UserIdentifierHash = ipHash,
                CreatedAt = DateTime.UtcNow
            };

            _db.AnalyticsEvents.Add(evt);
            await _db.SaveChangesAsync();
        }

        public async Task<AnalyticsOverviewDto> GetAnalyticsOverviewAsync()
        {
            var searches = await _db.AnalyticsEvents.CountAsync(e => e.EventType == "SEARCH");
            var jobViews = await _db.AnalyticsEvents.CountAsync(e => e.EventType == "JOB_VIEW");
            var compViews = await _db.AnalyticsEvents.CountAsync(e => e.EventType == "COMPANY_VIEW");
            var applyClicks = await _db.AnalyticsEvents.CountAsync(e => e.EventType == "APPLY_CLICK");
            var savedJobs = await _db.SavedJobs.CountAsync();
            var savedComps = await _db.SavedCompanies.CountAsync();

            var topEntitiesRaw = await _db.AnalyticsEvents
                .Where(e => e.EventType == "JOB_VIEW" && e.EntityId != null)
                .GroupBy(e => e.EntityId!)
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            var topEntities = topEntitiesRaw
                .Select(x => new KeyValuePair<string, int>(x.Key, x.Count))
                .ToList();

            var topCompsRaw = await _db.AnalyticsEvents
                .Where(e => e.EventType == "COMPANY_VIEW" && e.EntityId != null)
                .GroupBy(e => e.EntityId!)
                .Select(g => new { Key = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToListAsync();

            var topComps = topCompsRaw
                .Select(x => new KeyValuePair<string, int>(x.Key, x.Count))
                .ToList();

            return new AnalyticsOverviewDto
            {
                TotalSearches = searches,
                TotalJobViews = jobViews,
                TotalCompanyViews = compViews,
                TotalApplyClicks = applyClicks,
                TotalSavedJobs = savedJobs,
                TotalSavedCompanies = savedComps,
                TopViewedJobs = topEntities,
                TopViewedCompanies = topComps
            };
        }
    }
}
