import { TechHub, Coordinates } from '../types';

export type MapPrecision = 'exact' | 'approximate' | 'area' | 'city';

export interface ChennaiLocationInfo {
  name: string;
  hub: TechHub;
  area: string;
  pincode?: string;
  coordinates: Coordinates;
  precision: MapPrecision;
  keywords: string[];
  description: string;
}

export const CHENNAI_LOCATIONS: ChennaiLocationInfo[] = [
  {
    name: 'OMR Tech Corridor',
    hub: 'OMR (IT Corridor)',
    area: 'OMR',
    pincode: '600096',
    coordinates: { lat: 12.9300, lng: 80.2350 },
    precision: 'area',
    keywords: ['omr', 'omr corridor', 'old mahabalipuram road', 'it highway', 'omr road'],
    description: 'Chennai main Information Technology Expressway spanning Taramani to Siruseri.'
  },
  {
    name: 'Tidel Park & Tharamani IT Zone',
    hub: 'Tidel Park & Tharamani',
    area: 'Taramani',
    pincode: '600113',
    coordinates: { lat: 12.9892, lng: 80.2479 },
    precision: 'exact',
    keywords: ['tidel', 'tidel park', 'taramani', 'tharamani', 'ascendas', 'csir', 'kanagam'],
    description: 'Pioneer IT hub of Chennai established in 2000, housing major IT enterprises and tech labs.'
  },
  {
    name: 'Perungudi & Kandanchavadi IT Corridor',
    hub: 'Perungudi & Kandanchavadi',
    area: 'Perungudi',
    pincode: '600096',
    coordinates: { lat: 12.9644, lng: 80.2427 },
    precision: 'exact',
    keywords: ['perungudi', 'kandanchavadi', 'rmz millenia', 'world trade center chennai', 'wtc'],
    description: 'Major software tech park zone on upper OMR hosting SaaS companies, product teams, and startups.'
  },
  {
    name: 'Sholinganallur Junction Tech Hub',
    hub: 'Sholinganallur',
    area: 'Sholinganallur',
    pincode: '600119',
    coordinates: { lat: 12.8996, lng: 80.2279 },
    precision: 'exact',
    keywords: ['sholinganallur', 'sholinganalur', 'elcot sez', 'wipro junction', 'hcl campus'],
    description: 'Central nexus of Chennai IT Corridor (OMR & ECR link) housing massive SEZ developments and tech giants.'
  },
  {
    name: 'Siruseri SIPCOT IT Park',
    hub: 'Siruseri SIPCOT',
    area: 'Siruseri',
    pincode: '603103',
    coordinates: { lat: 12.8286, lng: 80.2185 },
    precision: 'exact',
    keywords: ['siruseri', 'sipcot', 'navalur', 'tcs siruseri', 'cmi', 'egattur'],
    description: 'Asia largest IT park spread over 1000 acres, housing mega campuses for TCS, Cognizant, and SaaS enterprises.'
  },
  {
    name: 'Guindy & Olympia Tech Park',
    hub: 'Guindy (SIDCO / Olympia)',
    area: 'Guindy',
    pincode: '600032',
    coordinates: { lat: 13.0102, lng: 80.2157 },
    precision: 'exact',
    keywords: ['guindy', 'olympia', 'olympia tech park', 'sidco', 'ekkatuthangal', 'kalaimagal nagar'],
    description: 'Central business & tech hub near Chennai Airport with top connectivity, housing SaaS, mobility, and financial tech firms.'
  },
  {
    name: 'DLF Cybercity Porur & Manapakkam',
    hub: 'DLF Porur & Manapakkam',
    area: 'Porur',
    pincode: '600125',
    coordinates: { lat: 13.0278, lng: 80.1633 },
    precision: 'exact',
    keywords: ['dlf', 'porur', 'manapakkam', 'dlf cybercity', 'mount poonamallee'],
    description: 'Massive IT/ITES campus cluster in West Chennai with over 45,000 technology professionals.'
  },
  {
    name: 'Ambattur Industrial Estate & IT Zone',
    hub: 'Ambattur Industrial Estate',
    area: 'Ambattur',
    pincode: '600058',
    coordinates: { lat: 13.1147, lng: 80.1548 },
    precision: 'exact',
    keywords: ['ambattur', 'ambattur estate', 'kosmo one', 'indiabulls IT park'],
    description: 'North-West technology & industrial estate renowned for data centers, auto-tech engineering, and IT services.'
  },
  {
    name: 'Anna Nagar & Central Tech Zone',
    hub: 'Anna Nagar / Central Chennai',
    area: 'Anna Nagar',
    pincode: '600040',
    coordinates: { lat: 13.0850, lng: 80.2101 },
    precision: 'approximate',
    keywords: ['anna nagar', 'shenoy nagar', 'kilpauk', 'koyambedu'],
    description: 'Vibrant urban center featuring boutique SaaS software houses, product studios, and edtech ventures.'
  },
  {
    name: 'Nungambakkam & Mount Road Corridor',
    hub: 'Nungambakkam & Mount Road',
    area: 'Nungambakkam',
    pincode: '600034',
    coordinates: { lat: 13.0627, lng: 80.2407 },
    precision: 'approximate',
    keywords: ['nungambakkam', 'mount road', 'anna salai', 'kodambakkam', 'thousand lights'],
    description: 'Historic commercial corridor hosting fintech startups, venture firms, and corporate tech offices.'
  },
  {
    name: 'T. Nagar & Alwarpet Startup Hub',
    hub: 'T. Nagar & Alwarpet',
    area: 'Alwarpet',
    pincode: '600018',
    coordinates: { lat: 13.0339, lng: 80.2496 },
    precision: 'approximate',
    keywords: ['t nagar', 't.nagar', 'alwarpet', 'teynampet', 'mott street', 'cenotaph road'],
    description: 'Prime startup & agency neighborhood home to early-stage founders, incubators, and product studios.'
  }
];

export const CHENNAI_BOUNDS = {
  minLat: 12.7500,
  maxLat: 13.2500,
  minLng: 80.0000,
  maxLng: 80.3500
};

export function findChennaiLocationInfo(locationText: string): ChennaiLocationInfo | undefined {
  if (!locationText) return undefined;
  const textLower = locationText.toLowerCase();
  return CHENNAI_LOCATIONS.find(loc => 
    loc.keywords.some(kw => textLower.includes(kw)) || textLower.includes(loc.area.toLowerCase())
  );
}
