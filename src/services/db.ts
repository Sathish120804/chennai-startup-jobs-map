import { 
  Company, 
  Job, 
  EcosystemNews, 
  UserSubmission, 
  JobDiscoveryQuery, 
  FilterState, 
  CompanyJobStats,
  TechHub
} from '../types';
import { INITIAL_COMPANIES, INITIAL_JOBS, INITIAL_NEWS } from './mockData';
import { DEFAULT_DISCOVERY_QUERIES } from './discoveryEngine';
import { analyzeChennaiRelevance } from './relevanceEngine';
import { classifyJob } from './classifierEngine';
import { calculateJobFreshness } from './freshnessEngine';
import { detectJobDuplicates } from './deduplicationEngine';

const STORAGE_KEYS = {
  COMPANIES: 'csjm_companies_v1',
  JOBS: 'csjm_jobs_v1',
  NEWS: 'csjm_news_v1',
  SUBMISSIONS: 'csjm_submissions_v1',
  DISCOVERY_QUERIES: 'csjm_discovery_queries_v1',
};

class DatabaseService {
  private companies: Company[] = [];
  private jobs: Job[] = [];
  private news: EcosystemNews[] = [];
  private submissions: UserSubmission[] = [];
  private discoveryQueries: JobDiscoveryQuery[] = [];
  private listeners: Set<() => void> = new Set();

  constructor() {
    this.init();
  }

  private init() {
    // Load Companies
    try {
      const storedCompanies = localStorage.getItem(STORAGE_KEYS.COMPANIES);
      this.companies = storedCompanies ? JSON.parse(storedCompanies) : INITIAL_COMPANIES;
    } catch {
      this.companies = INITIAL_COMPANIES;
    }

    // Load Jobs
    try {
      const storedJobs = localStorage.getItem(STORAGE_KEYS.JOBS);
      this.jobs = storedJobs ? JSON.parse(storedJobs) : INITIAL_JOBS;
    } catch {
      this.jobs = INITIAL_JOBS;
    }

    // Load News
    try {
      const storedNews = localStorage.getItem(STORAGE_KEYS.NEWS);
      this.news = storedNews ? JSON.parse(storedNews) : INITIAL_NEWS;
    } catch {
      this.news = INITIAL_NEWS;
    }

    // Load Submissions
    try {
      const storedSubs = localStorage.getItem(STORAGE_KEYS.SUBMISSIONS);
      this.submissions = storedSubs ? JSON.parse(storedSubs) : [];
    } catch {
      this.submissions = [];
    }

    // Load Discovery Queries
    try {
      const storedQueries = localStorage.getItem(STORAGE_KEYS.DISCOVERY_QUERIES);
      this.discoveryQueries = storedQueries ? JSON.parse(storedQueries) : DEFAULT_DISCOVERY_QUERIES;
    } catch {
      this.discoveryQueries = DEFAULT_DISCOVERY_QUERIES;
    }

    // Auto-refresh job freshness on load
    this.refreshAllJobFreshness();
  }

  public subscribe(listener: () => void) {
    this.listeners.add(listener);
    return () => {
      this.listeners.delete(listener);
    };
  }

  private notify() {
    this.listeners.forEach((l) => l());
  }

  private persist() {
    try {
      localStorage.setItem(STORAGE_KEYS.COMPANIES, JSON.stringify(this.companies));
      localStorage.setItem(STORAGE_KEYS.JOBS, JSON.stringify(this.jobs));
      localStorage.setItem(STORAGE_KEYS.NEWS, JSON.stringify(this.news));
      localStorage.setItem(STORAGE_KEYS.SUBMISSIONS, JSON.stringify(this.submissions));
      localStorage.setItem(STORAGE_KEYS.DISCOVERY_QUERIES, JSON.stringify(this.discoveryQueries));
    } catch (e) {
      console.error('Failed to persist database state', e);
    }
    this.notify();
  }

