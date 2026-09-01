import { 
  TechHub, 
  CompanyCategory, 
  CompanyType, 
  FundingStage, 
  WorkplaceType, 
  ExperienceLevel,
  EngineeringSubcategory,
  JobSourceType
} from '../types';

export const APP_CONFIG = {
  name: 'Chennai Startup & Jobs Map',
  shortName: 'ChennaiStartups',
  tagline: "South Asia's SaaS & DeepTech Capital — Startup & Job Discovery Engine",
  city: 'Chennai, Tamil Nadu, India',
  defaultCoordinates: {
    lat: 12.9815,
    lng: 80.2180,
  },
  defaultZoom: 12,
  mapBounds: {
    minZoom: 10,
    maxZoom: 18,
  }
};

export interface TechHubDetail {
  name: TechHub;
  coordinates: { lat: number; lng: number };
  badge: string;
  description: string;
  popularParks: string[];
}

export const CHENNAI_TECH_HUBS: TechHubDetail[] = [
  {
    name: 'OMR (IT Corridor)',
    coordinates: { lat: 12.9156, lng: 80.2290 },
    badge: 'SaaS Expressway',
    description: 'Arterial 20km software corridor spanning Perungudi, Sholinganallur, and Navalur.',
    popularParks: ['Tidel Park', 'Ascendas International Tech Park', 'RMZ Millenia', 'Futura Tech Park'],
  },
  {
    name: 'Guindy (SIDCO / Olympia)',
    coordinates: { lat: 13.0067, lng: 80.2052 },
    badge: 'Central Tech Gateway',
    description: 'Premier urban technology and engineering park destination next to Chennai International Airport.',
    popularParks: ['Olympia Tech Park', 'SIDCO Industrial Estate', 'Guindy Tech Park'],
  },
  {
    name: 'Tidel Park & Tharamani',
    coordinates: { lat: 12.9892, lng: 80.2458 },
    badge: 'Innovation Nucleus',
    description: 'Epicenter of deep research, deep tech incubation, and pioneering IT infrastructure.',
    popularParks: ['IIT Madras Research Park', 'Tidel Park Tharamani', 'Ramanujan IT City'],
  },
  {
    name: 'DLF Porur & Manapakkam',
    coordinates: { lat: 13.0232, lng: 80.1610 },
    badge: 'Western IT Powerhouse',
    description: 'Expansive IT SEZ cybercity housing global product engineering centers and enterprise operations.',
    popularParks: ['DLF Cybercity Chennai', 'L&T Infotech Park', 'Commerzone Porur'],
  },
  {
    name: 'Perungudi & Kandanchavadi',
    coordinates: { lat: 12.9644, lng: 80.2427 },
    badge: 'Startup & Scaleup Core',
    description: 'High-density tech cluster packed with co-working incubators, product studios, and unicorns.',
    popularParks: ['SP Infocity', 'World Trade Center Perungudi', 'Workafella / WeWork OMR'],
  },
  {
    name: 'Siruseri SIPCOT',
    coordinates: { lat: 12.8276, lng: 80.2214 },
    badge: 'Mega Tech Parks',
    description: 'Asia’s largest specialized IT SEZ campus zone hosting tens of thousands of technologists.',
    popularParks: ['SIPCOT IT Park Siruseri', 'TCS Siruseri Signature Campus', 'Syntel Global Campus'],
  },
  {
    name: 'Anna Nagar / Central Chennai',
    coordinates: { lat: 13.0850, lng: 80.2100 },
    badge: 'Urban Startup Hub',
    description: 'Dynamic central Chennai neighborhood with creative studios, boutique tech startups, and digital brands.',
    popularParks: ['Anna Nagar Tech Studios', 'Koyambedu Business Towers'],
  },
  {
    name: 'T. Nagar & Alwarpet',
    coordinates: { lat: 13.0368, lng: 80.2405 },
    badge: 'Venture & Fintech Node',
    description: 'Central hub for VC offices, financial technology firms, and product venture studios.',
    popularParks: ['Alwarpet Innovation Hub', 'Gopathi Narayanaswami Tech Hub'],
  },
  {
    name: 'Ambattur Industrial Estate',
    coordinates: { lat: 13.1147, lng: 80.1548 },
    badge: 'Data Centers & AutoTech',
    description: 'North-Western corridor experiencing massive transformation into tier-3 data centers and EV engineering.',
    popularParks: ['Ambattur IT Park', 'One IndiaBulls Park', 'Prince Info Park'],
  },
  {
    name: 'Sholinganallur',
    coordinates: { lat: 12.9010, lng: 80.2279 },
    badge: 'Global R&D Junction',
    description: 'Major junction on OMR connecting to ECR with immense campuses for global product development.',
    popularParks: ['ELCOT SEZ Sholinganallur', 'Infosys Sholinganallur', 'Wipro Campus'],
  }
];

