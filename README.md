# Chennai Startup & Jobs Map

> An independent Chennai-focused startup, company, tech ecosystem, and automated job discovery platform.

[![Status](https://img.shields.io/badge/status-Milestone--5--Complete-emerald)](#current-status)
[![Frontend](https://img.shields.io/badge/frontend-React%2018%20%7C%20TypeScript%20%7C%20Vite%20%7C%20Tailwind-blue)](#technology-stack)
[![Backend](https://img.shields.io/badge/backend-ASP.NET%20Core%20Web%20API%20%7C%20.NET%2010-purple)](#technology-stack)
[![Database](https://img.shields.io/badge/database-EF%20Core%20%7C%20PostgreSQL-blue)](#technology-stack)

---

## Project Overview

**Chennai Startup & Jobs Map** is South Asia's premier SaaS and DeepTech discovery engine designed to help students, freshers, developers, and professionals discover tech companies, startups, career opportunities, and internships across Chennai's key corridors (OMR, Guindy, Siruseri, Ambattur, Porur, Perungudi, Thoraipakkam, Taramani, etc.).

---

## Architecture Overview

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
       │             ASP.NET Core Web API (.NET 10)            │
       │  (CompaniesController, JobsController, SearchController)│
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │             Automated Data Ingestion Pipeline          │
       │ (Discovery -> Normalize -> Match -> Deduplicate -> Verify)│
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │        Entity Framework Core Relational Data Layer      │
       │     (Companies, Jobs, Technologies, IngestionRuns)     │
       └────────────────────────────────────────────────────────┘
```

---

## Key Milestone 5 Features

1. **Automated Ingestion Pipeline (`IngestionPipelineService.cs`)**:
   - Source Registry supporting `COMPANY_CAREERS`, `GREENHOUSE_ATS`, `LEVER_ATS`, `WORKDAY_ATS`, `AUTHORIZED_SEARCH_API`, `USER_SUBMISSION`.
   - Ingestion run tracking (`IngestionRun` model) & raw payload logging.
   - Idempotent execution: running discovery repeatedly updates existing postings without creating duplicates.
2. **Title, Company & Location Normalization (`NormalizationService.cs`)**:
   - Extracts technology tags (`.NET`, `React`, `Python`, `Java`, `Spring Boot`, `AWS`, `PostgreSQL`, `C#`).
   - Rule-based fresher classification & internship detection.
3. **Company Matching & Job Deduplication (`CompanyMatcher.cs`)**:
   - Matches incoming job postings to existing Chennai companies via domain names, normalized titles, and aliases with confidence scoring (`HIGH`, `MEDIUM`, `LOW`).
   - Preserves multi-portal source attribution (`JobSourceRecord`).
4. **Data Quality Scoring (`DataQualityService.cs`)**:
   - Computes score (0–100) based on verified entity links, geocoded coordinates, apply URL validity, and location signals.
5. **Full-Stack ASP.NET Core REST API (`backend/ChennaiStartupJobsMap.Api`)**:
   - `GET /api/companies` (with search, hubs, categories, hiring, tech, pagination).
   - `GET /api/jobs` (with search, hubs, categories, fresher, internship, tech, pagination).
   - `GET /api/search` (unified query with search intent parsing).
   - `POST /api/submissions/company` & `POST /api/submissions/job`.
   - `GET /api/admin/metrics`, `GET /api/admin/ingestion/runs`, `POST /api/admin/ingestion/trigger`.

---

## Local Development & Setup

### Prerequisites
- Node.js v18+ & npm
- .NET 10 SDK (or .NET 8+)

### Running Frontend
```bash
npm install
npm run dev
```
Open `http://localhost:5173`.

### Running ASP.NET Core Web API Backend
```bash
cd backend/ChennaiStartupJobsMap.Api
dotnet run
```
API running on `http://localhost:5241/api`.

### Running Tests

#### Backend xUnit Unit Tests
```bash
dotnet test backend/ChennaiStartupJobsMap.Tests/ChennaiStartupJobsMap.Tests.csproj
```

#### Frontend Vitest Unit Tests
```bash
npm test
```

#### Production Build
```bash
npm run build
```

---

## Environment Variables

Copy `.env.example` to `.env`:

```env
VITE_API_BASE_URL=http://localhost:5241/api
DATABASE_CONNECTION_STRING=Host=localhost;Database=chennaistartupjobs;Username=postgres;Password=postgres
```
