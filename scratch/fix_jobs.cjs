const fs = require('fs');

fs.writeFileSync('src/components/jobs/JobCard.tsx', `import React from 'react';
import { Job } from '../../types';
import { useAppStore } from '../../store/useAppStore';
import { 
  MapPin, 
  GraduationCap, 
  ExternalLink, 
  Layers
} from 'lucide-react';
import { Badge } from '../ui/Badge';
import { Card } from '../ui/Card';

export interface JobCardProps {
  job: Job;
}

export const JobCard: React.FC<JobCardProps> = ({ job }) => {
  const { setSelectedCompanyId, setSelectedJobId } = useAppStore();

  const totalSourcesCount = 1 + (job.alternateSources?.length || 0);

  return (
    <Card
      hoverable
      onClick={() => setSelectedJobId(job.id)}
      className="p-5 flex flex-col justify-between transition-all duration-200 border-slate-200 hover:border-brand-400 group"
    >
      <div className="space-y-3">
        <div className="flex items-start justify-between gap-3">
          <div className="flex items-start gap-3">
            <img
              src={job.companyLogo}
              alt={job.companyName}
              className="w-11 h-11 rounded-xl object-cover border border-slate-200 shadow-xs shrink-0"
            />
            <div className="space-y-1">
              <h3 className="font-bold text-slate-900 text-sm sm:text-base group-hover:text-brand-600 transition-colors leading-snug">
                {job.title}
              </h3>
              <div className="flex flex-wrap items-center gap-2 text-xs">
                <button
                  onClick={(e) => {
                    e.stopPropagation();
                    setSelectedCompanyId(job.companyId);
                  }}
                  className="font-semibold text-slate-700 hover:text-brand-600 hover:underline"
                >
                  {job.companyName}
                </button>
                <span>•</span>
                <span className="flex items-center gap-1 text-slate-500">
                  <MapPin className="w-3.5 h-3.5 text-teal-600" />
                  <span>{job.companyHub}</span>
                </span>
              </div>
            </div>
          </div>

          <div className="flex flex-col items-end gap-1 shrink-0">
            {job.isFresher && (
              <Badge variant="success" size="sm">
                <GraduationCap className="w-3 h-3 mr-1" />
                Fresher (0-1 yrs)
              </Badge>
            )}
            {job.isInternship && (
              <Badge variant="warning" size="sm">Internship</Badge>
            )}
            {job.freshnessStatus === 'NEW' && !job.isFresher && !job.isInternship && (
              <Badge variant="brand" size="sm">New</Badge>
            )}
          </div>
        </div>

        <p className="text-xs text-slate-600 line-clamp-2 leading-relaxed">
          {job.descriptionSnippet}
        </p>

        <div className="flex flex-wrap items-center gap-1.5 pt-1">
          {job.engineeringSubcategory && (
            <span className="px-2 py-0.5 rounded-md text-[11px] font-semibold bg-indigo-50 text-indigo-700 border border-indigo-200/60">
              {job.engineeringSubcategory}
            </span>
          )}
          {job.technologies.slice(0, 4).map((tech) => (
            <span
              key={tech}
              className="px-2 py-0.5 rounded-md text-[11px] font-mono bg-slate-100 text-slate-700 border border-slate-200/60"
            >
              {tech}
            </span>
          ))}
          {job.technologies.length > 4 && (
            <span className="text-[10px] text-slate-400 font-medium">
              +{job.technologies.length - 4}
            </span>
          )}
        </div>

        <div className="flex flex-wrap items-center gap-2.5 text-xs text-slate-500 pt-1">
          {job.salaryRange && (
            <span className="font-semibold text-slate-900 bg-slate-100 px-2 py-0.5 rounded">
              {job.salaryRange}
            </span>
          )}
          <span>{job.workplaceType}</span>
          <span>•</span>
          <span>{job.experienceLevel}</span>
        </div>
      </div>

      <div className="pt-4 mt-3 border-t border-slate-100 flex flex-col sm:flex-row sm:items-center justify-between gap-3">
        <div className="flex items-center gap-2 text-xs">
          <span className="text-slate-400">Source:</span>
          <span className="font-medium text-slate-700">{job.sourceName}</span>
          {totalSourcesCount > 1 && (
            <span className="inline-flex items-center gap-0.5 px-2 py-0.5 rounded-full text-[10px] font-semibold bg-purple-50 text-purple-700 border border-purple-200">
              <Layers className="w-3 h-3" />
              <span>Found on {totalSourcesCount} sources</span>
            </span>
          )}
        </div>

        <div className="flex items-center gap-2">
          <a
            href={job.originalUrl}
            target="_blank"
            rel="noreferrer"
            onClick={(e) => e.stopPropagation()}
            className="inline-flex items-center gap-1.5 px-3 py-1.5 rounded-lg text-xs font-semibold bg-brand-600 hover:bg-brand-700 text-white transition-colors shadow-xs"
          >
            <span>View Original Job</span>
            <ExternalLink className="w-3.5 h-3.5" />
          </a>
        </div>
      </div>
    </Card>
  );
};
`, 'utf8');

