export type CompanyType = 
  | 'STARTUP'
  | 'PRODUCT COMPANY'
  | 'IT SERVICES'
  | 'MNC'
  | 'ENTERPRISE'
  | 'SME'
  | 'MANUFACTURING';

export type TechHub = 
  | 'OMR (IT Corridor)'
  | 'Guindy (SIDCO / Olympia)'
  | 'DLF Porur & Manapakkam'
  | 'Tidel Park & Tharamani'
  | 'Perungudi & Kandanchavadi'
  | 'Siruseri SIPCOT'
  | 'Ambattur Industrial Estate'
  | 'Anna Nagar / Central Chennai'
  | 'T. Nagar & Alwarpet'
  | 'Nungambakkam & Mount Road'
  | 'Sholinganallur'
  | 'Other';

export type CompanyCategory = 
  | 'SaaS / Enterprise Software'
  | 'FinTech'
  | 'DeepTech & AI'
  | 'AutoTech & EV'
  | 'HealthTech & Bio'
  | 'EdTech'
  | 'E-Commerce & Quick Commerce'
  | 'Logistics & Supply Chain'
  | 'CleanTech & Climate'
  | 'Gaming & Media'
  | 'Hardware & IoT'
  | 'SpaceTech & Defense'
  | 'Semiconductor'
  | 'Cybersecurity'
  | 'Other';

export type FundingStage = 
  | 'Bootstrapped'
  | 'Pre-Seed'
  | 'Seed'
  | 'Series A'
  | 'Series B'
  | 'Series C+'
  | 'Public / IPO'
  | 'Acquired'
  | 'Profitable & Self-Sustained';

export type JobType = 'Full-time' | 'Part-time' | 'Contract' | 'Internship';
export type WorkplaceType = 'On-site' | 'Hybrid' | 'Remote';
export type ExperienceLevel = 'Fresher / Entry (0-1 yrs)' | 'Junior (1-3 yrs)' | 'Mid (3-5 yrs)' | 'Senior (6-9 yrs)' | 'Lead / Exec (10+ yrs)';

export type EngineeringSubcategory =
  | 'Software Engineering'
  | 'Backend'
  | 'Frontend'
  | 'Full Stack'
  | 'Mobile'
  | 'DevOps & Cloud'
  | 'QA & Automation'
  | 'Data Engineering'
  | 'ML & AI Engineering'
  | 'Security Engineering'
  | 'Embedded & IoT'
  | 'Hardware'
  | 'Other Engineering';

export type ChennaiRelevance = 
  | 'CHENNAI_CONFIRMED'
  | 'CHENNAI_LIKELY'
  | 'REMOTE_WITH_CHENNAI_COMPANY'
  | 'NOT_CHENNAI'
  | 'UNKNOWN';

export type FreshnessStatus = 
  | 'NEW'
  | 'ACTIVE'
  | 'RECENTLY_VERIFIED'
  | 'STALE'
  | 'EXPIRED'
  | 'REMOVED';

export type VerificationStatus = 
  | 'VERIFIED'
  | 'PENDING_VERIFICATION'
  | 'DISCOVERED'
  | 'NEEDS_REVIEW'
  | 'FLAGGED';

export type JobSourceType = 
  | 'Company Careers'
  | 'LinkedIn'
  | 'Naukri'
  | 'Wellfound'
  | 'Indeed'
  | 'Greenhouse / Lever ATS'
  | 'User Submission'
  | 'Recruiter Submission'
  | 'Discovery Engine';

export interface Coordinates {
  lat: number;
  lng: number;
}

export interface CompanyJobStats {
  activeJobsCount: number;
  engineeringJobsCount: number;
  fresherJobsCount: number;
  internshipsCount: number;
  lastJobDiscoveredAt?: string;
}

