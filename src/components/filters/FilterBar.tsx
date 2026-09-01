import React from 'react';
import { 
  Search, 
  SlidersHorizontal, 
  X, 
  MapPin, 
  Briefcase, 
  GraduationCap, 
  Code2, 
  Sparkles, 
  Rocket, 
  Building2 
} from 'lucide-react';
import { useAppStore } from '../../store/useAppStore';
import { CHENNAI_TECH_HUBS } from '../../config/constants';
import { Button } from '../ui/Button';
import { Badge } from '../ui/Badge';

export const FilterBar: React.FC = () => {
  const { 
    filters, 
    setSearchQuery, 
    setQuickFilter, 
    toggleHub, 
    resetFilters, 
    setAdvancedFilterOpen 
  } = useAppStore();

  const activeFilterCount = 
    filters.selectedHubs.length +
    filters.selectedCategories.length +
    filters.selectedCompanyTypes.length +
    filters.selectedFundingStages.length +
    filters.selectedExperienceLevels.length +
    filters.selectedWorkplaceTypes.length +
    filters.selectedEngineeringSubcategories.length +
    filters.selectedTechnologies.length +
    (filters.isHiringOnly ? 1 : 0) +
    (filters.isFresherOnly ? 1 : 0) +
    (filters.isEngineeringOnly ? 1 : 0) +
    (filters.isInternshipOnly ? 1 : 0) +
    (filters.isFeaturedOnly ? 1 : 0);

  const quickPills = [
    { key: 'hiring' as const, label: 'Hiring Now', icon: <Briefcase className="w-3.5 h-3.5" />, active: filters.isHiringOnly },
    { key: 'fresher' as const, label: 'Fresher (0-1 yrs)', icon: <GraduationCap className="w-3.5 h-3.5" />, active: filters.isFresherOnly },
    { key: 'engineering' as const, label: 'Engineering', icon: <Code2 className="w-3.5 h-3.5" />, active: filters.isEngineeringOnly },
    { key: 'internship' as const, label: 'Internships', icon: <Sparkles className="w-3.5 h-3.5" />, active: filters.isInternshipOnly },
    { key: 'startups' as const, label: 'Startups', icon: <Rocket className="w-3.5 h-3.5" />, active: filters.selectedCompanyTypes.includes('STARTUP') },
    { key: 'product_companies' as const, label: 'Product Companies', icon: <Building2 className="w-3.5 h-3.5" />, active: filters.selectedCompanyTypes.includes('PRODUCT COMPANY') },
  ];

  return (
    <div className="space-y-3 bg-white p-4 rounded-2xl border border-slate-200/90 shadow-xs">
      {/* Primary Search Bar with Action Buttons */}
      <div className="flex flex-col sm:flex-row items-stretch sm:items-center gap-2">
        <div className="relative flex-1">
          <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400 pointer-events-none" />
          <input
            type="text"
            placeholder="Search companies (Zoho, Kissflow...), roles (.NET, Java, React), or hubs (OMR, Guindy)..."
            value={filters.searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
            className="w-full bg-slate-50 hover:bg-slate-100/70 focus:bg-white text-sm text-slate-900 placeholder:text-slate-400 rounded-xl pl-10 pr-9 py-2.5 border border-slate-200 focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20 transition-all"
          />
          {filters.searchQuery && (
            <button
              onClick={() => setSearchQuery('')}
              className="absolute right-3 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600 p-0.5"
            >
              <X className="w-4 h-4" />
            </button>
          )}
        </div>

        <div className="flex items-center gap-2 shrink-0">
          <Button
            variant={activeFilterCount > 0 ? 'primary' : 'outline'}
            size="md"
            onClick={() => setAdvancedFilterOpen(true)}
            leftIcon={<SlidersHorizontal className="w-4 h-4" />}
            className="relative"
          >
            <span>Filters</span>
            {activeFilterCount > 0 && (
              <span className="ml-1.5 px-1.5 py-0.2 rounded-full text-[11px] font-bold bg-white text-brand-700">
                {activeFilterCount}
              </span>
            )}
          </Button>

          {activeFilterCount > 0 && (
            <Button
              variant="ghost"
              size="md"
              onClick={resetFilters}
              className="text-rose-600 hover:bg-rose-50 hover:text-rose-700"
            >
              Reset
            </Button>
          )}
        </div>
      </div>

      {/* Quick Filter Buttons & Corridors Row */}
      <div className="flex flex-wrap items-center gap-1.5 pt-1">
        <span className="text-[11px] font-semibold text-slate-400 uppercase tracking-wider mr-1">
          Quick:
        </span>
        {quickPills.map((pill) => (
          <button
            key={pill.key}
            onClick={() => setQuickFilter(pill.key)}
            className={`inline-flex items-center gap-1.5 px-3 py-1 rounded-lg text-xs font-medium border transition-all ${
              pill.active
                ? 'bg-brand-600 border-brand-600 text-white shadow-xs font-semibold'
                : 'bg-slate-50 border-slate-200 text-slate-700 hover:bg-white hover:border-slate-300'
            }`}
          >
            {pill.icon}
            <span>{pill.label}</span>
          </button>
        ))}

        <div className="h-4 w-px bg-slate-200 mx-1 hidden sm:block"></div>

        {/* Tech Corridor Dropdown / Quick Buttons */}
        <div className="flex flex-wrap items-center gap-1.5">
          {CHENNAI_TECH_HUBS.slice(0, 4).map((hub) => {
            const isSelected = filters.selectedHubs.includes(hub.name);
            return (
              <button
                key={hub.name}
                onClick={() => toggleHub(hub.name)}
                className={`inline-flex items-center gap-1 px-2.5 py-1 rounded-lg text-xs font-medium border transition-all ${
                  isSelected
                    ? 'bg-teal-700 border-teal-700 text-white shadow-xs'
                    : 'bg-slate-50 border-slate-200 text-slate-600 hover:bg-white hover:border-slate-300'
                }`}
              >
                <MapPin className="w-3 h-3 text-teal-600 shrink-0" />
                <span>{hub.name.split(' ')[0]}</span>
              </button>
            );
          })}
        </div>
      </div>
    </div>
  );
};