fs.writeFileSync('src/components/jobs/JobDetailModal.tsx', `import React from 'react';
import { 
  X, 
  MapPin, 
  Building2, 
  ExternalLink, 
  Sparkles, 
  Layers, 
  CheckCircle2, 
  ArrowRight,
  Cpu
} from 'lucide-react';
import { useAppStore } from '../../store/useAppStore';
import { db } from '../../services/db';
import { Badge } from '../ui/Badge';

export const JobDetailModal: React.FC = () => {
  const { selectedJobId, setSelectedJobId, setSelectedCompanyId } = useAppStore();

  if (!selectedJobId) return null;

  const job = db.getJobById(selectedJobId);
  if (!job) return null;

  return (
    <div className="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-3 sm:p-6 animate-fade-in">
      <div className="bg-white w-full max-w-3xl rounded-3xl shadow-2xl border border-slate-200 flex flex-col max-h-[90vh] overflow-hidden">
        <div className="flex items-start justify-between p-6 border-b border-slate-200 bg-slate-50/80">
          <div className="flex items-start gap-4">
            <img
              src={job.companyLogo}
              alt={job.companyName}
              className="w-14 h-14 rounded-2xl object-cover border border-slate-200 shadow-xs bg-white shrink-0"
            />
            <div className="space-y-1">
              <div className="flex items-center gap-2 flex-wrap">
                <h2 className="text-xl sm:text-2xl font-bold text-slate-900 leading-tight">
                  {job.title}
                </h2>
              </div>
              <div className="flex flex-wrap items-center gap-2 text-xs">
                <button
                  onClick={() => {
                    setSelectedJobId(null);
                    setSelectedCompanyId(job.companyId);
                  }}
                  className="font-bold text-brand-600 hover:underline flex items-center gap-1"
                >
                  <Building2 className="w-3.5 h-3.5" />
                  <span>{job.companyName}</span>
                </button>
                <span>•</span>
                <span className="flex items-center gap-1 text-slate-500">
                  <MapPin className="w-3.5 h-3.5 text-teal-600" />
                  <span>{job.location}</span>
                </span>
              </div>
            </div>
          </div>

          <button
            onClick={() => setSelectedJobId(null)}
            className="p-1.5 rounded-lg text-slate-400 hover:text-slate-700 hover:bg-slate-200/60 transition-colors"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        <div className="flex-1 overflow-y-auto p-6 space-y-6">
          <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
            <div className="p-3 rounded-xl bg-slate-50 border border-slate-200">
              <div className="text-[10px] font-bold text-slate-400 uppercase tracking-wider">Salary / Compensation</div>
              <div className="text-xs font-bold text-slate-900 mt-0.5">{job.salaryRange || 'Competitive (Market Std)'}</div>
            </div>

            <div className="p-3 rounded-xl bg-slate-50 border border-slate-200">
              <div className="text-[10px] font-bold text-slate-400 uppercase tracking-wider">Experience Level</div>
              <div className="text-xs font-bold text-slate-900 mt-0.5">{job.experienceLevel}</div>
            </div>

            <div className="p-3 rounded-xl bg-slate-50 border border-slate-200">
              <div className="text-[10px] font-bold text-slate-400 uppercase tracking-wider">Workplace Mode</div>
              <div className="text-xs font-bold text-slate-900 mt-0.5">{job.workplaceType} ({job.jobType})</div>
            </div>

            <div className="p-3 rounded-xl bg-slate-50 border border-slate-200">
              <div className="text-[10px] font-bold text-slate-400 uppercase tracking-wider">Job Freshness</div>
              <div className="text-xs font-bold text-emerald-700 mt-0.5 flex items-center gap-1">
                <CheckCircle2 className="w-3.5 h-3.5" />
                <span>{job.freshnessStatus}</span>
              </div>
            </div>
          </div>

          <div className="space-y-2">
            <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider">
              Role Summary & Requirements
            </h4>
            <p className="text-xs sm:text-sm text-slate-700 leading-relaxed bg-slate-50 p-4 rounded-2xl border border-slate-200">
              {job.descriptionSnippet}
            </p>
          </div>

          <div className="space-y-2">
            <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider flex items-center gap-1.5">
              <Cpu className="w-4 h-4 text-purple-600" />
              <span>Technologies & Skills Extracted</span>
            </h4>
            <div className="flex flex-wrap gap-1.5">
              {job.technologies.map((tech) => (
                <span
                  key={tech}
                  className="px-2.5 py-1 rounded-lg text-xs font-mono bg-purple-50 text-purple-800 border border-purple-200/80 font-semibold"
                >
                  {tech}
                </span>
              ))}
            </div>
          </div>

          <div className="p-4 rounded-2xl bg-brand-50/50 border border-brand-200/70 space-y-3">
            <div className="flex items-center gap-2 text-xs font-bold text-brand-900 uppercase tracking-wider">
              <Sparkles className="w-3.5 h-3.5 text-brand-600" />
              <span>Chennai Engine Classification Signals</span>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 text-xs text-slate-700">
              <div className="p-2.5 rounded-xl bg-white border border-brand-100 space-y-1">
                <div className="font-semibold text-slate-900 flex items-center justify-between">
                  <span>Location Relevance:</span>
                  <Badge variant="success" size="sm">{job.chennaiRelevance}</Badge>
                </div>
                <div className="text-[11px] text-slate-500">
                  Confidence: {job.relevanceConfidence}% • Verified within {job.companyHub}
                </div>
              </div>

              <div className="p-2.5 rounded-xl bg-white border border-brand-100 space-y-1">
                <div className="font-semibold text-slate-900 flex items-center justify-between">
                  <span>Fresher Classification:</span>
                  <Badge variant={job.isFresher ? 'success' : 'neutral'} size="sm">
                    {job.isFresher ? 'Fresher Friendly' : 'Experienced'}
                  </Badge>
                </div>
                <div className="text-[11px] text-slate-500">
                  Fresher Confidence: {job.fresherConfidence}%
                </div>
              </div>
            </div>
          </div>

          {job.alternateSources && job.alternateSources.length > 0 && (
            <div className="space-y-2">
              <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider flex items-center gap-1.5">
                <Layers className="w-4 h-4 text-purple-600" />
                <span>Found on Multiple Sources ({1 + job.alternateSources.length} Portals)</span>
              </h4>
              <div className="space-y-1.5">
                <div className="p-2.5 rounded-xl bg-slate-50 border border-slate-200 flex items-center justify-between text-xs">
                  <div className="font-medium text-slate-800">
                    Primary: <strong>{job.sourceName}</strong>
                  </div>
                  <a href={job.originalUrl} target="_blank" rel="noreferrer" className="text-brand-600 font-semibold hover:underline flex items-center gap-1">
                    <span>Source 1</span>
                    <ExternalLink className="w-3 h-3" />
                  </a>
                </div>
                {job.alternateSources.map((alt, i) => (
                  <div key={alt.url} className="p-2.5 rounded-xl bg-slate-50 border border-slate-200 flex items-center justify-between text-xs">
                    <div className="font-medium text-slate-800">
                      Alternate {i + 2}: <strong>{alt.sourceName}</strong>
                    </div>
                    <a href={alt.url} target="_blank" rel="noreferrer" className="text-brand-600 font-semibold hover:underline flex items-center gap-1">
                      <span>Source {i + 2}</span>
                      <ExternalLink className="w-3 h-3" />
                    </a>
                  </div>
                ))}
              </div>
            </div>
          )}
        </div>

        <div className="flex items-center justify-between p-6 border-t border-slate-200 bg-slate-50">
          <div className="text-xs text-slate-500">
            Source: <strong className="text-slate-800">{job.sourceName}</strong>
          </div>

          <a
            href={job.originalUrl}
            target="_blank"
            rel="noreferrer"
            className="inline-flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold bg-brand-600 hover:bg-brand-700 text-white transition-all shadow-md shadow-brand-500/20"
          >
            <span>View Original Job & Apply</span>
            <ArrowRight className="w-4 h-4" />
          </a>
        </div>
      </div>
    </div>
  );
};
`, 'utf8');