export interface Company {
  id: string;
  name: string;
  slug: string;
  tagline: string;
  description: string;
  logo: string;
  website: string;
  careersUrl: string;
  companyTypes: CompanyType[];
  categories: CompanyCategory[];
  hub: TechHub;
  address: string;
  coordinates: Coordinates;
  foundedYear: number;
  employeeCount: string;
  fundingStage: FundingStage;
  totalFundingRaised?: string;
  hiringStatus: 'Active' | 'Hiring Surge' | 'Selective' | 'Not Hiring';
  tags: string[];
  techStack: string[];
  verificationStatus: VerificationStatus;
  isFeatured: boolean;
  isSeedData: boolean;
  sourceName: string;
  sourceUrl?: string;
  discoveredAt: string;
  lastVerifiedAt: string;
  socialLinks?: {
    linkedin?: string;
    twitter?: string;
    github?: string;
  };
  keyLeaders?: {
    founders?: string[];
    leadership?: string[];
  };
}

export interface JobAlternateSource {
  sourceName: JobSourceType;
  url: string;
  discoveredAt: string;
  priceOrSalarySnippet?: string;
}

export interface Job {
  id: string;
  companyId: string;
  companyName: string;
  companyLogo: string;
  companyHub: TechHub;
  title: string;
  slug: string;
  descriptionSnippet: string;
  primaryCategory: CompanyCategory;
  isEngineering: boolean;
  engineeringSubcategory?: EngineeringSubcategory;
  technologies: string[];
  jobType: JobType;
  workplaceType: WorkplaceType;
  experienceLevel: ExperienceLevel;
  isFresher: boolean;
  fresherConfidence: number; // 0 to 100
  isInternship: boolean;
  salaryRange?: string;
  location: string;
  chennaiRelevance: ChennaiRelevance;
  relevanceConfidence: number; // 0 to 100
  sourceName: JobSourceType;
  originalUrl: string;
  sourceRecordId?: string;
  firstSeenAt: string;
  lastSeenAt: string;
  lastVerifiedAt: string;
  freshnessStatus: FreshnessStatus;
  verificationStatus: VerificationStatus;
  duplicateGroupId?: string;
  alternateSources?: JobAlternateSource[];
  isFeatured: boolean;
  isSeedData: boolean;
}

export interface JobDuplicateGroup {
  id: string;
  canonicalJobId: string;
  companyName: string;
  normalizedTitle: string;
  confidenceScore: number;
  sources: { sourceName: string; url: string; lastSeenAt: string }[];
}

export interface FilterState {
  searchQuery: string;
  selectedHubs: TechHub[];
  selectedCategories: CompanyCategory[];
  selectedCompanyTypes: CompanyType[];
  selectedFundingStages: FundingStage[];
  selectedExperienceLevels: ExperienceLevel[];
  selectedWorkplaceTypes: WorkplaceType[];
  selectedEngineeringSubcategories: EngineeringSubcategory[];
  selectedTechnologies: string[];
  isHiringOnly: boolean;
  isFresherOnly: boolean;
  isEngineeringOnly: boolean;
  isInternshipOnly: boolean;
  isFeaturedOnly: boolean;
  selectedRelevance: ChennaiRelevance[];
  selectedFreshness: FreshnessStatus[];
  sortBy: 'featured' | 'name' | 'foundedYear' | 'jobsCount' | 'recent';
}

export type ActiveTab = 'map' | 'directory' | 'jobs' | 'ecosystem' | 'admin';

export interface EcosystemNews {
  id: string;
  title: string;
  summary: string;
  sourceName: string;
  sourceUrl: string;
  category: 'Funding' | 'Launch' | 'Acquisition' | 'Expansion' | 'Hiring' | 'Policy / StartupTN';
  publishedDate: string;
  relatedCompanyId?: string;
  relatedCompanyName?: string;
  tags: string[];
}

export interface UserSubmission {
  id: string;
  type: 'company' | 'job' | 'url_discovery';
  submittedBy: string;
  email?: string;
  titleOrName: string;
  url: string;
  category?: string;
  hub?: TechHub;
  notes?: string;
  submittedAt: string;
  status: 'PENDING' | 'APPROVED' | 'REJECTED';
  extractedDataPreview?: any;
}

export interface JobDiscoveryQuery {
  id: string;
  query: string;
  category: CompanyCategory | 'All';
  technology?: string;
  location: string;
  experience?: string;
  priority: 'high' | 'medium' | 'low';
  active: boolean;
  lastRunAt?: string;
  resultsCount: number;
}