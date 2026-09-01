import { Job, JobDuplicateGroup } from '../types';

export function normalizeJobTitle(title: string): string {
  return title
    .toLowerCase()
    .replace(/\b(hiring|urgent|immediate joiner|openings?|batch|chennai)\b/g, '')
    .replace(/[^a-z0-9]/g, ' ')
    .replace(/\s+/g, ' ')
    .trim();
}

export function detectJobDuplicates(jobs: Job[]): {
  canonicalJobs: Job[];
  duplicateGroups: JobDuplicateGroup[];
} {
  const groups: { [key: string]: Job[] } = {};

  for (const job of jobs) {
    const normTitle = normalizeJobTitle(job.title);
    const key = `${job.companyId}_${normTitle}`;
    if (!groups[key]) {
      groups[key] = [];
    }
    groups[key].push(job);
  }

  const canonicalJobs: Job[] = [];
  const duplicateGroups: JobDuplicateGroup[] = [];

  for (const [key, cluster] of Object.entries(groups)) {
    if (cluster.length === 1) {
      canonicalJobs.push(cluster[0]);
    } else {
      // Pick Company Careers as canonical first, or the newest verified job
      const sorted = [...cluster].sort((a, b) => {
        if (a.sourceName === 'Company Careers' && b.sourceName !== 'Company Careers') return -1;
        if (b.sourceName === 'Company Careers' && a.sourceName !== 'Company Careers') return 1;
        return new Date(b.firstSeenAt).getTime() - new Date(a.firstSeenAt).getTime();
      });

      const canonical = sorted[0];
      const dupGroupId = `group_${key}_${Date.now()}`;
      canonical.duplicateGroupId = dupGroupId;

      // Attach alternate sources to canonical
      canonical.alternateSources = sorted.slice(1).map((j) => ({
        sourceName: j.sourceName,
        url: j.originalUrl,
        discoveredAt: j.firstSeenAt,
        priceOrSalarySnippet: j.salaryRange,
      }));

      duplicateGroups.push({
        id: dupGroupId,
        canonicalJobId: canonical.id,
        companyName: canonical.companyName,
        normalizedTitle: normalizeJobTitle(canonical.title),
        confidenceScore: 92,
        sources: sorted.map((s) => ({
          sourceName: s.sourceName,
          url: s.originalUrl,
          lastSeenAt: s.lastSeenAt,
        })),
      });

      canonicalJobs.push(canonical);
    }
  }

  return { canonicalJobs, duplicateGroups };
}