  // --- Dynamic Stats Engine ---
  public getCompanyStats(companyId: string): CompanyJobStats {
    const activeJobs = this.jobs.filter(
      (j) => j.companyId === companyId && j.freshnessStatus !== 'EXPIRED' && j.freshnessStatus !== 'REMOVED'
    );

    const engineeringJobs = activeJobs.filter((j) => j.isEngineering);
    const fresherJobs = activeJobs.filter((j) => j.isFresher);
    const internships = activeJobs.filter((j) => j.isInternship);

    const sortedByDate = [...activeJobs].sort(
      (a, b) => new Date(b.firstSeenAt).getTime() - new Date(a.firstSeenAt).getTime()
    );

    return {
      activeJobsCount: activeJobs.length,
      engineeringJobsCount: engineeringJobs.length,
      fresherJobsCount: fresherJobs.length,
      internshipsCount: internships.length,
      lastJobDiscoveredAt: sortedByDate[0]?.firstSeenAt,
    };
  }

  // --- Company Operations ---
  public getCompanies(): Company[] {
    return [...this.companies];
  }

  public getCompanyById(id: string): Company | undefined {
    return this.companies.find((c) => c.id === id);
  }

  public getCompanyBySlug(slug: string): Company | undefined {
    return this.companies.find((c) => c.slug === slug);
  }

  public getFilteredCompanies(filters: FilterState): Company[] {
    return this.companies.filter((company) => {
      const stats = this.getCompanyStats(company.id);

      // Search query (matches name, description, tags, techStack, hub, address)
      if (filters.searchQuery.trim()) {
        const q = filters.searchQuery.toLowerCase().trim();
        const matchesName = company.name.toLowerCase().includes(q);
        const matchesTagline = company.tagline.toLowerCase().includes(q);
        const matchesDesc = company.description.toLowerCase().includes(q);
        const matchesTags = company.tags.some((t) => t.toLowerCase().includes(q));
        const matchesTech = company.techStack.some((t) => t.toLowerCase().includes(q));
        const matchesHub = company.hub.toLowerCase().includes(q);

        // Also check if any of the company's jobs match the search query!
        const matchingJobs = this.jobs.some(
          (j) => j.companyId === company.id && (j.title.toLowerCase().includes(q) || j.technologies.some(t => t.toLowerCase().includes(q)))
        );

        if (!matchesName && !matchesTagline && !matchesDesc && !matchesTags && !matchesTech && !matchesHub && !matchingJobs) {
          return false;
        }
      }

      // Hub Filter
      if (filters.selectedHubs.length > 0 && !filters.selectedHubs.includes(company.hub)) {
        return false;
      }

      // Category Filter
      if (
        filters.selectedCategories.length > 0 &&
        !company.categories.some((c) => filters.selectedCategories.includes(c))
      ) {
        return false;
      }

      // Company Type Filter
      if (
        filters.selectedCompanyTypes.length > 0 &&
        !company.companyTypes.some((ct) => filters.selectedCompanyTypes.includes(ct))
      ) {
        return false;
      }

      // Funding Stage Filter
      if (
        filters.selectedFundingStages.length > 0 &&
        !filters.selectedFundingStages.includes(company.fundingStage)
      ) {
        return false;
      }

      // Hiring Only
      if (filters.isHiringOnly && stats.activeJobsCount === 0) {
        return false;
      }

      // Fresher Only
      if (filters.isFresherOnly && stats.fresherJobsCount === 0) {
        return false;
      }

      // Engineering Only
      if (filters.isEngineeringOnly && stats.engineeringJobsCount === 0) {
        return false;
      }

      // Internship Only
      if (filters.isInternshipOnly && stats.internshipsCount === 0) {
        return false;
      }

      // Featured Only
      if (filters.isFeaturedOnly && !company.isFeatured) {
        return false;
      }

      // Technology filter (if any tech is selected, company tech stack or job tech must contain it)
      if (filters.selectedTechnologies.length > 0) {
        const hasTech = filters.selectedTechnologies.some((t) =>
          company.techStack.includes(t) ||
          this.jobs.some((j) => j.companyId === company.id && j.technologies.includes(t))
        );
        if (!hasTech) return false;
      }

      return true;
    }).sort((a, b) => {
      if (filters.sortBy === 'featured') {
        if (a.isFeatured && !b.isFeatured) return -1;
        if (!a.isFeatured && b.isFeatured) return 1;
        return this.getCompanyStats(b.id).activeJobsCount - this.getCompanyStats(a.id).activeJobsCount;
      }
      if (filters.sortBy === 'name') {
        return a.name.localeCompare(b.name);
      }
      if (filters.sortBy === 'foundedYear') {
        return b.foundedYear - a.foundedYear;
      }
      if (filters.sortBy === 'jobsCount') {
        return this.getCompanyStats(b.id).activeJobsCount - this.getCompanyStats(a.id).activeJobsCount;
      }
      return 0;
    });
  }

