const fs = require('fs');

fs.writeFileSync('src/components/ecosystem/EcosystemPulse.tsx', `import React from 'react';
import { db } from '../../services/db';
import { CHENNAI_TECH_HUBS } from '../../config/constants';
import { 
  ExternalLink, 
  TrendingUp, 
  Sparkles, 
  MapPin, 
  Calendar
} from 'lucide-react';
import { Card } from '../ui/Card';
import { Badge } from '../ui/Badge';

export const EcosystemPulse: React.FC = () => {
  const news = db.getNews();

  return (
    <div className="space-y-8 animate-fade-in">
      <div className="bg-gradient-to-r from-slate-900 via-brand-950 to-slate-900 text-white p-6 sm:p-10 rounded-3xl border border-slate-800 relative overflow-hidden shadow-xl">
        <div className="relative z-10 max-w-3xl space-y-3">
          <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-white/10 text-brand-300 text-xs font-semibold">
            <Sparkles className="w-3.5 h-3.5" />
            <span>Chennai Ecosystem Intelligence</span>
          </div>
          <h2 className="text-2xl sm:text-4xl font-extrabold tracking-tight">
            South Asia's SaaS Capital & DeepTech Epicenter
          </h2>
          <p className="text-slate-300 text-xs sm:text-sm leading-relaxed max-w-2xl">
            Chennai houses over 450+ high-growth tech startups, generating $4.2B+ in cumulative ARR across global B2B SaaS, advanced space launch systems, autonomous flight eVTOL, and electric mobility.
          </p>
        </div>
      </div>

      <div className="space-y-4">
        <div className="flex items-center justify-between">
          <h3 className="text-base font-bold text-slate-900 flex items-center gap-2">
            <MapPin className="w-5 h-5 text-teal-600" />
            <span>Chennai Tech Corridors at a Glance</span>
          </h3>
          <span className="text-xs text-slate-500">8 Primary Tech Clusters</span>
        </div>

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
          {CHENNAI_TECH_HUBS.map((hub) => (
            <Card key={hub.name} className="p-4 space-y-2 hover:border-brand-300">
              <div className="flex items-center justify-between">
                <span className="text-xs font-bold text-slate-900">{hub.name.split(' ')[0]}</span>
                <span className="text-[10px] font-semibold px-2 py-0.5 rounded bg-brand-50 text-brand-700">
                  {hub.badge}
                </span>
              </div>
              <p className="text-xs text-slate-600 leading-relaxed line-clamp-2">
                {hub.description}
              </p>
              <div className="pt-2 border-t border-slate-100 flex flex-wrap gap-1">
                {hub.popularParks.slice(0, 2).map((park) => (
                  <span key={park} className="text-[10px] bg-slate-100 text-slate-600 px-1.5 py-0.5 rounded">
                    {park}
                  </span>
                ))}
              </div>
            </Card>
          ))}
        </div>
      </div>

      <div className="space-y-4">
        <div className="flex items-center justify-between">
          <h3 className="text-base font-bold text-slate-900 flex items-center gap-2">
            <TrendingUp className="w-5 h-5 text-brand-600" />
            <span>Latest Chennai Startup Ecosystem News</span>
          </h3>
          <span className="text-xs text-slate-500">Curated from public sources</span>
        </div>

        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {news.map((item) => (
            <Card key={item.id} className="p-5 flex flex-col justify-between space-y-4 hover:border-brand-300">
              <div className="space-y-2.5">
                <div className="flex items-center justify-between gap-2">
                  <Badge variant={item.category === 'Launch' ? 'success' : item.category === 'Funding' ? 'brand' : 'accent'} size="sm">
                    {item.category}
                  </Badge>
                  <span className="text-xs text-slate-400 flex items-center gap-1">
                    <Calendar className="w-3 h-3" />
                    <span>{item.publishedDate}</span>
                  </span>
                </div>

                <h4 className="font-bold text-slate-900 text-sm leading-snug">
                  {item.title}
                </h4>

                <p className="text-xs text-slate-600 leading-relaxed">
                  {item.summary}
                </p>

                <div className="flex flex-wrap gap-1 pt-1">
                  {item.tags.map((tag) => (
                    <span key={tag} className="text-[10px] bg-slate-100 text-slate-600 px-2 py-0.5 rounded-full">
                      #{tag}
                    </span>
                  ))}
                </div>
              </div>

              <div className="pt-3 border-t border-slate-100 flex items-center justify-between text-xs">
                <span className="text-slate-500">
                  Source: <strong className="text-slate-800">{item.sourceName}</strong>
                </span>

                <a
                  href={item.sourceUrl}
                  target="_blank"
                  rel="noreferrer"
                  className="text-brand-600 font-semibold hover:underline flex items-center gap-1"
                >
                  <span>Read Article</span>
                  <ExternalLink className="w-3.5 h-3.5" />
                </a>
              </div>
            </Card>
          ))}
        </div>
      </div>
    </div>
  );
};
`, 'utf8');

console.log('Fixed EcosystemPulse');
