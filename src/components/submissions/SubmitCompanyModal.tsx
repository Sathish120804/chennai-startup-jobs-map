import React, { useState } from 'react';
import { X, Building2, MapPin, Globe, CheckCircle2, Sparkles, Send } from 'lucide-react';
import { useAppStore } from '../../store/useAppStore';
import { db } from '../../services/db';
import { CHENNAI_TECH_HUBS } from '../../config/constants';
import { Button } from '../ui/Button';
import { TechHub } from '../../types';

export const SubmitCompanyModal: React.FC = () => {
  const { isSubmitCompanyOpen, setSubmitCompanyOpen } = useAppStore();

  const [name, setName] = useState('');
  const [website, setWebsite] = useState('');
  const [careersUrl, setCareersUrl] = useState('');
  const [hub, setHub] = useState<TechHub>('OMR (IT Corridor)');
  const [address, setAddress] = useState('');
  const [description, setDescription] = useState('');
  const [submittedBy, setSubmittedBy] = useState('');
  const [email, setEmail] = useState('');
  const [isSuccess, setIsSuccess] = useState(false);

  if (!isSubmitCompanyOpen) return null;

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!name || !website) return;

    db.submitCompany({
      name,
      website,
      careersUrl: careersUrl || website,
      hub,
      address: address || `${hub}, Chennai, Tamil Nadu`,
      description,
      submittedBy: submittedBy || 'Community Contributor',
      email,
    });

    setIsSuccess(true);
    setTimeout(() => {
      setIsSuccess(false);
      setSubmitCompanyOpen(false);
      // Reset form
      setName('');
      setWebsite('');
      setCareersUrl('');
      setAddress('');
      setDescription('');
    }, 1800);
  };

  return (
    <div className="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-3 sm:p-6 animate-fade-in">
      <div className="bg-white w-full max-w-xl rounded-3xl shadow-2xl border border-slate-200 flex flex-col overflow-hidden">
        {/* Header */}
        <div className="flex items-center justify-between px-6 py-4 border-b border-slate-200 bg-slate-50">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-xl bg-brand-50 flex items-center justify-center text-brand-600">
              <Building2 className="w-4 h-4" />
            </div>
            <div>
              <h3 className="text-base font-bold text-slate-900">Add a Chennai Startup / Company</h3>
              <p className="text-xs text-slate-500">Submit a company to be mapped and indexed in the discovery engine.</p>
            </div>
          </div>
          <button
            onClick={() => setSubmitCompanyOpen(false)}
            className="p-1.5 rounded-lg text-slate-400 hover:text-slate-700 hover:bg-slate-200/60 transition-colors"
          >
            <X className="w-5 h-5" />
          </button>
        </div>

        {/* Content */}
        {isSuccess ? (
          <div className="p-10 text-center space-y-3">
            <div className="w-12 h-12 rounded-full bg-emerald-100 text-emerald-600 flex items-center justify-center mx-auto">
              <CheckCircle2 className="w-6 h-6" />
            </div>
            <h4 className="text-base font-bold text-slate-900">Company Submitted Successfully!</h4>
            <p className="text-xs text-slate-500 max-w-sm mx-auto">
              Thank you for contributing to the Chennai tech ecosystem map. Our moderation queue will review and publish it.
            </p>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="p-6 space-y-4">
            <div className="space-y-1">
              <label className="text-xs font-semibold text-slate-700">Company / Startup Name *</label>
              <input
                type="text"
                required
                placeholder="e.g., Agnikul Cosmos, SuperOps, Kissflow..."
                value={name}
                onChange={(e) => setName(e.target.value)}
                className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
              />
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Official Website URL *</label>
                <input
                  type="url"
                  required
                  placeholder="https://example.com"
                  value={website}
                  onChange={(e) => setWebsite(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>

              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Careers Page / ATS URL</label>
                <input
                  type="url"
                  placeholder="https://example.com/careers"
                  value={careersUrl}
                  onChange={(e) => setCareersUrl(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Primary Tech Corridor *</label>
                <select
                  value={hub}
                  onChange={(e) => setHub(e.target.value as TechHub)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none"
                >
                  {CHENNAI_TECH_HUBS.map((h) => (
                    <option key={h.name} value={h.name}>
                      {h.name}
                    </option>
                  ))}
                </select>
              </div>

              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Chennai Office Address</label>
                <input
                  type="text"
                  placeholder="e.g., Tidel Park 4th Floor, Tharamani"
                  value={address}
                  onChange={(e) => setAddress(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>
            </div>

            <div className="space-y-1">
              <label className="text-xs font-semibold text-slate-700">Brief Description</label>
              <textarea
                rows={2}
                placeholder="What does this company build or solve?"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                className="w-full bg-slate-50 border border-slate-200 rounded-xl p-3 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
              />
            </div>

            <div className="pt-2 flex items-center justify-end gap-2">
              <Button type="button" variant="outline" size="sm" onClick={() => setSubmitCompanyOpen(false)}>
                Cancel
              </Button>
              <Button type="submit" variant="primary" size="md" rightIcon={<Send className="w-3.5 h-3.5" />}>
                Submit for Verification
              </Button>
            </div>
          </form>
        )}
      </div>
    </div>
  );
};
