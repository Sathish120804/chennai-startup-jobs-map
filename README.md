# Chennai Startup & Jobs Map

An interactive, production-quality web platform and mapping directory exploring Chennai'\''s thriving tech ecosystem — South Asia'\''s capital for SaaS, DeepTech, and AutoTech innovation.

## Tech Stack
- **Frontend Framework**: React 18 + TypeScript + Vite 6
- **Styling**: Tailwind CSS + PostCSS + Autoprefixer
- **Icons**: Lucide React
- **Mapping**: Leaflet + React-Leaflet
- **State Management**: Zustand
- **Deployment Compatibility**: Vercel, Netlify, Cloudflare Pages, GitHub Pages

## Project Architecture
```text
src/
├── components/
│   ├── layout/       # Navbar, Footer, Layout wrapper
│   ├── ui/           # Reusable UI elements (Button, Badge, Card, Input)
│   └── map/          # Leaflet map foundation & coordinates
├── config/           # App configuration, bounds, and Chennai hub data
├── store/            # Zustand state management for filters and tabs
├── types/            # TypeScript data interfaces for companies, jobs, hubs
├── utils/            # Styling and class merging helpers (cn)
├── App.tsx           # Primary platform application foundation
├── main.tsx          # Application entrypoint
└── index.css         # Tailwind & custom scrollbar directives
```

## Getting Started

### Prerequisites
- Node.js >= 18
- npm >= 9

### Install Dependencies
```bash
npm install
```

### Local Development
```bash
npm run dev
```
Open [http://localhost:3000](http://localhost:3000) in your browser.

### Build for Production
```bash
npm run build
```
The optimized production bundle will be generated in `dist/`.
