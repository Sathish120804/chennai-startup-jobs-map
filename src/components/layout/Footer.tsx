import React from 'react';
import { Heart, MapPin, ExternalLink } from 'lucide-react';
import { CHENNAI_TECH_HUBS } from '../../config/constants';

export const Footer: React.FC = () => {
  return (
    <footer className="bg-slate-900 text-slate-400 border-t border-slate-800 text-xs py-8">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8 mb-8">
          {/* Col 1: About */}
          <div className="space-y-3 md:col-span-2">
            <div className="flex items-center gap-2">
              <div className="w-6 h-6 rounded-lg bg-brand-500 flex items-center justify-center text-white">
                <MapPin className="w-3.5 h-3.5" />
              </div>
              <span className="font-bold text-white text-sm">Chennai Startup & Jobs Map</span>
            </div>
            <p className="text-slate-400 leading-relaxed max-w-md">
              The community-driven map and directory celebrating Chennai's vibrant tech ecosystem — from SaaS pioneers to hardware & EV innovators across the IT Corridor, Guindy, DLF, and beyond.
            </p>
            <div className="flex items-center gap-1.5 text-slate-300">
              <span>Made with</span>
              <Heart className="w-3.5 h-3.5 text-rose-500 fill-rose-500" />
              <span>for the Chennai tech & founder community.</span>
            </div>
          </div>

          {/* Col 2: Hubs */}
          <div>
            <h4 className="font-semibold text-slate-200 uppercase tracking-wider text-[11px] mb-3">
              Tech Corridors
            </h4>
            <ul className="space-y-1.5">
              {CHENNAI_TECH_HUBS.slice(0, 5).map((hub) => (
                <li key={hub.name} className="hover:text-white transition-colors cursor-pointer">
                  {hub.name}
                </li>
              ))}
            </ul>
          </div>

          {/* Col 3: Resources */}
          <div>
            <h4 className="font-semibold text-slate-200 uppercase tracking-wider text-[11px] mb-3">
              Community & Links
            </h4>
            <ul className="space-y-1.5">
              <li className="flex items-center gap-1 hover:text-white transition-colors cursor-pointer">
                <span>IIT Madras Research Park</span>
                <ExternalLink className="w-3 h-3" />
              </li>
              <li className="flex items-center gap-1 hover:text-white transition-colors cursor-pointer">
                <span>SaasBOOMi Chennai</span>
                <ExternalLink className="w-3 h-3" />
              </li>
              <li className="flex items-center gap-1 hover:text-white transition-colors cursor-pointer">
                <span>StartupTN Initiatives</span>
                <ExternalLink className="w-3 h-3" />
              </li>
              <li className="hover:text-white transition-colors cursor-pointer">
                Submit Feedback / Suggestion
              </li>
            </ul>
          </div>
        </div>

        <div className="border-t border-slate-800/80 pt-6 flex flex-col sm:flex-row items-center justify-between gap-4 text-slate-400">
          <p>© {new Date().getFullYear()} Chennai Startup Map. Open community project.</p>
          <div className="flex items-center gap-4">
            <span className="hover:text-slate-300 cursor-pointer">Privacy Policy</span>
            <span className="hover:text-slate-300 cursor-pointer">Terms</span>
            <span className="hover:text-slate-300 cursor-pointer">API</span>
          </div>
        </div>
      </div>
    </footer>
  );
};