  // --- Job Operations ---
  public getJobs(): Job[] {
    return [...this.jobs];
  }

  public getJobById(id: string): Job | undefined {
    return this.jobs.find((j) => j.id === id);
  }

  public getJobsForCompany(companyId: string): Job[] {
    return this.jobs.filter((j) => j.companyId === companyId);
  }

  public getFilteredJobs(filters: FilterState): Job[] {
    return this.jobs.filter((job) => {
      // Search query (title, description, technologies, companyName, hub)
      if (filters.searchQuery.trim()) {
        const q = filters.searchQuery.toLowerCase().trim();
        const matchesTitle = job.title.toLowerCase().includes(q);
        const matchesDesc = job.descriptionSnippet.toLowerCase().includes(q);
        const matchesCompany = job.companyName.toLowerCase().includes(q);
        const matchesHub = job.companyHub.toLowerCase().includes(q);
        const matchesTech = job.technologies.some((t) => t.toLowerCase().includes(q));
        const matchesSubcat = job.engineeringSubcategory?.toLowerCase().includes(q);

        if (!matchesTitle && !matchesDesc && !matchesCompany && !matchesHub && !matchesTech && !matchesSubcat) {
          return false;
        }
      }

      // Hub Filter
      if (filters.selectedHubs.length > 0 && !filters.selectedHubs.includes(job.companyHub)) {
        return false;
      }

      // Category Filter
      if (filters.selectedCategories.length > 0 && !filters.selectedCategories.includes(job.primaryCategory)) {
        return false;
      }

      // Fresher Filter
      if (filters.isFresherOnly && !job.isFresher) {
        return false;
      }

      // Engineering Filter
      if (filters.isEngineeringOnly && !job.isEngineering) {
        return false;
      }

      // Internship Filter
      if (filters.isInternshipOnly && !job.isInternship) {
        return false;
      }

      // Engineering Subcategories
      if (
        filters.selectedEngineeringSubcategories.length > 0 &&
        (!job.engineeringSubcategory || !filters.selectedEngineeringSubcategories.includes(job.engineeringSubcategory))
      ) {
        return false;
      }

      // Technologies
      if (
        filters.selectedTechnologies.length > 0 &&
        !filters.selectedTechnologies.some((t) => job.technologies.includes(t))
      ) {
        return false;
      }

      // Experience Level
      if (
        filters.selectedExperienceLevels.length > 0 &&
        !filters.selectedExperienceLevels.includes(job.experienceLevel)
      ) {
        return false;
      }

      // Workplace Type
      if (
        filters.selectedWorkplaceTypes.length > 0 &&
        !filters.selectedWorkplaceTypes.includes(job.workplaceType)
      ) {
        return false;
      }

      // Chennai Relevance Filter
      if (
        filters.selectedRelevance.length > 0 &&
        !filters.selectedRelevance.includes(job.chennaiRelevance)
      ) {
        return false;
      }

      // Freshness Filter
      if (
        filters.selectedFreshness.length > 0 &&
        !filters.selectedFreshness.includes(job.freshnessStatus)
      ) {
        return false;
      }

      return true;
    }).sort((a, b) => {
      if (filters.sortBy === 'recent') {
        return new Date(b.firstSeenAt).getTime() - new Date(a.firstSeenAt).getTime();
      }
      if (a.isFeatured && !b.isFeatured) return -1;
      if (!a.isFeatured && b.isFeatured) return 1;
      return new Date(b.firstSeenAt).getTime() - new Date(a.firstSeenAt).getTime();
    });
  }

