# Chennai Startup & Jobs Map

> An independent Chennai-focused startup, company, tech ecosystem, and automated job discovery platform.

[![Status](https://img.shields.io/badge/status-Milestone--6--Enterprise--Complete-emerald)](#current-status)
[![Frontend](https://img.shields.io/badge/frontend-React%2018%20%7C%20TypeScript%20%7C%20Vite%20%7C%20Tailwind-blue)](#technology-stack)
[![Backend](https://img.shields.io/badge/backend-ASP.NET%20Core%20Web%20API%20%7C%20.NET%2010-purple)](#technology-stack)
[![Swagger](https://img.shields.io/badge/OpenAPI%20v3-Swagger%20UI-brightgreen)](http://localhost:5241/swagger)
[![Hangfire](https://img.shields.io/badge/jobs-Hangfire%20Scheduler-red)](http://localhost:5241/hangfire)
[![Database](https://img.shields.io/badge/database-EF%20Core%20%7C%20PostgreSQL-blue)](#technology-stack)

---

## Project Overview

**Chennai Startup & Jobs Map** is South Asia's premier SaaS and DeepTech discovery engine designed to help students, freshers, developers, and professionals discover tech companies, startups, career opportunities, and internships across Chennai's key corridors (OMR, Guindy, Siruseri, Ambattur, Porur, Perungudi, Thoraipakkam, Taramani, etc.).

---

## Enterprise Architecture Overview

```text
       ┌────────────────────────────────────────────────────────┐
       │             React + TypeScript Frontend                │
       │    (Leaflet Map, Search Bar, Filters, Company/Job Cards)│
       └──────────────────────────┬─────────────────────────────┘
                                  │
                   API Client Layer (VITE_API_BASE_URL)
             (With automatic Fallback Dev Mode when offline)
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │      ASP.NET Core Web API (.NET 10) — /api/v1/         │
       │  (Swagger UI, JWT Bearer, RateLimiter, CORS, Logging)  │
       └──────────┬───────────────────────────────┬─────────────┘
                  │                               │
                  ▼                               ▼
       ┌──────────────────────┐       ┌────────────────────────┐
       │ Hangfire Background  │       │ Role-Based Auth Engine │
       │ Job Scheduler        │       │ (Admin, Moderator,     │
       │ (/hangfire)          │       │  Recruiter, User)      │
       └──────────┬───────────┘       └───────────┬────────────┘
                  │                               │
                  └───────────────┬───────────────┘
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │        Entity Framework Core Relational Data Layer      │
       │     (Users, Companies, Jobs, IngestionRuns, Sources)   │
       └────────────────────────────────────────────────────────┘
```

---

## Milestone 6 Enterprise Features

1. **Swagger / OpenAPI v3 Portal (`/swagger`)**:
   - Interactive public API explorer with JWT Bearer authentication (`Bearer <token>`).
   - Grouped by tags: `Authentication & Identity`, `Companies & Startups`, `Jobs & Internships`, `Unified Search`, `Admin & Moderation`, `Community Submissions`, and `Health Checks`.
   - Comprehensive XML comments, parameter documentation, and response schemas.

2. **JWT Authentication & Refresh Tokens (`/api/v1/auth`)**:
   - `POST /api/v1/auth/register` — Register User / Recruiter.
   - `POST /api/v1/auth/login` — Authenticate and receive JWT access token (4-hour) & refresh token (7-day).
   - `POST /api/v1/auth/refresh` — Rotate refresh tokens and refresh access tokens.
   - `POST /api/v1/auth/logout` & `GET /api/v1/auth/me`.
   - Identity Password Hashing with cryptographic salting.

3. **Role-Based Policy Authorization**:
   - `ADMIN` — Full access to metrics, ingestion pipelines, triggers, and user management.
   - `MODERATOR` — Review and moderate incoming community company and job submissions.
   - `RECRUITER` — Manage company job vacancies and talent pipelines.
   - `USER` — Search, browse, save jobs, and submit new companies/jobs.

4. **Hangfire Background Job Scheduler (`/hangfire`)**:
   - `job-discovery-job` — Hourly automated Chennai career portal poller.
   - `verification-job` — Daily job status and link verification sweep.
   - `expire-stale-jobs-job` — Daily expiration of vacancies older than 60 days.

5. **Automated Data Ingestion & Quality Scoring**:
   - Source Registry supporting ATS, company career pages, and search feeds.
   - Company matching via domain, normalized name, and aliases with confidence rating.
   - Data quality scoring (0–100) with diagnostic breakdown.

6. **System Health & Observability (`/health` & `/api/v1/health`)**:
   - Live diagnostics for database connectivity, Hangfire engine, cache, and API latency.

7. **Containerization**:
   - Production multi-stage `Dockerfile` and `docker-compose.yml` for API and PostgreSQL.

---

## Default Seeded Accounts

For testing authentication in Swagger or via API:

| Role | Email | Password |
|---|---|---|
| **ADMIN** | `admin@chennaistartups.in` | `Chennai@2026` |
| **MODERATOR** | `moderator@chennaistartups.in` | `Chennai@2026` |
| **RECRUITER** | `recruiter@zoho.com` | `Chennai@2026` |
| **USER** | `user@chennaistartups.in` | `Chennai@2026` |

---

## Local Development & Setup

### Prerequisites
- Node.js v18+ & npm
- .NET 10 SDK (or .NET 8+)
- Docker & Docker Compose (optional for containerized deployment)

### Running Backend API (.NET)
```bash
cd backend/ChennaiStartupJobsMap.Api
dotnet run
```
- API Base: `http://localhost:5241/api/v1`
- Swagger UI: `http://localhost:5241/swagger`
- Hangfire Dashboard: `http://localhost:5241/hangfire`
- Health Endpoint: `http://localhost:5241/health`

### Running Frontend (React)
```bash
npm install
npm run dev
```
Open `http://localhost:5173`.

### Running with Docker Compose
```bash
docker-compose up --build
```

---

## Automated Test Suites

### Backend xUnit Unit Tests (8/8 Passed)
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
