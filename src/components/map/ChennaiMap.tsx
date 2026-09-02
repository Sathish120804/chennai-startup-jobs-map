import React, { useEffect, useState, useMemo } from 'react';
import { MapContainer, TileLayer, Marker, Popup, useMap } from 'react-leaflet';
import L from 'leaflet';
import { useAppStore } from '../../store/useAppStore';
import { db } from '../../services/db';
import { APP_CONFIG, CHENNAI_TECH_HUBS } from '../../config/constants';
import { Badge } from '../ui/Badge';
import { Button } from '../ui/Button';
import { 
  Briefcase, 
  Navigation, 
  CheckCircle2, 
  GraduationCap
} from 'lucide-react';
import { Company } from '../../types';

// Component to handle dynamic fly-to and map center changes
const MapController: React.FC<{ targetCoords: [number, number] | null; zoom?: number }> = ({ targetCoords, zoom }) => {
  const map = useMap();
  useEffect(() => {
    if (targetCoords) {
      map.flyTo(targetCoords, zoom || 14, { duration: 1.2 });
    }
  }, [targetCoords, zoom, map]);
  return null;
};

export const ChennaiMap: React.FC = () => {
  const { 
    filters, 
    setSelectedCompanyId, 
    selectedCompanyId, 
    hoveredCompanyId, 
    setHoveredCompanyId 
  } = useAppStore();

  const [mounted, setMounted] = useState(false);
  const [dbVersion, setDbVersion] = useState(0);
  const [targetCoords, setTargetCoords] = useState<[number, number] | null>(null);
  const [targetZoom, setTargetZoom] = useState<number>(APP_CONFIG.defaultZoom);

  useEffect(() => {
    setMounted(true);
    const unsubscribe = db.subscribe(() => {
      setDbVersion((v) => v + 1);
    });
    return unsubscribe;
  }, []);

  const filteredCompanies = useMemo(() => {
    return db.getFilteredCompanies(filters);
  }, [filters, dbVersion]);

  // Create custom marker icons dynamically
  const createCompanyIcon = (company: Company, isSelected: boolean, isHovered: boolean) => {
    const stats = db.getCompanyStats(company.id);
    const hasJobs = stats.activeJobsCount > 0;

    let bgColor = hasJobs ? '#0284c7' : '#475569';
    let ringColor = isSelected ? '#f59e0b' : isHovered ? '#38bdf8' : hasJobs ? '#10b981' : '#cbd5e1';
    let pulseHtml = hasJobs ? '<span class="absolute -top-1 -right-1 flex h-3 w-3"><span class="animate-ping absolute inline-flex h-full w-full rounded-full bg-emerald-400 opacity-75"></span><span class="relative inline-flex rounded-full h-3 w-3 bg-emerald-500"></span></span>' : '';

    return L.divIcon({
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
  };

  const handleCorridorClick = (hubCoords: { lat: number; lng: number }) => {
    setTargetCoords([hubCoords.lat, hubCoords.lng]);
    setTargetZoom(14);
  };

  const resetToCityView = () => {
    setTargetCoords([APP_CONFIG.defaultCoordinates.lat, APP_CONFIG.defaultCoordinates.lng]);
    setTargetZoom(APP_CONFIG.defaultZoom);
  };

  if (!mounted) {
    return (
      <div className="w-full h-[550px] lg:h-[650px] bg-slate-100 rounded-3xl flex items-center justify-center text-slate-400 animate-pulse">
        <p className="text-sm font-semibold">Loading Chennai Map Engine...</p>
      </div>
    );
  }

  return (
    <div className="relative w-full h-[550px] lg:h-[680px] rounded-3xl overflow-hidden border border-slate-200 shadow-lg bg-slate-100 flex flex-col">
      {/* Top Floating Bar: Corridors Fly-to Shortcuts */}
      <div className="absolute top-3 left-3 right-3 z-[400] flex flex-wrap items-center justify-between gap-2 pointer-events-none">
        <div className="bg-white/95 backdrop-blur-md px-3 py-1.5 rounded-xl border border-slate-200 shadow-md flex items-center gap-1.5 overflow-x-auto max-w-full pointer-events-auto">
          <span className="text-[11px] font-bold text-slate-500 uppercase tracking-wider shrink-0 flex items-center gap-1">
            <Navigation className="w-3 h-3 text-brand-600" />
            <span>Hubs:</span>
          </span>
          {CHENNAI_TECH_HUBS.map((hub) => (
            <button
              key={hub.name}
              onClick={() => handleCorridorClick(hub.coordinates)}
              className="px-2 py-0.5 rounded-md text-[11px] font-medium text-slate-700 bg-slate-100 hover:bg-brand-50 hover:text-brand-700 hover:border-brand-200 border border-transparent whitespace-nowrap transition-all"
            >
              {hub.name.split(' ')[0]}
            </button>
          ))}
        </div>

        <button
          onClick={resetToCityView}
          className="bg-white/95 backdrop-blur-md px-3 py-1.5 rounded-xl border border-slate-200 shadow-md text-xs font-semibold text-slate-700 hover:bg-slate-50 hover:text-brand-600 pointer-events-auto flex items-center gap-1.5 transition-all shrink-0"
        >
          <Navigation className="w-3.5 h-3.5 text-brand-600" />
          <span>Center Chennai</span>
        </button>
      </div>

      {/* Map Container */}
      <MapContainer
        center={[APP_CONFIG.defaultCoordinates.lat, APP_CONFIG.defaultCoordinates.lng]}
        zoom={APP_CONFIG.defaultZoom}
        minZoom={APP_CONFIG.mapBounds.minZoom}
        maxZoom={APP_CONFIG.mapBounds.maxZoom}
        scrollWheelZoom={true}
        className="w-full h-full"
      >
        <TileLayer
          attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors &copy; <a href="https://carto.com/attributions">CARTO</a>'
          url="https://{s}.basemaps.cartocdn.com/rastertiles/voyager/{z}/{x}/{y}{r}.png"
        />

        <MapController targetCoords={targetCoords} zoom={targetZoom} />

        {filteredCompanies.map((company) => {
          const stats = db.getCompanyStats(company.id);
          const isSelected = selectedCompanyId === company.id;
          const isHovered = hoveredCompanyId === company.id;

          return (
            <Marker
              key={company.id}
              position={[company.coordinates.lat, company.coordinates.lng]}
              icon={createCompanyIcon(company, isSelected, isHovered)}
              eventHandlers={{
                mouseover: () => setHoveredCompanyId(company.id),
                mouseout: () => setHoveredCompanyId(null),
              }}
            >
              <Popup className="custom-popup" closeButton={false}>
                <div className="p-2 max-w-[260px] space-y-2">
                  <div className="flex items-start justify-between gap-2">
                    <div className="flex items-center gap-2">
                      <img
                        src={company.logo}
                        alt={company.name}
                        className="w-8 h-8 rounded-lg object-cover border border-slate-200"
                      />
                      <div>
                        <div className="flex items-center gap-1 font-bold text-slate-900 text-xs leading-tight">
                          <span>{company.name}</span>
                          {company.verificationStatus === 'VERIFIED' && (
                            <CheckCircle2 className="w-3.5 h-3.5 text-brand-600 shrink-0" />
                          )}
                        </div>
                        <div className="text-[10px] text-slate-500">{company.hub}</div>
                      </div>
                    </div>
                  </div>

                  <p className="text-[11px] text-slate-600 line-clamp-2 leading-relaxed">
                    {company.tagline}
                  </p>

                  <div className="flex items-center gap-1.5 flex-wrap">
                    <Badge variant={stats.activeJobsCount > 0 ? 'success' : 'neutral'} size="sm">
                      {stats.activeJobsCount > 0 ? `${stats.activeJobsCount} Active Jobs` : 'Not Hiring'}
                    </Badge>
                    {stats.fresherJobsCount > 0 && (
                      <Badge variant="brand" size="sm">
                        <GraduationCap className="w-3 h-3 mr-1" />
                        {stats.fresherJobsCount} Fresher
                      </Badge>
                    )}
                  </div>

                  <div className="pt-1 flex items-center gap-1.5">
                    <Button
                      size="sm"
                      variant="primary"
                      className="w-full text-xs py-1.5"
                      onClick={() => setSelectedCompanyId(company.id)}
                    >
                      Inspect Company & Vacancies
                    </Button>
                  </div>
                </div>
              </Popup>
            </Marker>
          );
        })}
      </MapContainer>

      {/* Bottom Floating Stats Bar */}
      <div className="absolute bottom-3 left-3 right-3 z-[400] flex items-center justify-between bg-white/95 backdrop-blur-md px-4 py-2 rounded-2xl border border-slate-200 shadow-md">
        <div className="flex items-center gap-3 text-xs">
          <div className="flex items-center gap-1.5">
            <span className="w-2.5 h-2.5 rounded-full bg-emerald-500 animate-pulse"></span>
            <span className="font-semibold text-slate-900">{filteredCompanies.length} Startups Showing</span>
          </div>
          <div className="hidden sm:flex items-center gap-1.5 text-slate-500 border-l border-slate-200 pl-3">
            <Briefcase className="w-3.5 h-3.5 text-brand-600" />
            <span>
              {filteredCompanies.reduce((acc, c) => acc + db.getCompanyStats(c.id).activeJobsCount, 0)} Open Chennai Positions
            </span>
          </div>
        </div>

        <div className="flex items-center gap-2 text-[11px] text-slate-500">
          <span className="hidden md:inline">Numbers in pins = live job vacancies</span>
        </div>
      </div>
    </div>
  );
};
