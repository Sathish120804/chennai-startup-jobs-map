# Chennai Startup & Jobs Map

> An independent Chennai-focused company, startup, tech ecosystem, and career discovery platform.

[![Status](https://img.shields.io/badge/status-Milestone--11--Complete-emerald)](#current-status)
[![Frontend](https://img.shields.io/badge/frontend-React%2018%20%7C%20TypeScript%20%7C%20Vite%20%7C%20Tailwind-blue)](#technology-stack)
[![Backend](https://img.shields.io/badge/backend-ASP.NET%20Core%20Web%20API%20%7C%20.NET%2010-purple)](#technology-stack)
[![Swagger](https://img.shields.io/badge/OpenAPI%20v3-Interactive%20Swagger%20UI-brightgreen)](http://localhost:5241/swagger)
[![Database](https://img.shields.io/badge/database-PostgreSQL%20%7C%20EF%20Core%20Migrations-blue)](#technology-stack)
[![Map](https://img.shields.io/badge/map-OpenStreetMap%20%2B%20Leaflet%20Clustering-brightgreen)](#map-scalability)

---

## Project Overview

**Chennai Startup & Jobs Map** is South Asia's premier SaaS and DeepTech discovery engine designed to help students, freshers, developers, and professionals discover tech companies, startups, career opportunities, and internships across Chennai's key corridors (OMR, Guindy, Siruseri, Ambattur, Porur, Perungudi, Thoraipakkam, Taramani, etc.).

---

## Architecture & Systems (Milestone 11)

```text
       ┌────────────────────────────────────────────────────────┐
       │             React + TypeScript Frontend                │
       │  (OSM Keyless Map, Marker Clustering, Bulk Importer,   │
       │   Quality Dashboard, Directory, Recruiter Portal)      │
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │      ASP.NET Core Web API (.NET 10) — /api/v1/         │
       │   (Bulk Ingestion, Data Quality, N+1 Query Optimizer)   │
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │         PostgreSQL Database via EF Core Migrations     │
       │   (Companies, Jobs, Sources, Composite Performance     │
       │    Indexes on Hubs, Verification, Freshness & Roles)   │
       └────────────────────────────────────────────────────────┘
```

---

## Milestone 11 Key Highlights

### 1. Production Data Foundation & PostgreSQL Migration
- Configured resilient EF Core DbContext supporting PostgreSQL (`Npgsql.EntityFrameworkCore.PostgreSQL` 10.0.3) with In-Memory fallback for development and testing.
- Created EF Core initial migration `20260912060222_InitialCreate` with composite performance indexes:
  - `(Hub, VerificationStatus)`
  - `(IsActive, HiringStatus)`
  - `(IsActive, FoundedYear)`
  - `(CompanyId, IsActive)`
  - `(IsActive, IsFresher)`

### 2. $N+1$ Query Elimination
- Replaced per-company individual job count queries in `CompanyService.GetCompaniesAsync` with a single correlated database `GroupBy(j => j.CompanyId)` aggregate query, slashing database round trips from $O(N)$ to $O(1)$.

### 3. Keyless Map Scalability & Marker Clustering
- Migrated map tile layers to standard, keyless OpenStreetMap (OSM) tiles (`https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png`) with proper attribution — requiring zero commercial API keys.
- Integrated `leaflet.markercluster` with chunked rendering (`chunkedLoading: true`, `maxClusterRadius: 50`) and custom styled cluster pins to smoothly support 100 to 1,000+ companies.

### 4. Admin Bulk Import Pipeline & Data Quality Dashboard
- **Admin CSV / JSON Bulk Ingestion**: `POST /api/v1/admin/import/companies/csv` and `POST /api/v1/admin/import/companies/json` with `dryRun=true` preview support, RFC 4180 parsing, 10MB upload limits, and domain/slug deduplication.
- **Data Quality Dashboard**: `GET /api/v1/admin/quality/dashboard` tracking verified count against the 700+ target goal, missing careers/coordinates, and sector distributions.
- **Frontend Admin Integration**: Two new dedicated subtabs for **Company Bulk Import** (file drag & drop, raw paste, preview report) and **Data Quality Dashboard** (target goal meter, health cards, sector breakdown).

### 5. Verified Real Chennai Tech Directory
- Seed dataset expanded to **105+ verified real companies** across all 15 key sectors (MNCs, GCCs, SaaS, FinTech, DeepTech, AutoTech, HealthTech, EdTech, Semiconductor, etc.) adhering to a strict anti-hallucination policy with authentic Chennai tech park presences.

---

## Key Milestone 8, 9, 10 Deliverables

### 1. Verified Chennai Company Directory & Career Sources
- Source-backed real company records covering:
  - **MNCs & Global Capability Centers (GCCs)**: Amazon, Microsoft IDC, PayPal, Cisco, Ford, Caterpillar, Shell, BNY Mellon, Standard Chartered, AstraZeneca, Barclays, Citi, Cognizant, TCS, Infosys, Wipro, HCLTech, LTIMindtree, Hexaware, Aspire Systems, Siemens Healthineers, Trimble, Verizon, Alstom, Qualcomm.
  - **SaaS & Cloud Titans**: Zoho, Freshworks, Kissflow, Chargebee, Facilio, Hippo Video, Kaar Tech, Ramco Systems, Intellect Design, Kovai.co, SuperOps.ai, GoFrugal.
  - **DeepTech, AI & Robotics**: Agnikul Cosmos, The ePlane Company, Detect Technologies, Mad Street Den, Mindgrove, Planys Technologies, Uniphore.
  - **FinTech & BFSI Tech**: BankBazaar, M2P Fintech, Kaleidofin, Financial Software & Systems (FSS).
  - **HealthTech, EdTech & AutoTech**: Apollo 24|7, GUVI, Skill-Lync, Ather Energy, TVS Motor Digital, Raptee, Matrimony.com, CaratLane, WayCool, Pickyourtrail, Sulekha.
- **Career Sources & ATS Tracking**: Maps official career portals and ATS providers (Workday, Lever, Greenhouse, SmartRecruiters) with direct "View Careers" links.
- **Company Source Provenance**: Tracks source name, URL, verification timestamp, and confidence rating.

### 2. User Platform & Personalization (`/api/v1/users/me`)
- **Saved Jobs**: `GET /api/v1/users/me/saved-jobs`, `POST .../{jobId}`, `DELETE .../{jobId}`
- **Saved Companies**: `GET /api/v1/users/me/saved-companies`, `POST .../{companyId}`, `DELETE .../{companyId}`
- **Job Alerts**: `GET /api/v1/users/me/job-alerts`, `POST ...`, `DELETE .../{id}` (`Daily` / `Weekly` frequencies)
- **User Notifications**: `GET /api/v1/notifications`, `PUT /api/v1/notifications/{id}/read`

### 3. Recruiter Portal & Claim Moderation (`/api/v1/recruiters`)
- **Company Claiming**: Recruiters submit formal corporate ownership claims (`PENDING` state until Admin approval).
- **Direct Job Posting**: Recruiters post vacancies that start as `PENDING_REVIEW` to ensure strict content verification and eliminate spam.
- **Moderation Actions**: Admins can approve, reject, or verify claims and jobs (`/api/v1/admin/jobs/{id}/approve`, `reject`).

### 4. Privacy-Conscious Platform Analytics (`/api/v1/analytics`)
- Logs aggregate, anonymized interaction events: `SEARCH`, `JOB_VIEW`, `COMPANY_VIEW`, and `APPLY_CLICK`.
- Admin overview dashboard (`GET /api/v1/admin/analytics/overview`) summarizes platform trends without collecting personal information.

### 5. Production Swagger / OpenAPI Portal (`/swagger`)
- Complete API explorer with JWT Bearer authentication (`Bearer <token>`).
- Grouped by tags:
  - `Authentication & Identity`
  - `User Platform & Preferences`
  - `User Notifications`
  - `Recruiter & Company Portal`
  - `Platform Analytics`
  - `Companies & Startups`
  - `Jobs & Internships`
  - `AI Recommendations`
  - `Unified Search`
  - `Community Submissions`
  - `Admin and Moderation`
  - `Health Checks`

---

## Creator Attribution & Personal Branding

- **Creator Attribution**:
  > *"Built by an unsuccessful engineer — Sathish A"*  
  > *"Still looking for the opportunity. Helping others find theirs along the way."*
- **Creator LinkedIn**: [https://www.linkedin.com/in/sathish-a-3204aa27b/](https://www.linkedin.com/in/sathish-a-3204aa27b/)
- **Creator GitHub**: Configured via `CREATOR_PROFILE.githubUrl` in [`Footer.tsx`](file:///c:/Users/sathi/OneDrive/Desktop/chennai-startup-jobs-map/src/components/layout/Footer.tsx).
- **Creator Story**: Prominently featured on the homepage: *"Finding opportunities shouldn't be harder than finding talent."*

---

## Automated Test Suites

### Backend xUnit Unit Tests (19/19 Passed)
```bash
dotnet test backend/ChennaiStartupJobsMap.Tests/ChennaiStartupJobsMap.Tests.csproj
```

### Frontend Vitest Engine Tests (9/9 Passed)
```bash
npm test
```

### Production Build
```bash
npm run build
```
