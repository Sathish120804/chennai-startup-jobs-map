import React, { useMemo } from 'react';
import { useAppStore } from '../../store/useAppStore';
import { db } from '../../services/db';
import { CompanyCard } from './CompanyCard';
import { Building2, RotateCcw } from 'lucide-react';
import { Button } from '../ui/Button';

export const CompanyGrid: React.FC = () => {
  const { filters, setSortBy, resetFilters, setAdvancedFilterOpen } = useAppStore();

  const filteredCompanies = useMemo(() => {
    return db.getFilteredCompanies(filters);
  }, [filters]);

  return (
    <div className="space-y-4">
      <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-3 bg-white px-5 py-3.5 rounded-2xl border border-slate-200 shadow-xs">
        <div className="flex items-center gap-2 text-sm text-slate-700">
          <Building2 className="w-4 h-4 text-brand-600" />
          <span>
            Showing <strong className="text-slate-900 font-bold">{filteredCompanies.length}</strong> Chennai Startups & Companies
          </span>
        </div>

        <div className="flex items-center gap-3">
          <label className="text-xs text-slate-500 font-medium">Sort by:</label>
          <select
            value={filters.sortBy}
            onChange={(e) => setSortBy(e.target.value as any)}
            className="text-xs font-semibold bg-slate-50 border border-slate-200 rounded-lg px-2.5 py-1.5 text-slate-800 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
          >
            <option value="featured">Featured & Active Hiring</option>
            <option value="jobsCount">Most Job Openings</option>
            <option value="name">Company Name (A-Z)</option>
            <option value="foundedYear">Recently Founded</option>
          </select>
        </div>
      </div>

      {filteredCompanies.length === 0 ? (
        <div className="p-12 text-center bg-white rounded-3xl border border-slate-200 shadow-xs space-y-4">
          <div className="w-12 h-12 rounded-2xl bg-brand-50 flex items-center justify-center text-brand-600 mx-auto">
            <Building2 className="w-6 h-6" />
          </div>
          <div className="space-y-1 max-w-md mx-auto">
            <h3 className="text-base font-bold text-slate-900">No matching Chennai startups found</h3>
            <p className="text-xs text-slate-500">
              Try adjusting your search terms, removing filter pills, or broadening the Chennai tech corridor selection.
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
        <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          {filteredCompanies.map((company) => (
            <CompanyCard key={company.id} company={company} />
          ))}
        </div>
      )}
    </div>
  );
};
