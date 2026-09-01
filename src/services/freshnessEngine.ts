import { FreshnessStatus, Job } from '../types';

export function calculateJobFreshness(job: Job): FreshnessStatus {
  const now = new Date().getTime();
  const firstSeen = new Date(job.firstSeenAt).getTime();
  const lastVerified = new Date(job.lastVerifiedAt).getTime();
  
  const hoursSinceFirstSeen = (now - firstSeen) / (1000 * 60 * 60);
  const daysSinceLastVerified = (now - lastVerified) / (1000 * 60 * 60 * 24);

  if (hoursSinceFirstSeen <= 48) {
    return 'NEW';
  }
  if (daysSinceLastVerified <= 7) {
    return 'RECENTLY_VERIFIED';
  }
  if (daysSinceLastVerified <= 30) {
    return 'ACTIVE';
  }
  if (daysSinceLastVerified <= 60) {
    return 'STALE';
  }
  return 'EXPIRED';
}