  // --- News & Ecosystem ---
  public getNews(): EcosystemNews[] {
    return [...this.news];
  }

  // --- Submissions & Community ---
  public getSubmissions(): UserSubmission[] {
    return [...this.submissions];
  }

  public submitCompany(data: {
    name: string;
    website: string;
    careersUrl?: string;
    hub: TechHub;
    address: string;
    description: string;
    submittedBy: string;
    email?: string;
  }): UserSubmission {
    const submission: UserSubmission = {
      id: `sub-comp-${Date.now()}`,
      type: 'company',
      submittedBy: data.submittedBy,
      email: data.email,
      titleOrName: data.name,
      url: data.website,
      hub: data.hub,
      notes: data.description,
      submittedAt: new Date().toISOString(),
      status: 'PENDING',
      extractedDataPreview: {
        ...data,
      },
    };

    this.submissions.unshift(submission);
    this.persist();
    return submission;
  }

  public submitJob(data: {
    companyName: string;
    title: string;
    originalUrl: string;
    location: string;
    descriptionSnippet: string;
    salaryRange?: string;
    submittedBy: string;
    email?: string;
  }): UserSubmission {
    // Run real-time AI classification & Chennai relevance check on submission!
    const classification = classifyJob(data.title, data.descriptionSnippet);
    const relevance = analyzeChennaiRelevance(data.location, data.descriptionSnippet);

    const submission: UserSubmission = {
      id: `sub-job-${Date.now()}`,
      type: 'job',
      submittedBy: data.submittedBy,
      email: data.email,
      titleOrName: `${data.title} @ ${data.companyName}`,
      url: data.originalUrl,
      notes: data.descriptionSnippet,
      submittedAt: new Date().toISOString(),
      status: relevance.relevance === 'CHENNAI_CONFIRMED' && classification.isEngineering ? 'PENDING' : 'PENDING',
      extractedDataPreview: {
        ...data,
        classification,
        relevance,
      },
    };

    this.submissions.unshift(submission);
    this.persist();
    return submission;
  }

