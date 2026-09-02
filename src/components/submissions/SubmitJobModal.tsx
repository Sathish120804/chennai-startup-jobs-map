import React, { useState, useMemo } from 'react';
import { X, Briefcase, CheckCircle2, Sparkles, Send, GraduationCap, Code2 } from 'lucide-react';
import { useAppStore } from '../../store/useAppStore';
import { db } from '../../services/db';
import { classifyJob } from '../../services/classifierEngine';
import { analyzeChennaiRelevance } from '../../services/relevanceEngine';
import { Button } from '../ui/Button';
import { Badge } from '../ui/Badge';

export const SubmitJobModal: React.FC = () => {
  const { isSubmitJobOpen, setSubmitJobOpen } = useAppStore();

  const [companyName, setCompanyName] = useState('');
  const [title, setTitle] = useState('');
  const [originalUrl, setOriginalUrl] = useState('');
  const [location, setLocation] = useState('Chennai, Tamil Nadu');
  const [descriptionSnippet, setDescriptionSnippet] = useState('');
  const [salaryRange, setSalaryRange] = useState('');
  const [submittedBy, setSubmittedBy] = useState('Recruiter / Community Contributor');
  const [email, setEmail] = useState('');
  const [isSuccess, setIsSuccess] = useState(false);

  // Live real-time classification preview
  const livePreview = useMemo(() => {
    if (!title && !descriptionSnippet) return null;
    const classification = classifyJob(title, descriptionSnippet);
    const relevance = analyzeChennaiRelevance(location, descriptionSnippet);
    return { classification, relevance };
  }, [title, descriptionSnippet, location]);

  if (!isSubmitJobOpen) return null;

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (!companyName || !title || !originalUrl) return;

    db.submitJob({
      companyName,
      title,
      originalUrl,
      location: location || 'Chennai',
      descriptionSnippet,
      salaryRange: salaryRange || 'Competitive',
      submittedBy: submittedBy || 'Recruiter / Community Contributor',
      email: email || undefined,
    });

    setIsSuccess(true);
    setTimeout(() => {
      setIsSuccess(false);
      setSubmitJobOpen(false);
      // Reset form
      setCompanyName('');
      setTitle('');
      setOriginalUrl('');
      setDescriptionSnippet('');
      setSalaryRange('');
    }, 1800);
  };

  return (
    <div className="fixed inset-0 z-50 overflow-y-auto bg-slate-900/60 backdrop-blur-xs flex items-center justify-center p-3 sm:p-6 animate-fade-in">
      <div className="bg-white w-full max-w-xl rounded-3xl shadow-2xl border border-slate-200 flex flex-col overflow-hidden">
        {/* Header */}
        <div className="flex items-center justify-between px-6 py-4 border-b border-slate-200 bg-slate-50">
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-xl bg-brand-50 flex items-center justify-center text-brand-600">
              <Briefcase className="w-4 h-4" />
            </div>
            <div>
              <h3 className="text-base font-bold text-slate-900">Post / Submit a Chennai Job Vacancy</h3>
              <p className="text-xs text-slate-500">Submit public vacancy to be indexed and matched to company profile.</p>
            </div>
          </div>
          <button
            onClick={() => setSubmitJobOpen(false)}
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
            <h4 className="text-base font-bold text-slate-900">Job Opportunity Submitted!</h4>
            <p className="text-xs text-slate-500 max-w-sm mx-auto">
              Our automated classification and Chennai relevance engine has indexed this opening for review.
            </p>
          </div>
        ) : (
          <form onSubmit={handleSubmit} className="p-6 space-y-4">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Company Name *</label>
                <input
                  type="text"
                  required
                  placeholder="e.g., Zoho, Freshworks, Kovai.co..."
                  value={companyName}
                  onChange={(e) => setCompanyName(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>

              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Job Title *</label>
                <input
                  type="text"
                  required
                  placeholder="e.g., .NET Engineer (Fresher), React Dev..."
                  value={title}
                  onChange={(e) => setTitle(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Original Job URL / ATS Link *</label>
                <input
                  type="url"
                  required
                  placeholder="https://company.com/jobs/123 or LinkedIn URL"
                  value={originalUrl}
                  onChange={(e) => setOriginalUrl(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>

              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Chennai Location / Tech Hub</label>
                <input
                  type="text"
                  placeholder="e.g., OMR, DLF Porur, Guindy, Chennai"
                  value={location}
                  onChange={(e) => setLocation(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>
            </div>

            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Submitted By (Name / Role)</label>
                <input
                  type="text"
                  placeholder="e.g. Priya (HR / Recruiter)"
                  value={submittedBy}
                  onChange={(e) => setSubmittedBy(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>

              <div className="space-y-1">
                <label className="text-xs font-semibold text-slate-700">Contact Email (Optional)</label>
                <input
                  type="email"
                  placeholder="recruiter@company.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  className="w-full bg-slate-50 border border-slate-200 rounded-xl px-3.5 py-2 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
                />
              </div>
            </div>

            <div className="space-y-1">
              <label className="text-xs font-semibold text-slate-700">Role Requirements / Description Snippet</label>
              <textarea
                rows={3}
                placeholder="Paste key responsibilities, required skills (React, .NET, Java, Python), or batch passouts (2025/2026)..."
                value={descriptionSnippet}
                onChange={(e) => setDescriptionSnippet(e.target.value)}
                className="w-full bg-slate-50 border border-slate-200 rounded-xl p-3 text-sm text-slate-900 focus:bg-white focus:border-brand-500 focus:outline-none focus:ring-2 focus:ring-brand-500/20"
              />
            </div>

            {/* Live AI Classification Inspector */}
            {livePreview && (
              <div className="p-3.5 rounded-2xl bg-brand-50/70 border border-brand-200 space-y-2 text-xs">
                <div className="flex items-center gap-1.5 font-bold text-brand-900 uppercase tracking-wider text-[11px]">
                  <Sparkles className="w-3.5 h-3.5 text-brand-600" />
                  <span>Real-Time Engine Classification Preview</span>
                </div>

                <div className="flex flex-wrap items-center gap-1.5">
                  <Badge variant={livePreview.relevance.relevance === 'CHENNAI_CONFIRMED' ? 'success' : 'neutral'} size="sm">
                    {livePreview.relevance.relevance} ({livePreview.relevance.confidence}%)
                  </Badge>
                  {livePreview.classification.isFresher && (
                    <Badge variant="brand" size="sm">
                      <GraduationCap className="w-3 h-3 mr-1" />
                      Fresher Detected ({livePreview.classification.fresherConfidence}%)
                    </Badge>
                  )}
                  {livePreview.classification.engineeringSubcategory && (
                    <Badge variant="outline" size="sm">
                      <Code2 className="w-3 h-3 mr-1" />
                      {livePreview.classification.engineeringSubcategory}
                    </Badge>
                  )}
                </div>

                {livePreview.classification.technologies.length > 0 && (
                  <div className="flex items-center gap-1 text-[11px] text-slate-600">
                    <span className="font-semibold">Detected Tech:</span>
                    <span>{livePreview.classification.technologies.join(', ')}</span>
                  </div>
                )}
              </div>
            )}

            <div className="pt-2 flex items-center justify-end gap-2">
              <Button type="button" variant="outline" size="sm" onClick={() => setSubmitJobOpen(false)}>
                Cancel
              </Button>
              <Button type="submit" variant="primary" size="md" rightIcon={<Send className="w-3.5 h-3.5" />}>
                Submit Vacancy
              </Button>
            </div>
          </form>
        )}
      </div>
    </div>
  );
};
