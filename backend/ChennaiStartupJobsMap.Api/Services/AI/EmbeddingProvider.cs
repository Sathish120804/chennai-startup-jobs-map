using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ChennaiStartupJobsMap.Api.Services.AI
{
    public interface IEmbeddingProvider
    {
        float[] GenerateEmbedding(string text);
        float CosineSimilarity(float[] vectorA, float[] vectorB);
    }

    /// <summary>
    /// High-performance deterministic semantic embedding provider for development, testing, and offline modes.
    /// Maps technical taxonomies, corridor keywords, and role concepts into normalized 64-dimensional semantic space.
    /// </summary>
    public class DeterministicEmbeddingProvider : IEmbeddingProvider
    {
        private const int Dimension = 64;
        private readonly ConcurrentDictionary<string, float[]> _cache = new();

        private static readonly Dictionary<string, int> SemanticConceptBuckets = new(StringComparer.OrdinalIgnoreCase)
        {
            // Core Software Disciplines (Dimensions 0 - 9)
            { "backend", 0 }, { "api", 0 }, { "server", 0 }, { "microservices", 0 },
            { "frontend", 1 }, { "ui", 1 }, { "ux", 1 }, { "web", 1 },
            { "fullstack", 2 }, { "full stack", 2 },
            { "mobile", 3 }, { "android", 3 }, { "ios", 3 }, { "flutter", 3 },
            { "ai", 4 }, { "ml", 4 }, { "machine learning", 4 }, { "deep learning", 4 }, { "nlp", 4 }, { "genai", 4 },
            { "devops", 5 }, { "cloud", 5 }, { "sre", 5 }, { "infrastructure", 5 },
            { "qa", 6 }, { "testing", 6 }, { "automation", 6 },
            { "data", 7 }, { "analytics", 7 }, { "bi", 7 }, { "sql", 7 },
            { "product", 8 }, { "saas", 8 }, { "enterprise", 8 },
            { "hardware", 9 }, { "embedded", 9 }, { "iot", 9 }, { "ev", 9 },

            // Technologies (Dimensions 10 - 29)
            { ".net", 10 }, { "dotnet", 10 }, { "c#", 10 }, { "asp.net", 10 },
            { "react", 11 }, { "reactjs", 11 }, { "nextjs", 11 },
            { "angular", 12 },
            { "vue", 13 },
            { "node", 14 }, { "nodejs", 14 }, { "express", 14 },
            { "python", 15 }, { "django", 15 }, { "fastapi", 15 },
            { "java", 16 }, { "spring", 16 }, { "springboot", 16 },
            { "golang", 17 }, { "go", 17 },
            { "aws", 18 }, { "azure", 18 }, { "gcp", 18 },
            { "docker", 19 }, { "kubernetes", 19 },
            { "postgresql", 20 }, { "postgres", 20 }, { "mongodb", 20 },
            { "typescript", 21 }, { "javascript", 21 },
            { "tensorflow", 22 }, { "pytorch", 22 },

            // Experience / Level (Dimensions 30 - 39)
            { "fresher", 30 }, { "entry", 30 }, { "junior", 30 }, { "0-1", 30 }, { "trainee", 30 }, { "graduate", 30 },
            { "intern", 31 }, { "internship", 31 }, { "stipend", 31 },
            { "mid", 32 }, { "senior", 33 }, { "lead", 34 }, { "architect", 35 },

            // Chennai Tech Corridors (Dimensions 40 - 49)
            { "omr", 40 }, { "old mahabalipuram", 40 },
            { "guindy", 41 }, { "olympia", 41 },
            { "perungudi", 42 }, { "kandanchavadi", 42 },
            { "taramani", 43 }, { "tideltid", 43 }, { "tidel", 43 },
            { "siruseri", 44 }, { "sipcot", 44 },
            { "porur", 45 }, { "dlf", 45 },
            { "ambattur", 46 },
            { "velachery", 47 }, { "adyar", 48 }, { "chennai", 49 }
        };

        public float[] GenerateEmbedding(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return new float[Dimension];

            var key = text.Trim().ToLower();
            if (_cache.TryGetValue(key, out var cached)) return cached;

            var vector = new float[Dimension];

            // 1. Exact semantic dictionary concept activations
            foreach (var kvp in SemanticConceptBuckets)
            {
                if (key.Contains(kvp.Key))
                {
                    vector[kvp.Value] += 1.0f;
                }
            }

            // 2. Hash-based semantic projection for open vocabulary words
            using var sha = SHA256.Create();
            var tokens = key.Split(new[] { ' ', ',', '-', '/', '.', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                var hashBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(token));
                int index = Math.Abs(BitConverter.ToInt32(hashBytes, 0)) % Dimension;
                float sign = (hashBytes[4] % 2 == 0) ? 1.0f : -1.0f;
                vector[index] += 0.25f * sign;
            }

            // 3. Normalize vector to unit length (L2 norm)
            float norm = (float)Math.Sqrt(vector.Sum(x => x * x));
            if (norm > 0)
            {
                for (int i = 0; i < Dimension; i++)
                {
                    vector[i] /= norm;
                }
            }

            _cache.TryAdd(key, vector);
            return vector;
        }

        public float CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA == null || vectorB == null || vectorA.Length != vectorB.Length) return 0.0f;

            float dot = 0.0f;
            float normA = 0.0f;
            float normB = 0.0f;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dot += vectorA[i] * vectorB[i];
                normA += vectorA[i] * vectorA[i];
                normB += vectorB[i] * vectorB[i];
            }

            float denominator = (float)(Math.Sqrt(normA) * Math.Sqrt(normB));
            if (denominator <= 0.00001f) return 0.0f;

            var sim = dot / denominator;
            return Math.Clamp(sim, 0.0f, 1.0f);
        }
    }
}
