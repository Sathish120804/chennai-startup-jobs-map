import { create } from 'zustand';
import { 
  ActiveTab, 
  FilterState, 
  TechHub, 
  CompanyCategory, 
  CompanyType,
  FundingStage, 
  ExperienceLevel, 
  WorkplaceType,
  EngineeringSubcategory,
  ChennaiRelevance,
  FreshnessStatus
} from '../types';

interface AppStoreState {
  activeTab: ActiveTab;
  setActiveTab: (tab: ActiveTab) => void;
  
  // Selection & inspection
  selectedCompanyId: string | null;
  setSelectedCompanyId: (id: string | null) => void;

  selectedJobId: string | null;
  setSelectedJobId: (id: string | null) => void;
  
  hoveredCompanyId: string | null;
  setHoveredCompanyId: (id: string | null) => void;

  // Modals
  isSubmitCompanyOpen: boolean;
  setSubmitCompanyOpen: (open: boolean) => void;

  isSubmitJobOpen: boolean;
  setSubmitJobOpen: (open: boolean) => void;

  isAdvancedFilterOpen: boolean;
  setAdvancedFilterOpen: (open: boolean) => void;

  isMobileDrawerOpen: boolean;
  setMobileDrawerOpen: (open: boolean) => void;

  // Filters
  filters: FilterState;
  setSearchQuery: (query: string) => void;
  toggleHub: (hub: TechHub) => void;
  toggleCategory: (category: CompanyCategory) => void;
  toggleCompanyType: (type: CompanyType) => void;
  toggleFundingStage: (stage: FundingStage) => void;
  toggleExperienceLevel: (level: ExperienceLevel) => void;
  toggleWorkplaceType: (type: WorkplaceType) => void;
  toggleEngineeringSubcategory: (subcat: EngineeringSubcategory) => void;
  toggleTechnology: (tech: string) => void;
  toggleRelevance: (relevance: ChennaiRelevance) => void;
  toggleFreshness: (freshness: FreshnessStatus) => void;
  toggleHiringOnly: () => void;
  toggleFresherOnly: () => void;
  toggleEngineeringOnly: () => void;
  toggleInternshipOnly: () => void;
  toggleFeaturedOnly: () => void;
  setSortBy: (sort: FilterState['sortBy']) => void;
  resetFilters: () => void;
  setQuickFilter: (filterKey: 'hiring' | 'fresher' | 'engineering' | 'internship' | 'startups' | 'product_companies') => void;
}

import { parseUrlState, syncStateToUrl } from '../services/urlStateSync';

const initialUrlState = parseUrlState();

const initialFilters: FilterState = {
  searchQuery: initialUrlState.searchQuery || '',
  selectedHubs: (initialUrlState.hubs as TechHub[]) || [],
  selectedCategories: (initialUrlState.categories as CompanyCategory[]) || [],
  selectedCompanyTypes: (initialUrlState.types as CompanyType[]) || [],
  selectedFundingStages: [],
  selectedExperienceLevels: [],
  selectedWorkplaceTypes: [],
  selectedEngineeringSubcategories: [],
  selectedTechnologies: initialUrlState.tech || [],
  isHiringOnly: initialUrlState.hiring || false,
  isFresherOnly: initialUrlState.fresher || false,
  isEngineeringOnly: false,
  isInternshipOnly: initialUrlState.internship || false,
  isFeaturedOnly: false,
  selectedRelevance: [],
  selectedFreshness: [],
  sortBy: 'featured',
};

