import React from 'react';
import { MapPin, ExternalLink, Github, Linkedin } from 'lucide-react';
import { CHENNAI_TECH_HUBS } from '../../config/constants';
import { useAppStore } from '../../store/useAppStore';

// Configurable creator profiles (User can replace with their actual profile URLs)
export const CREATOR_PROFILE = {
  name: 'Sathish A',
  githubUrl: 'https://github.com/Sathish120804', // Configurable GitHub profile
  linkedinUrl: 'https://www.linkedin.com/in/sathish-a-placeholder', // Configurable LinkedIn profile placeholder
};

export const Footer: React.FC = () => {
  const { setActiveTab } = useAppStore();

  return (
    <footer className="bg-slate-900 text-slate-400 border-t border-slate-800 text-xs py-10">
      <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 space-y-8">
        <div className="grid grid-cols-1 md:grid-cols-4 gap-8">
          {/* Col 1: About & Mission */}
          <div className="space-y-3 md:col-span-2">
            <div className="flex items-center gap-2">
              <div className="w-7 h-7 rounded-lg bg-brand-500 flex items-center justify-center text-white">
                <MapPin className="w-4 h-4" />
              </div>
              <span className="font-bold text-white text-base">Chennai Startup & Jobs Map</span>
            </div>
            <p className="text-slate-400 leading-relaxed max-w-md">
              Helping people discover Chennai's companies and opportunities — from SaaS pioneers to hardware and EV innovators across OMR, Guindy, Siruseri, and DLF Porur.
            </p>

            {/* Creator Attribution */}
            <div className="pt-2 border-t border-slate-800/80 space-y-1">
              <p className="text-slate-300 font-medium">
                Built by an unsuccessful engineer — <span className="text-brand-300 font-bold">{CREATOR_PROFILE.name}</span>
              </p>
              <p className="text-slate-400 text-[11px] italic">
                "Still looking for the opportunity. Helping others find theirs along the way."
              </p>
              <div className="flex items-center gap-3 pt-1.5">
                <a
                  href={CREATOR_PROFILE.githubUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="inline-flex items-center gap-1.5 text-slate-300 hover:text-white transition-colors"
                >
                  <Github className="w-3.5 h-3.5" />
                  <span>GitHub</span>
                </a>
                <span className="text-slate-600">•</span>
                <a
                  href={CREATOR_PROFILE.linkedinUrl}
                  target="_blank"
                  rel="noopener noreferrer"
                  className="inline-flex items-center gap-1.5 text-slate-300 hover:text-white transition-colors"
                >
                  <Linkedin className="w-3.5 h-3.5 text-sky-400" />
                  <span>LinkedIn</span>
                </a>
              </div>
            </div>
          </div>

          {/* Col 2: Hubs Navigation */}
          <div>
            <h4 className="font-semibold text-slate-200 uppercase tracking-wider text-[11px] mb-3">
              Tech Corridors
            </h4>
            <ul className="space-y-1.5">
              {CHENNAI_TECH_HUBS.slice(0, 5).map((hub) => (
                <li
                  key={hub.name}
                  onClick={() => setActiveTab('map')}
                  className="hover:text-white transition-colors cursor-pointer"
                >
                  {hub.name}
                </li>
              ))}
            </ul>
          </div>

          {/* Col 3: Quick Navigation */}
          <div>
            <h4 className="font-semibold text-slate-200 uppercase tracking-wider text-[11px] mb-3">
              Navigation & Resources
            </h4>
            <ul className="space-y-1.5">
              <li onClick={() => setActiveTab('directory')} className="hover:text-white transition-colors cursor-pointer">
                Companies Directory
              </li>
              <li onClick={() => setActiveTab('jobs')} className="hover:text-white transition-colors cursor-pointer">
                Job & Internship Board
              </li>
              <li onClick={() => setActiveTab('map')} className="hover:text-white transition-colors cursor-pointer">
                Interactive Map
              </li>
              <li onClick={() => setActiveTab('ecosystem')} className="hover:text-white transition-colors cursor-pointer">
                Ecosystem Pulse
              </li>
              <li>
                <a
                  href="http://localhost:5241/swagger"
                  target="_blank"
                  rel="noopener noreferrer"
                  className="inline-flex items-center gap-1 text-brand-400 hover:text-brand-300 transition-colors"
                >
                  <span>Enterprise API (Swagger)</span>
                  <ExternalLink className="w-3 h-3" />
                </a>
              </li>
            </ul>
          </div>
        </div>

        <div className="border-t border-slate-800 pt-6 flex flex-col sm:flex-row items-center justify-between gap-4 text-slate-400">
          <p>© {new Date().getFullYear()} Chennai Startup & Jobs Map. Open community project.</p>
          <div className="flex items-center gap-4">
            <span className="hover:text-slate-300 cursor-pointer">Privacy Policy</span>
            <span className="hover:text-slate-300 cursor-pointer">Terms of Service</span>
            <a href="http://localhost:5241/swagger" target="_blank" rel="noopener noreferrer" className="hover:text-slate-300">
              OpenAPI v3
            </a>
          </div>
        </div>
      </div>
    </footer>
  );
};
