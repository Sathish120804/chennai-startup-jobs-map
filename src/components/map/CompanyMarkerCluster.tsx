import React, { useEffect, useRef } from 'react';
import { useMap } from 'react-leaflet';
import L from 'leaflet';
import 'leaflet.markercluster';
import { Company } from '../../types';
import { db } from '../../services/db';

interface CompanyMarkerClusterProps {
  companies: Company[];
  selectedCompanyId: string | null;
  hoveredCompanyId: string | null;
  onSelectCompany: (id: string) => void;
  onHoverCompany: (id: string | null) => void;
}

export const CompanyMarkerCluster: React.FC<CompanyMarkerClusterProps> = ({
  companies,
  selectedCompanyId,
  hoveredCompanyId,
  onSelectCompany,
  onHoverCompany,
}) => {
  const map = useMap();
  const clusterGroupRef = useRef<L.MarkerClusterGroup | null>(null);

  useEffect(() => {
    // 1. Initialize MarkerClusterGroup with performance tuning for 700+ companies
    const clusterGroup = L.markerClusterGroup({
      chunkedLoading: true,
      chunkInterval: 100,
      chunkDelay: 25,
      maxClusterRadius: 50,
      spiderfyOnMaxZoom: true,
      showCoverageOnHover: false,
      zoomToBoundsOnClick: true,
      removeOutsideVisibleBounds: true,
      iconCreateFunction: (cluster) => {
        const count = cluster.getChildCount();
        const diameter = count < 15 ? 36 : count < 60 ? 44 : 54;
        const bg =
          count < 15
            ? 'linear-gradient(135deg, #0284c7, #0369a1)'
            : count < 60
            ? 'linear-gradient(135deg, #0f766e, #0e7490)'
            : 'linear-gradient(135deg, #1e293b, #0f172a)';

        return L.divIcon({
          html: `<div style="background: ${bg}; width: ${diameter}px; height: ${diameter}px;" class="rounded-full flex flex-col items-center justify-center text-white font-extrabold text-xs shadow-xl border-2 border-white ring-2 ring-sky-300/40 transition-transform duration-200 hover:scale-110">
            <span>${count}</span>
            <span style="font-size: 8px; font-weight: 500; opacity: 0.85;">units</span>
          </div>`,
          className: 'custom-cluster-badge',
          iconSize: [diameter, diameter],
          iconAnchor: [diameter / 2, diameter / 2],
        });
      },
    });

    clusterGroupRef.current = clusterGroup;

    // 2. Add individual company markers
    companies.forEach((company) => {
      const lat = company.coordinates?.lat;
      const lng = company.coordinates?.lng;
      if (!lat || !lng || isNaN(lat) || isNaN(lng)) return;

      const stats = db.getCompanyStats(company.id);
      const hasJobs = stats.activeJobsCount > 0;
      const isSelected = selectedCompanyId === company.id;
      const isHovered = hoveredCompanyId === company.id;

      const bgColor = hasJobs ? '#0284c7' : '#475569';
      const ringColor = isSelected ? '#f59e0b' : isHovered ? '#38bdf8' : hasJobs ? '#10b981' : '#cbd5e1';
      const pulseHtml = hasJobs
        ? '<span class="absolute -top-1 -right-1 flex h-3 w-3"><span class="animate-ping absolute inline-flex h-full w-full rounded-full bg-emerald-400 opacity-75"></span><span class="relative inline-flex rounded-full h-3 w-3 bg-emerald-500"></span></span>'
        : '';

      const icon = L.divIcon({
        className: 'custom-company-pin',
        html: `
          <div class="relative group cursor-pointer transition-transform duration-200 ${isSelected || isHovered ? 'scale-125 z-50' : 'hover:scale-110'}">
            <div style="background-color: ${bgColor}; border: 2.5px solid ${ringColor};" class="w-8 h-8 rounded-full shadow-lg flex items-center justify-center text-white font-bold text-xs">
              ${stats.activeJobsCount > 0 ? stats.activeJobsCount : '🏢'}
            </div>
            ${pulseHtml}
          </div>
        `,
        iconSize: [32, 32],
        iconAnchor: [16, 16],
        popupAnchor: [0, -18],
      });

      const marker = L.marker([lat, lng], { icon });

      // Build popup content
      const verifiedBadge =
        company.verificationStatus === 'VERIFIED'
          ? '<span class="inline-flex items-center text-[10px] font-semibold text-sky-600 bg-sky-50 px-1.5 py-0.5 rounded ml-1">✓ Verified</span>'
          : '';

      const jobsBadge =
        stats.activeJobsCount > 0
          ? `<span class="inline-block bg-emerald-50 text-emerald-700 font-semibold text-[11px] px-2 py-0.5 rounded border border-emerald-200">${stats.activeJobsCount} Active Vacancies</span>`
          : '<span class="inline-block bg-slate-100 text-slate-600 text-[11px] px-2 py-0.5 rounded">Not Actively Hiring</span>';

      const popupHtml = `
        <div class="p-2 max-w-[260px] space-y-2 font-sans">
          <div class="flex items-start gap-2">
            <img src="${company.logo || 'https://ui-avatars.com/api/?name=' + encodeURIComponent(company.name) + '&background=0284c7&color=fff'}" 
                 alt="${company.name}" 
                 class="w-8 h-8 rounded-lg object-cover border border-slate-200 shrink-0" 
                 onerror="this.src='https://ui-avatars.com/api/?name=TN&background=0284c7&color=fff'" />
            <div>
              <div class="font-bold text-slate-900 text-xs leading-tight">${company.name} ${verifiedBadge}</div>
              <div class="text-[10px] text-slate-500">${company.hub || 'Chennai'}</div>
            </div>
          </div>
          <p class="text-[11px] text-slate-600 line-clamp-2 leading-relaxed">${company.tagline || company.description || ''}</p>
          <div class="pt-1">${jobsBadge}</div>
          <button id="inspect-btn-${company.id}" class="w-full mt-2 bg-sky-600 hover:bg-sky-700 text-white font-medium text-xs py-1.5 px-3 rounded-lg shadow-sm transition-all text-center block">
            Inspect Company & Vacancies
          </button>
        </div>
      `;

      marker.bindPopup(popupHtml, { closeButton: false });

      marker.on('popupopen', () => {
        const btn = document.getElementById(`inspect-btn-${company.id}`);
        if (btn) {
          btn.onclick = () => onSelectCompany(company.id);
        }
      });

      marker.on('mouseover', () => onHoverCompany(company.id));
      marker.on('mouseout', () => onHoverCompany(null));

      clusterGroup.addLayer(marker);
    });

    map.addLayer(clusterGroup);

    return () => {
      map.removeLayer(clusterGroup);
    };
  }, [map, companies, selectedCompanyId, hoveredCompanyId, onSelectCompany, onHoverCompany]);

  return null;
};
