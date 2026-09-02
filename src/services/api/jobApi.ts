import { fetchJson } from './apiClient';
import { Job, FilterState } from '../../types';
import { db } from '../db';
import { PagedResult } from './companyApi';

export const jobApi = {
  async getJobs(filters: FilterState): Promise<PagedResult<Job>> {
    try {
      const params = new URLSearchParams();
      if (filters.searchQuery) params.set('q', filters.searchQuery);
      if (filters.selectedHubs.length) params.set('hubs', filters.selectedHubs.join(','));
      if (filters.selectedCategories.length) params.set('categories', filters.selectedCategories.join(','));
      if (filters.isFresherOnly) params.set('fresher', 'true');
      if (filters.isInternshipOnly) params.set('internship', 'true');
      if (filters.isEngineeringOnly) params.set('engineering', 'true');
      if (filters.selectedTechnologies.length) params.set('tech', filters.selectedTechnologies.join(','));

      return await fetchJson<PagedResult<Job>>(`/jobs?${params.toString()}`);
    } catch {
      // Fallback mode
      const items = db.getFilteredJobs(filters);
      return {
        items,
        page: 1,
        pageSize: items.length,
        total: items.length,
        totalPages: 1,
      };
    }
  },

  async getJobById(id: string): Promise<Job | undefined> {
    try {
      return await fetchJson<Job>(`/jobs/${id}`);
    } catch {
      return db.getJobById(id);
    }
  }
};
