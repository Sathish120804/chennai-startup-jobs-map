import React, { useState, useEffect } from 'react';
import { 
  ShieldCheck, 
  CheckCircle2, 
  XCircle, 
  Search, 
  Layers, 
  Play, 
  Trash2, 
  ExternalLink
} from 'lucide-react';
import { db } from '../../services/db';
import { Button } from '../ui/Button';
import { Badge } from '../ui/Badge';
import { Card } from '../ui/Card';

export const AdminDashboard: React.FC = () => {
  const [, setVersion] = useState(0);
  const [activeSubTab, setActiveSubTab] = useState<'submissions' | 'jobs' | 'discovery'>('submissions');

  useEffect(() => {
    const unsub = db.subscribe(() => setVersion((v) => v + 1));
    return unsub;
  }, []);

  const companies = db.getCompanies();
  const jobs = db.getJobs();
  const submissions = db.getSubmissions();
  const discoveryQueries = db.getDiscoveryQueries();

  const pendingSubmissions = submissions.filter((s) => s.status === 'PENDING');

  return (
    <div className="space-y-6 animate-fade-in">
      <div className="bg-slate-900 text-white p-6 sm:p-8 rounded-3xl border border-slate-800 flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div className="space-y-1">
          <div className="flex items-center gap-2">
            <ShieldCheck className="w-6 h-6 text-brand-400" />
            <h2 className="text-xl sm:text-2xl font-bold">Chennai Discovery Engine — Admin Control</h2>
          </div>
          <p className="text-xs sm:text-sm text-slate-400 max-w-2xl leading-relaxed">
            Monitor automated web discovery queries, review incoming community submissions, inspect duplicate clusters, and verify Chennai location relevance signals.
          </p>
        </div>

        <div className="flex items-center gap-2">
          <Button
            variant="outline"
            size="sm"
            onClick={() => db.resetToDefaults()}
            leftIcon={<Trash2 className="w-3.5 h-3.5 text-rose-500" />}
            className="text-white border-slate-700 bg-slate-800 hover:bg-slate-700"
          >
            Reset Seed Data
          </Button>
        </div>
      </div>

      <div className="grid grid-cols-2 sm:grid-cols-4 gap-4">
        <Card className="p-4">
          <div className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Indexed Companies</div>
          <div className="text-2xl font-extrabold text-slate-900 mt-1">{companies.length}</div>
          <div className="text-[11px] text-brand-600 font-medium mt-0.5">Across 8 Chennai Tech Hubs</div>
        </Card>

        <Card className="p-4">
          <div className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Active Job Postings</div>
          <div className="text-2xl font-extrabold text-slate-900 mt-1">{jobs.length}</div>
          <div className="text-[11px] text-emerald-600 font-medium mt-0.5">{jobs.filter(j => j.isFresher).length} Fresher Friendly</div>
        </Card>

        <Card className="p-4">
          <div className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Submissions Queue</div>
          <div className="text-2xl font-extrabold text-slate-900 mt-1">{pendingSubmissions.length}</div>
          <div className="text-[11px] text-amber-600 font-medium mt-0.5">{submissions.length} Total Submissions</div>
        </Card>

        <Card className="p-4">
          <div className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Discovery Queries</div>
          <div className="text-2xl font-extrabold text-slate-900 mt-1">{discoveryQueries.length}</div>
          <div className="text-[11px] text-indigo-600 font-medium mt-0.5">Automated Search Jobs</div>
        </Card>
      </div>

      <div className="flex items-center gap-2 border-b border-slate-200 pb-2">
        <button
          onClick={() => setActiveSubTab('submissions')}
          className={`px-4 py-2 rounded-xl text-xs font-bold transition-all ${
            activeSubTab === 'submissions'
              ? 'bg-brand-600 text-white shadow-xs'
              : 'bg-white text-slate-600 hover:bg-slate-100'
          }`}
        >
          Review Submissions ({pendingSubmissions.length})
        </button>

        <button
          onClick={() => setActiveSubTab('discovery')}
          className={`px-4 py-2 rounded-xl text-xs font-bold transition-all ${
            activeSubTab === 'discovery'
              ? 'bg-brand-600 text-white shadow-xs'
              : 'bg-white text-slate-600 hover:bg-slate-100'
          }`}
        >
          Scheduled Discovery Queries ({discoveryQueries.length})
        </button>

        <button
          onClick={() => setActiveSubTab('jobs')}
          className={`px-4 py-2 rounded-xl text-xs font-bold transition-all ${
            activeSubTab === 'jobs'
              ? 'bg-brand-600 text-white shadow-xs'
              : 'bg-white text-slate-600 hover:bg-slate-100'
          }`}
        >
          All Indexed Jobs ({jobs.length})
        </button>
      </div>

      {activeSubTab === 'submissions' && (
        <div className="space-y-4">
          {submissions.length === 0 ? (
            <div className="p-12 text-center bg-white rounded-3xl border border-slate-200 text-slate-500 text-sm">
              No user or company submissions yet. Submit one from the top navigation bar!
            </div>
          ) : (
            <div className="space-y-3">
              {submissions.map((sub) => (
                <div
                  key={sub.id}
                  className="p-5 rounded-2xl border border-slate-200 bg-white flex flex-col md:flex-row md:items-center justify-between gap-4"
                >
                  <div className="space-y-1.5 flex-1">
                    <div className="flex items-center gap-2">
                      <span className="font-bold text-slate-900 text-sm">{sub.titleOrName}</span>
                      <Badge
                        variant={sub.status === 'APPROVED' ? 'success' : sub.status === 'REJECTED' ? 'neutral' : 'warning'}
                        size="sm"
                      >
                        {sub.status}
                      </Badge>
                      <span className="text-[11px] text-slate-400 capitalize">Type: {sub.type}</span>
                    </div>

                    <p className="text-xs text-slate-600 line-clamp-2">
                      {sub.notes || 'No extra notes provided.'}
                    </p>

                    <div className="flex flex-wrap items-center gap-3 text-[11px] text-slate-500">
                      <span>Submitted by: <strong>{sub.submittedBy}</strong></span>
                      <span>•</span>
                      <a href={sub.url} target="_blank" rel="noreferrer" className="text-brand-600 hover:underline flex items-center gap-1">
                        <span>Original Link</span>
                        <ExternalLink className="w-3 h-3" />
                      </a>
                    </div>
                  </div>

                  {sub.status === 'PENDING' && (
                    <div className="flex items-center gap-2 shrink-0">
                      <Button
                        size="sm"
                        variant="primary"
                        onClick={() => db.approveSubmission(sub.id)}
                        leftIcon={<CheckCircle2 className="w-3.5 h-3.5" />}
                      >
                        Approve & Index
                      </Button>
                      <Button
                        size="sm"
                        variant="ghost"
                        onClick={() => db.rejectSubmission(sub.id)}
                        leftIcon={<XCircle className="w-3.5 h-3.5 text-rose-500" />}
                        className="text-rose-600 hover:bg-rose-50"
                      >
                        Reject
                      </Button>
                    </div>
                  )}
                </div>
              ))}
            </div>
          )}
        </div>
      )}

      {activeSubTab === 'discovery' && (
        <div className="space-y-4">
          <div className="flex items-center justify-between">
            <p className="text-xs text-slate-500">
              Extensible automated search queries configured for continuous Chennai job discovery.
            </p>
            <Button
              size="sm"
              variant="outline"
              onClick={() => db.runDeduplicationCheck()}
              leftIcon={<Layers className="w-3.5 h-3.5 text-purple-600" />}
            >
              Run Deduplication Clustering
            </Button>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
            {discoveryQueries.map((dq) => (
              <Card key={dq.id} className="p-4 space-y-3">
                <div className="flex items-start justify-between gap-2">
                  <div>
                    <h4 className="font-bold text-slate-900 text-sm flex items-center gap-1.5">
                      <Search className="w-3.5 h-3.5 text-brand-600" />
                      <span>{dq.query}</span>
                    </h4>
                    <div className="text-[11px] text-slate-500 mt-0.5">
                      Sector: {dq.category} {dq.technology ? `• Tech: ${dq.technology}` : ''}
                    </div>
                  </div>
                  <Badge variant={dq.priority === 'high' ? 'brand' : 'neutral'} size="sm">
                    {dq.priority} priority
                  </Badge>
                </div>

                <div className="flex items-center justify-between pt-2 border-t border-slate-100 text-xs text-slate-500">
                  <div>
                    Discovered: <strong className="text-slate-800">{dq.resultsCount} postings</strong>
                  </div>
                  <Button
                    size="sm"
                    variant="outline"
                    onClick={() => db.triggerDiscoveryRun(dq.id)}
                    leftIcon={<Play className="w-3 h-3 text-emerald-600" />}
                  >
                    Run Now
                  </Button>
                </div>
              </Card>
            ))}
          </div>
        </div>
      )}

      {activeSubTab === 'jobs' && (
        <div className="space-y-3">
          {jobs.map((job) => (
            <div
              key={job.id}
              className="p-4 rounded-2xl border border-slate-200 bg-white flex flex-col sm:flex-row sm:items-center justify-between gap-3 text-xs"
            >
              <div className="space-y-1 flex-1">
                <div className="flex items-center gap-2">
                  <span className="font-bold text-slate-900">{job.title}</span>
                  <span className="text-slate-500">@ {job.companyName}</span>
                  <Badge variant={job.freshnessStatus === 'NEW' ? 'brand' : 'neutral'} size="sm">
                    {job.freshnessStatus}
                  </Badge>
                </div>
                <div className="text-slate-500 flex items-center gap-2">
                  <span>Relevance: <strong>{job.chennaiRelevance} ({job.relevanceConfidence}%)</strong></span>
                  <span>•</span>
                  <span>Source: <strong>{job.sourceName}</strong></span>
                </div>
              </div>

              <div className="flex items-center gap-2">
                <Button
                  size="sm"
                  variant="outline"
                  onClick={() => db.markJobVerified(job.id)}
                  leftIcon={<CheckCircle2 className="w-3 h-3 text-emerald-600" />}
                >
                  Verify
                </Button>
                <Button
                  size="sm"
                  variant="ghost"
                  onClick={() => db.markJobExpired(job.id)}
                  className="text-rose-600 hover:bg-rose-50"
                >
                  Expire
                </Button>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};
