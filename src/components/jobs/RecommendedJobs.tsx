import React, { useMemo } from 'react';
import { Sparkles, CheckCircle2, ChevronRight, Zap } from 'lucide-react';
import { db } from '../../services/db';
import { useAppStore } from '../../store/useAppStore';
import { Badge } from '../ui/Badge';
import { Card } from '../ui/Card';
import { Button } from '../ui/Button';
import { Job } from '../../types';

interface ScoredJob {
  job: Job;
  score: number;
  strength: 'Strong match' | 'Good match' | 'Relevant match';
  reasons: string[];
}

export const RecommendedJobs: React.FC = () => {
  const { filters, setSelectedJobId, setActiveTab } = useAppStore();
  const jobs = db.getJobs();

  const recommendedJobs = useMemo<ScoredJob[]>(() => {
    const q = (filters.searchQuery || '').toLowerCase();
    const hubs = filters.selectedHubs;
    const techs = filters.selectedTechnologies;

    return jobs.map((job) => {
      let score = 55;
      const reasons: string[] = [];

      if (q) {
        if (job.title.toLowerCase().includes(q)) {
          score += 25;
          reasons.push(`Direct title match for "${q}"`);
        }
        if (job.technologies.some(t => q.includes(t.toLowerCase()))) {
          score += 20;
          reasons.push(`Matches requested tech stack`);
        }
      }

      if (techs.length > 0) {
        const matched = job.technologies.filter(t => techs.includes(t));
        if (matched.length > 0) {
          score += 20;
          reasons.push(`Skills: ${matched.slice(0, 2).join(', ')}`);
        }
      }

      if (filters.isFresherOnly && job.isFresher) {
        score += 15;
        reasons.push('Fresher / Entry opportunity');
      } else if (job.isFresher) {
        reasons.push('Fresher friendly');
      }

      if (hubs.length > 0 && hubs.includes(job.companyHub)) {
        score += 15;
        reasons.push(`Office located in ${job.companyHub}`);
      } else {
        reasons.push(`Corridor: ${job.companyHub}`);
      }

      if (job.verificationStatus === 'VERIFIED') {
        score += 5;
        reasons.push('Direct verified application link');
      }

      const finalScore = Math.min(score, 98);
      const strength: 'Strong match' | 'Good match' | 'Relevant match' =
        finalScore >= 80 ? 'Strong match' : finalScore >= 65 ? 'Good match' : 'Relevant match';

      return {
        job,
        score: finalScore,
        strength,
        reasons: reasons.slice(0, 2)
      };
    })
    .sort((a, b) => b.score - a.score)
    .slice(0, 4);
  }, [jobs, filters]);

  if (recommendedJobs.length === 0) return null;

  return (
    <section className="space-y-4">
      <div className="flex items-center justify-between">
        <div>
          <h3 className="text-base sm:text-lg font-bold text-slate-900 flex items-center gap-2">
            <Sparkles className="w-4 h-4 text-brand-600" />
            <span>Opportunities Worth Exploring</span>
          </h3>
          <p className="text-xs text-slate-500">
            Intelligently ranked based on skills, tech stack affinity, and Chennai tech corridor proximity.
          </p>
        </div>

        <Button
          variant="outline"
          size="sm"
          onClick={() => setActiveTab('jobs')}
          rightIcon={<ChevronRight className="w-3.5 h-3.5" />}
        >
          View All Jobs
        </Button>
      </div>

      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        {recommendedJobs.map(({ job, score, strength, reasons }) => (
          <Card
            key={job.id}
            onClick={() => setSelectedJobId(job.id)}
            className="p-4 sm:p-5 flex flex-col justify-between hover:border-brand-300 hover:shadow-md cursor-pointer transition-all border border-slate-200"
          >
            <div className="space-y-3">
              <div className="flex items-start justify-between gap-3">
                <div className="flex items-center gap-3">
                  <img
                    src={job.companyLogo}
                    alt={job.companyName}
                    className="w-10 h-10 rounded-xl object-cover border border-slate-100 shadow-2xs"
                  />
                  <div>
                    <h4 className="font-bold text-slate-900 text-sm hover:text-brand-600 line-clamp-1">
                      {job.title}
                    </h4>
                    <p className="text-xs text-slate-600 font-medium">
                      {job.companyName} • <span className="text-slate-400">{job.companyHub}</span>
                    </p>
                  </div>
                </div>

                <Badge
                  variant={strength === 'Strong match' ? 'brand' : strength === 'Good match' ? 'success' : 'neutral'}
                  size="sm"
                >
                  <Zap className="w-3 h-3 mr-1" />
                  {strength} ({score}%)
                </Badge>
              </div>

              {/* Match Reasons */}
              <div className="bg-slate-50 p-2.5 rounded-xl border border-slate-100 flex flex-wrap gap-1.5 items-center text-[11px] text-slate-600">
                <span className="font-semibold text-slate-700">Why this matches:</span>
                {reasons.map((r, i) => (
                  <span key={i} className="inline-flex items-center gap-1 bg-white px-2 py-0.5 rounded-md border border-slate-200 text-slate-700">
                    <CheckCircle2 className="w-3 h-3 text-emerald-600" />
                    <span>{r}</span>
                  </span>
                ))}
              </div>

              {/* Technologies */}
              <div className="flex flex-wrap gap-1.5">
                {job.technologies.slice(0, 4).map((tech) => (
                  <Badge key={tech} variant="neutral" size="sm">
                    {tech}
                  </Badge>
                ))}
              </div>
            </div>

            <div className="mt-4 pt-3 border-t border-slate-100 flex items-center justify-between text-xs text-slate-500">
              <span className="font-medium text-slate-700">{job.salaryRange || 'Competitive Std'}</span>
              <span className="text-brand-600 font-semibold hover:underline">
                View & Apply →
              </span>
            </div>
          </Card>
        ))}
      </div>
    </section>
  );
};
