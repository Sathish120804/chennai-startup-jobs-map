import { ChennaiRelevance } from '../types';

export interface LocationAnalysisResult {
  relevance: ChennaiRelevance;
  confidence: number; // 0 to 100
  matchedSignals: string[];
  isRemoteWithChennaiHq: boolean;
}

const CHENNAI_KEYWORDS = [
  'chennai',
  'madras',
  'omr',
  'guindy',
  'porur',
  'tharamani',
  'tidel park',
  'siruseri',
  'sholinganallur',
  'perungudi',
  'kandanchavadi',
  'ambattur',
  'velachery',
  't nagar',
  'alwarpet',
  'dlf cybercity',
  'olympia tech park',
  'iit madras research park',
  'ramanujan it city',
  'ascendas'
];

const NON_CHENNAI_LOCATIONS = [
  'bangalore',
  'bengaluru',
  'hyderabad',
  'pune',
  'gurgaon',
  'noida',
  'delhi',
  'mumbai',
  'coimbatore',
  'kochi',
  'trivandrum',
  'usa',
  'san francisco',
  'london',
  'singapore'
];

export function analyzeChennaiRelevance(
  locationField: string,
  description: string,
  companyHub?: string,
  companyHasChennaiHq: boolean = true
): LocationAnalysisResult {
  const locLower = (locationField || '').toLowerCase();
  const descLower = (description || '').toLowerCase();
  const matchedSignals: string[] = [];
  let score = 0;

  // 1. Direct location field check (Strongest signal)
  for (const kw of CHENNAI_KEYWORDS) {
    if (locLower.includes(kw)) {
      matchedSignals.push(`Location field matched "${kw}"`);
      score += 65;
      break;
    }
  }

  // 2. Hub alignment
  if (companyHub && companyHub !== 'Other') {
    matchedSignals.push(`Associated with verified hub: ${companyHub}`);
    score += 20;
  }

  // 3. Description location signals
  for (const kw of CHENNAI_KEYWORDS) {
    if (descLower.includes(kw)) {
      matchedSignals.push(`Description mentions Chennai zone "${kw}"`);
      score += 15;
      break;
    }
  }

  // 4. Remote with Chennai Company
  const isRemote = locLower.includes('remote') || descLower.includes('work from home') || descLower.includes('remote opportunity');
  if (isRemote && companyHasChennaiHq) {
    matchedSignals.push('Remote role with Chennai-based entity/office');
  }

  // 5. Negative check: if explicit other Indian or global city is stated exclusively in location field
  let hasOtherCity = false;
  for (const nonLoc of NON_CHENNAI_LOCATIONS) {
    if (locLower.includes(nonLoc) && !locLower.includes('chennai')) {
      hasOtherCity = true;
      matchedSignals.push(`Exclusive non-Chennai location detected: ${nonLoc}`);
      score -= 50;
      break;
    }
  }

  // Clamp score
  const finalConfidence = Math.max(0, Math.min(100, score));

  let relevance: ChennaiRelevance = 'UNKNOWN';
  if (finalConfidence >= 75) {
    relevance = 'CHENNAI_CONFIRMED';
  } else if (finalConfidence >= 45) {
    relevance = 'CHENNAI_LIKELY';
  } else if (isRemote && companyHasChennaiHq) {
    relevance = 'REMOTE_WITH_CHENNAI_COMPANY';
  } else if (hasOtherCity || finalConfidence < 20) {
    relevance = 'NOT_CHENNAI';
  }

  return {
    relevance,
    confidence: finalConfidence,
    matchedSignals,
    isRemoteWithChennaiHq: isRemote && companyHasChennaiHq,
  };
}