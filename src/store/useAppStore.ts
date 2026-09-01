import { create } from 'zustand';
import { ActiveTab, FilterState, TechHub, CompanyCategory, FundingStage, ExperienceLevel, WorkplaceType } from '../types';

interface AppStoreState {
  activeTab: ActiveTab;
  setActiveTab: (tab: ActiveTab) => void;
  
  selectedCompanyId: string | null;
  setSelectedCompanyId: (id: string | null) => void;
  
  hoveredCompanyId: string | null;
  setHoveredCompanyId: (id: string | null) => void;

  filters: FilterState;
  setSearchQuery: (query: string) => void;
  toggleHub: (hub: TechHub) => void;
  toggleCategory: (category: CompanyCategory) => void;
  toggleFundingStage: (stage: FundingStage) => void;
  toggleExperienceLevel: (level: ExperienceLevel) => void;
  toggleWorkplaceType: (type: WorkplaceType) => void;
  toggleHiringOnly: () => void;
  toggleFeaturedOnly: () => void;
  setSortBy: (sort: FilterState['sortBy']) => void;
  resetFilters: () => void;
  
  isMobileDrawerOpen: boolean;
  setMobileDrawerOpen: (open: boolean) => void;
}

const initialFilters: FilterState = {
  searchQuery: '',
  selectedHubs: [],
  selectedCategories: [],
  selectedFundingStages: [],
  selectedExperienceLevels: [],
  selectedWorkplaceTypes: [],
  isHiringOnly: false,
  isFeaturedOnly: false,
  sortBy: 'featured',
};

export const useAppStore = create<AppStoreState>((set) => ({
  activeTab: 'map',
  setActiveTab: (tab) => set({ activeTab: tab }),

  selectedCompanyId: null,
  setSelectedCompanyId: (id) => set({ selectedCompanyId: id }),

  hoveredCompanyId: null,
  setHoveredCompanyId: (id) => set({ hoveredCompanyId: id }),

  filters: initialFilters,

  setSearchQuery: (query) => 
    set((state) => ({ filters: { ...state.filters, searchQuery: query } })),

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

  toggleHiringOnly: () =>
    set((state) => ({
      filters: { ...state.filters, isHiringOnly: !state.filters.isHiringOnly },
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

  isMobileDrawerOpen: false,
  setMobileDrawerOpen: (open) => set({ isMobileDrawerOpen: open }),
}));