export const useAppStore = create<AppStoreState>((set, get) => ({
  activeTab: initialUrlState.tab || 'map',
  setActiveTab: (tab) => {
    set({ activeTab: tab });
    syncStateToUrl(tab, get().filters);
  },

  selectedCompanyId: null,
  setSelectedCompanyId: (id) => set({ selectedCompanyId: id }),

  selectedJobId: null,
  setSelectedJobId: (id) => set({ selectedJobId: id }),

  hoveredCompanyId: null,
  setHoveredCompanyId: (id) => set({ hoveredCompanyId: id }),

  isSubmitCompanyOpen: false,
  setSubmitCompanyOpen: (open) => set({ isSubmitCompanyOpen: open }),

  isSubmitJobOpen: false,
  setSubmitJobOpen: (open) => set({ isSubmitJobOpen: open }),

  isAdvancedFilterOpen: false,
  setAdvancedFilterOpen: (open) => set({ isAdvancedFilterOpen: open }),

  isMobileDrawerOpen: false,
  setMobileDrawerOpen: (open) => set({ isMobileDrawerOpen: open }),

  filters: initialFilters,

  setSearchQuery: (query) => 
    set((state) => {
      const nextFilters = { ...state.filters, searchQuery: query };
      syncStateToUrl(state.activeTab, nextFilters);
      return { filters: nextFilters };
    }),

  toggleHub: (hub) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedHubs: state.filters.selectedHubs.includes(hub)
          ? state.filters.selectedHubs.filter((h) => h !== hub)
          : [...state.filters.selectedHubs, hub],
      },
    })),

  toggleCategory: (category) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedCategories: state.filters.selectedCategories.includes(category)
          ? state.filters.selectedCategories.filter((c) => c !== category)
          : [...state.filters.selectedCategories, category],
      },
    })),

  toggleCompanyType: (type) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedCompanyTypes: state.filters.selectedCompanyTypes.includes(type)
          ? state.filters.selectedCompanyTypes.filter((t) => t !== type)
          : [...state.filters.selectedCompanyTypes, type],
      },
    })),

  toggleFundingStage: (stage) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedFundingStages: state.filters.selectedFundingStages.includes(stage)
          ? state.filters.selectedFundingStages.filter((s) => s !== stage)
          : [...state.filters.selectedFundingStages, stage],
      },
    })),

  toggleExperienceLevel: (level) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedExperienceLevels: state.filters.selectedExperienceLevels.includes(level)
          ? state.filters.selectedExperienceLevels.filter((l) => l !== level)
          : [...state.filters.selectedExperienceLevels, level],
      },
    })),

  toggleWorkplaceType: (type) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedWorkplaceTypes: state.filters.selectedWorkplaceTypes.includes(type)
          ? state.filters.selectedWorkplaceTypes.filter((t) => t !== type)
          : [...state.filters.selectedWorkplaceTypes, type],
      },
    })),

  toggleEngineeringSubcategory: (subcat) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedEngineeringSubcategories: state.filters.selectedEngineeringSubcategories.includes(subcat)
          ? state.filters.selectedEngineeringSubcategories.filter((s) => s !== subcat)
          : [...state.filters.selectedEngineeringSubcategories, subcat],
      },
    })),

  toggleTechnology: (tech) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedTechnologies: state.filters.selectedTechnologies.includes(tech)
          ? state.filters.selectedTechnologies.filter((t) => t !== tech)
          : [...state.filters.selectedTechnologies, tech],
      },
    })),

  toggleRelevance: (relevance) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedRelevance: state.filters.selectedRelevance.includes(relevance)
          ? state.filters.selectedRelevance.filter((r) => r !== relevance)
          : [...state.filters.selectedRelevance, relevance],
      },
    })),

  toggleFreshness: (freshness) =>
    set((state) => ({
      filters: {
        ...state.filters,
        selectedFreshness: state.filters.selectedFreshness.includes(freshness)
          ? state.filters.selectedFreshness.filter((f) => f !== freshness)
          : [...state.filters.selectedFreshness, freshness],
      },
    })),

  toggleHiringOnly: () =>
    set((state) => ({
      filters: { ...state.filters, isHiringOnly: !state.filters.isHiringOnly },
    })),

  toggleFresherOnly: () =>
    set((state) => ({
      filters: { ...state.filters, isFresherOnly: !state.filters.isFresherOnly },
    })),

  toggleEngineeringOnly: () =>
    set((state) => ({
      filters: { ...state.filters, isEngineeringOnly: !state.filters.isEngineeringOnly },
    })),

  toggleInternshipOnly: () =>
    set((state) => ({
      filters: { ...state.filters, isInternshipOnly: !state.filters.isInternshipOnly },
    })),

  toggleFeaturedOnly: () =>
    set((state) => ({
      filters: { ...state.filters, isFeaturedOnly: !state.filters.isFeaturedOnly },
    })),

  setSortBy: (sort) =>
    set((state) => ({
      filters: { ...state.filters, sortBy: sort },
    })),

  resetFilters: () => set({ filters: initialFilters }),

  setQuickFilter: (filterKey) =>
    set((state) => {
      const reset = { ...initialFilters, searchQuery: state.filters.searchQuery };
      switch (filterKey) {
        case 'hiring':
          return { filters: { ...reset, isHiringOnly: true } };
        case 'fresher':
          return { filters: { ...reset, isFresherOnly: true } };
        case 'engineering':
          return { filters: { ...reset, isEngineeringOnly: true } };
        case 'internship':
          return { filters: { ...reset, isInternshipOnly: true } };
        case 'startups':
          return { filters: { ...reset, selectedCompanyTypes: ['STARTUP'] } };
        case 'product_companies':
          return { filters: { ...reset, selectedCompanyTypes: ['PRODUCT COMPANY'] } };
        default:
          return { filters: reset };
      }
    }),
}));
