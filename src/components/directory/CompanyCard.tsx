import React from 'react';
import { Company } from '../../types';
import { db } from '../../services/db';
import { useAppStore } from '../../store/useAppStore';
import { 
  MapPin, 
  Briefcase, 
  GraduationCap, 
  CheckCircle2, 
  ArrowUpRight
} from 'lucide-react';
import { Badge } from '../ui/Badge';
import { Card } from '../ui/Card';

export interface CompanyCardProps {
  company: Company;
}

export const CompanyCard: React.FC<CompanyCardProps> = ({ company }) => {
  const { setSelectedCompanyId, setHoveredCompanyId, hoveredCompanyId } = useAppStore();
  const stats = db.getCompanyStats(company.id);
  const isHovered = hoveredCompanyId === company.id;

  return (
    <Card
      hoverable
      onMouseEnter={() => setHoveredCompanyId(company.id)}
      onMouseLeave={() => setHoveredCompanyId(null)}
      onClick={() => setSelectedCompanyId(company.id)}
      className={`p-5 flex flex-col justify-between transition-all duration-200 ${
        isHovered ? 'border-brand-500 shadow-md ring-2 ring-brand-500/10' : ''
      }`}
    >
      <div className="space-y-3">
        <div className="flex items-start justify-between gap-3">
          <div className="flex items-center gap-3">
            <img
              src={company.logo}
              alt={company.name}
              className="w-12 h-12 rounded-xl object-cover border border-slate-200/80 shadow-xs"
            />
            <div>
              <div className="flex items-center gap-1.5">
                <h3 className="font-bold text-slate-900 text-base group-hover:text-brand-600 transition-colors">
                  {company.name}
                </h3>
                {company.verificationStatus === 'VERIFIED' && (
                  <CheckCircle2 className="w-4 h-4 text-brand-600" />
                )}
              </div>
              <div className="flex items-center gap-1 text-xs text-slate-500">
                <MapPin className="w-3.5 h-3.5 text-teal-600 shrink-0" />
                <span>{company.hub}</span>
              </div>
            </div>
          </div>

          <Badge
            variant={stats.activeJobsCount > 0 ? 'success' : 'neutral'}
            size="sm"
            className="shrink-0"
          >
            {stats.activeJobsCount > 0 ? `${stats.activeJobsCount} Hiring` : 'Selective'}
          </Badge>
        </div>

        <p className="text-xs text-slate-600 line-clamp-2 leading-relaxed">
          {company.tagline}
        </p>

        <div className="flex flex-wrap items-center gap-1.5 pt-0.5">
          <span className="px-2 py-0.5 rounded text-[11px] font-semibold bg-slate-100 text-slate-700">
            {company.categories[0]}
          </span>
          <span className="px-2 py-0.5 rounded text-[11px] font-medium bg-brand-50 text-brand-700">
            {company.fundingStage}
          </span>
          <span className="px-2 py-0.5 rounded text-[11px] text-slate-500">
            Est. {company.foundedYear}
          </span>
        </div>

        <div className="flex flex-wrap items-center gap-1">
          {company.techStack.slice(0, 4).map((tech) => (
            <span
              key={tech}
              className="px-1.5 py-0.5 rounded text-[10px] font-mono bg-slate-100 text-slate-600 border border-slate-200/60"
            >
              {tech}
            </span>
          ))}
          {company.techStack.length > 4 && (
            <span className="text-[10px] text-slate-400 font-medium">
              +{company.techStack.length - 4}
            </span>
          )}
        </div>
      </div>

      <div className="pt-4 mt-3 border-t border-slate-100 flex items-center justify-between">
        <div className="flex items-center gap-2 text-xs">
          {stats.fresherJobsCount > 0 && (
            <span className="inline-flex items-center gap-1 text-emerald-700 font-medium text-[11px]">
              <GraduationCap className="w-3.5 h-3.5" />
              <span>{stats.fresherJobsCount} Fresher</span>
            </span>
          )}
          {stats.engineeringJobsCount > 0 && stats.fresherJobsCount === 0 && (
            <span className="inline-flex items-center gap-1 text-indigo-700 font-medium text-[11px]">
              <Briefcase className="w-3.5 h-3.5" />
              <span>{stats.engineeringJobsCount} Engineering</span>
            </span>
          )}
          {stats.activeJobsCount === 0 && (
            <span className="text-slate-400 text-[11px]">No open positions</span>
          )}
        </div>

        <div className="flex items-center gap-1 text-xs font-semibold text-brand-600 hover:text-brand-700">
          <span>Explore</span>
          <ArrowUpRight className="w-3.5 h-3.5" />
        </div>
      </div>
    </Card>
  );
};
