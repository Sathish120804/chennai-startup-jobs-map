using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ChennaiStartupJobsMap.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnalyticsEvents",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    EntityId = table.Column<string>(type: "text", nullable: true),
                    MetadataJson = table.Column<string>(type: "text", nullable: true),
                    UserIdentifierHash = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnalyticsEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    NormalizedName = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Tagline = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: false),
                    Website = table.Column<string>(type: "text", nullable: false),
                    CareersUrl = table.Column<string>(type: "text", nullable: false),
                    CompanyTypes = table.Column<string>(type: "text", nullable: false),
                    Categories = table.Column<string>(type: "text", nullable: false),
                    Hub = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    MapPrecision = table.Column<string>(type: "text", nullable: false),
                    FoundedYear = table.Column<int>(type: "integer", nullable: false),
                    EmployeeCount = table.Column<string>(type: "text", nullable: false),
                    FundingStage = table.Column<string>(type: "text", nullable: false),
                    TotalFundingRaised = table.Column<string>(type: "text", nullable: true),
                    HiringStatus = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: false),
                    TechStack = table.Column<string>(type: "text", nullable: false),
                    VerificationStatus = table.Column<string>(type: "text", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsSeedData = table.Column<bool>(type: "boolean", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceName = table.Column<string>(type: "text", nullable: false),
                    SourceUrl = table.Column<string>(type: "text", nullable: true),
                    SourceRecordId = table.Column<string>(type: "text", nullable: true),
                    VerificationMethod = table.Column<string>(type: "text", nullable: false),
                    ConfidenceScore = table.Column<int>(type: "integer", nullable: false),
                    ChennaiRelevanceScore = table.Column<int>(type: "integer", nullable: false),
                    Industry = table.Column<string>(type: "text", nullable: false),
                    SubCategory = table.Column<string>(type: "text", nullable: true),
                    Headquarters = table.Column<string>(type: "text", nullable: false),
                    ChennaiPresence = table.Column<string>(type: "text", nullable: false),
                    ChennaiLocations = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    DiscoveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ShortDescription = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: false),
                    OfficialWebsite = table.Column<string>(type: "text", nullable: false),
                    OfficialCareersUrl = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    CompanyType = table.Column<string>(type: "text", nullable: false),
                    EmployeeRange = table.Column<string>(type: "text", nullable: false),
                    TechnologyTags = table.Column<List<string>>(type: "text[]", nullable: false),
                    Skills = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IngestionRuns",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SourceId = table.Column<string>(type: "text", nullable: false),
                    EntityType = table.Column<string>(type: "text", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    RecordsDiscovered = table.Column<int>(type: "integer", nullable: false),
                    RecordsCreated = table.Column<int>(type: "integer", nullable: false),
                    RecordsUpdated = table.Column<int>(type: "integer", nullable: false),
                    RecordsSkipped = table.Column<int>(type: "integer", nullable: false),
                    DuplicatesFound = table.Column<int>(type: "integer", nullable: false),
                    ErrorsCount = table.Column<int>(type: "integer", nullable: false),
                    ErrorSummary = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngestionRuns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "JobAlerts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Query = table.Column<string>(type: "text", nullable: false),
                    FiltersJson = table.Column<string>(type: "text", nullable: true),
                    Frequency = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobAlerts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Area = table.Column<string>(type: "text", nullable: false),
                    Hub = table.Column<string>(type: "text", nullable: false),
                    Pincode = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Precision = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    Link = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RawIngestionRecords",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    SourceName = table.Column<string>(type: "text", nullable: false),
                    ExternalId = table.Column<string>(type: "text", nullable: false),
                    RawTitle = table.Column<string>(type: "text", nullable: false),
                    RawCompany = table.Column<string>(type: "text", nullable: false),
                    RawLocation = table.Column<string>(type: "text", nullable: false),
                    RawUrl = table.Column<string>(type: "text", nullable: false),
                    DiscoveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ErrorMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawIngestionRecords", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Submissions",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    SubmittedBy = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    TitleOrName = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Hub = table.Column<string>(type: "text", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Submissions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Technologies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<string>(type: "text", nullable: false),
                    Synonyms = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technologies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: true),
                    IsVerified = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CareerSources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false),
                    Provider = table.Column<string>(type: "text", nullable: false),
                    CareersUrl = table.Column<string>(type: "text", nullable: false),
                    JobsApiUrl = table.Column<string>(type: "text", nullable: true),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    LastCheckedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareerSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareerSources_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanyClaims",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CorporateEmail = table.Column<string>(type: "text", nullable: false),
                    ProofNotes = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    ReviewedBy = table.Column<string>(type: "text", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyClaims_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompanySources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    SourceName = table.Column<string>(type: "text", nullable: false),
                    SourceUrl = table.Column<string>(type: "text", nullable: false),
                    SourceRecordId = table.Column<string>(type: "text", nullable: true),
                    Confidence = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false),
                    DiscoveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanySources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanySources_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    CompanyLogo = table.Column<string>(type: "text", nullable: false),
                    CompanyHub = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    NormalizedTitle = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    DescriptionSnippet = table.Column<string>(type: "text", nullable: false),
                    PrimaryCategory = table.Column<string>(type: "text", nullable: false),
                    IsEngineering = table.Column<bool>(type: "boolean", nullable: false),
                    EngineeringSubcategory = table.Column<string>(type: "text", nullable: true),
                    Technologies = table.Column<string>(type: "text", nullable: false),
                    JobType = table.Column<string>(type: "text", nullable: false),
                    WorkplaceType = table.Column<string>(type: "text", nullable: false),
                    ExperienceLevel = table.Column<string>(type: "text", nullable: false),
                    ExperienceMin = table.Column<double>(type: "double precision", nullable: true),
                    ExperienceMax = table.Column<double>(type: "double precision", nullable: true),
                    IsFresher = table.Column<bool>(type: "boolean", nullable: false),
                    FresherConfidence = table.Column<int>(type: "integer", nullable: false),
                    IsInternship = table.Column<bool>(type: "boolean", nullable: false),
                    SalaryRange = table.Column<string>(type: "text", nullable: true),
                    SalaryMin = table.Column<double>(type: "double precision", nullable: true),
                    SalaryMax = table.Column<double>(type: "double precision", nullable: true),
                    SalaryCurrency = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: false),
                    ChennaiRelevance = table.Column<string>(type: "text", nullable: false),
                    RelevanceConfidence = table.Column<int>(type: "integer", nullable: false),
                    SourceName = table.Column<string>(type: "text", nullable: false),
                    OriginalUrl = table.Column<string>(type: "text", nullable: false),
                    ApplyUrl = table.Column<string>(type: "text", nullable: true),
                    SourceRecordId = table.Column<string>(type: "text", nullable: true),
                    FirstSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastSeenAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastVerifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FreshnessStatus = table.Column<string>(type: "text", nullable: false),
                    VerificationStatus = table.Column<string>(type: "text", nullable: false),
                    DuplicateGroupId = table.Column<string>(type: "text", nullable: true),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsSeedData = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedCompanies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedCompanies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedCompanies_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedJobs",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    JobId = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedJobs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "Address", "CareersUrl", "Categories", "Category", "ChennaiLocations", "ChennaiPresence", "ChennaiRelevanceScore", "City", "CompanyType", "CompanyTypes", "ConfidenceScore", "Country", "CreatedAt", "Description", "DiscoveredAt", "EmployeeCount", "EmployeeRange", "FoundedYear", "FundingStage", "Headquarters", "HiringStatus", "Hub", "Industry", "IsActive", "IsFeatured", "IsSeedData", "LastVerifiedAt", "Latitude", "Logo", "LogoUrl", "Longitude", "MapPrecision", "Name", "NormalizedName", "OfficialCareersUrl", "OfficialWebsite", "ShortDescription", "Skills", "Slug", "SourceName", "SourceRecordId", "SourceType", "SourceUrl", "State", "SubCategory", "Tagline", "Tags", "TechStack", "TechnologyTags", "TotalFundingRaised", "UpdatedAt", "VerificationMethod", "VerificationStatus", "Website" },
                values: new object[,]
                {
                    { "comp-1", "Estancia IT Park, Plot No. 140 & 151, GST Road / OMR Corridor, Chennai", "https://www.zoho.com/careers/", "SaaS / Enterprise Software,DeepTech & AI", "SaaS / Enterprise Software", "", "Headquarters / Technology Center", 100, "Chennai", "PRODUCT COMPANY", "PRODUCT COMPANY,ENTERPRISE,STARTUP", 95, "India", new DateTime(2026, 9, 12, 6, 2, 19, 59, DateTimeKind.Utc).AddTicks(2307), "Zoho offers a comprehensive suite of cloud software applications for businesses worldwide. Bootstrapped from Chennai with tens of millions of users across 150+ countries.", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "15,000+", "15,000+", 1996, "Bootstrapped", "Chennai, Tamil Nadu", "Hiring Surge", "OMR (IT Corridor)", "Technology", true, true, true, new DateTime(2026, 9, 12, 6, 2, 19, 63, DateTimeKind.Utc).AddTicks(8863), 12.825200000000001, "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80", "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80", 80.043499999999995, "exact", "Zoho Corporation", "zoho corporation", "https://www.zoho.com/careers/", "https://www.zoho.com", "Bootstrapped Global SaaS Titan & Tech Powerhouse", new List<string> { "Java", "C++", "React", "Python", "PostgreSQL" }, "zoho", "Company Careers", null, "OFFICIAL_WEBSITE", "https://www.zoho.com/careers/", "Tamil Nadu", null, "Bootstrapped Global SaaS Titan & Tech Powerhouse", "SaaS,Cloud CRM,Bootstrapped", "Java,C++,React,Python,PostgreSQL", new List<string> { "SaaS", "Cloud CRM", "Bootstrapped" }, "Self-Funded ($1B+ Annual Revenue)", new DateTime(2026, 9, 12, 6, 2, 19, 59, DateTimeKind.Utc).AddTicks(2308), "OFFICIAL_DOMAIN_AUDIT", "VERIFIED", "https://www.zoho.com" },
                    { "comp-2", "Block B, Global Infocity Park, 40 MGR Salai, Perungudi, Chennai 600096", "https://www.freshworks.com/company/careers/", "SaaS / Enterprise Software,DeepTech & AI", "SaaS / Enterprise Software", "", "Headquarters / Technology Center", 100, "Chennai", "PRODUCT COMPANY", "PRODUCT COMPANY,MNC,ENTERPRISE", 95, "India", new DateTime(2026, 9, 12, 6, 2, 19, 64, DateTimeKind.Utc).AddTicks(327), "Born in Chennai, Freshworks makes innovative customer engagement and ITSM software for over 60,000 businesses globally.", new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), "5,000+", "5,000+", 2010, "Public / IPO", "Chennai, Tamil Nadu", "Active", "Perungudi & Kandanchavadi", "Technology", true, true, true, new DateTime(2026, 9, 12, 6, 2, 19, 64, DateTimeKind.Utc).AddTicks(439), 12.964399999999999, "https://images.unsplash.com/photo-1614680376593-902f749f7ffc?w=128&auto=format&fit=crop&q=80", "https://images.unsplash.com/photo-1614680376593-902f749f7ffc?w=128&auto=format&fit=crop&q=80", 80.242699999999999, "exact", "Freshworks", "freshworks", "https://www.freshworks.com/company/careers/", "https://www.freshworks.com", "Modern AI-powered Customer Experience & IT Service Software", new List<string> { "Ruby on Rails", "Java", "React", "AWS", "Python" }, "freshworks", "Company Careers", null, "OFFICIAL_WEBSITE", "https://www.freshworks.com/company/careers/", "Tamil Nadu", null, "Modern AI-powered Customer Experience & IT Service Software", "SaaS,Nasdaq Listed,CRM", "Ruby on Rails,Java,React,AWS,Python", new List<string> { "SaaS", "Nasdaq Listed", "CRM" }, "Nasdaq: FRSH ($1.03B IPO)", new DateTime(2026, 9, 12, 6, 2, 19, 64, DateTimeKind.Utc).AddTicks(327), "OFFICIAL_DOMAIN_AUDIT", "VERIFIED", "https://www.freshworks.com" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CompanyId", "CreatedAt", "Email", "IsVerified", "LastLogin", "Name", "PasswordHash", "RefreshToken", "RefreshTokenExpiry", "Role", "UpdatedAt" },
                values: new object[,]
                {
                    { "usr-admin-1", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@chennaistartups.in", true, null, "Chennai Admin", "vN/1fEw76LdJ14wXlD3F14l5f9U1lH/eQ+F5uK4jE1w=", null, null, "ADMIN", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "usr-demo-1", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "user@chennaistartups.in", true, null, "Karthik Developer", "vN/1fEw76LdJ14wXlD3F14l5f9U1lH/eQ+F5uK4jE1w=", null, null, "USER", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "usr-mod-1", null, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "moderator@chennaistartups.in", true, null, "Ecosystem Moderator", "vN/1fEw76LdJ14wXlD3F14l5f9U1lH/eQ+F5uK4jE1w=", null, null, "MODERATOR", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { "usr-recruiter-1", "comp-1", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "recruiter@zoho.com", true, null, "Zoho Talent Team", "vN/1fEw76LdJ14wXlD3F14l5f9U1lH/eQ+F5uK4jE1w=", null, null, "RECRUITER", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Jobs",
                columns: new[] { "Id", "ApplyUrl", "ChennaiRelevance", "CompanyHub", "CompanyId", "CompanyLogo", "CompanyName", "CreatedAt", "DescriptionSnippet", "DuplicateGroupId", "EngineeringSubcategory", "ExperienceLevel", "ExperienceMax", "ExperienceMin", "ExpiresAt", "FirstSeenAt", "FresherConfidence", "FreshnessStatus", "IsActive", "IsEngineering", "IsFeatured", "IsFresher", "IsInternship", "IsSeedData", "JobType", "LastSeenAt", "LastVerifiedAt", "Location", "NormalizedTitle", "OriginalUrl", "PrimaryCategory", "RelevanceConfidence", "SalaryCurrency", "SalaryMax", "SalaryMin", "SalaryRange", "Slug", "SourceName", "SourceRecordId", "Technologies", "Title", "UpdatedAt", "VerificationStatus", "WorkplaceType" },
                values: new object[,]
                {
                    { "job-1", "https://www.zoho.com/careers/job-details.html?id=fresher-dev-chennai", "CHENNAI_CONFIRMED", "OMR (IT Corridor)", "comp-1", "https://images.unsplash.com/photo-1618005182384-a83a8bd57fbe?w=128&auto=format&fit=crop&q=80", "Zoho Corporation", new DateTime(2026, 9, 12, 6, 2, 19, 66, DateTimeKind.Utc).AddTicks(1829), "We are hiring fresh engineering graduates to work on Zoho core product suites, database engines, and AI applications. Open to 0-1 years of experience in Java, C++, or Python.", null, "Software Engineering", "Fresher / Entry (0-1 yrs)", 1.0, 0.0, null, new DateTime(2026, 9, 10, 6, 2, 19, 69, DateTimeKind.Utc).AddTicks(2196), 98, "NEW", true, true, true, true, false, true, "Full-time", new DateTime(2026, 9, 12, 6, 2, 19, 69, DateTimeKind.Utc).AddTicks(3052), new DateTime(2026, 9, 12, 6, 2, 19, 69, DateTimeKind.Utc).AddTicks(3685), "Estancia IT Park / OMR, Chennai", "associate software developer freshers 2025 2026", "https://www.zoho.com/careers/job-details.html?id=fresher-dev-chennai", "SaaS / Enterprise Software", 100, "INR", 950000.0, 650000.0, "₹6,50,000 - ₹9,50,000 / yr", "zoho-associate-software-developer-fresher", "Company Careers", null, "Java,C++,Python,SQL", "Associate Software Developer (Freshers 2025/2026)", new DateTime(2026, 9, 12, 6, 2, 19, 66, DateTimeKind.Utc).AddTicks(1829), "VERIFIED", "On-site" },
                    { "job-2", "https://www.freshworks.com/company/careers/job-frontend-intern", "CHENNAI_CONFIRMED", "Perungudi & Kandanchavadi", "comp-2", "https://images.unsplash.com/photo-1614680376593-902f749f7ffc?w=128&auto=format&fit=crop&q=80", "Freshworks", new DateTime(2026, 9, 12, 6, 2, 19, 69, DateTimeKind.Utc).AddTicks(7391), "Looking for a passionate React frontend intern to build slick customer experience UI components for Freshdesk.", null, "Frontend", "Fresher / Entry (0-1 yrs)", 0.5, 0.0, null, new DateTime(2026, 9, 11, 6, 2, 19, 69, DateTimeKind.Utc).AddTicks(7430), 95, "NEW", true, true, true, true, true, true, "Internship", new DateTime(2026, 9, 12, 6, 2, 19, 69, DateTimeKind.Utc).AddTicks(7450), new DateTime(2026, 9, 12, 6, 2, 19, 69, DateTimeKind.Utc).AddTicks(7450), "Global Infocity, Perungudi, OMR, Chennai", "frontend engineer intern react typescript", "https://www.freshworks.com/company/careers/job-frontend-intern", "SaaS / Enterprise Software", 100, "INR", null, null, "₹35,000 / month Stipend", "freshworks-frontend-engineer-intern", "Company Careers", null, "React,TypeScript,JavaScript", "Frontend Engineer Intern (React / TypeScript)", new DateTime(2026, 9, 12, 6, 2, 19, 69, DateTimeKind.Utc).AddTicks(7391), "VERIFIED", "Hybrid" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_CreatedAt",
                table: "AnalyticsEvents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AnalyticsEvents_EventType",
                table: "AnalyticsEvents",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_CareerSources_CompanyId",
                table: "CareerSources",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_City",
                table: "Companies",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_HiringStatus",
                table: "Companies",
                column: "HiringStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Hub",
                table: "Companies",
                column: "Hub");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Hub_VerificationStatus",
                table: "Companies",
                columns: new[] { "Hub", "VerificationStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Industry",
                table: "Companies",
                column: "Industry");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IsActive",
                table: "Companies",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IsActive_FoundedYear",
                table: "Companies",
                columns: new[] { "IsActive", "FoundedYear" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IsActive_HiringStatus",
                table: "Companies",
                columns: new[] { "IsActive", "HiringStatus" });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_NormalizedName",
                table: "Companies",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Slug",
                table: "Companies",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_VerificationStatus",
                table: "Companies",
                column: "VerificationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Website",
                table: "Companies",
                column: "Website");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClaims_CompanyId",
                table: "CompanyClaims",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyClaims_UserId",
                table: "CompanyClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanySources_CompanyId",
                table: "CompanySources",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyHub",
                table: "Jobs",
                column: "CompanyHub");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyId",
                table: "Jobs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyId_IsActive",
                table: "Jobs",
                columns: new[] { "CompanyId", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_FreshnessStatus",
                table: "Jobs",
                column: "FreshnessStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsActive_IsFresher",
                table: "Jobs",
                columns: new[] { "IsActive", "IsFresher" });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsEngineering",
                table: "Jobs",
                column: "IsEngineering");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_IsFresher",
                table: "Jobs",
                column: "IsFresher");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_NormalizedTitle",
                table: "Jobs",
                column: "NormalizedTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_Slug",
                table: "Jobs",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedCompanies_CompanyId",
                table: "SavedCompanies",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedCompanies_UserId_CompanyId",
                table: "SavedCompanies",
                columns: new[] { "UserId", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedJobs_JobId",
                table: "SavedJobs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedJobs_UserId_JobId",
                table: "SavedJobs",
                columns: new[] { "UserId", "JobId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AnalyticsEvents");

            migrationBuilder.DropTable(
                name: "CareerSources");

            migrationBuilder.DropTable(
                name: "CompanyClaims");

            migrationBuilder.DropTable(
                name: "CompanySources");

            migrationBuilder.DropTable(
                name: "IngestionRuns");

            migrationBuilder.DropTable(
                name: "JobAlerts");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RawIngestionRecords");

            migrationBuilder.DropTable(
                name: "SavedCompanies");

            migrationBuilder.DropTable(
                name: "SavedJobs");

            migrationBuilder.DropTable(
                name: "Submissions");

            migrationBuilder.DropTable(
                name: "Technologies");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
