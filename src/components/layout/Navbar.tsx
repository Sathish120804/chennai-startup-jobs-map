import React from 'react';
import { 
  MapPin, 
  Map, 
  Building2, 
  Briefcase, 
  BarChart3, 
  PlusCircle, 
  Search,
  Sparkles
} from 'lucide-react';
import { useAppStore } from '../../store/useAppStore';
import { Button } from '../ui/Button';
import { ActiveTab } from '../../types';

export const Navbar: React.FC = () => {
  const { activeTab, setActiveTab, filters, setSearchQuery } = useAppStore();

  const navItems: { tab: ActiveTab; label: string; icon: React.ReactNode; badge?: string }[] = [
    { tab: 'map', label: 'Live Map', icon: <Map className="w-4 h-4" /> },
    { tab: 'directory', label: 'Startups Directory', icon: <Building2 className="w-4 h-4" /> },
    { tab: 'jobs', label: 'Job Board', icon: <Briefcase className="w-4 h-4" /> },
    { tab: 'ecosystem', label: 'Ecosystem Pulse', icon: <BarChart3 className="w-4 h-4" /> },
  ];

  return (
    <header className="sticky top-0 z-40 bg-white/95 backdrop-blur-md border-b border-slate-200/80 shadow-xs">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="flex items-center justify-between h-16 gap-4">
          {/* Logo & Brand Identity */}
          <div className="flex items-center gap-3 shrink-0 cursor-pointer" onClick={() => setActiveTab('map')}>
            <div className="w-10 h-10 rounded-xl bg-gradient-to-tr from-brand-600 to-teal-500 flex items-center justify-center text-white shadow-md shadow-brand-500/20">
              <MapPin className="w-5 h-5 animate-bounce-subtle" />
            </div>
            <div>
              <div className="flex items-center gap-2">
                <span className="font-bold text-slate-900 tracking-tight text-base sm:text-lg">
                  CHENNAI<span className="text-brand-600">STARTUPS</span>
                </span>
                <span className="hidden sm:inline-flex items-center px-1.5 py-0.5 rounded text-[10px] font-semibold bg-brand-50 text-brand-700 border border-brand-200">
                  MAP & JOBS
                </span>
              </div>
              <p className="text-[11px] text-slate-500 hidden md:block">
                South Asia's SaaS & DeepTech Capital
              </p>
            </div>
          </div>

          {/* Search bar in Navbar */}
          <div className="hidden lg:flex items-center flex-1 max-w-md mx-4">
            <div className="relative w-full">
              <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400 pointer-events-none" />
              <input
                type="text"
                placeholder="Search Zoho, Freshworks, FinTech, OMR..."
                value={filters.searchQuery}
                onChange={(e) => setSearchQuery(e.target.value)}
                className="w-full bg-slate-100 hover:bg-slate-100/80 focus:bg-white text-sm text-slate-800 placeholder:text-slate-400 rounded-full pl-9 pr-4 py-2 border border-transparent focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20 transition-all"
              />
            </div>
          </div>

          {/* Navigation Tabs */}
          <nav className="hidden md:flex items-center gap-1 bg-slate-100/80 p-1 rounded-xl border border-slate-200/60">
            {navItems.map(({ tab, label, icon }) => {
              const isActive = activeTab === tab;
              return (
                <button
                  key={tab}
                  onClick={() => setActiveTab(tab)}
                  className={`flex items-center gap-2 px-3.5 py-1.5 rounded-lg text-xs font-medium transition-all ${
                    isActive
                      ? 'bg-white text-brand-700 shadow-xs font-semibold'
                      : 'text-slate-600 hover:text-slate-900 hover:bg-white/60'
                  }`}
                >
                  {icon}
                  <span>{label}</span>
                </button>
              );
            })}
          </nav>

          {/* Action CTAs */}
          <div className="flex items-center gap-2">
            <Button
              size="sm"
              variant="outline"
              className="hidden sm:inline-flex"
              leftIcon={<PlusCircle className="w-4 h-4 text-brand-600" />}
            >
              Add Startup
            </Button>

            <Button
              size="sm"
              variant="primary"
              leftIcon={<Sparkles className="w-4 h-4" />}
            >
              Post a Job
            </Button>
          </div>
        </div>
      </div>

      {/* Mobile navigation tab bar */}
      <div className="md:hidden flex items-center justify-around border-t border-slate-200/80 bg-slate-50/90 px-2 py-1.5 overflow-x-auto">
        {navItems.map(({ tab, label, icon }) => {
          const isActive = activeTab === tab;
          return (
            <button
              key={tab}
              onClick={() => setActiveTab(tab)}
              className={`flex flex-col items-center gap-0.5 px-3 py-1 rounded-lg text-[11px] font-medium whitespace-nowrap transition-colors ${
                isActive ? 'text-brand-600 font-semibold' : 'text-slate-500'
              }`}
            >
              {icon}
              <span>{label}</span>
            </button>
          );
        })}
      </div>
    </header>
  );
};
