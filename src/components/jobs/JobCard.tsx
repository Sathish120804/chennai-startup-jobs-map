import React from 'react';
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
