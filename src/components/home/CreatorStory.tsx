import React from 'react';
import { Compass, ArrowRight, Heart } from 'lucide-react';
import { Button } from '../ui/Button';
import { useAppStore } from '../../store/useAppStore';

export const CreatorStory: React.FC = () => {
  const { setActiveTab } = useAppStore();

  return (
    <section className="relative overflow-hidden rounded-3xl bg-gradient-to-b from-slate-900 via-slate-900 to-slate-950 text-white p-6 sm:p-10 border border-slate-800 shadow-xl">
      {/* Subtle background glow */}
      <div className="absolute top-0 right-1/4 w-72 h-72 bg-brand-500/10 rounded-full blur-3xl pointer-events-none" />
      <div className="absolute bottom-0 left-1/4 w-72 h-72 bg-teal-500/10 rounded-full blur-3xl pointer-events-none" />

      <div className="relative z-10 max-w-3xl mx-auto text-center space-y-5">
        {/* Label */}
        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-brand-500/10 border border-brand-400/20 text-[11px] font-bold tracking-widest text-brand-300 uppercase">
          <Heart className="w-3 h-3 text-brand-400 fill-brand-400" />
          <span>Why I Built This</span>
        </div>

        {/* Headline */}
        <h2 className="text-2xl sm:text-3xl lg:text-4xl font-extrabold text-white tracking-tight leading-tight">
          Finding opportunities shouldn't be harder than finding talent.
        </h2>

        {/* Body Copy */}
        <p className="text-slate-300 text-sm sm:text-base leading-relaxed">
          I built Chennai Startup & Jobs Map from the perspective of an engineer who knows how difficult it can be to find that first opportunity. The goal is simple: make it easier to discover the companies, people and opportunities that are already out there.
        </p>

        {/* Closing Line */}
        <p className="text-brand-300 font-medium text-sm sm:text-base italic">
          "Let's take you to where the opportunities are."
        </p>

        {/* CTAs */}
        <div className="pt-3 flex flex-wrap items-center justify-center gap-3">
          <Button
            variant="primary"
            size="md"
            onClick={() => setActiveTab('map')}
            leftIcon={<Compass className="w-4 h-4" />}
          >
            Explore Corridors Map
          </Button>

          <Button
            variant="outline"
            size="md"
            onClick={() => setActiveTab('jobs')}
            rightIcon={<ArrowRight className="w-4 h-4" />}
            className="text-white border-slate-700 bg-slate-800/80 hover:bg-slate-700 hover:border-slate-600"
          >
            Browse Verified Vacancies
          </Button>
        </div>
      </div>
    </section>
  );
};
