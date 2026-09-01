import React from 'react';
import { X, Check, RotateCcw, MapPin, Sparkles, Code2, Layers, Building, Cpu, Briefcase } from 'lucide-react';
import { useAppStore } from '../../store/useAppStore';
import { 
  CHENNAI_TECH_HUBS, 
  COMPANY_CATEGORIES, 
  COMPANY_TYPES, 
  ENGINEERING_SUBCATEGORIES, 
  POPULAR_TECHNOLOGIES, 
  EXPERIENCE_LEVELS, 
  WORKPLACE_TYPES, 
  FUNDING_STAGES 
} from '../../config/constants';
import { Button } from '../ui/Button';

export const AdvancedFilterModal: React.FC = () => {
  const { 
    isAdvancedFilterOpen, 
    setAdvancedFilterOpen, 
    filters, 
    toggleHub, 
    toggleCategory, 
    toggleCompanyType, 
    toggleFundingStage, 
    toggleExperienceLevel, 
    toggleWorkplaceType, 
    toggleEngineeringSubcategory, 
    toggleTechnology, 
    toggleFresherOnly, 
    toggleEngineeringOnly, 
    toggleHiringOnly, 
    toggleInternshipOnly, 
    resetFilters 
  } = useAppStore();

  if (!isAdvancedFilterOpen) return null;

  return (
    <div className="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-3 sm:p-6 animate-fade-in">
      <div className="bg-white w-full max-w-4xl rounded-2xl shadow-2xl border border-slate-200 flex flex-col max-h-[90vh] overflow-hidden">
        {/* Header */}
        <div className="flex items-center justify-between px-6 py-4 border-b border-slate-200 bg-slate-50/80">
          <div>
            <h3 className="text-lg font-bold text-slate-900 flex items-center gap-2">
              <Layers className="w-5 h-5 text-brand-600" />
              <span>Advanced Discovery Filters</span>
            </h3>
            <p className="text-xs text-slate-500">
              Narrow down by Chennai corridors, technologies (.NET, Java, React), fresher roles, or company stages.
            </p>
          </div>
          <button
            onClick={() => setAdvancedFilterOpen(false)}
            className="p-1.5 rounded-lg text-slate-400 hover:text-slate-700 hover:bg-slate-200/60 transition-colors"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Scrollable Filter Body */}
        <div className="flex-1 overflow-y-auto p-6 space-y-6">
          {/* Section 1: Core Toggles */}
          <div>
            <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5 flex items-center gap-1.5">
              <Sparkles className="w-4 h-4 text-brand-600" />
              <span>Priority Fast Toggles</span>
            </h4>
            <div className="grid grid-cols-2 sm:grid-cols-4 gap-2.5">
              <button
                onClick={toggleHiringOnly}
                className={`p-3 rounded-xl border text-left flex items-center justify-between transition-all ${
                  filters.isHiringOnly
                    ? 'border-brand-500 bg-brand-50/70 text-brand-900 font-semibold'
                    : 'border-slate-200 bg-white hover:bg-slate-50 text-slate-700'
                }`}
              >
                <div className="text-xs">Hiring Startups Only</div>
                {filters.isHiringOnly && <Check className="w-4 h-4 text-brand-600" />}
              </button>

              <button
                onClick={toggleFresherOnly}
                className={`p-3 rounded-xl border text-left flex items-center justify-between transition-all ${
                  filters.isFresherOnly
                    ? 'border-emerald-500 bg-emerald-50/70 text-emerald-900 font-semibold'
                    : 'border-slate-200 bg-white hover:bg-slate-50 text-slate-700'
                }`}
              >
                <div className="text-xs">Fresher Friendly (0-1 yrs)</div>
                {filters.isFresherOnly && <Check className="w-4 h-4 text-emerald-600" />}
              </button>

              <button
                onClick={toggleEngineeringOnly}
                className={`p-3 rounded-xl border text-left flex items-center justify-between transition-all ${
                  filters.isEngineeringOnly
                    ? 'border-indigo-500 bg-indigo-50/70 text-indigo-900 font-semibold'
                    : 'border-slate-200 bg-white hover:bg-slate-50 text-slate-700'
                }`}
              >
                <div className="text-xs">Engineering Roles Only</div>
                {filters.isEngineeringOnly && <Check className="w-4 h-4 text-indigo-600" />}
              </button>

              <button
                onClick={toggleInternshipOnly}
                className={`p-3 rounded-xl border text-left flex items-center justify-between transition-all ${
                  filters.isInternshipOnly
                    ? 'border-amber-500 bg-amber-50/70 text-amber-900 font-semibold'
                    : 'border-slate-200 bg-white hover:bg-slate-50 text-slate-700'
                }`}
              >
                <div className="text-xs">Internship Opportunities</div>
                {filters.isInternshipOnly && <Check className="w-4 h-4 text-amber-600" />}
              </button>
            </div>
          </div>

          {/* Section 2: Chennai Tech Corridors */}
          <div>
            <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5 flex items-center gap-1.5">
              <MapPin className="w-4 h-4 text-teal-600" />
              <span>Chennai Tech Corridors & Zones</span>
            </h4>
            <div className="flex flex-wrap gap-2">
              {CHENNAI_TECH_HUBS.map((hub) => {
                const isSelected = filters.selectedHubs.includes(hub.name);
                return (
                  <button
                    key={hub.name}
                    onClick={() => toggleHub(hub.name)}
                    className={`px-3 py-1.5 rounded-lg text-xs font-medium border transition-all ${
                      isSelected
                        ? 'bg-teal-700 border-teal-700 text-white shadow-xs font-semibold'
                        : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50 hover:border-slate-300'
                    }`}
                  >
                    📍 {hub.name}
                  </button>
                );
              })}
            </div>
          </div>

          {/* Section 3: Engineering Subcategories */}
          <div>
            <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5 flex items-center gap-1.5">
              <Code2 className="w-4 h-4 text-indigo-600" />
              <span>Engineering Subcategories</span>
            </h4>
            <div className="flex flex-wrap gap-2">
              {ENGINEERING_SUBCATEGORIES.map((subcat) => {
                const isSelected = filters.selectedEngineeringSubcategories.includes(subcat);
                return (
                  <button
                    key={subcat}
                    onClick={() => toggleEngineeringSubcategory(subcat)}
                    className={`px-3 py-1.5 rounded-lg text-xs font-medium border transition-all ${
                      isSelected
                        ? 'bg-indigo-600 border-indigo-600 text-white shadow-xs font-semibold'
                        : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50 hover:border-slate-300'
                    }`}
                  >
                    {subcat}
                  </button>
                );
              })}
            </div>
          </div>

          {/* Section 4: Technologies & Skills */}
          <div>
            <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5 flex items-center gap-1.5">
              <Cpu className="w-4 h-4 text-purple-600" />
              <span>Technologies & Tech Stack</span>
            </h4>
            <div className="flex flex-wrap gap-1.5">
              {POPULAR_TECHNOLOGIES.map((tech) => {
                const isSelected = filters.selectedTechnologies.includes(tech);
                return (
                  <button
                    key={tech}
                    onClick={() => toggleTechnology(tech)}
                    className={`px-2.5 py-1 rounded-md text-xs font-medium border transition-all ${
                      isSelected
                        ? 'bg-purple-700 border-purple-700 text-white shadow-xs font-semibold'
                        : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50 hover:border-slate-300'
                    }`}
                  >
                    {tech}
                  </button>
                );
              })}
            </div>
          </div>

          {/* Section 5: Company Types & Categories */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5 flex items-center gap-1.5">
                <Building className="w-4 h-4 text-amber-600" />
                <span>Company Classification</span>
              </h4>
              <div className="flex flex-wrap gap-2">
                {COMPANY_TYPES.map((type) => {
                  const isSelected = filters.selectedCompanyTypes.includes(type);
                  return (
                    <button
                      key={type}
                      onClick={() => toggleCompanyType(type)}
                      className={`px-3 py-1.5 rounded-lg text-xs font-medium border transition-all ${
                        isSelected
                          ? 'bg-amber-600 border-amber-600 text-white shadow-xs font-semibold'
                          : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50 hover:border-slate-300'
                      }`}
                    >
                      {type}
                    </button>
                  );
                })}
              </div>
            </div>

            <div>
              <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5 flex items-center gap-1.5">
                <Briefcase className="w-4 h-4 text-blue-600" />
                <span>Industry Domains</span>
              </h4>
              <div className="flex flex-wrap gap-2">
                {COMPANY_CATEGORIES.map((cat) => {
                  const isSelected = filters.selectedCategories.includes(cat);
                  return (
                    <button
                      key={cat}
                      onClick={() => toggleCategory(cat)}
                      className={`px-3 py-1.5 rounded-lg text-xs font-medium border transition-all ${
                        isSelected
                          ? 'bg-brand-600 border-brand-600 text-white shadow-xs font-semibold'
                          : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50 hover:border-slate-300'
                      }`}
                    >
                      {cat}
                    </button>
                  );
                })}
              </div>
            </div>
          </div>

          {/* Section 6: Workplace & Experience */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div>
              <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5">
                Workplace Mode
              </h4>
              <div className="flex flex-wrap gap-2">
                {WORKPLACE_TYPES.map((wpt) => {
                  const isSelected = filters.selectedWorkplaceTypes.includes(wpt);
                  return (
                    <button
                      key={wpt}
                      onClick={() => toggleWorkplaceType(wpt)}
                      className={`px-3 py-1.5 rounded-lg text-xs font-medium border transition-all ${
                        isSelected
                          ? 'bg-slate-800 border-slate-800 text-white font-semibold'
                          : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50'
                      }`}
                    >
                      {wpt}
                    </button>
                  );
                })}
              </div>
            </div>

            <div>
              <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5">
                Experience Level
              </h4>
              <div className="flex flex-wrap gap-2">
                {EXPERIENCE_LEVELS.map((exp) => {
                  const isSelected = filters.selectedExperienceLevels.includes(exp);
                  return (
                    <button
                      key={exp}
                      onClick={() => toggleExperienceLevel(exp)}
                      className={`px-3 py-1.5 rounded-lg text-xs font-medium border transition-all ${
                        isSelected
                          ? 'bg-slate-800 border-slate-800 text-white font-semibold'
                          : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50'
                      }`}
                    >
                      {exp}
                    </button>
                  );
                })}
              </div>
            </div>
          </div>

          {/* Section 7: Funding Stage */}
          <div>
            <h4 className="text-xs font-bold text-slate-900 uppercase tracking-wider mb-2.5">
              Funding Stage
            </h4>
            <div className="flex flex-wrap gap-2">
              {FUNDING_STAGES.map((stage) => {
                const isSelected = filters.selectedFundingStages.includes(stage);
                return (
                  <button
                    key={stage}
                    onClick={() => toggleFundingStage(stage)}
                    className={`px-3 py-1.5 rounded-lg text-xs font-medium border transition-all ${
                      isSelected
                        ? 'bg-slate-800 border-slate-800 text-white font-semibold'
                        : 'bg-white border-slate-200 text-slate-700 hover:bg-slate-50'
                    }`}
                  >
                    {stage}
                  </button>
                );
              })}
            </div>
          </div>
        </div>

        {/* Footer */}
        <div className="flex items-center justify-between px-6 py-4 border-t border-slate-200 bg-slate-50">
          <Button
            variant="ghost"
            size="sm"
            onClick={resetFilters}
            leftIcon={<RotateCcw className="w-4 h-4 text-slate-500" />}
          >
            Reset All
          </Button>

          <Button
            variant="primary"
            size="md"
            onClick={() => setAdvancedFilterOpen(false)}
          >
            Apply Filters
          </Button>
        </div>
      </div>
    </div>
  );
};