fs.writeFileSync('src/components/jobs/JobList.tsx', `import React, { useMemo } from 'react';
import { useAppStore } from '../../store/useAppStore';
import { db } from '../../services/db';
import { JobCard } from './JobCard';
import { Briefcase, RotateCcw } from 'lucide-react';
import { Button } from '../ui/Button';

export const JobList: React.FC = () => {
  const { filters, setSortBy, resetFilters, setAdvancedFilterOpen } = useAppStore();

  const filteredJobs = useMemo(() => {
    return db.getFilteredJobs(filters);
  }, [filters]);

  return (
    <div className="space-y-4">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-3 bg-white px-5 py-3.5 rounded-2xl border border-slate-200 shadow-xs">
        <div className="flex items-center gap-2 text-sm text-slate-700">
          <Briefcase className="w-4 h-4 text-brand-600" />
          <span>
            Showing <strong className="text-slate-900 font-bold">{filteredJobs.length}</strong> Open Job Opportunities in Chennai
          </span>
        </div>

        <div className="flex items-center gap-3">
          <label className="text-xs text-slate-500 font-medium">Sort by:</label>
          <select
            value={filters.sortBy}
            onChange={(e) => setSortBy(e.target.value as any)}
            className="text-xs font-semibold bg-slate-50 border border-slate-200 rounded-lg px-2.5 py-1.5 text-slate-800 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
          >
            <option value="recent">Latest Discovered</option>
            <option value="featured">Featured Roles</option>
          </select>
        </div>
      </div>

      {filteredJobs.length === 0 ? (
        <div className="p-12 text-center bg-white rounded-3xl border border-slate-200 shadow-xs space-y-4">
          <div className="w-12 h-12 rounded-2xl bg-brand-50 flex items-center justify-center text-brand-600 mx-auto">
            <Briefcase className="w-6 h-6" />
          </div>
          <div className="space-y-1 max-w-md mx-auto">
            <h3 className="text-base font-bold text-slate-900">No matching job vacancies found</h3>
            <p className="text-xs text-slate-500">
              Try adjusting your tech keywords (.NET, React, Python), clearing experience constraints, or broadening location corridors.
            </p>
          </div>
          <div className="flex items-center justify-center gap-2 pt-2">
            <Button variant="outline" size="sm" onClick={resetFilters} leftIcon={<RotateCcw className="w-3.5 h-3.5" />}>
              Reset Filters
            </Button>
            <Button variant="primary" size="sm" onClick={() => setAdvancedFilterOpen(true)}>
              Adjust Filters
            </Button>
          </div>
        </div>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {filteredJobs.map((job) => (
            <JobCard key={job.id} job={job} />
          ))}
        </div>
      )}
    </div>
  );
};
`, 'utf8');

console.log('Fixed job components');
