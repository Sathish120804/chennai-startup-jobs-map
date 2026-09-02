/// <reference types="vite/client" />

const API_BASE_URL = (import.meta as any).env?.VITE_API_BASE_URL || 'http://localhost:5241/api';

export async function fetchJson<T>(endpoint: string, options?: RequestInit): Promise<T> {
  const url = `${API_BASE_URL}${endpoint.startsWith('/') ? endpoint : '/' + endpoint}`;
  try {
    const res = await fetch(url, {
      ...options,
      headers: {
        'Content-Type': 'application/json',
        ...(options?.headers || {}),
      },
    });

    if (!res.ok) {
      const errorPayload = await res.json().catch(() => ({ message: res.statusText }));
      throw new Error(errorPayload.message || `API Error ${res.status}`);
    }

    return await res.json();
  } catch (err: any) {
    console.warn(`[API Client Fallback] Failed to connect to backend at ${url}. Falling back to client-side data engine.`, err);
    throw err;
  }
}
