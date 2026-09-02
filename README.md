# Chennai Startup & Jobs Map

> An independent Chennai-focused company, startup, tech ecosystem, and career discovery platform.

[![Status](https://img.shields.io/badge/status-Milestones--8--9--10--Complete-emerald)](#current-status)
[![Frontend](https://img.shields.io/badge/frontend-React%2018%20%7C%20TypeScript%20%7C%20Vite%20%7C%20Tailwind-blue)](#technology-stack)
[![Backend](https://img.shields.io/badge/backend-ASP.NET%20Core%20Web%20API%20%7C%20.NET%2010-purple)](#technology-stack)
[![Swagger](https://img.shields.io/badge/OpenAPI%20v3-Interactive%20Swagger%20UI-brightgreen)](http://localhost:5241/swagger)
[![Hangfire](https://img.shields.io/badge/jobs-Hangfire%20Scheduler-red)](http://localhost:5241/hangfire)
[![Database](https://img.shields.io/badge/database-EF%20Core%20%7C%20PostgreSQL-blue)](#technology-stack)

---

## Project Overview

**Chennai Startup & Jobs Map** is South Asia's premier SaaS and DeepTech discovery engine designed to help students, freshers, developers, and professionals discover tech companies, startups, career opportunities, and internships across Chennai's key corridors (OMR, Guindy, Siruseri, Ambattur, Porur, Perungudi, Thoraipakkam, Taramani, etc.).

---

## Architecture & Systems (Milestones 8 + 9 + 10)

```text
       ┌────────────────────────────────────────────────────────┐
       │             React + TypeScript Frontend                │
       │  (Companies Directory, Map Clustering, Job Boards,     │
       │   User Saved Jobs/Alerts, Recruiter Job Posting)       │
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │      ASP.NET Core Web API (.NET 10) — /api/v1/         │
       │   (Swagger UI with JWT Bearer, Global Error Handling)  │
       └───────┬───────────────────┬────────────────────┬───────┘
               │                   │                    │
               ▼                   ▼                    ▼
       ┌───────────────┐   ┌───────────────┐   ┌────────────────┐
       │ User Platform │   │ Recruiter     │   │ Platform       │
       │ & Bookmarks   │   │ Portal &      │   │ Analytics      │
       │ (/users/me)   │   │ Claims (/rec) │   │ (/analytics)   │
       └───────┬───────┘   └───────┬───────┘   └────────┬───────┘
               │                   │                    │
               └───────────────────┼────────────────────┘
                                   ▼
       ┌────────────────────────────────────────────────────────┐
       │              Entity Framework Core Relational          │
       │     (Companies, Jobs, CompanySources, CareerSources,   │
       │      SavedJobs, SavedCompanies, Alerts, Claims)        │
       └────────────────────────────────────────────────────────┘
```

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

### Backend xUnit Unit Tests (15/15 Passed)
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
