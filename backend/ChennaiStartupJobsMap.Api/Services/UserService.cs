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
    public interface IUserService
    {
        Task<List<JobDto>> GetSavedJobsAsync(string userId);
        Task<bool> SaveJobAsync(string userId, string jobId);
        Task<bool> UnsaveJobAsync(string userId, string jobId);

        Task<List<CompanyDto>> GetSavedCompaniesAsync(string userId);
        Task<bool> SaveCompanyAsync(string userId, string companyId);
        Task<bool> UnsaveCompanyAsync(string userId, string companyId);

        Task<List<JobAlert>> GetJobAlertsAsync(string userId);
        Task<JobAlert> CreateJobAlertAsync(string userId, string name, string query, string? filtersJson, string frequency);
        Task<bool> DeleteJobAlertAsync(string userId, string alertId);

        Task<List<Notification>> GetNotificationsAsync(string userId);
        Task<bool> MarkNotificationReadAsync(string userId, string notificationId);
    }

    public class UserService : IUserService
    {
        private readonly ChennaiDbContext _db;
        private readonly IJobService _jobService;
        private readonly ICompanyService _companyService;

        public UserService(ChennaiDbContext db, IJobService jobService, ICompanyService companyService)
        {
            _db = db;
            _jobService = jobService;
            _companyService = companyService;
        }

        public async Task<List<JobDto>> GetSavedJobsAsync(string userId)
        {
            var jobIds = await _db.SavedJobs
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => s.JobId)
                .ToListAsync();

            var list = new List<JobDto>();
            foreach (var jId in jobIds)
            {
                var job = await _jobService.GetJobByIdAsync(jId);
                if (job != null) list.Add(job);
            }
            return list;
        }

        public async Task<bool> SaveJobAsync(string userId, string jobId)
        {
            var jobExists = await _db.Jobs.AnyAsync(j => j.Id == jobId);
            if (!jobExists) return false;

            var alreadySaved = await _db.SavedJobs.AnyAsync(s => s.UserId == userId && s.JobId == jobId);
            if (alreadySaved) return true;

            _db.SavedJobs.Add(new SavedJob
            {
                UserId = userId,
                JobId = jobId,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnsaveJobAsync(string userId, string jobId)
        {
            var item = await _db.SavedJobs.FirstOrDefaultAsync(s => s.UserId == userId && s.JobId == jobId);
            if (item == null) return false;

            _db.SavedJobs.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<CompanyDto>> GetSavedCompaniesAsync(string userId)
        {
            var companyIds = await _db.SavedCompanies
                .Where(s => s.UserId == userId)
                .OrderByDescending(s => s.CreatedAt)
                .Select(s => s.CompanyId)
                .ToListAsync();

            var list = new List<CompanyDto>();
            foreach (var cId in companyIds)
            {
                var comp = await _companyService.GetCompanyByIdAsync(cId);
                if (comp != null) list.Add(comp);
            }
            return list;
        }

        public async Task<bool> SaveCompanyAsync(string userId, string companyId)
        {
            var compExists = await _db.Companies.AnyAsync(c => c.Id == companyId);
            if (!compExists) return false;

            var alreadySaved = await _db.SavedCompanies.AnyAsync(s => s.UserId == userId && s.CompanyId == companyId);
            if (alreadySaved) return true;

            _db.SavedCompanies.Add(new SavedCompany
            {
                UserId = userId,
                CompanyId = companyId,
                CreatedAt = DateTime.UtcNow
            });

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnsaveCompanyAsync(string userId, string companyId)
        {
            var item = await _db.SavedCompanies.FirstOrDefaultAsync(s => s.UserId == userId && s.CompanyId == companyId);
            if (item == null) return false;

            _db.SavedCompanies.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<JobAlert>> GetJobAlertsAsync(string userId)
        {
            return await _db.JobAlerts
                .Where(a => a.UserId == userId && a.IsActive)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<JobAlert> CreateJobAlertAsync(string userId, string name, string query, string? filtersJson, string frequency)
        {
            var alert = new JobAlert
            {
                UserId = userId,
                Name = name,
                Query = query,
                FiltersJson = filtersJson,
                Frequency = frequency,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _db.JobAlerts.Add(alert);
            await _db.SaveChangesAsync();
            return alert;
        }

        public async Task<bool> DeleteJobAlertAsync(string userId, string alertId)
        {
            var alert = await _db.JobAlerts.FirstOrDefaultAsync(a => a.Id == alertId && a.UserId == userId);
            if (alert == null) return false;

            alert.IsActive = false;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<List<Notification>> GetNotificationsAsync(string userId)
        {
            return await _db.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Take(50)
                .ToListAsync();
        }

        public async Task<bool> MarkNotificationReadAsync(string userId, string notificationId)
        {
            var notif = await _db.Notifications.FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);
            if (notif == null) return false;

            notif.IsRead = true;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
