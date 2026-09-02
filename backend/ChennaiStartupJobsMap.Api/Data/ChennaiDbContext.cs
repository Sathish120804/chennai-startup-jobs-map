using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Data
{
    public class ChennaiDbContext : DbContext
    {
        public ChennaiDbContext(DbContextOptions<ChennaiDbContext> options) : base(options) { }

        public DbSet<Company> Companies => Set<Company>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<Technology> Technologies => Set<Technology>();
        public DbSet<Location> Locations => Set<Location>();
        public DbSet<UserSubmission> Submissions => Set<UserSubmission>();
        public DbSet<RawIngestionRecord> RawIngestionRecords => Set<RawIngestionRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Company Indexes
            modelBuilder.Entity<Company>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasIndex(c => c.NormalizedName);
                entity.HasIndex(c => c.Slug).IsUnique();
                entity.HasIndex(c => c.Hub);
                entity.HasIndex(c => c.HiringStatus);

                entity.Property(c => c.CompanyTypes)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );

                entity.Property(c => c.Categories)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );

                entity.Property(c => c.Tags)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );

                entity.Property(c => c.TechStack)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });

            // Job Indexes & Configuration
            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasKey(j => j.Id);
                entity.HasIndex(j => j.NormalizedTitle);
                entity.HasIndex(j => j.CompanyId);
                entity.HasIndex(j => j.Slug).IsUnique();
                entity.HasIndex(j => j.CompanyHub);
                entity.HasIndex(j => j.IsFresher);
                entity.HasIndex(j => j.IsEngineering);
                entity.HasIndex(j => j.FreshnessStatus);

                entity.HasOne(j => j.Company)
                    .WithMany(c => c.Jobs)
                    .HasForeignKey(j => j.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(j => j.Technologies)
                    .HasConversion(
                        v => string.Join(',', v),
                        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                    );
            });

            // Seed Data
            SeedInitialData(modelBuilder);
        }

        private static void SeedInitialData(ModelBuilder modelBuilder)
        {
            var zoho = new Company
            {
                Id = "comp-1",
                Name = "Zoho Corporation",
                NormalizedName = "zoho corporation",
                Slug = "zoho",
                Tagline = "Bootstrapped Global SaaS Titan & Tech Powerhouse",
                Description = "Zoho offers a comprehensive suite of cloud software applications for businesses worldwide. Bootstrapped from Chennai with tens of millions of users across 150+ countries.",
                Logo = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80",
                Website = "https://www.zoho.com",
                CareersUrl = "https://www.zoho.com/careers/",
                CompanyTypes = new List<string> { "PRODUCT COMPANY", "ENTERPRISE", "STARTUP" },
                Categories = new List<string> { "SaaS / Enterprise Software", "DeepTech & AI" },
                Hub = "OMR (IT Corridor)",
                Address = "Estancia IT Park, Plot No. 140 & 151, GST Road / OMR Corridor, Chennai",
                Latitude = 12.8252,
                Longitude = 80.0435,
                MapPrecision = "exact",
                FoundedYear = 1996,
                EmployeeCount = "15,000+",
                FundingStage = "Bootstrapped",
                TotalFundingRaised = "Self-Funded ($1B+ Annual Revenue)",
                HiringStatus = "Hiring Surge",
                Tags = new List<string> { "SaaS", "Cloud CRM", "Bootstrapped" },
                TechStack = new List<string> { "Java", "C++", "React", "Python", "PostgreSQL" },
                VerificationStatus = "VERIFIED",
                IsFeatured = true,
                IsActive = true,
                IsSeedData = true,
                SourceName = "Company Careers",
                SourceUrl = "https://www.zoho.com/careers/",
                DiscoveredAt = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                LastVerifiedAt = DateTime.UtcNow
            };

            var freshworks = new Company
            {
                Id = "comp-2",
                Name = "Freshworks",
                NormalizedName = "freshworks",
                Slug = "freshworks",
                Tagline = "Modern AI-powered Customer Experience & IT Service Software",
                Description = "Born in Chennai, Freshworks makes innovative customer engagement and ITSM software for over 60,000 businesses globally.",
                Logo = "https://images.unsplash.com/photo-1614680376593-902f749f7ffc?w=128&auto=format&fit=crop&q=80",
                Website = "https://www.freshworks.com",
                CareersUrl = "https://www.freshworks.com/company/careers/",
                CompanyTypes = new List<string> { "PRODUCT COMPANY", "MNC", "ENTERPRISE" },
                Categories = new List<string> { "SaaS / Enterprise Software", "DeepTech & AI" },
                Hub = "Perungudi & Kandanchavadi",
                Address = "Block B, Global Infocity Park, 40 MGR Salai, Perungudi, Chennai 600096",
                Latitude = 12.9644,
                Longitude = 80.2427,
                MapPrecision = "exact",
                FoundedYear = 2010,
                EmployeeCount = "5,000+",
                FundingStage = "Public / IPO",
                TotalFundingRaised = "Nasdaq: FRSH ($1.03B IPO)",
                HiringStatus = "Active",
                Tags = new List<string> { "SaaS", "Nasdaq Listed", "CRM" },
                TechStack = new List<string> { "Ruby on Rails", "Java", "React", "AWS", "Python" },
                VerificationStatus = "VERIFIED",
                IsFeatured = true,
                IsActive = true,
                IsSeedData = true,
                SourceName = "Company Careers",
                SourceUrl = "https://www.freshworks.com/company/careers/",
                DiscoveredAt = new DateTime(2026, 8, 1, 0, 0, 0, DateTimeKind.Utc),
                LastVerifiedAt = DateTime.UtcNow
            };

            modelBuilder.Entity<Company>().HasData(zoho, freshworks);

            var job1 = new Job
            {
                Id = "job-1",
                CompanyId = "comp-1",
                CompanyName = "Zoho Corporation",
                CompanyLogo = "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80",
                CompanyHub = "OMR (IT Corridor)",
                Title = "Associate Software Developer (Freshers 2025/2026)",
                NormalizedTitle = "associate software developer freshers 2025 2026",
                Slug = "zoho-associate-software-developer-fresher",
                DescriptionSnippet = "We are hiring fresh engineering graduates to work on Zoho core product suites, database engines, and AI applications. Open to 0-1 years of experience in Java, C++, or Python.",
                PrimaryCategory = "SaaS / Enterprise Software",
                IsEngineering = true,
                EngineeringSubcategory = "Software Engineering",
                Technologies = new List<string> { "Java", "C++", "Python", "SQL" },
                JobType = "Full-time",
                WorkplaceType = "On-site",
                ExperienceLevel = "Fresher / Entry (0-1 yrs)",
                ExperienceMin = 0,
                ExperienceMax = 1,
                IsFresher = true,
                FresherConfidence = 98,
                IsInternship = false,
                SalaryRange = "₹6,50,000 - ₹9,50,000 / yr",
                SalaryMin = 650000,
                SalaryMax = 950000,
                SalaryCurrency = "INR",
                Location = "Estancia IT Park / OMR, Chennai",
                ChennaiRelevance = "CHENNAI_CONFIRMED",
                RelevanceConfidence = 100,
                SourceName = "Company Careers",
                OriginalUrl = "https://www.zoho.com/careers/job-details.html?id=fresher-dev-chennai",
                ApplyUrl = "https://www.zoho.com/careers/job-details.html?id=fresher-dev-chennai",
                FirstSeenAt = DateTime.UtcNow.AddDays(-2),
                LastSeenAt = DateTime.UtcNow,
                LastVerifiedAt = DateTime.UtcNow,
                FreshnessStatus = "NEW",
                VerificationStatus = "VERIFIED",
                IsFeatured = true,
                IsActive = true,
                IsSeedData = true
            };

            var job2 = new Job
            {
                Id = "job-2",
                CompanyId = "comp-2",
                CompanyName = "Freshworks",
                CompanyLogo = "https://images.unsplash.com/photo-1614680376593-902f749f7ffc?w=128&auto=format&fit=crop&q=80",
                CompanyHub = "Perungudi & Kandanchavadi",
                Title = "Frontend Engineer Intern (React / TypeScript)",
                NormalizedTitle = "frontend engineer intern react typescript",
                Slug = "freshworks-frontend-engineer-intern",
                DescriptionSnippet = "Looking for a passionate React frontend intern to build slick customer experience UI components for Freshdesk.",
                PrimaryCategory = "SaaS / Enterprise Software",
                IsEngineering = true,
                EngineeringSubcategory = "Frontend",
                Technologies = new List<string> { "React", "TypeScript", "JavaScript" },
                JobType = "Internship",
                WorkplaceType = "Hybrid",
                ExperienceLevel = "Fresher / Entry (0-1 yrs)",
                ExperienceMin = 0,
                ExperienceMax = 0.5,
                IsFresher = true,
                FresherConfidence = 95,
                IsInternship = true,
                SalaryRange = "₹35,000 / month Stipend",
                Location = "Global Infocity, Perungudi, OMR, Chennai",
                ChennaiRelevance = "CHENNAI_CONFIRMED",
                RelevanceConfidence = 100,
                SourceName = "Company Careers",
                OriginalUrl = "https://www.freshworks.com/company/careers/job-frontend-intern",
                ApplyUrl = "https://www.freshworks.com/company/careers/job-frontend-intern",
                FirstSeenAt = DateTime.UtcNow.AddDays(-1),
                LastSeenAt = DateTime.UtcNow,
                LastVerifiedAt = DateTime.UtcNow,
                FreshnessStatus = "NEW",
                VerificationStatus = "VERIFIED",
                IsFeatured = true,
                IsActive = true,
                IsSeedData = true
            };

            modelBuilder.Entity<Job>().HasData(job1, job2);
        }
    }
}
