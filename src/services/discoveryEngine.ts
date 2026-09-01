import { JobDiscoveryQuery } from '../types';

export const DEFAULT_DISCOVERY_QUERIES: JobDiscoveryQuery[] = [
  { id: 'dq-1', query: 'Software Engineer Chennai', category: 'SaaS / Enterprise Software', location: 'Chennai', priority: 'high', active: true, resultsCount: 42, lastRunAt: '2026-09-01T10:00:00Z' },
  { id: 'dq-2', query: 'Fresher Software Engineer Chennai', category: 'All', experience: '0-1 years', location: 'Chennai', priority: 'high', active: true, resultsCount: 28, lastRunAt: '2026-09-01T10:15:00Z' },
  { id: 'dq-3', query: '.NET Developer Chennai', category: 'SaaS / Enterprise Software', technology: '.NET', location: 'Chennai', priority: 'high', active: true, resultsCount: 19, lastRunAt: '2026-09-01T10:30:00Z' },
  { id: 'dq-4', query: 'Java Developer Chennai OMR', category: 'FinTech', technology: 'Java', location: 'OMR Chennai', priority: 'high', active: true, resultsCount: 31, lastRunAt: '2026-09-01T11:00:00Z' },
  { id: 'dq-5', query: 'React Developer Chennai', category: 'SaaS / Enterprise Software', technology: 'React', location: 'Chennai', priority: 'high', active: true, resultsCount: 35, lastRunAt: '2026-09-01T11:30:00Z' },
  { id: 'dq-6', query: 'Python AI Engineer IIT Madras', category: 'DeepTech & AI', technology: 'Python', location: 'Tharamani Chennai', priority: 'high', active: true, resultsCount: 14, lastRunAt: '2026-09-01T12:00:00Z' },
  { id: 'dq-7', query: 'EV Embedded Systems Engineer Chennai', category: 'AutoTech & EV', technology: 'Embedded & IoT', location: 'Guindy Chennai', priority: 'medium', active: true, resultsCount: 12, lastRunAt: '2026-09-01T12:30:00Z' },
  { id: 'dq-8', query: 'Software Internship Chennai 2025 2026', category: 'All', experience: 'Internship', location: 'Chennai', priority: 'high', active: true, resultsCount: 22, lastRunAt: '2026-09-01T13:00:00Z' },
];