export const COMPANY_TYPES: CompanyType[] = [
  'STARTUP',
  'PRODUCT COMPANY',
  'IT SERVICES',
  'MNC',
  'ENTERPRISE',
  'SME',
  'MANUFACTURING',
];

export const COMPANY_CATEGORIES: CompanyCategory[] = [
  'SaaS / Enterprise Software',
  'FinTech',
  'DeepTech & AI',
  'AutoTech & EV',
  'HealthTech & Bio',
  'EdTech',
  'E-Commerce & Quick Commerce',
  'Logistics & Supply Chain',
  'CleanTech & Climate',
  'Gaming & Media',
  'Hardware & IoT',
  'SpaceTech & Defense',
  'Semiconductor',
  'Cybersecurity',
  'Other',
];

export const ENGINEERING_SUBCATEGORIES: EngineeringSubcategory[] = [
  'Software Engineering',
  'Backend',
  'Frontend',
  'Full Stack',
  'Mobile',
  'DevOps & Cloud',
  'QA & Automation',
  'Data Engineering',
  'ML & AI Engineering',
  'Security Engineering',
  'Embedded & IoT',
  'Hardware',
  'Other Engineering',
];

export const POPULAR_TECHNOLOGIES: string[] = [
  'React',
  'TypeScript',
  'JavaScript',
  'Node.js',
  'Python',
  'Java',
  '.NET',
  'C#',
  'Go',
  'Rust',
  'AWS',
  'Azure',
  'GCP',
  'Docker',
  'Kubernetes',
  'PostgreSQL',
  'SQL',
  'MongoDB',
  'Flutter',
  'Android',
  'iOS / Swift',
  'Machine Learning',
  'PyTorch',
  'TensorFlow',
  'Next.js',
  'Tailwind CSS',
  'GraphQL',
  'Spring Boot',
  'FastAPI',
  'Kafka',
  'Redis',
];

export const FUNDING_STAGES: FundingStage[] = [
  'Bootstrapped',
  'Pre-Seed',
  'Seed',
  'Series A',
  'Series B',
  'Series C+',
  'Public / IPO',
  'Acquired',
  'Profitable & Self-Sustained',
];

export const WORKPLACE_TYPES: WorkplaceType[] = ['On-site', 'Hybrid', 'Remote'];

export const EXPERIENCE_LEVELS: ExperienceLevel[] = [
  'Fresher / Entry (0-1 yrs)',
  'Junior (1-3 yrs)',
  'Mid (3-5 yrs)',
  'Senior (6-9 yrs)',
  'Lead / Exec (10+ yrs)',
];

export const JOB_SOURCES: JobSourceType[] = [
  'Company Careers',
  'LinkedIn',
  'Naukri',
  'Wellfound',
  'Indeed',
  'Greenhouse / Lever ATS',
  'User Submission',
  'Recruiter Submission',
  'Discovery Engine',
];