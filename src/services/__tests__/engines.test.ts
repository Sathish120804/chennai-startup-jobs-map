import { describe, it, expect } from 'vitest';
import { classifyJob } from '../classifierEngine';
import { analyzeChennaiRelevance } from '../relevanceEngine';
import { detectJobDuplicates } from '../deduplicationEngine';
import { parseSearchIntent } from '../searchIntentParser';
import { Job } from '../../types';

describe('Chennai Engine Unit Tests', () => {

  describe('Classifier Engine', () => {
    it('correctly identifies fresher software roles', () => {
      const result = classifyJob(
        'Associate Software Developer (Freshers 2025/2026)',
        'We are hiring fresh engineering graduates for Java and Python backend systems.'
      );
      expect(result.isFresher).toBe(true);
      expect(result.fresherConfidence).toBeGreaterThanOrEqual(50);
      expect(result.technologies).toContain('Java');
      expect(result.technologies).toContain('Python');
    });

    it('correctly identifies internships and subcategories', () => {
      const result = classifyJob(
        'React Frontend Intern',
        'Looking for a React.js summer intern to build user interfaces.'
      );
      expect(result.isInternship).toBe(true);
      expect(result.isFresher).toBe(true);
      expect(result.engineeringSubcategory).toBe('Frontend');
      expect(result.technologies).toContain('React');
    });

    it('rejects senior roles as freshers', () => {
      const result = classifyJob(
        'Senior Software Architect (8+ years)',
        'Lead microservices development in Java and Kubernetes.'
      );
      expect(result.isFresher).toBe(false);
      expect(result.experienceLevel).not.toBe('Fresher / Entry (0-1 yrs)');
    });
  });

  describe('Relevance Engine', () => {
    it('confirms explicit OMR / Chennai locations', () => {
      const result = analyzeChennaiRelevance(
        'Tidel Park, Tharamani, OMR, Chennai',
        'Office located at Tidel Park IT corridor.'
      );
      expect(result.relevance).toBe('CHENNAI_CONFIRMED');
      expect(result.confidence).toBeGreaterThanOrEqual(75);
    });

    it('identifies non-Chennai city locations', () => {
      const result = analyzeChennaiRelevance(
        'Electronic City, Bangalore',
        'Bangalore engineering center office.'
      );
      expect(result.relevance).toBe('NOT_CHENNAI');
    });
  });

  describe('Search Intent Parser', () => {
    it('parses technology synonym "dotnet" and fresher intent', () => {
      const intent = parseSearchIntent('dotnet fresher chennai');
      expect(intent.technology).toBe('.NET');
      expect(intent.isFresher).toBe(true);
      expect(intent.hasLocationIntent).toBe(true);
    });

    it('parses internship and hub intent "React internship OMR"', () => {
      const intent = parseSearchIntent('React internship OMR');
      expect(intent.technology).toBe('React');
      expect(intent.isInternship).toBe(true);
      expect(intent.hub).toBe('OMR (IT Corridor)');
    });

    it('parses startup intent "AI startups Chennai"', () => {
      const intent = parseSearchIntent('AI startups Chennai');
      expect(intent.companyType).toBe('STARTUP');
      expect(intent.category).toBe('DeepTech & AI');
      expect(intent.hasLocationIntent).toBe(true);
    });
  });

  describe('Deduplication Engine', () => {
    it('groups duplicate postings from different sources', () => {
      const mockJobs: Partial<Job>[] = [
        {
          id: 'job-a',
          companyName: 'Zoho Corporation',
          title: 'Associate Software Developer',
          location: 'Chennai',
          firstSeenAt: '2026-09-01T00:00:00Z',
          lastSeenAt: '2026-09-01T00:00:00Z',
          sourceName: 'Company Careers',
          originalUrl: 'https://zoho.com/careers/1',
        },
        {
          id: 'job-b',
          companyName: 'Zoho Corp',
          title: 'Associate Software Developer',
          location: 'Chennai',
          firstSeenAt: '2026-09-01T01:00:00Z',
          lastSeenAt: '2026-09-01T01:00:00Z',
          sourceName: 'LinkedIn',
          originalUrl: 'https://linkedin.com/jobs/2',
        }
      ];

      const { canonicalJobs, duplicateGroups } = detectJobDuplicates(mockJobs as Job[]);
      expect(duplicateGroups.length).toBe(1);
      expect(canonicalJobs.length).toBe(1);
      expect(canonicalJobs[0].alternateSources?.length).toBe(1);
    });
  });

});
