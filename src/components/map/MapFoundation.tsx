import React, { useEffect, useState } from 'react';
import { MapContainer, TileLayer, Marker, Popup } from 'react-leaflet';
import L from 'leaflet';
import { APP_CONFIG, CHENNAI_TECH_HUBS } from '../../config/constants';
import { Badge } from '../ui/Badge';
import { MapPin, Navigation, Layers, Compass } from 'lucide-react';

// Fix Leaflet default icon paths in bundlers
const customHubIcon = L.divIcon({
  className: 'custom-hub-marker',
  html: `<div style="background-color: #0284c7; width: 28px; height: 28px; border-radius: 50%; display: flex; align-items: center; justify-content: center; color: white; border: 2px solid white; box-shadow: 0 4px 6px -1px rgba(0, 0, 0, 0.2);">
          <svg style="width: 14px; height: 14px;" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
            <path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"/>
            <circle cx="12" cy="10" r="3"/>
          </svg>
        </div>`,
  iconSize: [28, 28],
  iconAnchor: [14, 28],
  popupAnchor: [0, -28],
});

export const MapFoundation: React.FC = () => {
  const [mounted, setMounted] = useState(false);

  useEffect(() => {
    setMounted(true);
  }, []);

  if (!mounted) {
    return (
      <div className="w-full h-full min-h-[450px] bg-slate-100 animate-pulse rounded-2xl flex items-center justify-center text-slate-400">
        <div className="flex flex-col items-center gap-2">
          <Compass className="w-8 h-8 animate-spin text-brand-500" />
          <p className="text-sm font-medium">Initializing Chennai Geo Engine...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="relative w-full h-[520px] lg:h-[620px] rounded-2xl overflow-hidden border border-slate-200 shadow-sm bg-white">
      {/* Floating Map Controls & Indicators */}
      <div className="absolute top-4 left-4 z-[400] bg-white/90 backdrop-blur-md rounded-xl p-3 border border-slate-200/80 shadow-md max-w-xs">
        <div className="flex items-center gap-2 mb-1.5">
          <div className="w-2.5 h-2.5 rounded-full bg-emerald-500 animate-ping"></div>
          <span className="text-xs font-semibold text-slate-800">Chennai Tech Corridors</span>
          <Badge size="sm" variant="brand">8 Major Hubs</Badge>
        </div>
        <p className="text-[11px] text-slate-500 leading-tight">
          Click any hub pin on the map to explore startup clusters and enterprise parks.
        </p>
      </div>

      <div className="absolute top-4 right-4 z-[400] flex flex-col gap-2">
        <button
          title="Map Layer"
          className="bg-white hover:bg-slate-50 text-slate-700 p-2 rounded-lg border border-slate-200 shadow-sm transition-all"
        >
          <Layers className="w-4 h-4 text-brand-600" />
        </button>
        <button
          title="Center on Chennai"
          className="bg-white hover:bg-slate-50 text-slate-700 p-2 rounded-lg border border-slate-200 shadow-sm transition-all"
        >
          <Navigation className="w-4 h-4 text-slate-600" />
        </button>
      </div>

      {/* Leaflet Map */}
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

        {CHENNAI_TECH_HUBS.map((hub) => (
          <Marker
            key={hub.name}
            position={[hub.coordinates.lat, hub.coordinates.lng]}
            icon={customHubIcon}
          >
            <Popup className="custom-popup">
              <div className="p-1 max-w-[200px]">
                <div className="flex items-center gap-1.5 font-bold text-slate-900 text-xs mb-1">
                  <MapPin className="w-3.5 h-3.5 text-brand-600 shrink-0" />
                  <span>{hub.name}</span>
                </div>
                <p className="text-[11px] text-slate-600 leading-normal mb-2">
                  {hub.description}
                </p>
                <div className="text-[10px] text-brand-600 font-semibold uppercase tracking-wider">
                  Tech Cluster Pin
                </div>
              </div>
            </Popup>
          </Marker>
        ))}
      </MapContainer>
    </div>
  );
};
