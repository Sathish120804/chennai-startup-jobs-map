import React, { useEffect, useState } from 'react';
import { Layout } from './components/layout/Layout';
import { ChennaiMap } from './components/map/ChennaiMap';
import { CompanyGrid } from './components/directory/CompanyGrid';
import { JobList } from './components/jobs/JobList';
import { EcosystemPulse } from './components/ecosystem/EcosystemPulse';
import { AdminDashboard } from './components/admin/AdminDashboard';
import { FilterBar } from './components/filters/FilterBar';
import { CompanyDetailModal } from './components/directory/CompanyDetailModal';
import { JobDetailModal } from './components/jobs/JobDetailModal';
import { AdvancedFilterModal } from './components/filters/AdvancedFilterModal';
import { SubmitCompanyModal } from './components/submissions/SubmitCompanyModal';
import { SubmitJobModal } from './components/submissions/SubmitJobModal';
import { useAppStore } from './store/useAppStore';
import { db } from './services/db';
import { 
  Building2, 
  Briefcase, 
  TrendingUp, 
  Users, 
  Compass, 
  Sparkles, 
  GraduationCap, 
  ArrowRight
} from 'lucide-react';
import { Card } from './components/ui/Card';
import { Badge } from './components/ui/Badge';
import { Button } from './components/ui/Button';

