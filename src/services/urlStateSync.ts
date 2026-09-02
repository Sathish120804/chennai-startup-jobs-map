import { FilterState, ActiveTab } from '../types';

export function syncStateToUrl(
  activeTab: ActiveTab,
  filters: FilterState,
  selectedCompanySlug?: string | null,
  selectedJobSlug?: string | null
) {
  if (typeof window === 'undefined') return;

  const params = new URLSearchParams();

  if (activeTab !== 'map') {
    params.set('tab', activeTab);
  }

  if (filters.searchQuery.trim()) {
    params.set('q', filters.searchQuery.trim());
  }

  if (filters.selectedHubs.length > 0) {
    params.set('hubs', filters.selectedHubs.join(','));
  }

  if (filters.selectedCategories.length > 0) {
    params.set('categories', filters.selectedCategories.join(','));
  }

  if (filters.selectedCompanyTypes.length > 0) {
    params.set('types', filters.selectedCompanyTypes.join(','));
  }

  if (filters.isFresherOnly) {
    params.set('fresher', 'true');
  }

  if (filters.isInternshipOnly) {
    params.set('internship', 'true');
  }

  if (filters.isHiringOnly) {
    params.set('hiring', 'true');
  }

  if (filters.selectedTechnologies.length > 0) {
    params.set('tech', filters.selectedTechnologies.join(','));
  }

  if (selectedCompanySlug) {
    params.set('company', selectedCompanySlug);
  }

  if (selectedJobSlug) {
    params.set('job', selectedJobSlug);
  }

  const queryString = params.toString();
  const newUrl = queryString ? `${window.location.pathname}?${queryString}` : window.location.pathname;

  if (window.location.search !== `?${queryString}`) {
    window.history.replaceState(null, '', newUrl);
  }
}

export function parseUrlState(): {
  tab?: ActiveTab;
  searchQuery?: string;
  hubs?: string[];
  categories?: string[];
  types?: string[];
  fresher?: boolean;
  internship?: boolean;
  hiring?: boolean;
  tech?: string[];
  companySlug?: string;
  jobSlug?: string;
} {
  if (typeof window === 'undefined') return {};

  const params = new URLSearchParams(window.location.search);

  const tab = (params.get('tab') as ActiveTab) || undefined;
  const searchQuery = params.get('q') || undefined;
  const hubs = params.get('hubs')?.split(',') || undefined;
  const categories = params.get('categories')?.split(',') || undefined;
  const types = params.get('types')?.split(',') || undefined;
  const fresher = params.get('fresher') === 'true' ? true : undefined;
  const internship = params.get('internship') === 'true' ? true : undefined;
  const hiring = params.get('hiring') === 'true' ? true : undefined;
  const tech = params.get('tech')?.split(',') || undefined;
  const companySlug = params.get('company') || undefined;
  const jobSlug = params.get('job') || undefined;

  return {
    tab,
    searchQuery,
    hubs,
    categories,
    types,
    fresher,
    internship,
    hiring,
    tech,
    companySlug,
    jobSlug,
  };
}
