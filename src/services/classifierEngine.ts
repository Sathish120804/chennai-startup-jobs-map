import { EngineeringSubcategory, ExperienceLevel } from '../types';
import { POPULAR_TECHNOLOGIES } from '../config/constants';

export interface ClassificationResult {
  isEngineering: boolean;
  engineeringSubcategory?: EngineeringSubcategory;
  technologies: string[];
  isFresher: boolean;
  fresherConfidence: number;
  isInternship: boolean;
  experienceLevel: ExperienceLevel;
  tags: string[];
}

const ENGINEERING_PATTERNS: { subcategory: EngineeringSubcategory; patterns: RegExp[] }[] = [
  {
    subcategory: 'Frontend',
    patterns: [/frontend/i, /front-end/i, /react\b/i, /angular\b/i, /vue\b/i, /ui engineer/i, /web developer/i, /next\.?js/i],
  },
  {
    subcategory: 'Backend',
    patterns: [/backend/i, /back-end/i, /api developer/i, /\.net developer/i, /java developer/i, /node\.?js/i, /python developer/i, /golang/i, /c# developer/i, /spring boot/i],
  },
  {
    subcategory: 'Full Stack',
    patterns: [/full[- ]?stack/i, /mern/i, /mean stack/i, /full stack developer/i, /software development engineer/i],
  },
  {
    subcategory: 'Mobile',
    patterns: [/android/i, /ios/i, /flutter/i, /react native/i, /mobile app/i, /swift/i, /kotlin/i],
  },
  {
    subcategory: 'DevOps & Cloud',
    patterns: [/devops/i, /sre\b/i, /site reliability/i, /cloud engineer/i, /aws engineer/i, /kubernetes/i, /infrastructure/i, /platform engineer/i],
  },
  {
    subcategory: 'ML & AI Engineering',
    patterns: [/machine learning/i, /\bai\b/i, /ml engineer/i, /data scientist/i, /deep learning/i, /nlp/i, /computer vision/i, /llm/i, /genai/i],
  },
  {
    subcategory: 'Data Engineering',
    patterns: [/data engineer/i, /etl\b/i, /spark/i, /data warehouse/i, /big data/i, /kafka/i, /snowflake/i],
  },
  {
    subcategory: 'QA & Automation',
    patterns: [/qa\b/i, /quality assurance/i, /sdet/i, /test engineer/i, /automation tester/i, /selenium/i, /cypress/i],
  },
  {
    subcategory: 'Security Engineering',
    patterns: [/security engineer/i, /cybersecurity/i, /infosec/i, /soc analyst/i, /penetration/i],
  },
  {
    subcategory: 'Embedded & IoT',
    patterns: [/embedded/i, /iot\b/i, /firmware/i, /microcontroller/i, /rtos/i, /automotive software/i],
  },
  {
    subcategory: 'Hardware',
    patterns: [/hardware engineer/i, /vlsi/i, /pcb/i, /asic/i, /fpga/i, /semiconductor/i],
  },
  {
    subcategory: 'Software Engineering',
    patterns: [/software engineer/i, /software developer/i, /sde\b/i, /programmer/i, /systems engineer/i],
  }
];

const FRESHER_POSITIVE_PATTERNS = [
  /\bfreshers?\b/i,
  /\bfresh\s+(engineering|graduates?)\b/i,
  /\bentry[- ]?level\b/i,
  /\b0[- ]?1\s*years?\b/i,
  /\b0\s*years?\b/i,
  /\bgraduate trainee\b/i,
  /\bgraduate engineer\b/i,
  /\bcollege graduate\b/i,
  /\bcampus\b/i,
  /\bjunior developer\b/i,
  /\bassociate software (developer|engineer)\b/i,
  /\b(2024|2025|2026)\b/i,
];

const FRESHER_NEGATIVE_PATTERNS = [
  /\b(senior|lead|staff|principal|head|director|architect|manager)\b/i,
  /\b([3-9]|1[0-9])\+?\s*years?\s*of\s*experience\b/i,
  /\b(mid[- ]?senior|experienced professional)\b/i,
];

export function classifyJob(
  title: string,
  description: string,
  experienceText?: string
): ClassificationResult {
  const combined = `${title} ${description} ${experienceText || ''}`;
  const titleLower = title.toLowerCase();

  // 1. Engineering detection & subcategory
  let isEngineering = false;
  let engineeringSubcategory: EngineeringSubcategory | undefined;

  for (const item of ENGINEERING_PATTERNS) {
    for (const pattern of item.patterns) {
      if (pattern.test(titleLower) || pattern.test(combined)) {
        isEngineering = true;
        if (!engineeringSubcategory) {
          engineeringSubcategory = item.subcategory;
        }
        break;
      }
    }
  }

  // 2. Internship Detection
  const isInternship = /\bintern\b/i.test(titleLower) || /\binternship\b/i.test(combined);

  // 3. Fresher Detection & Confidence Scoring
  let fresherScore = 0;
  if (isInternship) {
    fresherScore = 90;
  } else {
    for (const pattern of FRESHER_POSITIVE_PATTERNS) {
      if (pattern.test(titleLower)) fresherScore += 45;
      else if (pattern.test(combined)) fresherScore += 25;
    }

    // Penalize if senior keywords exist
    for (const pattern of FRESHER_NEGATIVE_PATTERNS) {
      if (pattern.test(titleLower)) fresherScore -= 60;
      else if (pattern.test(combined)) fresherScore -= 30;
    }
  }

  const fresherConfidence = Math.max(0, Math.min(100, fresherScore));
  const isFresher = fresherConfidence >= 50;

  // 4. Experience Level Mapping
  let experienceLevel: ExperienceLevel = 'Mid (3-5 yrs)';
  if (isFresher || isInternship || fresherConfidence >= 50) {
    experienceLevel = 'Fresher / Entry (0-1 yrs)';
  } else if (/\b(1|2)\s*years?\b/i.test(combined) || /\bjunior\b/i.test(titleLower)) {
    experienceLevel = 'Junior (1-3 yrs)';
  } else if (/\b(senior|sr\.|6|7|8)\s*years?\b/i.test(combined)) {
    experienceLevel = 'Senior (6-9 yrs)';
  } else if (/\b(lead|principal|architect|director|10\+)\b/i.test(combined)) {
    experienceLevel = 'Lead / Exec (10+ yrs)';
  }

  // 5. Technology extraction
  const technologies: string[] = [];
  for (const tech of POPULAR_TECHNOLOGIES) {
    // Regex boundary match
    const escaped = tech.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
    const regex = new RegExp(`\\b${escaped}\\b`, 'i');
    if (regex.test(combined)) {
      technologies.push(tech);
    }
  }

  // 6. Generic Tags
  const tags: string[] = [...technologies];
  if (isFresher) tags.push('Fresher Friendly');
  if (isInternship) tags.push('Internship');
  if (engineeringSubcategory) tags.push(engineeringSubcategory);

  return {
    isEngineering,
    engineeringSubcategory,
    technologies,
    isFresher,
    fresherConfidence,
    isInternship,
    experienceLevel,
    tags,
  };
}