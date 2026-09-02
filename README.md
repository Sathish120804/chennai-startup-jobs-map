# Chennai Startup & Jobs Map

> An independent Chennai-focused startup, company, tech ecosystem, and intelligent job discovery platform.

[![Status](https://img.shields.io/badge/status-Milestone--7--Complete-emerald)](#current-status)
[![Frontend](https://img.shields.io/badge/frontend-React%2018%20%7C%20TypeScript%20%7C%20Vite%20%7C%20Tailwind-blue)](#technology-stack)
[![Backend](https://img.shields.io/badge/backend-ASP.NET%20Core%20Web%20API%20%7C%20.NET%2010-purple)](#technology-stack)
[![AI Search](https://img.shields.io/badge/AI-Semantic%20Search%20%26%20Recommendations-violet)](#ai-architecture)
[![Swagger](https://img.shields.io/badge/OpenAPI%20v3-Swagger%20UI-brightgreen)](http://localhost:5241/swagger)
[![Hangfire](https://img.shields.io/badge/jobs-Hangfire%20Scheduler-red)](http://localhost:5241/hangfire)
[![Database](https://img.shields.io/badge/database-EF%20Core%20%7C%20PostgreSQL-blue)](#technology-stack)

---

## Project Overview

**Chennai Startup & Jobs Map** is South Asia's premier SaaS and DeepTech discovery engine designed to help students, freshers, developers, and professionals discover tech companies, startups, career opportunities, and internships across Chennai's key corridors (OMR, Guindy, Siruseri, Ambattur, Porur, Perungudi, Thoraipakkam, Taramani, etc.).

---

## AI Architecture & Semantic Search (Milestone 7)

```text
       ┌────────────────────────────────────────────────────────┐
       │                 User Search / Natural Query            │
       │           ("React internship OMR", ".NET fresher")     │
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │              Semantic Concept Normalization            │
       │       (Maps synonyms, skills, fresher/intern intent)   │
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │              Embedding Provider Abstraction            │
       │       (Deterministic 64-dim vectors with cosine sim)   │
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │              Hybrid Relevance Ranking Engine           │
       │  FinalScore = (0.3*Keyword) + (0.3*Semantic) +         │
       │               (0.2*Location) + (0.1*Freshness) +       │
       │               (0.1*Verification)                       │
       └──────────────────────────┬─────────────────────────────┘
                                  │
                                  ▼
       ┌────────────────────────────────────────────────────────┐
       │           Explainable Recommendations Output           │
       │    ("Why this matches", Match score, Ranked Jobs)      │
       └────────────────────────────────────────────────────────┘
```

### 1. Semantic Concept Matching
- Maps abstract queries such as `"backend developer"` to technical requirements (`.NET`, `C#`, `Java`, `Spring Boot`, `Node.js`, `Python`).
- Maps `"frontend developer"` to `React`, `TypeScript`, `Angular`, and `UI/UX`.
- Maps `"machine learning"` / `"AI engineer"` to `Python`, `TensorFlow`, `PyTorch`, and `NLP`.

### 2. Explainable AI Match Signals
For every recommended position, transparent explanations are generated directly from ground-truth data:
- *"Matches your .NET skill"*
- *"Entry-level / Fresher opportunity"*
- *"Office located in OMR (IT Corridor)"*
- *"Directly verified application link"*

### 3. Graceful Offline Fallback
- No external AI API key or third-party cloud subscription is required for local development or testing.
- When external LLMs are unconfigured or unavailable, the deterministic embedding provider runs locally with high-performance cached cosine similarity.

---

## Creator Story & Personal Branding

The platform includes an authentic founder section and footer attribution:

### Homepage Creator Story
> **WHY I BUILT THIS**  
> *"Finding opportunities shouldn't be harder than finding talent."*  
> Built from the perspective of an engineer who knows how difficult it can be to find that first opportunity. The goal is simple: make it easier to discover the companies, people, and opportunities that are already out there.  
> *"Let's take you to where the opportunities are."*

### Configurable Footer Attribution
```text
Built by an unsuccessful engineer — Sathish A
"Still looking for the opportunity. Helping others find theirs along the way."
GitHub • LinkedIn
```
Configurable profile URLs are centralized in [`src/components/layout/Footer.tsx`](file:///c:/Users/sathi/OneDrive/Desktop/chennai-startup-jobs-map/src/components/layout/Footer.tsx) (`CREATOR_PROFILE.githubUrl` and `CREATOR_PROFILE.linkedinUrl`).

---

## Enterprise API Endpoints

### Recommendations & AI
- `GET /api/v1/recommendations/jobs` — Scored job recommendations with explainable match reasons.
- `GET /api/v1/recommendations/companies` — Scored company recommendations.
- `GET /api/v1/search` — Unified natural language search with intent parsing.

### Authentication & Core CRUD
- `POST /api/v1/auth/register`, `/login`, `/refresh`, `/logout`, `/me`
- `GET /api/v1/companies`, `/api/v1/companies/{id}`, `/api/v1/companies/slug/{slug}`
- `GET /api/v1/jobs`, `/api/v1/jobs/{id}`
- `POST /api/v1/submissions/company`, `/api/v1/submissions/job`
- `GET /api/v1/admin/metrics`, `/api/v1/admin/ingestion/runs`
- `GET /health` & `/api/v1/health`

---

## Automated Test Suites

### Backend xUnit Unit Tests (10/10 Passed)
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
