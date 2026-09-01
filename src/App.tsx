import React from 'react';
import { Layout } from './components/layout/Layout';
import { MapFoundation } from './components/map/MapFoundation';
import { useAppStore } from './store/useAppStore';
import { 
  Building2, 
  Briefcase, 
  TrendingUp, 
  Users, 
  Search, 
  SlidersHorizontal,
  Compass,
  ArrowRight,
  Sparkles
} from 'lucide-react';
import { Badge } from './components/ui/Badge';
import { Button } from './components/ui/Button';
import { Card } from './components/ui/Card';
import { CHENNAI_TECH_HUBS, COMPANY_CATEGORIES } from './config/constants';

export const App: React.FC = () => {
  const { setActiveTab, filters, setSearchQuery, toggleCategory, toggleHub } = useAppStore();

  const metrics = [
    { label: 'Active Tech Startups', value: '450+', subtext: 'SaaS, DeepTech & AutoTech', icon: <Building2 className="w-5 h-5 text-brand-600" /> },
    { label: 'Open Job Opportunities', value: '1,200+', subtext: 'Engineering, AI, Product', icon: <Briefcase className="w-5 h-5 text-emerald-600" /> },
    { label: 'Ecosystem Funding', value: '$4.2B+', subtext: 'Raised across rounds', icon: <TrendingUp className="w-5 h-5 text-amber-600" /> },
    { label: 'Major Tech Hubs', value: '8 Corridors', subtext: 'OMR, Guindy, DLF & Tidel', icon: <Users className="w-5 h-5 text-indigo-600" /> },
  ];

  return (
    <Layout>
      <div className="flex-1 max-w-7xl w-full mx-auto px-4 sm:px-6 lg:px-8 py-6 space-y-8">
        {/* Hero Section */}
        <section className="relative overflow-hidden rounded-3xl bg-gradient-to-br from-slate-900 via-slate-800 to-brand-950 text-white p-6 sm:p-10 shadow-xl border border-slate-700/50">
          <div className="absolute top-0 right-0 -mr-20 -mt-20 w-96 h-96 bg-brand-500/10 rounded-full blur-3xl pointer-events-none"></div>
          <div className="absolute bottom-0 left-1/3 -mb-20 w-80 h-80 bg-teal-500/10 rounded-full blur-3xl pointer-events-none"></div>

          <div className="relative z-10 max-w-3xl space-y-4">
            <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-white/10 backdrop-blur-md border border-white/15 text-xs text-brand-200">
              <Sparkles className="w-3.5 h-3.5 text-brand-400" />
              <span>Interactive Chennai Tech Ecosystem Platform</span>
            </div>

            <h1 className="text-3xl sm:text-4xl lg:text-5xl font-extrabold tracking-tight text-white leading-tight">
              Discover Startups & Jobs across <span className="bg-clip-text text-transparent bg-gradient-to-r from-brand-300 via-teal-200 to-amber-200">Chennai</span>
            </h1>

            <p className="text-slate-300 text-sm sm:text-base leading-relaxed max-w-2xl">
              Explore South Asia's premier SaaS and DeepTech capital. Pinpoint company headquarters across OMR, Guindy, and DLF Porur, filter by funding or tech stack, and find your next career move.
            </p>

            {/* Quick search input */}
            <div className="pt-2 flex flex-col sm:flex-row items-stretch sm:items-center gap-2 max-w-xl">
              <div className="relative flex-1">
                <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
                <input
                  type="text"
                  placeholder="Search startups (e.g., Zoho, Kissflow, Chargebee) or tech domains..."
                  value={filters.searchQuery}
                  onChange={(e) => setSearchQuery(e.target.value)}
                  className="w-full bg-white/10 border border-white/20 rounded-xl pl-10 pr-4 py-2.5 text-sm text-white placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-brand-400 focus:bg-slate-900/90 transition-all"
                />
              </div>
              <Button
                variant="primary"
                onClick={() => setActiveTab('directory')}
                rightIcon={<ArrowRight className="w-4 h-4" />}
                className="whitespace-nowrap"
              >
                Browse All Startups
              </Button>
            </div>
          </div>
        </section>

        {/* Ecosystem Metric Highlights */}
        <section className="grid grid-cols-2 md:grid-cols-4 gap-4">
          {metrics.map((metric) => (
            <Card key={metric.label} className="p-4 sm:p-5 flex flex-col justify-between hover:border-brand-200">
              <div className="flex items-center justify-between mb-3">
                <div className="w-9 h-9 rounded-lg bg-slate-50 flex items-center justify-center border border-slate-100">
                  {metric.icon}
                </div>
                <Badge variant="neutral" size="sm">Chennai</Badge>
              </div>
              <div>
                <div className="text-2xl sm:text-3xl font-bold text-slate-900 tracking-tight">
                  {metric.value}
                </div>
                <div className="text-xs font-semibold text-slate-700 mt-0.5">
                  {metric.label}
                </div>
                <div className="text-[11px] text-slate-500 mt-1">
                  {metric.subtext}
                </div>
              </div>
            </Card>
          ))}
        </section>

        {/* Filter Quick Pills */}
        <section className="space-y-3">
          <div className="flex items-center justify-between">
            <div className="flex items-center gap-2 text-sm font-semibold text-slate-800">
              <SlidersHorizontal className="w-4 h-4 text-brand-600" />
              <span>Popular Categories & Hubs</span>
            </div>
            <div className="text-xs text-slate-500">
              Interactive filter ready
            </div>
          </div>

          <div className="flex flex-wrap items-center gap-2">
            {COMPANY_CATEGORIES.slice(0, 6).map((cat) => {
              const isSelected = filters.selectedCategories.includes(cat);
              return (
                <button
                  key={cat}
                  onClick={() => toggleCategory(cat)}
                  className={`px-3 py-1.5 rounded-full text-xs font-medium border transition-all ${
                    isSelected
                      ? 'bg-brand-600 border-brand-600 text-white shadow-xs'
                      : 'bg-white border-slate-200 text-slate-700 hover:border-slate-300 hover:bg-slate-50'
                  }`}
                >
                  {cat}
                </button>
              );
            })}

            {CHENNAI_TECH_HUBS.slice(0, 4).map((hub) => {
              const isSelected = filters.selectedHubs.includes(hub.name);
              return (
                <button
                  key={hub.name}
                  onClick={() => toggleHub(hub.name)}
                  className={`px-3 py-1.5 rounded-full text-xs font-medium border transition-all ${
                    isSelected
                      ? 'bg-teal-600 border-teal-600 text-white shadow-xs'
                      : 'bg-white border-slate-200 text-slate-700 hover:border-slate-300 hover:bg-slate-50'
                  }`}
                >
                  📍 {hub.name}
                </button>
              );
            })}
          </div>
        </section>

        {/* Foundation Map Section */}
        <section className="space-y-4">
          <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-2">
            <div>
              <h2 className="text-xl font-bold text-slate-900 tracking-tight flex items-center gap-2">
                <Compass className="w-5 h-5 text-brand-600" />
                <span>Live Chennai Tech Corridors Map</span>
              </h2>
              <p className="text-xs text-slate-500">
                Visualizing physical startup nodes, tech parks, and innovation centers.
              </p>
            </div>

            <div className="flex items-center gap-2">
              <Badge variant="success" size="md">Foundation Ready</Badge>
              <Badge variant="outline" size="md">Vite + React + Leaflet</Badge>
            </div>
          </div>

          <MapFoundation />
        </section>
      </div>
    </Layout>
  );
};

export default App;