  // --- Admin Moderation & Control ---
  public approveSubmission(submissionId: string) {
    const sub = this.submissions.find((s) => s.id === submissionId);
    if (!sub) return;

    sub.status = 'APPROVED';

    if (sub.type === 'job' && sub.extractedDataPreview) {
      const data = sub.extractedDataPreview;
      const matchingComp = this.companies.find(
        (c) => c.name.toLowerCase() === data.companyName.toLowerCase()
      );

      const compId = matchingComp ? matchingComp.id : `comp-${Date.now()}`;
      if (!matchingComp) {
        // Create company record
        const newComp: Company = {
          id: compId,
          name: data.companyName,
          slug: data.companyName.toLowerCase().replace(/[^a-z0-9]/g, '-'),
          tagline: 'Discovered Chennai Tech Company',
          description: data.notes || 'Recently approved tech company.',
          logo: 'https://images.unsplash.com/photo-1572021335469-31706a17aaef?w=128&auto=format&fit=crop&q=80',
          website: data.url,
          careersUrl: data.url,
          companyTypes: ['STARTUP'],
          categories: ['SaaS / Enterprise Software'],
          hub: (data.hub as TechHub) || 'OMR (IT Corridor)',
          address: data.location || 'Chennai, Tamil Nadu',
          coordinates: { lat: 12.9644, lng: 80.2427 },
          foundedYear: 2022,
          employeeCount: '20-50',
          fundingStage: 'Seed',
          hiringStatus: 'Active',
          tags: ['Discovered'],
          techStack: data.classification?.technologies || ['JavaScript'],
          verificationStatus: 'VERIFIED',
          isFeatured: false,
          isSeedData: false,
          sourceName: 'User Submission',
          discoveredAt: new Date().toISOString(),
          lastVerifiedAt: new Date().toISOString(),
        };
        this.companies.push(newComp);
      }

      // Add Job
      const newJob: Job = {
        id: `job-${Date.now()}`,
        companyId: compId,
        companyName: data.companyName,
        companyLogo: matchingComp?.logo || 'https://images.unsplash.com/photo-1572021335469-31706a17aaef?w=128&auto=format&fit=crop&q=80',
        companyHub: matchingComp?.hub || 'OMR (IT Corridor)',
        title: data.title,
        slug: `${data.companyName}-${data.title}`.toLowerCase().replace(/[^a-z0-9]/g, '-'),
        descriptionSnippet: data.descriptionSnippet,
        primaryCategory: matchingComp?.categories[0] || 'SaaS / Enterprise Software',
        isEngineering: data.classification?.isEngineering ?? true,
        engineeringSubcategory: data.classification?.engineeringSubcategory || 'Software Engineering',
        technologies: data.classification?.technologies || [],
        jobType: 'Full-time',
        workplaceType: 'On-site',
        experienceLevel: data.classification?.experienceLevel || 'Fresher / Entry (0-1 yrs)',
        isFresher: data.classification?.isFresher ?? false,
        fresherConfidence: data.classification?.fresherConfidence ?? 80,
        isInternship: data.classification?.isInternship ?? false,
        salaryRange: data.salaryRange,
        location: data.location || 'Chennai',
        chennaiRelevance: data.relevance?.relevance || 'CHENNAI_CONFIRMED',
        relevanceConfidence: data.relevance?.confidence || 95,
        sourceName: 'User Submission',
        originalUrl: data.originalUrl,
        firstSeenAt: new Date().toISOString(),
        lastSeenAt: new Date().toISOString(),
        lastVerifiedAt: new Date().toISOString(),
        freshnessStatus: 'NEW',
        verificationStatus: 'VERIFIED',
        isFeatured: false,
        isSeedData: false,
      };

      this.jobs.unshift(newJob);
    }

    this.persist();
  }

  public rejectSubmission(submissionId: string) {
    const sub = this.submissions.find((s) => s.id === submissionId);
    if (sub) {
      sub.status = 'REJECTED';
      this.persist();
    }
  }

  public markJobVerified(jobId: string) {
    const job = this.jobs.find((j) => j.id === jobId);
    if (job) {
      job.lastVerifiedAt = new Date().toISOString();
      job.verificationStatus = 'VERIFIED';
      job.freshnessStatus = 'RECENTLY_VERIFIED';
      this.persist();
    }
  }

  public markJobExpired(jobId: string) {
    const job = this.jobs.find((j) => j.id === jobId);
    if (job) {
      job.freshnessStatus = 'EXPIRED';
      this.persist();
    }
  }

  // --- Discovery Queries & Engine Simulation ---
  public getDiscoveryQueries(): JobDiscoveryQuery[] {
    return [...this.discoveryQueries];
  }

  public triggerDiscoveryRun(queryId: string) {
    const query = this.discoveryQueries.find((q) => q.id === queryId);
    if (query) {
      query.lastRunAt = new Date().toISOString();
      query.resultsCount += Math.floor(Math.random() * 5) + 1;
      this.persist();
    }
  }

  public refreshAllJobFreshness() {
    let changed = false;
    for (const job of this.jobs) {
      const calculated = calculateJobFreshness(job);
      if (job.freshnessStatus !== calculated && job.freshnessStatus !== 'REMOVED') {
        job.freshnessStatus = calculated;
        changed = true;
      }
    }
    if (changed) {
      this.persist();
    }
  }

  public runDeduplicationCheck() {
    const { canonicalJobs } = detectJobDuplicates(this.jobs);
    this.jobs = canonicalJobs;
    this.persist();
  }

  public resetToDefaults() {
    this.companies = INITIAL_COMPANIES;
    this.jobs = INITIAL_JOBS;
    this.news = INITIAL_NEWS;
    this.submissions = [];
    this.discoveryQueries = DEFAULT_DISCOVERY_QUERIES;
    this.persist();
  }
}

export const db = new DatabaseService();
