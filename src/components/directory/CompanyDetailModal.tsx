import React from 'react';
import { 
  X, 
  MapPin, 
  Globe, 
  ExternalLink, 
  Briefcase, 
  GraduationCap, 
  Users, 
  Calendar, 
  ArrowRight,
  ShieldCheck,
  Code2
} from 'lucide-react';
import { useAppStore } from '../../store/useAppStore';
import { db } from '../../services/db';
import { Badge } from '../ui/Badge';
import { Button } from '../ui/Button';

export const CompanyDetailModal: React.FC = () => {
  const { selectedCompanyId, setSelectedCompanyId, setSelectedJobId } = useAppStore();

  if (!selectedCompanyId) return null;

  const company = db.getCompanyById(selectedCompanyId);
  if (!company) return null;

  const stats = db.getCompanyStats(company.id);
  const companyJobs = db.getJobsForCompany(company.id);

  return (
    <div className="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-3 sm:p-6 animate-fade-in">
      <div className="bg-white w-full max-w-4xl rounded-3xl shadow-2xl border border-slate-200 flex flex-col max-h-[92vh] overflow-hidden">
        <div className="relative bg-gradient-to-r from-slate-900 via-slate-800 to-brand-950 text-white p-6 sm:p-8">
          <button
            onClick={() => setSelectedCompanyId(null)}
            className="absolute top-4 right-4 p-2 rounded-full bg-white/10 hover:bg-white/20 text-white transition-colors"
          >
            <X className="w-5 h-5" />
          </button>

          <div className="flex flex-col sm:flex-row items-start sm:items-center gap-4">
            <img
              src={company.logo}
              alt={company.name}
              className="w-16 h-16 sm:w-20 sm:h-20 rounded-2xl object-cover border-2 border-white/20 shadow-lg bg-white"
            />
            <div className="space-y-1.5 flex-1">
              <div className="flex items-center gap-2 flex-wrap">
                <h2 className="text-2xl sm:text-3xl font-extrabold tracking-tight text-white">
                  {company.name}
                </h2>
                {company.verificationStatus === 'VERIFIED' && (
                  <Badge variant="success" size="sm">
                    <ShieldCheck className="w-3 h-3 mr-1" />
                    Verified Chennai Entity
                  </Badge>
                )}
                {company.isSeedData && (
                  <Badge variant="neutral" size="sm">Seed Record</Badge>
                )}
              </div>

              <p className="text-sm text-slate-300 max-w-2xl leading-relaxed">
                {company.tagline}
              </p>

              <div className="flex flex-wrap items-center gap-3 text-xs text-slate-300 pt-1">
                <span className="flex items-center gap-1">
                  <MapPin className="w-3.5 h-3.5 text-teal-400" />
                  <span>{company.hub}</span>
                </span>
                <span>•</span>
                <span className="flex items-center gap-1">
                  <Users className="w-3.5 h-3.5 text-slate-400" />
                  <span>{company.employeeCount}</span>
                </span>
                <span>•</span>
                <span className="flex items-center gap-1">
                  <Calendar className="w-3.5 h-3.5 text-slate-400" />
                  <span>Founded {company.foundedYear}</span>
                </span>
              </div>
            </div>
          </div>
        </div>

        <div className="flex-1 overflow-y-auto p-6 sm:p-8 space-y-8">
          <div className="grid grid-cols-2 sm:grid-cols-4 gap-3">
            <div className="p-3.5 rounded-2xl bg-slate-50 border border-slate-200">
              <div className="text-[11px] font-semibold text-slate-500 uppercase tracking-wider">Active Vacancies</div>
              <div className="text-xl font-bold text-slate-900 mt-0.5">{stats.activeJobsCount} Openings</div>
              <div className="text-[10px] text-emerald-600 font-medium mt-0.5">{stats.fresherJobsCount} Fresher Friendly</div>
            </div>

            <div className="p-3.5 rounded-2xl bg-slate-50 border border-slate-200">
              <div className="text-[11px] font-semibold text-slate-500 uppercase tracking-wider">Funding Stage</div>
              <div className="text-xl font-bold text-slate-900 mt-0.5">{company.fundingStage}</div>
              <div className="text-[10px] text-slate-500 truncate mt-0.5">{company.totalFundingRaised || 'Confidential'}</div>
            </div>

            <div className="p-3.5 rounded-2xl bg-slate-50 border border-slate-200">
              <div className="text-[11px] font-semibold text-slate-500 uppercase tracking-wider">Primary Sector</div>
              <div className="text-sm font-bold text-slate-900 mt-1 truncate">{company.categories[0]}</div>
              <div className="text-[10px] text-brand-600 font-medium mt-0.5">{company.companyTypes.join(', ')}</div>
            </div>

            <div className="p-3.5 rounded-2xl bg-slate-50 border border-slate-200 flex flex-col justify-between">
              <div className="text-[11px] font-semibold text-slate-500 uppercase tracking-wider">Official Links</div>
              <div className="flex items-center gap-2 pt-1">
                <a
                  href={company.website}
                  target="_blank"
                  rel="noreferrer"
                  className="p-2 rounded-lg bg-white border border-slate-200 text-slate-700 hover:text-brand-600 hover:border-brand-300 transition-colors"
                  title="Official Website"
                >
                  <Globe className="w-4 h-4" />
                </a>
                <a
                  href={company.careersUrl}
                  target="_blank"
                  rel="noreferrer"
                  className="flex-1 py-1.5 px-2.5 rounded-lg bg-brand-600 text-white text-xs font-semibold hover:bg-brand-700 flex items-center justify-center gap-1 transition-colors"
                >
                  <span>Careers</span>
                  <ExternalLink className="w-3 h-3" />
                </a>
              </div>
            </div>
          </div>

          <div className="space-y-3">
            <h3 className="text-sm font-bold text-slate-900 uppercase tracking-wider">
              About the Company
            </h3>
            <p className="text-sm text-slate-600 leading-relaxed">
              {company.description}
            </p>
            {company.keyLeaders?.founders && (
              <div className="flex items-center gap-2 text-xs text-slate-600">
                <span className="font-semibold text-slate-800">Founders:</span>
                <span>{company.keyLeaders.founders.join(', ')}</span>
              </div>
            )}
          </div>

          <div className="space-y-3">
            <h3 className="text-sm font-bold text-slate-900 uppercase tracking-wider flex items-center gap-1.5">
              <Code2 className="w-4 h-4 text-brand-600" />
              <span>Technology Stack</span>
            </h3>
            <div className="flex flex-wrap gap-1.5">
              {company.techStack.map((tech) => (
                <span
                  key={tech}
                  className="px-2.5 py-1 rounded-lg text-xs font-mono bg-slate-100 text-slate-800 border border-slate-200"
                >
                  {tech}
                </span>
              ))}
            </div>
          </div>

          <div className="space-y-2 bg-slate-50 p-4 rounded-2xl border border-slate-200/80">
            <div className="text-xs font-bold text-slate-700 uppercase tracking-wider flex items-center gap-1">
              <MapPin className="w-3.5 h-3.5 text-teal-600" />
              <span>Chennai Headquarters / Office Location</span>
            </div>
            <p className="text-xs text-slate-600 leading-relaxed">
              {company.address}
            </p>
          </div>

          <div className="space-y-4 pt-2">
            <div className="flex items-center justify-between">
              <div>
                <h3 className="text-base font-bold text-slate-900 flex items-center gap-2">
                  <Briefcase className="w-5 h-5 text-brand-600" />
                  <span>Current Opportunities ({companyJobs.length})</span>
                </h3>
                <p className="text-xs text-slate-500">
                  Direct public postings verified by the Chennai discovery engine.
                </p>
              </div>

              <a
                href={company.careersUrl}
                target="_blank"
                rel="noreferrer"
                className="text-xs font-semibold text-brand-600 hover:text-brand-700 flex items-center gap-1"
              >
                <span>View All on Careers Page</span>
                <ExternalLink className="w-3.5 h-3.5" />
              </a>
            </div>

            {companyJobs.length === 0 ? (
              <div className="p-8 text-center bg-slate-50 rounded-2xl border border-slate-200 text-slate-500 text-sm">
                No active vacancies currently tracked for this company. Check their official careers portal.
              </div>
            ) : (
              <div className="space-y-3">
                {companyJobs.map((job) => (
                  <div
                    key={job.id}
                    className="p-4 rounded-2xl border border-slate-200 bg-white hover:border-brand-300 hover:shadow-sm transition-all flex flex-col sm:flex-row items-start sm:items-center justify-between gap-4"
                  >
                    <div className="space-y-1.5 flex-1">
                      <div className="flex items-center gap-2 flex-wrap">
                        <span className="font-bold text-slate-900 text-sm">{job.title}</span>
                        {job.isFresher && (
                          <Badge variant="success" size="sm">
                            <GraduationCap className="w-3 h-3 mr-1" />
                            Fresher Friendly
                          </Badge>
                        )}
                        {job.isEngineering && (
                          <Badge variant="brand" size="sm">
                            {job.engineeringSubcategory || 'Engineering'}
                          </Badge>
                        )}
                      </div>

                      <p className="text-xs text-slate-600 line-clamp-2">
                        {job.descriptionSnippet}
                      </p>

                      <div className="flex flex-wrap items-center gap-2 text-[11px] text-slate-500 pt-1">
                        <span className="font-medium text-slate-700">{job.salaryRange || 'Competitive'}</span>
                        <span>•</span>
                        <span>{job.workplaceType}</span>
                        <span>•</span>
                        <span>Source: <strong className="text-slate-700">{job.sourceName}</strong></span>
                        {job.alternateSources && job.alternateSources.length > 0 && (
                          <span className="text-indigo-600 font-medium">
                            (+{job.alternateSources.length} other sources)
                          </span>
                        )}
                      </div>
                    </div>

                    <div className="flex items-center gap-2 shrink-0 w-full sm:w-auto">
                      <Button
                        size="sm"
                        variant="outline"
                        onClick={() => setSelectedJobId(job.id)}
                      >
                        Details
                      </Button>

                      <a
                        href={job.originalUrl}
                        target="_blank"
                        rel="noreferrer"
                        className="flex-1 sm:flex-none inline-flex items-center justify-center gap-1.5 px-4 py-2 rounded-lg text-xs font-semibold bg-brand-600 text-white hover:bg-brand-700 transition-colors shadow-xs"
                      >
                        <span>Apply on {job.sourceName.split(' ')[0]}</span>
                        <ArrowRight className="w-3.5 h-3.5" />
                      </a>
                    </div>
                  </div>
                ))}
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};