export const App: React.FC = () => {
  const { 
    activeTab, 
    setActiveTab, 
    setQuickFilter 
  } = useAppStore();

  const [, setDbVersion] = useState(0);

  useEffect(() => {
    const unsub = db.subscribe(() => setDbVersion((v) => v + 1));
    return unsub;
  }, []);

  const totalCompanies = db.getCompanies().length;
  const totalJobs = db.getJobs().length;
  const fresherJobsCount = db.getJobs().filter(j => j.isFresher).length;

  const metrics = [
    { label: 'Active Tech Companies', value: `${totalCompanies}+`, subtext: 'SaaS, DeepTech & AutoTech', icon: <Building2 className="w-5 h-5 text-brand-600" /> },
    { label: 'Current Job Openings', value: `${totalJobs}+`, subtext: `${fresherJobsCount} Fresher Roles`, icon: <Briefcase className="w-5 h-5 text-emerald-600" /> },
    { label: 'Ecosystem Funding / ARR', value: '$4.2B+', subtext: 'SaaS Capital of South Asia', icon: <TrendingUp className="w-5 h-5 text-amber-600" /> },
    { label: 'Major Tech Hubs', value: '8 Corridors', subtext: 'OMR, Guindy, DLF & Tidel', icon: <Users className="w-5 h-5 text-indigo-600" /> },
  ];

  return (
    <Layout>
      <div className="flex-1 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-6 space-y-6">
        {/* Hero Section */}
        {(activeTab === 'map' || activeTab === 'directory') && (
          <section className="relative overflow-hidden rounded-3xl bg-gradient-to-br from-slate-900 via-slate-800 to-brand-950 text-white p-6 sm:p-10 shadow-xl border border-slate-700/50">
            <div className="absolute top-0 right-0 -mr-20 -mt-20 w-96 h-96 bg-brand-500/15 rounded-full blur-3xl pointer-events-none"></div>
            <div className="absolute bottom-0 left-1/3 -mb-20 w-80 h-80 bg-teal-500/15 rounded-full blur-3xl pointer-events-none"></div>

            <div className="relative z-10 max-w-3xl space-y-4">
              <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-white/10 backdrop-blur-md border border-white/15 text-xs text-brand-200">
                <Sparkles className="w-3.5 h-3.5 text-brand-400" />
                <span>Chennai Startup & Jobs Discovery Engine</span>
              </div>

              <h1 className="text-3xl sm:text-4xl lg:text-5xl font-extrabold tracking-tight text-white leading-tight">
                Discover Chennai's <span className="bg-clip-text text-transparent bg-gradient-to-r from-brand-300 via-teal-200 to-amber-200">Startups, Tech Parks & Jobs</span>
              </h1>

              <p className="text-slate-300 text-xs sm:text-sm leading-relaxed max-w-2xl">
                Explore South Asia's SaaS and DeepTech capital on an interactive map. Discover company headquarters across OMR, Guindy, and DLF Porur, filter by engineering stack or fresher opportunities, and apply directly to verified vacancies.
              </p>

              {/* Fast navigation buttons */}
              <div className="pt-2 flex flex-wrap items-center gap-2.5">
                <Button
                  variant="primary"
                  onClick={() => {
                    setActiveTab('map');
                    setQuickFilter('hiring');
                  }}
                  leftIcon={<Briefcase className="w-4 h-4" />}
                >
                  Explore Hiring Startups on Map
                </Button>

                <Button
                  variant="outline"
                  onClick={() => {
                    setActiveTab('jobs');
                    setQuickFilter('fresher');
                  }}
                  leftIcon={<GraduationCap className="w-4 h-4 text-emerald-400" />}
                  className="text-white border-white/20 bg-white/10 hover:bg-white/20 hover:border-white/30"
                >
                  Fresher Engineering Jobs ({fresherJobsCount})
                </Button>
              </div>
            </div>
          </section>
        )}

        {/* Ecosystem Metric Highlights */}
        {(activeTab === 'map' || activeTab === 'directory') && (
          <section className="grid grid-cols-2 md:grid-cols-4 gap-3 sm:gap-4">
            {metrics.map((metric) => (
              <Card key={metric.label} className="p-4 sm:p-5 flex flex-col justify-between hover:border-brand-200">
                <div className="flex items-center justify-between mb-2 sm:mb-3">
                  <div className="w-9 h-9 rounded-xl bg-slate-50 flex items-center justify-center border border-slate-100 shadow-xs">
                    {metric.icon}
                  </div>
                  <Badge variant="neutral" size="sm">Chennai</Badge>
                </div>
                <div>
                  <div className="text-xl sm:text-2xl font-extrabold text-slate-900 tracking-tight">
                    {metric.value}
                  </div>
                  <div className="text-xs font-semibold text-slate-700 mt-0.5">
                    {metric.label}
                  </div>
                  <div className="text-[11px] text-slate-500 mt-0.5 truncate">
                    {metric.subtext}
                  </div>
                </div>
              </Card>
            ))}
          </section>
        )}

        {/* Global Filter Bar */}
        {activeTab !== 'admin' && activeTab !== 'ecosystem' && (
          <FilterBar />
        )}

        {/* Main View Switcher */}
        <section className="space-y-4">
          {activeTab === 'map' && (
            <div className="space-y-6">
              <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-2">
                <div>
                  <h2 className="text-lg sm:text-xl font-bold text-slate-900 tracking-tight flex items-center gap-2">
                    <Compass className="w-5 h-5 text-brand-600" />
                    <span>Live Interactive Tech Corridors Map</span>
                  </h2>
                  <p className="text-xs text-slate-500">
                    Click company pins to inspect hiring status, active openings, and office location.
                  </p>
                </div>

                <div className="flex items-center gap-2">
                  <Badge variant="success" size="md">Live Sync Active</Badge>
                </div>
              </div>

              <ChennaiMap />

              {/* Synchronized Directory Preview below Map */}
              <div className="space-y-3 pt-4">
                <div className="flex items-center justify-between">
                  <h3 className="text-base font-bold text-slate-900 flex items-center gap-2">
                    <Building2 className="w-4 h-4 text-brand-600" />
                    <span>Startups Matching Current Map Filters</span>
                  </h3>
                  <Button
                    variant="outline"
                    size="sm"
                    onClick={() => setActiveTab('directory')}
                    rightIcon={<ArrowRight className="w-3.5 h-3.5" />}
                  >
                    View All in Directory View
                  </Button>
                </div>
                <CompanyGrid />
              </div>
            </div>
          )}

          {activeTab === 'directory' && (
            <div className="space-y-4">
              <CompanyGrid />
            </div>
          )}

          {activeTab === 'jobs' && (
            <div className="space-y-4">
              <JobList />
            </div>
          )}

          {activeTab === 'ecosystem' && (
            <EcosystemPulse />
          )}

          {activeTab === 'admin' && (
            <AdminDashboard />
          )}
        </section>
      </div>

      {/* Global Modals & Drawers */}
      <CompanyDetailModal />
      <JobDetailModal />
      <AdvancedFilterModal />
      <SubmitCompanyModal />
      <SubmitJobModal />
    </Layout>
  );
};

export default App;
