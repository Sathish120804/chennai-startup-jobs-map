import React, { useState, useEffect } from 'react';
import { 
  ShieldCheck, 
  CheckCircle2, 
  XCircle, 
  Search, 
  Layers, 
  Play, 
  Trash2, 
  ExternalLink,
  Zap,
  Upload,
  FileSpreadsheet,
  BarChart3,
  Target,
  AlertTriangle,
  AlertCircle,
  RefreshCw,
  FileText
} from 'lucide-react';
import { db } from '../../services/db';
import { 
  adminApi, 
  AdminMetrics, 
  IngestionRunDto, 
  CompanyImportReportDto, 
  DataQualityDashboardDto 
} from '../../services/api/adminApi';
import { Button } from '../ui/Button';
import { Badge } from '../ui/Badge';
import { Card } from '../ui/Card';

export const AdminDashboard: React.FC = () => {
  const [, setVersion] = useState(0);
  const [activeSubTab, setActiveSubTab] = useState<'quality' | 'import' | 'ingestion' | 'submissions' | 'discovery' | 'jobs'>('quality');
  const [apiMetrics, setApiMetrics] = useState<AdminMetrics | null>(null);
  const [ingestionRuns, setIngestionRuns] = useState<IngestionRunDto[]>([]);
  const [qualityData, setQualityData] = useState<DataQualityDashboardDto | null>(null);
  const [isTriggering, setIsTriggering] = useState(false);
  const [isLoadingQuality, setIsLoadingQuality] = useState(false);

  // Import State
  const [importFormat, setImportFormat] = useState<'csv' | 'json'>('csv');
  const [rawContent, setRawContent] = useState('');
  const [importFile, setImportFile] = useState<File | null>(null);
  const [isDryRun, setIsDryRun] = useState(true);
  const [isImporting, setIsImporting] = useState(false);
  const [importReport, setImportReport] = useState<CompanyImportReportDto | null>(null);

  const fetchBackendData = async () => {
    const metrics = await adminApi.getMetrics();
    const runs = await adminApi.getIngestionRuns();
    setApiMetrics(metrics);
    setIngestionRuns(runs);
  };

  const fetchQualityData = async () => {
    setIsLoadingQuality(true);
    try {
      const data = await adminApi.getDataQualityDashboard();
      setQualityData(data);
    } finally {
      setIsLoadingQuality(false);
    }
  };

  useEffect(() => {
    fetchBackendData();
    fetchQualityData();
    const unsub = db.subscribe(() => setVersion((v) => v + 1));
    return unsub;
  }, []);

  const handleTriggerIngestion = async () => {
    setIsTriggering(true);
    await adminApi.triggerIngestion('src-careers');
    await fetchBackendData();
    await fetchQualityData();
    setIsTriggering(false);
  };

  const handleRunImport = async (dryRunOverride?: boolean) => {
    const dryRun = dryRunOverride !== undefined ? dryRunOverride : isDryRun;
    setIsImporting(true);
    try {
      let report: CompanyImportReportDto;
      const payload = importFile || rawContent;
      if (importFormat === 'csv') {
        report = await adminApi.importCompaniesCsv(payload, dryRun);
      } else {
        report = await adminApi.importCompaniesJson(payload, dryRun);
      }
      setImportReport(report);
      if (!dryRun) {
        await fetchQualityData();
        await fetchBackendData();
      }
    } finally {
      setIsImporting(false);
    }
  };

  const loadSampleCsv = () => {
    const sample = `name,website,careers_url,hub,category,company_type,city,founded_year,tech_stack
Karkinos Healthcare,https://karkinos.in,https://karkinos.in/careers/,Taramani (Tidel Park & Ascendas),HealthTech & BioTech,STARTUP,Chennai,2020,"Python; React; AWS; PostgreSQL"
Mindgrove Technologies,https://mindgrovetech.in,https://mindgrovetech.in/careers,Taramani (Tidel Park & Ascendas),Semiconductor & Hardware,STARTUP,Chennai,2021,"RISC-V; C++; Verilog; Linux"
Prodapt Solutions,https://prodapt.com,https://prodapt.com/careers,OMR (IT Corridor),IT Services & Digital Transformation,ENTERPRISE,Chennai,1999,"Java; Python; React; Cloud; DevOps"`;
    setRawContent(sample);
    setImportFile(null);
    setImportFormat('csv');
  };

  const loadSampleJson = () => {
    const sample = JSON.stringify([
      {
        name: "Detect Technologies",
        website: "https://detecttechnologies.com",
        careersUrl: "https://detecttechnologies.com/careers",
        hub: "Taramani (Tidel Park & Ascendas)",
        category: "DeepTech & AI",
        companyType: "STARTUP",
        city: "Chennai",
        foundedYear: 2016,
        description: "AI-powered industrial safety monitoring and IoT drone analytics spun out of IIT Madras."
      },
      {
        name: "Yubi (CredAvenue)",
        website: "https://go-yubi.com",
        careersUrl: "https://go-yubi.com/careers",
        hub: "Guindy (SIDCO / Olympia)",
        category: "FinTech",
        companyType: "STARTUP",
        city: "Chennai",
        foundedYear: 2020,
        description: "Unified digital debt marketplace platform connecting enterprise borrowers with financial institutions."
      }
    ], null, 2);
    setRawContent(sample);
    setImportFile(null);
    setImportFormat('json');
  };

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
            {apiMetrics && (
              <Badge variant="success" size="sm">
                API: {apiMetrics.environment}
              </Badge>
            )}
          </div>
          <p className="text-xs sm:text-sm text-slate-400 max-w-2xl leading-relaxed">
            Manage bulk CSV/JSON company ingestion, monitor Chennai data quality toward the 700+ company target, inspect duplicate clusters, and audit automated discovery.
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
          <div className="text-2xl font-extrabold text-slate-900 mt-1">
            {qualityData?.currentVerifiedCount || companies.length}
          </div>
          <div className="text-[11px] text-brand-600 font-medium mt-0.5">
            Target Goal: 700 Verified
          </div>
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
          <div className="text-xs font-semibold text-slate-500 uppercase tracking-wider">Data Quality Score</div>
          <div className="text-2xl font-extrabold text-slate-900 mt-1">
            {qualityData ? `${qualityData.targetProgressPercentage}%` : '95%'}
          </div>
          <div className="text-[11px] text-indigo-600 font-medium mt-0.5">Verified Chennai Presence</div>
        </Card>
      </div>

      <div className="flex items-center gap-2 border-b border-slate-200 pb-2 flex-wrap">
        <button
          onClick={() => setActiveSubTab('quality')}
          className={`px-4 py-2 rounded-xl text-xs font-bold transition-all flex items-center gap-1.5 ${
            activeSubTab === 'quality'
              ? 'bg-brand-600 text-white shadow-xs'
              : 'bg-white text-slate-600 hover:bg-slate-100'
          }`}
        >
          <BarChart3 className="w-3.5 h-3.5" />
          <span>Data Quality & Target (700)</span>
        </button>

        <button
          onClick={() => setActiveSubTab('import')}
          className={`px-4 py-2 rounded-xl text-xs font-bold transition-all flex items-center gap-1.5 ${
            activeSubTab === 'import'
              ? 'bg-brand-600 text-white shadow-xs'
              : 'bg-white text-slate-600 hover:bg-slate-100'
          }`}
        >
          <Upload className="w-3.5 h-3.5" />
          <span>Company Bulk Import (CSV/JSON)</span>
        </button>

        <button
          onClick={() => setActiveSubTab('ingestion')}
          className={`px-4 py-2 rounded-xl text-xs font-bold transition-all ${
            activeSubTab === 'ingestion'
              ? 'bg-brand-600 text-white shadow-xs'
              : 'bg-white text-slate-600 hover:bg-slate-100'
          }`}
        >
          Automated Pipeline ({ingestionRuns.length})
        </button>

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
          Discovery Queries ({discoveryQueries.length})
        </button>

        <button
          onClick={() => setActiveSubTab('jobs')}
          className={`px-4 py-2 rounded-xl text-xs font-bold transition-all ${
            activeSubTab === 'jobs'
              ? 'bg-brand-600 text-white shadow-xs'
              : 'bg-white text-slate-600 hover:bg-slate-100'
          }`}
        >
          All Jobs ({jobs.length})
        </button>
      </div>

      {/* 1. DATA QUALITY DASHBOARD TAB */}
      {activeSubTab === 'quality' && (
        <div className="space-y-6">
          {/* Target Progress Banner */}
          <Card className="p-6 bg-gradient-to-br from-slate-900 via-brand-950 to-slate-900 text-white border-slate-800">
            <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
              <div className="space-y-2">
                <div className="flex items-center gap-2">
                  <Target className="w-5 h-5 text-brand-400" />
                  <h3 className="text-lg font-bold">Chennai Ecosystem Expansion Target: 700 Companies</h3>
                </div>
                <p className="text-xs text-slate-300 max-w-xl">
                  Progress towards comprehensive coverage of all verified tech hubs (OMR, Guindy, Siruseri, DLF Porur, Ambattur, Tidel Park) across 15 high-growth sectors.
                </p>
              </div>

              <Button
                size="sm"
                variant="outline"
                disabled={isLoadingQuality}
                onClick={fetchQualityData}
                leftIcon={<RefreshCw className={`w-3.5 h-3.5 ${isLoadingQuality ? 'animate-spin' : ''}`} />}
                className="text-white border-slate-700 bg-white/10 hover:bg-white/20 self-start md:self-center"
              >
                Refresh Metrics
              </Button>
            </div>

            {qualityData && (
              <div className="mt-6 space-y-2">
                <div className="flex justify-between items-center text-xs font-semibold">
                  <span className="text-brand-300">
                    {qualityData.currentVerifiedCount} of {qualityData.targetCompanyGoal} verified companies indexed
                  </span>
                  <span className="text-white">{qualityData.targetProgressPercentage}% achieved</span>
                </div>
                <div className="w-full bg-slate-800 rounded-full h-3.5 overflow-hidden border border-slate-700">
                  <div 
                    className="bg-gradient-to-r from-brand-500 via-teal-400 to-emerald-400 h-full rounded-full transition-all duration-700 ease-out"
                    style={{ width: `${Math.max(5, Math.min(100, qualityData.targetProgressPercentage))}%` }}
                  />
                </div>
              </div>
            )}
          </Card>

          {/* Quality Breakdown Cards */}
          {qualityData && (
            <>
              <div className="grid grid-cols-2 sm:grid-cols-3 lg:grid-cols-6 gap-3">
                <Card className="p-3.5 text-center">
                  <div className="text-[11px] font-semibold text-slate-500 uppercase">Verified</div>
                  <div className="text-xl font-extrabold text-emerald-600 mt-1">{qualityData.verifiedCompanies}</div>
                  <div className="text-[10px] text-slate-400 mt-0.5">Official domain audit</div>
                </Card>

                <Card className="p-3.5 text-center">
                  <div className="text-[11px] font-semibold text-slate-500 uppercase">Source-Backed</div>
                  <div className="text-xl font-extrabold text-brand-600 mt-1">{qualityData.sourceBackedCompanies}</div>
                  <div className="text-[10px] text-slate-400 mt-0.5">With source links</div>
                </Card>

                <Card className="p-3.5 text-center">
                  <div className="text-[11px] font-semibold text-slate-500 uppercase">Missing Careers</div>
                  <div className="text-xl font-extrabold text-slate-800 mt-1">
                    {qualityData.missingCareersUrlCount}
                  </div>
                  <div className="text-[10px] text-slate-400 mt-0.5">Null if unverified</div>
                </Card>

                <Card className="p-3.5 text-center">
                  <div className="text-[11px] font-semibold text-slate-500 uppercase">Missing Coords</div>
                  <div className="text-xl font-extrabold text-slate-800 mt-1">
                    {qualityData.missingCoordinatesCount}
                  </div>
                  <div className="text-[10px] text-slate-400 mt-0.5">Exact map pins</div>
                </Card>

                <Card className="p-3.5 text-center">
                  <div className="text-[11px] font-semibold text-slate-500 uppercase">Stale Records</div>
                  <div className="text-xl font-extrabold text-slate-800 mt-1">{qualityData.staleCompanies}</div>
                  <div className="text-[10px] text-slate-400 mt-0.5">&gt;90 days unverified</div>
                </Card>

                <Card className="p-3.5 text-center">
                  <div className="text-[11px] font-semibold text-slate-500 uppercase">Duplicates</div>
                  <div className="text-xl font-extrabold text-purple-600 mt-1">{qualityData.duplicatesIdentified}</div>
                  <div className="text-[10px] text-slate-400 mt-0.5">Clusters resolved</div>
                </Card>
              </div>

              {/* Sector & Company Type Distributions */}
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <Card className="p-5 space-y-4">
                  <div className="flex items-center justify-between border-b border-slate-100 pb-3">
                    <h4 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                      <BarChart3 className="w-4 h-4 text-brand-600" />
                      <span>Coverage by Industry Category</span>
                    </h4>
                    <Badge variant="neutral" size="sm">
                      {Object.keys(qualityData.companiesByCategory || {}).length} Sectors
                    </Badge>
                  </div>

                  <div className="space-y-2.5">
                    {Object.entries(qualityData.companiesByCategory || {})
                      .sort(([, a], [, b]) => b - a)
                      .map(([category, count]) => {
                        const pct = qualityData.totalCompanies > 0 ? (count / qualityData.totalCompanies) * 100 : 0;
                        return (
                          <div key={category} className="space-y-1">
                            <div className="flex justify-between text-xs">
                              <span className="font-medium text-slate-700">{category}</span>
                              <span className="font-bold text-slate-900">{count} ({pct.toFixed(0)}%)</span>
                            </div>
                            <div className="w-full bg-slate-100 rounded-full h-2 overflow-hidden">
                              <div 
                                className="bg-brand-600 h-full rounded-full" 
                                style={{ width: `${Math.max(4, pct)}%` }} 
                              />
                            </div>
                          </div>
                        );
                      })}
                  </div>
                </Card>

                <Card className="p-5 space-y-4">
                  <div className="flex items-center justify-between border-b border-slate-100 pb-3">
                    <h4 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                      <Layers className="w-4 h-4 text-indigo-600" />
                      <span>Company Entity Types</span>
                    </h4>
                    <Badge variant="neutral" size="sm">
                      {Object.keys(qualityData.companiesByCompanyType || {}).length} Types
                    </Badge>
                  </div>

                  <div className="space-y-2.5">
                    {Object.entries(qualityData.companiesByCompanyType || {})
                      .sort(([, a], [, b]) => b - a)
                      .map(([type, count]) => {
                        const pct = qualityData.totalCompanies > 0 ? (count / qualityData.totalCompanies) * 100 : 0;
                        return (
                          <div key={type} className="space-y-1">
                            <div className="flex justify-between text-xs">
                              <span className="font-medium text-slate-700">{type}</span>
                              <span className="font-bold text-slate-900">{count} ({pct.toFixed(0)}%)</span>
                            </div>
                            <div className="w-full bg-slate-100 rounded-full h-2 overflow-hidden">
                              <div 
                                className="bg-indigo-600 h-full rounded-full" 
                                style={{ width: `${Math.max(4, pct)}%` }} 
                              />
                            </div>
                          </div>
                        );
                      })}
                  </div>
                </Card>
              </div>
            </>
          )}
        </div>
      )}

      {/* 2. COMPANY BULK IMPORT TAB */}
      {activeSubTab === 'import' && (
        <div className="space-y-6">
          <Card className="p-6 space-y-5">
            <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-3 border-b border-slate-100 pb-4">
              <div>
                <h3 className="text-base font-bold text-slate-900 flex items-center gap-2">
                  <FileSpreadsheet className="w-5 h-5 text-brand-600" />
                  <span>Bulk Company Ingestion Pipeline</span>
                </h3>
                <p className="text-xs text-slate-500 mt-0.5">
                  Import tech companies via CSV or JSON with automatic RFC 4180 parsing, Chennai relevance validation, and deduplication.
                </p>
              </div>

              <div className="flex items-center gap-2">
                <Button
                  size="sm"
                  variant="outline"
                  onClick={loadSampleCsv}
                  leftIcon={<FileSpreadsheet className="w-3.5 h-3.5 text-emerald-600" />}
                >
                  Load Sample CSV
                </Button>
                <Button
                  size="sm"
                  variant="outline"
                  onClick={loadSampleJson}
                  leftIcon={<FileText className="w-3.5 h-3.5 text-brand-600" />}
                >
                  Load Sample JSON
                </Button>
              </div>
            </div>

            {/* Format and Dry-Run Selectors */}
            <div className="flex flex-wrap items-center justify-between gap-4 bg-slate-50 p-3.5 rounded-2xl border border-slate-200 text-xs">
              <div className="flex items-center gap-4">
                <span className="font-semibold text-slate-700">Format:</span>
                <label className="inline-flex items-center gap-1.5 cursor-pointer">
                  <input
                    type="radio"
                    name="importFormat"
                    value="csv"
                    checked={importFormat === 'csv'}
                    onChange={() => setImportFormat('csv')}
                    className="text-brand-600 focus:ring-brand-500"
                  />
                  <span>CSV File / Text</span>
                </label>
                <label className="inline-flex items-center gap-1.5 cursor-pointer">
                  <input
                    type="radio"
                    name="importFormat"
                    value="json"
                    checked={importFormat === 'json'}
                    onChange={() => setImportFormat('json')}
                    className="text-brand-600 focus:ring-brand-500"
                  />
                  <span>JSON Payload</span>
                </label>
              </div>

              <label className="inline-flex items-center gap-2 cursor-pointer bg-white px-3 py-1.5 rounded-xl border border-slate-200">
                <input
                  type="checkbox"
                  checked={isDryRun}
                  onChange={(e) => setIsDryRun(e.target.checked)}
                  className="rounded text-brand-600 focus:ring-brand-500 w-4 h-4"
                />
                <span className="font-semibold text-slate-700">Dry Run (Preview validation & duplicates only)</span>
              </label>
            </div>

            {/* File Upload / Paste Box */}
            <div className="space-y-2">
              <div className="flex items-center justify-between text-xs">
                <span className="font-semibold text-slate-700">
                  Upload file or paste raw {importFormat.toUpperCase()} below:
                </span>
                {importFile && (
                  <span className="text-brand-600 font-medium">
                    Selected: {importFile.name} ({(importFile.size / 1024).toFixed(1)} KB)
                  </span>
                )}
              </div>

              <div className="border-2 border-dashed border-slate-300 rounded-2xl p-4 text-center hover:bg-slate-50 transition-colors">
                <input
                  type="file"
                  id="companyFileInput"
                  accept={importFormat === 'csv' ? '.csv,.txt' : '.json,.txt'}
                  onChange={(e) => {
                    if (e.target.files && e.target.files[0]) {
                      setImportFile(e.target.files[0]);
                      setRawContent('');
                    }
                  }}
                  className="hidden"
                />
                <label htmlFor="companyFileInput" className="cursor-pointer flex flex-col items-center gap-2">
                  <Upload className="w-8 h-8 text-slate-400" />
                  <span className="text-xs font-semibold text-brand-600 hover:underline">
                    Click to browse or drop {importFormat.toUpperCase()} file
                  </span>
                  <span className="text-[11px] text-slate-400">Max file size 10MB</span>
                </label>
              </div>

              <div className="pt-2">
                <textarea
                  rows={6}
                  placeholder={`Or paste ${importFormat.toUpperCase()} records directly here...`}
                  value={rawContent}
                  onChange={(e) => {
                    setRawContent(e.target.value);
                    setImportFile(null);
                  }}
                  className="w-full text-xs font-mono p-3 bg-slate-900 text-slate-100 rounded-2xl border border-slate-800 focus:ring-2 focus:ring-brand-500 focus:outline-none"
                />
              </div>
            </div>

            {/* Action Buttons */}
            <div className="flex items-center justify-between pt-2">
              <span className="text-xs text-slate-500">
                {isDryRun ? '🛡️ Safe mode: Database records will NOT be modified.' : '⚠️ Live mode: Records will be committed to database.'}
              </span>

              <div className="flex items-center gap-2">
                <Button
                  variant="outline"
                  size="sm"
                  disabled={isImporting || (!importFile && !rawContent.trim())}
                  onClick={() => handleRunImport(true)}
                  leftIcon={<Play className="w-3.5 h-3.5" />}
                >
                  {isImporting && isDryRun ? 'Validating...' : 'Dry-Run Preview'}
                </Button>

                <Button
                  variant="primary"
                  size="sm"
                  disabled={isImporting || (!importFile && !rawContent.trim())}
                  onClick={() => handleRunImport(false)}
                  leftIcon={<Upload className="w-3.5 h-3.5" />}
                >
                  {isImporting && !isDryRun ? 'Committing...' : 'Commit Import'}
                </Button>
              </div>
            </div>
          </Card>

          {/* Import Results & Preview Table */}
          {importReport && (
            <Card className="p-6 space-y-5">
              <div className="flex items-center justify-between border-b border-slate-100 pb-3">
                <h4 className="text-base font-bold text-slate-900 flex items-center gap-2">
                  <CheckCircle2 className="w-5 h-5 text-emerald-600" />
                  <span>Import Analysis Report</span>
                </h4>
                <Badge variant={importReport.errors.length > 0 ? 'warning' : 'success'} size="sm">
                  {importReport.totalRows} Records Processed
                </Badge>
              </div>

              {/* Summary Metrics */}
              <div className="grid grid-cols-2 sm:grid-cols-6 gap-3 text-center">
                <div className="p-3 bg-slate-50 rounded-xl border border-slate-200">
                  <div className="text-[11px] font-semibold text-slate-500">Total Rows</div>
                  <div className="text-lg font-bold text-slate-900 mt-0.5">{importReport.totalRows}</div>
                </div>
                <div className="p-3 bg-emerald-50 rounded-xl border border-emerald-200">
                  <div className="text-[11px] font-semibold text-emerald-700">Valid</div>
                  <div className="text-lg font-bold text-emerald-800 mt-0.5">{importReport.validRows}</div>
                </div>
                <div className="p-3 bg-brand-50 rounded-xl border border-brand-200">
                  <div className="text-[11px] font-semibold text-brand-700">New Companies</div>
                  <div className="text-lg font-bold text-brand-800 mt-0.5">{importReport.newCompanies}</div>
                </div>
                <div className="p-3 bg-purple-50 rounded-xl border border-purple-200">
                  <div className="text-[11px] font-semibold text-purple-700">Duplicates / Updated</div>
                  <div className="text-lg font-bold text-purple-800 mt-0.5">{importReport.duplicates}</div>
                </div>
                <div className="p-3 bg-amber-50 rounded-xl border border-amber-200">
                  <div className="text-[11px] font-semibold text-amber-700">Rejected (Off-Chennai)</div>
                  <div className="text-lg font-bold text-amber-800 mt-0.5">{importReport.rejectedCompanies}</div>
                </div>
                <div className="p-3 bg-rose-50 rounded-xl border border-rose-200">
                  <div className="text-[11px] font-semibold text-rose-700">Errors</div>
                  <div className="text-lg font-bold text-rose-800 mt-0.5">{importReport.errors.length}</div>
                </div>
              </div>

              {/* Warnings and Errors */}
              {importReport.warnings.length > 0 && (
                <div className="p-3.5 bg-amber-50 rounded-2xl border border-amber-200 text-xs space-y-1">
                  <div className="font-bold text-amber-900 flex items-center gap-1.5">
                    <AlertTriangle className="w-4 h-4 text-amber-600" />
                    <span>Relevance & Validation Warnings ({importReport.warnings.length})</span>
                  </div>
                  <ul className="list-disc list-inside text-amber-800 space-y-0.5 pl-1 max-h-32 overflow-y-auto">
                    {importReport.warnings.map((w, idx) => (
                      <li key={idx}>{w}</li>
                    ))}
                  </ul>
                </div>
              )}

              {importReport.errors.length > 0 && (
                <div className="p-3.5 bg-rose-50 rounded-2xl border border-rose-200 text-xs space-y-1">
                  <div className="font-bold text-rose-900 flex items-center gap-1.5">
                    <AlertCircle className="w-4 h-4 text-rose-600" />
                    <span>Critical Parsing Errors ({importReport.errors.length})</span>
                  </div>
                  <ul className="list-disc list-inside text-rose-800 space-y-0.5 pl-1">
                    {importReport.errors.map((e, idx) => (
                      <li key={idx}>{e}</li>
                    ))}
                  </ul>
                </div>
              )}

              {/* Sample Preview Table */}
              {importReport.samplePreview && importReport.samplePreview.length > 0 && (
                <div className="space-y-2">
                  <div className="text-xs font-bold text-slate-800">Sample Ingested Entities Preview:</div>
                  <div className="overflow-x-auto border border-slate-200 rounded-2xl">
                    <table className="w-full text-left text-xs">
                      <thead className="bg-slate-50 text-slate-500 font-semibold border-b border-slate-200">
                        <tr>
                          <th className="p-3">Company Name</th>
                          <th className="p-3">Hub</th>
                          <th className="p-3">Category</th>
                          <th className="p-3">Chennai Score</th>
                          <th className="p-3">Careers Link</th>
                        </tr>
                      </thead>
                      <tbody className="divide-y divide-slate-100">
                        {importReport.samplePreview.map((s, idx) => (
                          <tr key={idx} className="hover:bg-slate-50/80">
                            <td className="p-3 font-bold text-slate-900">{s.name}</td>
                            <td className="p-3 text-slate-600">{s.hub || 'OMR (IT Corridor)'}</td>
                            <td className="p-3 text-slate-600">{s.category || 'Technology'}</td>
                            <td className="p-3">
                              <Badge variant={s.chennaiRelevanceScore >= 75 ? 'success' : 'warning'} size="sm">
                                {s.chennaiRelevanceScore}/100
                              </Badge>
                            </td>
                            <td className="p-3 text-slate-500">
                              {s.careersUrl ? (
                                <a href={s.careersUrl} target="_blank" rel="noreferrer" className="text-brand-600 hover:underline">
                                  Careers Portal
                                </a>
                              ) : (
                                <span className="text-slate-400 italic">Unverified</span>
                              )}
                            </td>
                          </tr>
                        ))}
                      </tbody>
                    </table>
                  </div>
                </div>
              )}
            </Card>
          )}
        </div>
      )}

      {/* 3. AUTOMATED INGESTION PIPELINE TAB */}
      {activeSubTab === 'ingestion' && (
        <div className="space-y-4">
          <div className="flex items-center justify-between bg-white p-4 rounded-2xl border border-slate-200">
            <div>
              <h3 className="text-sm font-bold text-slate-900 flex items-center gap-2">
                <Zap className="w-4 h-4 text-brand-600" />
                <span>Idempotent Data Ingestion Pipeline Engine</span>
              </h3>
              <p className="text-xs text-slate-500">
                Executes discovery, title/company normalization, tech extraction, duplicate resolution, and freshness verification.
              </p>
            </div>

            <Button
              size="sm"
              variant="primary"
              disabled={isTriggering}
              onClick={handleTriggerIngestion}
              leftIcon={<Play className="w-3.5 h-3.5" />}
            >
              {isTriggering ? 'Running Ingestion...' : 'Trigger Discovery Ingestion'}
            </Button>
          </div>

          <div className="space-y-3">
            {ingestionRuns.map((run) => (
              <div
                key={run.id}
                className="p-4 rounded-2xl border border-slate-200 bg-white flex flex-col sm:flex-row items-start sm:items-center justify-between gap-3 text-xs"
              >
                <div className="space-y-1">
                  <div className="flex items-center gap-2">
                    <span className="font-bold text-slate-900">Run ID: {run.id.slice(0, 14)}...</span>
                    <Badge variant={run.status === 'COMPLETED' ? 'success' : run.status === 'FAILED' ? 'warning' : 'neutral'} size="sm">
                      {run.status}
                    </Badge>
                    <span className="text-slate-400">• Source: {run.sourceId}</span>
                  </div>
                  <div className="text-slate-500 flex flex-wrap items-center gap-3 text-[11px]">
                    <span>Discovered: <strong>{run.recordsDiscovered}</strong></span>
                    <span>•</span>
                    <span className="text-emerald-700">Created: <strong>{run.recordsCreated}</strong></span>
                    <span>•</span>
                    <span className="text-brand-700">Updated: <strong>{run.recordsUpdated}</strong></span>
                    <span>•</span>
                    <span className="text-purple-700">Duplicates: <strong>{run.duplicatesFound}</strong></span>
                  </div>
                </div>

                <div className="text-[11px] text-slate-400 text-right shrink-0">
                  <span>Started: {new Date(run.startedAt).toLocaleTimeString()}</span>
                </div>
              </div>
            ))}
          </div>
        </div>
      )}

      {/* 4. SUBMISSIONS REVIEW TAB */}
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

      {/* 5. DISCOVERY QUERIES TAB */}
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

      {/* 6. ALL JOBS TAB */}
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

