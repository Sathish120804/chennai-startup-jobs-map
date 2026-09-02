import { fetchJson } from './apiClient';
import { Company, FilterState } from '../../types';
import { db } from '../db';

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  total: number;
  totalPages: number;
}

export const companyApi = {
  async getCompanies(filters: FilterState): Promise<PagedResult<Company>> {
    try {
      const params = new URLSearchParams();
      if (filters.searchQuery) params.set('q', filters.searchQuery);
      if (filters.selectedHubs.length) params.set('hubs', filters.selectedHubs.join(','));
      if (filters.selectedCategories.length) params.set('categories', filters.selectedCategories.join(','));
      if (filters.selectedCompanyTypes.length) params.set('types', filters.selectedCompanyTypes.join(','));
      if (filters.isHiringOnly) params.set('hiring', 'true');
      if (filters.isFresherOnly) params.set('fresher', 'true');
      if (filters.selectedTechnologies.length) params.set('tech', filters.selectedTechnologies.join(','));
      params.set('sortBy', filters.sortBy);

      return await fetchJson<PagedResult<Company>>(`/companies?${params.toString()}`);
    } catch {
      // Development Fallback to local IndexedDB/mock database engine
      const items = db.getFilteredCompanies(filters);
      return {
        items,
        page: 1,
        pageSize: items.length,
        total: items.length,
        totalPages: 1,
      };
    }
  },

  async getCompanyBySlug(slug: string): Promise<Company | undefined> {
    try {
      return await fetchJson<Company>(`/companies/slug/${slug}`);
    } catch {
      return db.getCompanyBySlug(slug);
    }
  }
};
