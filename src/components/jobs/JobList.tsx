import React, { useMemo } from 'react';
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
