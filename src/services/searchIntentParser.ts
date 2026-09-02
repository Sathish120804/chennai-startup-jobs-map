import { TechHub, CompanyType, CompanyCategory, WorkplaceType } from '../types';
import { CHENNAI_LOCATIONS } from '../config/chennaiLocations';

export interface ParsedSearchIntent {
  rawQuery: string;
  normalizedText: string;
  technology?: string;
  isFresher?: boolean;
  isInternship?: boolean;
  hub?: TechHub;
  companyType?: CompanyType;
  category?: CompanyCategory;
  workMode?: WorkplaceType;
  hasLocationIntent: boolean;
  matchedSynonyms: string[];
}

import { POPULAR_TECHNOLOGIES } from '../config/constants';

// Synonyms map
const TECH_SYNONYMS: Record<string, string> = {
  'react': 'React',
  'dotnet': '.NET',
  'dot net': '.NET',
  'c#': 'C#',
  'asp.net': 'ASP.NET Core',
  'reactjs': 'React',
  'react.js': 'React',
  'node': 'Node.js',
  'nodejs': 'Node.js',
  'node.js': 'Node.js',
  'vuejs': 'Vue.js',
  'angularjs': 'Angular',
  'py': 'Python',
  'python3': 'Python',
  'ml': 'Machine Learning',
  'ai': 'AI / ML',
  'genai': 'Generative AI',
  'ts': 'TypeScript',
  'js': 'JavaScript',
  'postgres': 'PostgreSQL',
  'mongo': 'MongoDB',
  'aws': 'AWS',
  'azure': 'Azure',
  'k8s': 'Kubernetes',
  'docker': 'Docker',
};

const FRESHER_KEYWORDS = ['fresher', 'freshers', 'entry level', '0 years', '0-1 years', 'trainee', 'graduate'];
const INTERNSHIP_KEYWORDS = ['intern', 'internship', 'stipend', 'summer intern'];
const STARTUP_KEYWORDS = ['startup', 'startups', 'early stage', 'funded startup'];
const PRODUCT_KEYWORDS = ['product company', 'product startup', 'saas'];

export function parseSearchIntent(rawQuery: string): ParsedSearchIntent {
  const queryLower = (rawQuery || '').trim().toLowerCase();
  if (!queryLower) {
    return {
      rawQuery: '',
      normalizedText: '',
      hasLocationIntent: false,
      matchedSynonyms: [],
    };
  }

  const matchedSynonyms: string[] = [];
  let technology: string | undefined;
  let isFresher: boolean | undefined;
  let isInternship: boolean | undefined;
  let hub: TechHub | undefined;
  let companyType: CompanyType | undefined;
  let category: CompanyCategory | undefined;
  let workMode: WorkplaceType | undefined;
  let hasLocationIntent = false;

  // 1. Fresher / Entry level check
  if (FRESHER_KEYWORDS.some(kw => queryLower.includes(kw))) {
    isFresher = true;
    matchedSynonyms.push('Fresher / Entry Level Intent');
  }

  // 2. Internship check
  if (INTERNSHIP_KEYWORDS.some(kw => queryLower.includes(kw))) {
    isInternship = true;
    matchedSynonyms.push('Internship Intent');
  }

  // 3. Location / Hub check
  for (const locInfo of CHENNAI_LOCATIONS) {
    if (locInfo.keywords.some(kw => queryLower.includes(kw))) {
      hub = locInfo.hub;
      hasLocationIntent = true;
      matchedSynonyms.push(`Location Intent: ${locInfo.area}`);
      break;
    }
  }

  if (!hasLocationIntent && (queryLower.includes('chennai') || queryLower.includes('madras'))) {
    hasLocationIntent = true;
    matchedSynonyms.push('City Intent: Chennai');
  }

  // 4. Technology & Synonym Matching
  for (const [key, canonical] of Object.entries(TECH_SYNONYMS)) {
    const regex = new RegExp(`\\b${key.replace('.', '\\.')}\\b`, 'i');
    if (regex.test(queryLower)) {
      technology = canonical;
      matchedSynonyms.push(`Tech Intent: ${canonical} (from "${key}")`);
      break;
    }
  }

  if (!technology) {
    for (const tech of POPULAR_TECHNOLOGIES) {
      const escaped = tech.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
      const regex = new RegExp(`\\b${escaped}\\b`, 'i');
      if (regex.test(queryLower)) {
        technology = tech;
        matchedSynonyms.push(`Tech Intent: ${tech}`);
        break;
      }
    }
  }

  // 5. Company Type matching
  if (STARTUP_KEYWORDS.some(kw => queryLower.includes(kw))) {
    companyType = 'STARTUP';
    matchedSynonyms.push('Type: Startup');
  } else if (PRODUCT_KEYWORDS.some(kw => queryLower.includes(kw))) {
    companyType = 'PRODUCT COMPANY';
    matchedSynonyms.push('Type: Product Company');
  }

  // 6. Category matching
  if (queryLower.includes('saas')) {
    category = 'SaaS / Enterprise Software';
  } else if (queryLower.includes('fintech')) {
    category = 'FinTech';
  } else if (queryLower.includes('deeptech') || queryLower.includes('ai')) {
    category = 'DeepTech & AI';
  } else if (queryLower.includes('autotech') || queryLower.includes('ev')) {
    category = 'AutoTech & EV';
  }

  // 7. Work Mode matching
  if (queryLower.includes('remote')) {
    workMode = 'Remote';
  } else if (queryLower.includes('hybrid')) {
    workMode = 'Hybrid';
  } else if (queryLower.includes('onsite') || queryLower.includes('on-site')) {
    workMode = 'On-site';
  }

  return {
    rawQuery,
    normalizedText: queryLower,
    technology,
    isFresher,
    isInternship,
    hub,
    companyType,
    category,
    workMode,
    hasLocationIntent,
    matchedSynonyms,
  };
}
