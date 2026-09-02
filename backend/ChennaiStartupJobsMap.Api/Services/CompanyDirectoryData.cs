using System;
using System.Collections.Generic;
using ChennaiStartupJobsMap.Api.Models;

namespace ChennaiStartupJobsMap.Api.Services
{
    public static class CompanyDirectoryData
    {
        public static List<Company> GetVerifiedChennaiCompanies()
        {
            var list = new List<Company>();

            void AddCompany(
                string id, string name, string slug, string tagline, string desc,
                string website, string careersUrl,
                List<string> types, List<string> categories, string hub, string address,
                double lat, double lng, int founded, string empCount, string hiring,
                List<string> tech, List<string> tags, string sourceUrl = "")
            {
                list.Add(new Company
                {
                    Id = id,
                    Name = name,
                    NormalizedName = name.Trim().ToLower(),
                    Slug = slug,
                    Tagline = tagline,
                    Description = desc,
                    Logo = $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(name)}&background=0284c7&color=fff&size=128&bold=true",
                    Website = website,
                    CareersUrl = careersUrl,
                    CompanyTypes = types,
                    Categories = categories,
                    Hub = hub,
                    Address = address,
                    Latitude = lat,
                    Longitude = lng,
                    MapPrecision = "exact",
                    FoundedYear = founded,
                    EmployeeCount = empCount,
                    FundingStage = types.Contains("MNC") || types.Contains("ENTERPRISE") ? "Public / Enterprise" : "Venture Funded / Profitable",
                    HiringStatus = hiring,
                    Tags = tags,
                    TechStack = tech,
                    VerificationStatus = "VERIFIED",
                    IsFeatured = types.Contains("SAAS") || types.Contains("STARTUP") || types.Contains("GCC"),
                    IsActive = true,
                    IsSeedData = true,
                    SourceName = "Official Company Website / Careers",
                    SourceUrl = string.IsNullOrWhiteSpace(sourceUrl) ? careersUrl : sourceUrl,
                    DiscoveredAt = DateTime.UtcNow.AddDays(-30),
                    LastVerifiedAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    UpdatedAt = DateTime.UtcNow
                });
            }

            // ==========================================
            // 1. SAAS & CLOUD PRODUCT TITANS
            // ==========================================
            AddCompany("comp-1", "Zoho Corporation", "zoho", "Bootstrapped Global SaaS Titan",
                "Zoho offers 55+ cloud software applications for businesses worldwide. Bootstrapped from Chennai with 15,000+ employees globally.",
                "https://www.zoho.com", "https://www.zoho.com/careers/",
                new() { "PRODUCT COMPANY", "SAAS", "ENTERPRISE" }, new() { "SaaS / Enterprise Software", "DeepTech & AI" },
                "OMR (IT Corridor)", "Estancia IT Park, Plot No. 140 & 151, GST Road / OMR Corridor, Chennai",
                12.8252, 80.0435, 1996, "10,000+", "Hiring Surge",
                new() { "Java", "C++", "React", "Python", "Deluge", "PostgreSQL" },
                new() { "SaaS", "Cloud CRM", "Enterprise Suites", "Bootstrapped" });

            AddCompany("comp-2", "Freshworks", "freshworks", "AI-Powered Customer & Employee Engagement SaaS",
                "Freshworks makes business software people love. Founded in Chennai, listed on NASDAQ (FRSH), serving 65,000+ businesses worldwide.",
                "https://www.freshworks.com", "https://careers.freshworks.com/",
                new() { "PRODUCT COMPANY", "SAAS", "ENTERPRISE" }, new() { "SaaS / Enterprise Software", "DeepTech & AI" },
                "Perungudi & Kandanchavadi", "SP Infocity, Block B, 40 MGR Salai, Kandanchavadi, Perungudi, Chennai 600096",
                12.9648, 80.2447, 2010, "5,000+", "Active",
                new() { "Ruby on Rails", "Java", "React", "AWS", "Python", "Kafka" },
                new() { "SaaS", "Customer Experience", "ITSM", "NASDAQ" });

            AddCompany("comp-3", "Kissflow", "kissflow", "Pioneering Low-Code & Digital Workplace Platform",
                "Kissflow offers a unified low-code work management platform used by Fortune 500 enterprises. Headquartered in Tidel Park.",
                "https://kissflow.com", "https://kissflow.com/careers/",
                new() { "PRODUCT COMPANY", "SAAS", "STARTUP" }, new() { "SaaS / Enterprise Software" },
                "Taramani (Tidel Park & Ascendas)", "World Trade Center / Tidel Park, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9892, 80.2494, 2003, "500-1,000", "Active",
                new() { "Java", "Node.js", "React", "Python", "AWS", "MongoDB" },
                new() { "Low-Code", "BPM", "Workflow", "SaaS" });

            AddCompany("comp-4", "Chargebee", "chargebee", "Subscription Billing & Revenue Management Platform",
                "Chargebee simplifies recurring billing and subscription management for thousands of high-growth SaaS companies worldwide.",
                "https://www.chargebee.com", "https://www.chargebee.com/careers/",
                new() { "PRODUCT COMPANY", "SAAS", "STARTUP" }, new() { "SaaS / Enterprise Software", "FinTech" },
                "Perungudi & Kandanchavadi", "Prince Info City II, 283/3 & 283/4, Rajiv Gandhi Salai, Perungudi, Chennai 600096",
                12.9632, 80.2458, 2011, "1,000-5,000", "Active",
                new() { "Java", "Spring Boot", "React", "AWS", "MySQL", "Kafka" },
                new() { "FinTech", "Subscription Billing", "SaaS", "Unicorn" });

            AddCompany("comp-5", "Facilio", "facilio", "AI-Driven Connected Real Estate Operations",
                "Facilio helps real estate owners and facilities teams aggregate building data, optimize energy, and automate maintenance.",
                "https://facilio.com", "https://facilio.com/careers/",
                new() { "PRODUCT COMPANY", "SAAS", "STARTUP" }, new() { "SaaS / Enterprise Software", "DeepTech & AI" },
                "OMR (IT Corridor)", "Global Infocity, MGR Salai, Kandanchavadi, Perungudi, Chennai 600096",
                12.9678, 80.2471, 2017, "200-500", "Active",
                new() { "Node.js", "Python", "React", "AWS", "IoT", "TimescaleDB" },
                new() { "PropTech", "IoT", "AI Facilities", "SaaS" });

            AddCompany("comp-6", "Hippo Video", "hippo-video", "Generative AI Video & Video Personalization Platform",
                "Hippo Video is an interactive video customer experience platform empowering sales and marketing teams worldwide with AI video workflows.",
                "https://www.hippovideo.io", "https://www.hippovideo.io/careers.html",
                new() { "PRODUCT COMPANY", "SAAS", "STARTUP" }, new() { "SaaS / Enterprise Software", "DeepTech & AI" },
                "Guindy (SIDCO / Olympia)", "Olympia Technology Park, Guindy, Chennai 600032",
                13.0112, 80.2084, 2016, "100-250", "Active",
                new() { "Python", "FFmpeg", "React", "TensorFlow", "AWS", "WebRTC" },
                new() { "Generative AI", "Video SaaS", "Sales Enablement" });

            AddCompany("comp-7", "SuperOps.ai", "superops.ai", "Unified PSA-RMM Platform for Modern MSPs",
                "SuperOps is building a future-ready unified Professional Services Automation and Remote Monitoring platform powered by AI.",
                "https://superops.ai", "https://superops.ai/careers",
                new() { "PRODUCT COMPANY", "SAAS", "STARTUP" }, new() { "SaaS / Enterprise Software" },
                "Guindy (SIDCO / Olympia)", "Olympia Platina, Guindy Industrial Estate, Chennai 600032",
                13.0125, 80.2091, 2020, "100-250", "Active",
                new() { "Go", "React", "Node.js", "AWS", "PostgreSQL", "Docker" },
                new() { "ITSM", "RMM", "PSA", "SaaS" });

            AddCompany("comp-8", "Kaar Technologies", "kaartech", "Enterprise SAP Digital Transformation Consulting",
                "Kaar Tech is an enterprise pure-play SAP consultancy and cloud digital transformation partner founded in Chennai.",
                "https://www.kaartech.com", "https://www.kaartech.com/careers/",
                new() { "ENTERPRISE", "IT SERVICES", "SAAS" }, new() { "SaaS / Enterprise Software", "IT Services & Consulting" },
                "Porur & DLF Cybercity", "Level 8, Block 9, DLF Cyber City, Manapakkam, Porur, Chennai 600089",
                13.0315, 80.1652, 2005, "2,000-5,000", "Active",
                new() { "SAP S/4HANA", "ABAP", "Java", "Azure", "SAP BTP" },
                new() { "SAP", "ERP", "Enterprise Cloud" });

            AddCompany("comp-9", "Ramco Systems", "ramco-systems", "Enterprise Cloud ERP, Aviation M&E, and Global Payroll",
                "Ramco Systems creates enterprise multi-tenant cloud software for Aviation Maintenance, Global Payroll, and Logistics.",
                "https://www.ramco.com", "https://www.ramco.com/careers/",
                new() { "PRODUCT COMPANY", "ENTERPRISE", "SAAS" }, new() { "SaaS / Enterprise Software" },
                "Taramani (Tidel Park & Ascendas)", "64, Sardar Patel Road, Taramani, Chennai 600113",
                12.9904, 80.2458, 1997, "2,000-5,000", "Active",
                new() { ".NET", "C#", "SQL Server", "Angular", "Azure", "AI/ML" },
                new() { "Enterprise Cloud", "ERP", "Aviation Software", "Payroll" });

            AddCompany("comp-10", "Intellect Design Arena", "intellect-design", "Next-Gen Composable FinTech & Banking Platform",
                "Intellect Design Arena builds cloud-native FinTech software architecture (eGov, Core Banking, Wealth) powering 250+ global banks.",
                "https://www.intellectdesign.com", "https://www.intellectdesign.com/careers/",
                new() { "PRODUCT COMPANY", "ENTERPRISE", "SAAS" }, new() { "FinTech", "SaaS / Enterprise Software" },
                "Siruseri (SIPCOT IT Park)", "Plot No. 3/A-6, SIPCOT IT Park, Siruseri, Chennai 603103",
                12.8288, 80.2195, 2014, "5,000+", "Active",
                new() { "Java", "Spring Boot", "Microservices", "React", "Kafka", "Oracle" },
                new() { "FinTech", "Core Banking", "Composable Architecture" });

            AddCompany("comp-11", "Kovai.co", "kovai-co", "Multi-Product Enterprise SaaS Software Company",
                "Kovai.co powers enterprise products Document360 (knowledge base) and Serverless360 (Azure management).",
                "https://www.kovai.co", "https://www.kovai.co/careers/",
                new() { "PRODUCT COMPANY", "SAAS" }, new() { "SaaS / Enterprise Software" },
                "OMR (IT Corridor)", "Rajiv Gandhi Salai, Navalur, Chennai 603103",
                12.8456, 80.2268, 2011, "250-500", "Active",
                new() { ".NET", "C#", "Azure", "React", "Angular", "CosmosDB" },
                new() { "SaaS", "Azure Monitoring", "Document360" });

            AddCompany("comp-12", "GoFrugal Technologies", "gofrugal", "Omnichannel ERP Software for Retail & Distribution",
                "GoFrugal provides retail point-of-sale and supply-chain management software to 35,000+ businesses across 75+ countries.",
                "https://www.gofrugal.com", "https://www.gofrugal.com/careers.html",
                new() { "PRODUCT COMPANY", "SAAS" }, new() { "SaaS / Enterprise Software", "E-Commerce & Retail Tech" },
                "Perungudi & Kandanchavadi", "Rayala Techno Park, 144/7 OMR, Kottivakkam / Perungudi, Chennai 600041",
                12.9712, 80.2482, 2004, "500-1,000", "Active",
                new() { "Java", "Android", "React", "PostgreSQL", "Cloud POS" },
                new() { "Retail ERP", "Omnichannel", "SaaS" });

            // ==========================================
            // 2. MNCs & GLOBAL CAPABILITY CENTERS (GCCs)
            // ==========================================
            AddCompany("comp-13", "PayPal India", "paypal-india", "Global Digital Payments Technology Hub",
                "PayPal's Chennai Technology Center is one of its largest global engineering hubs, developing core checkout, fraud AI, and crypto engines.",
                "https://www.paypal.com", "https://careers.pypl.com/home/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "FinTech", "DeepTech & AI" },
                "OMR (IT Corridor)", "Futura Tech Park, 334 Rajiv Gandhi Salai, Sholinganallur, Chennai 600119",
                12.9015, 80.2285, 1998, "5,000+", "Hiring Surge",
                new() { "Java", "Spring Boot", "React", "Python", "Kafka", "Hadoop", "AI/ML" },
                new() { "FinTech", "Digital Payments", "MNC", "GCC" });

            AddCompany("comp-14", "Amazon Development Centre Chennai", "amazon-chennai", "Amazon Global Operations, Cloud & Retail Engineering",
                "Amazon's Chennai tech campus drives critical services across AWS, Consumer Retail, Kindle Devices, and Logistics Systems.",
                "https://www.amazon.jobs", "https://www.amazon.jobs/en/locations/chennai-india",
                new() { "MNC", "GCC", "PRODUCT COMPANY" }, new() { "SaaS / Enterprise Software", "DeepTech & AI" },
                "Perungudi & Kandanchavadi", "Brigade Vantage & World Trade Center, Perungudi, Chennai 600096",
                12.9638, 80.2462, 1994, "10,000+", "Hiring Surge",
                new() { "Java", "C++", "Python", "AWS", "React", "Distributed Systems" },
                new() { "MNC", "Cloud", "E-Commerce", "Big Tech" });

            AddCompany("comp-15", "Microsoft IDC Chennai", "microsoft-chennai", "Global Cloud, Azure & Enterprise Engineering",
                "Microsoft's Chennai technology center develops core enterprise products, Azure cloud infrastructure, and partner engineering solutions.",
                "https://careers.microsoft.com", "https://careers.microsoft.com/us/en/search-results?qcountry=India&qcity=Chennai",
                new() { "MNC", "GCC", "PRODUCT COMPANY" }, new() { "SaaS / Enterprise Software", "DeepTech & AI" },
                "Guindy (SIDCO / Olympia)", "Olympia Technology Park, Guindy Industrial Estate, Chennai 600032",
                13.0118, 80.2088, 1975, "2,000+", "Active",
                new() { "C#", ".NET", "Azure", "C++", "TypeScript", "Python" },
                new() { "MNC", "Cloud", "Operating Systems", "Enterprise" });

            AddCompany("comp-16", "Cisco Systems Chennai", "cisco-chennai", "Enterprise Networking, Security & Cloud Infrastructure Hub",
                "Cisco's Chennai engineering site contributes to secure agile networks, SD-WAN, catalyst switches, and collaboration software.",
                "https://www.cisco.com", "https://jobs.cisco.com/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "SaaS / Enterprise Software", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "Ascendas International Tech Park, CSIR Road, Taramani, Chennai 600113",
                12.9868, 80.2452, 1984, "2,000+", "Active",
                new() { "C", "C++", "Python", "Go", "Kubernetes", "Linux Kernel" },
                new() { "Networking", "Cybersecurity", "MNC", "GCC" });

            AddCompany("comp-17", "Ford Global Technology and Business Center", "ford-india", "Next-Gen Connected Vehicle Software & Mobility GCC",
                "Ford's premier Global Technology and Business Center (GTBC) in Chennai engineers autonomous vehicle algorithms, infotainment, and telematics.",
                "https://www.ford.com", "https://corporate.ford.com/careers.html",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Automotive Tech & EV", "DeepTech & AI" },
                "OMR (IT Corridor)", "ELCOT SEZ, Sholinganallur, OMR, Chennai 600119",
                12.8988, 80.2272, 1903, "10,000+", "Hiring Surge",
                new() { "C++", "Python", "Embedded Linux", "AUTOSAR", "Cloud IoT", "Java" },
                new() { "Automotive", "Connected Vehicles", "MNC", "GCC" });

            AddCompany("comp-18", "Caterpillar India Engineering", "caterpillar-india", "Heavy Machinery Digital Automation & Electronics R&D",
                "Caterpillar's Chennai Engineering Design Center leads design for heavy mining, autonomous hauling, and electric machinery systems.",
                "https://www.caterpillar.com", "https://www.caterpillar.com/en/careers.html",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Manufacturing & Industrial Tech", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "Ascendas International Tech Park, Taramani, Chennai 600113",
                12.9865, 80.2449, 1925, "5,000+", "Active",
                new() { "Embedded C", "Simulink", "Python", "IoT", "CAD/CAE" },
                new() { "Industrial Tech", "Autonomous Machinery", "MNC" });

            AddCompany("comp-19", "Shell Information Technology Centre", "shell-it-chennai", "Energy Transition & Computational Analytics Tech Hub",
                "Shell Information Technology International in Chennai develops digital energy platforms, decarbonization models, and IoT analytics.",
                "https://www.shell.com", "https://www.shell.com/careers.html",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "DeepTech & AI", "SaaS / Enterprise Software" },
                "Porur & DLF Cybercity", "DLF Cybercity, 1/124 Shivaji Gardens, Manapakkam, Porur, Chennai 600089",
                13.0322, 80.1648, 1907, "3,000+", "Active",
                new() { "Python", "R", "Azure", "Java", "Power BI", "PyTorch" },
                new() { "Energy Tech", "Data Science", "MNC", "GCC" });

            AddCompany("comp-20", "BNY Mellon International Operations", "bny-mellon-chennai", "Global Custody, Asset Servicing & Financial Engineering GCC",
                "BNY Mellon's Chennai Innovation Center develops ultra-low latency custody software, clearing APIs, and wealth risk models.",
                "https://www.bnymellon.com", "https://jobs.bnymellon.com/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "FinTech" },
                "Porur & DLF Cybercity", "DLF IT Park, 1/124 Mount Poonamallee Road, Porur, Chennai 600089",
                13.0308, 80.1658, 1784, "5,000+", "Active",
                new() { "Java", "Spring Boot", "Kafka", "Angular", "Oracle", "Python" },
                new() { "FinTech", "Investment Banking", "MNC", "GCC" });

            AddCompany("comp-21", "Standard Chartered Global Business Services", "standard-chartered-gbs", "Global Digital Banking & Cyber Defense GCC",
                "Standard Chartered GBS Chennai is the flagship engineering hub creating core mobile banking, algorithmic compliance, and trade finance.",
                "https://www.sc.com", "https://www.sc.com/en/careers/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "FinTech", "Cybersecurity" },
                "OMR (IT Corridor)", "Standard Chartered Tower, 1 Rajiv Gandhi Salai, Haddows Road / OMR, Chennai",
                12.9125, 80.2312, 1853, "10,000+", "Hiring Surge",
                new() { "Java", "React", "AWS", "Python", "Kubernetes", "Cybersecurity" },
                new() { "Banking GCC", "FinTech", "MNC" });

            AddCompany("comp-22", "AstraZeneca India", "astrazeneca-chennai", "Global Clinical Trials, Genomics & Healthcare Informatics GCC",
                "AstraZeneca's Chennai Global Technology Center delivers clinical trial data analytics, digital biopharma pipelines, and health AI.",
                "https://www.astrazeneca.com", "https://careers.astrazeneca.com/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "HealthTech", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "Ramanujan IT City, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9878, 80.2465, 1999, "2,000+", "Active",
                new() { "Python", "R", "SAS", "AWS", "Genomics", "Machine Learning" },
                new() { "BioPharma", "HealthTech", "MNC", "GCC" });

            AddCompany("comp-23", "Barclays Global Service Centre", "barclays-chennai", "Enterprise Cards, Payments & Investment Tech GCC",
                "Barclays Chennai Technology Centre builds transaction engines, fraud mitigation AI, and international payments rails.",
                "https://home.barclays", "https://search.jobs.barclays/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "FinTech", "DeepTech & AI" },
                "Porur & DLF Cybercity", "DLF Cyber City, Block 1A, Mount Poonamallee Road, Manapakkam, Chennai 600089",
                13.0312, 80.1655, 1690, "5,000+", "Active",
                new() { "Java", "Spring Cloud", "React", "Kafka", "Python", "Hadoop" },
                new() { "Investment Banking", "FinTech", "MNC", "GCC" });

            AddCompany("comp-24", "Citi Chennai Technology Center", "citi-chennai", "Global Markets, Treasury & Enterprise Wealth Tech Hub",
                "Citi's Chennai software technology center engineers treasury trade workflows, consumer finance, and real-time ledger settlement.",
                "https://www.citigroup.com", "https://jobs.citi.com/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "FinTech" },
                "Porur & DLF Cybercity", "DLF Cybercity, Manapakkam, Chennai 600089",
                13.0328, 80.1662, 1812, "5,000+", "Active",
                new() { "Java", "C#", "React", "Python", "Oracle", "Cloud Microservices" },
                new() { "FinTech", "MNC", "Global Banking" });

            AddCompany("comp-25", "Renault Nissan Technology & Business Centre India", "rntbci", "Automotive Embedded Software & Connected Vehicle Center",
                "RNTBCI Chennai serves as the global engineering backbone for Renault and Nissan alliance, engineering ECUs, telematics, and CAD.",
                "https://www.rntbci.com", "https://www.rntbci.com/careers/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Automotive Tech & EV", "Manufacturing & Industrial Tech" },
                "Siruseri (SIPCOT IT Park)", "Ascendas Mahindra World City & SIPCOT IT Park, Siruseri, Chennai 603103",
                12.8295, 80.2212, 2007, "8,000+", "Hiring Surge",
                new() { "C", "C++", "MATLAB", "AUTOSAR", "Python", "Cybersecurity" },
                new() { "Automotive", "EV Engineering", "MNC", "GCC" });

            AddCompany("comp-26", "Siemens Healthineers Chennai", "siemens-healthineers", "Medical Imaging, Ultrasound & Healthcare AI GCC",
                "Siemens Healthineers Chennai engineers ultrasound imaging algorithms, diagnostic CT/MRI scanners, and hospital cloud systems.",
                "https://www.siemens-healthineers.com", "https://www.siemens-healthineers.com/careers",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "HealthTech", "DeepTech & AI" },
                "OMR (IT Corridor)", "RMZ Millenia Business Park, Campus 2, MGR Salai, Kandanchavadi, Perungudi, Chennai 600096",
                12.9692, 80.2478, 1847, "1,000+", "Active",
                new() { "C++", "C#", ".NET", "Python", "DICOM", "CUDA", "Medical AI" },
                new() { "MedTech", "Medical Imaging", "MNC", "GCC" });

            AddCompany("comp-27", "Trimble Information Technologies", "trimble-chennai", "Geospatial, Construction Software & IoT Hardware Hub",
                "Trimble's major Chennai R&D campus builds Tekla Structures, geospatial mapping devices, and agricultural precision automation.",
                "https://www.trimble.com", "https://careers.trimble.com/",
                new() { "MNC", "GCC", "PRODUCT COMPANY" }, new() { "SaaS / Enterprise Software", "Manufacturing & Industrial Tech" },
                "Taramani (Tidel Park & Ascendas)", "TIDEL Park, Module 402, 4th Floor, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9895, 80.2492, 1978, "1,500+", "Active",
                new() { "C++", "C#", ".NET", "React", "Python", "Computer Vision" },
                new() { "Geospatial", "Civil Engineering", "BIM Software", "MNC" });

            AddCompany("comp-28", "Verizon Data Services India", "verizon-chennai", "5G Edge Cloud, Telecommunications & Network Automation GCC",
                "Verizon India Chennai engineers SDN networks, 5G MEC platforms, enterprise fiber orchestration, and streaming network stacks.",
                "https://www.verizon.com", "https://www.verizon.com/about/careers",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "SaaS / Enterprise Software", "DeepTech & AI" },
                "Guindy (SIDCO / Olympia)", "Olympia Technology Park, 1 Sidco Industrial Estate, Guindy, Chennai 600032",
                13.0115, 80.2085, 2000, "5,000+", "Active",
                new() { "Java", "Python", "Kubernetes", "Go", "AWS", "5G Network" },
                new() { "Telecom", "5G", "MNC", "GCC" });

            AddCompany("comp-29", "Alstom Transport India", "alstom-chennai", "High-Speed Rail Signaling, Metro Fleet Automation & Rolling Stock R&D",
                "Alstom's Chennai Engineering Centre develops computer-based train control (CBTC), metro signaling, and propulsion for Vande Bharat & global rail.",
                "https://www.alstom.com", "https://jobsearch.alstom.com/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Manufacturing & Industrial Tech", "Automotive Tech & EV" },
                "Taramani (Tidel Park & Ascendas)", "Ascendas International Tech Park, Taramani, Chennai 600113",
                12.9862, 80.2455, 1928, "2,000+", "Active",
                new() { "C", "C++", "Ada", "Safety Critical Systems", "MATLAB", "Embedded" },
                new() { "Railways", "Signaling", "Green Mobility", "MNC" });

            AddCompany("comp-30", "Qualcomm India", "qualcomm-chennai", "Wireless Connectivity, WiFi 7 & 6G Semiconductor R&D",
                "Qualcomm Chennai engineers next-generation Wi-Fi chips, cellular modem firmware, automotive cockpit silicon, and Bluetooth stacks.",
                "https://www.qualcomm.com", "https://qualcomm.wd5.myworkdayjobs.com/External",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "DeepTech & AI", "Manufacturing & Industrial Tech" },
                "Taramani (Tidel Park & Ascendas)", "Ramanujan IT City, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9875, 80.2468, 1985, "1,000+", "Hiring Surge",
                new() { "C", "C++", "Verilog", "VHDL", "Linux Kernel", "Semiconductor" },
                new() { "Semiconductors", "Wireless", "Silicon", "MNC" });

            // ==========================================
            // 3. DEEPTECH, SPACE, AI & ROBOTICS
            // ==========================================
            AddCompany("comp-31", "Agnikul Cosmos", "agnikul", "Private Space Launch Vehicles & 3D Printed Rocket Engines",
                "Agnikul Cosmos designs, manufactures, and launches orbital launch vehicles from India. Pioneered Agnibaan powered by single-piece 3D printed rocket engines.",
                "https://agnikul.in", "https://agnikul.in/#/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "DeepTech & AI", "Manufacturing & Industrial Tech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Kanagam Road, Taramani, Chennai 600113",
                12.9912, 80.2428, 2017, "100-250", "Hiring Surge",
                new() { "C++", "Python", "MATLAB", "CFD", "3D Printing", "Avionics" },
                new() { "SpaceTech", "Aerospace", "DeepTech", "IIT Madras" });

            AddCompany("comp-32", "The ePlane Company", "eplane", "Electric Flying Taxis & Urban Air Mobility eVTOLs",
                "The ePlane Company builds compact, quiet, electric vertical takeoff and landing (eVTOL) aircraft for intra-city passenger commutes and emergency response.",
                "https://eplane.ai", "https://eplane.ai/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "Automotive Tech & EV", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9915, 80.2432, 2019, "50-100", "Active",
                new() { "C++", "Python", "Flight Dynamics", "Battery Tech", "Aerodynamics" },
                new() { "eVTOL", "Aviation", "DeepTech", "IIT Madras" });

            AddCompany("comp-33", "Detect Technologies", "detect-technologies", "Industrial AI, Computer Vision & Autonomous Asset Inspection",
                "Detect Technologies builds real-time computer vision AI and drone sensing platforms monitoring safety and integrity across oil & gas and heavy industry.",
                "https://detecttechnologies.com", "https://detecttechnologies.com/careers/",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "DeepTech & AI", "Manufacturing & Industrial Tech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Module 2B, Taramani, Chennai 600113",
                12.9918, 80.2435, 2016, "200-500", "Active",
                new() { "Python", "PyTorch", "OpenCV", "TensorFlow", "React", "Docker" },
                new() { "Computer Vision", "Industrial AI", "Drones", "IIT Madras" });

            AddCompany("comp-34", "Mad Street Den (Vue.ai)", "mad-street-den", "Computer Vision & Enterprise Neuro-Symbolic AI",
                "Mad Street Den's flagship product Vue.ai transforms global retail and enterprise operations using artificial intelligence, vision, and NLP.",
                "https://www.madstreetden.com", "https://www.madstreetden.com/careers/",
                new() { "PRODUCT COMPANY", "STARTUP", "SAAS" }, new() { "DeepTech & AI", "SaaS / Enterprise Software" },
                "Taramani (Tidel Park & Ascendas)", "TIDEL Park, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9898, 80.2496, 2013, "200-500", "Active",
                new() { "Python", "PyTorch", "Kubernetes", "React", "FastAPI", "Computer Vision" },
                new() { "Artificial Intelligence", "Retail AI", "Neuro-Symbolic" });

            AddCompany("comp-35", "Mindgrove Technologies", "mindgrove", "High-Performance Edge AI Microcontrollers & RISC-V Silicon",
                "Mindgrove Technologies designs indigenous, cost-efficient, secure SoC chips on RISC-V architecture (Secure IoT, Vision) for global hardware innovators.",
                "https://mindgrovetech.in", "https://mindgrovetech.in/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "DeepTech & AI", "Manufacturing & Industrial Tech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Kanagam Road, Taramani, Chennai 600113",
                12.9910, 80.2425, 2021, "20-50", "Active",
                new() { "RISC-V", "Verilog", "SystemVerilog", "Embedded C", "SoC Design" },
                new() { "Semiconductor", "RISC-V", "Fabless Silicon", "IIT Madras" });

            AddCompany("comp-36", "Planys Technologies", "planys-tech", "Underwater Robotics & Marine Infrastructure Inspection",
                "Planys designs remotely operated underwater robotic vehicles (ROVs) equipped with acoustic NDT sensors to inspect dams, ports, and offshore energy.",
                "https://planystech.com", "https://planystech.com/careers/",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "Manufacturing & Industrial Tech", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9922, 80.2438, 2015, "50-100", "Active",
                new() { "ROS", "C++", "Python", "Embedded Robotics", "Sonar Analysis" },
                new() { "Underwater Robotics", "Marine Tech", "DeepTech" });

            AddCompany("comp-37", "Uniphore", "uniphore", "Conversational AI & Multimodal Enterprise Intelligence",
                "Uniphore combines conversational AI, emotion analytics, and speech recognition to automate enterprise customer contact centers globally.",
                "https://www.uniphore.com", "https://www.uniphore.com/careers/",
                new() { "PRODUCT COMPANY", "SAAS", "STARTUP" }, new() { "DeepTech & AI", "SaaS / Enterprise Software" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9914, 80.2430, 2008, "1,000+", "Active",
                new() { "Python", "C++", "PyTorch", "Speech-to-Text", "Kafka", "AWS" },
                new() { "Conversational AI", "Speech AI", "Unicorn", "Enterprise" });

            // ==========================================
            // 4. FINTECH, BFSI & WEALTH TECH
            // ==========================================
            AddCompany("comp-38", "BankBazaar", "bankbazaar", "Fintech Marketplace for Credit Cards & Loans",
                "BankBazaar is India's pioneer digital credit co-brand platform connecting millions of consumers with credit cards, loans, and credit score monitoring.",
                "https://www.bankbazaar.com", "https://www.bankbazaar.com/careers.html",
                new() { "PRODUCT COMPANY", "FINTECH", "STARTUP" }, new() { "FinTech" },
                "OMR (IT Corridor)", "Prince Infocity II, 283/3, Rajiv Gandhi Salai, Kandanchavadi, Chennai 600096",
                12.9642, 80.2452, 2008, "1,000+", "Active",
                new() { "Java", "Python", "React", "AWS", "MySQL", "Fintech APIs" },
                new() { "FinTech", "Credit Score", "Loans", "Consumer Tech" });

            AddCompany("comp-39", "M2P Fintech", "m2p-fintech", "API Infrastructure for Banking, Cards & Financial Services",
                "M2P is Asia's leading API banking and card issuance infrastructure company, empowering banks, fintechs, and NBFCs across 20+ countries.",
                "https://m2pfintech.com", "https://m2pfintech.com/careers/",
                new() { "PRODUCT COMPANY", "FINTECH", "STARTUP" }, new() { "FinTech", "SaaS / Enterprise Software" },
                "Guindy (SIDCO / Olympia)", "Plot No. 10, Guindy Industrial Estate, SIDCO, Chennai 600032",
                13.0132, 80.2078, 2014, "500-1,000", "Hiring Surge",
                new() { "Java", "Go", "Node.js", "PostgreSQL", "Kafka", "Docker" },
                new() { "API Banking", "FinTech", "Card Issuance", "Unicorn" });

            AddCompany("comp-40", "Kaleidofin", "kaleidofin", "Financial Solutions Platform for Informal Economy Households",
                "Kaleidofin provides credit analytics, tailored savings, and micro-insurance to millions of underbanked consumers in India.",
                "https://kaleidofin.com", "https://kaleidofin.com/careers/",
                new() { "PRODUCT COMPANY", "FINTECH", "STARTUP" }, new() { "FinTech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9908, 80.2429, 2017, "100-250", "Active",
                new() { "Python", "Django", "React", "PostgreSQL", "AWS" },
                new() { "Financial Inclusion", "FinTech", "Credit AI" });

            AddCompany("comp-41", "Financial Software & Systems (FSS)", "fss-technologies", "Global Payments Technology & ATM Switch Software",
                "FSS powers payments processing, merchant acquirers, debit card issuance, and core ATM switches for major banks across 50+ countries.",
                "https://www.fsstechnologies.com", "https://www.fsstechnologies.com/careers/",
                new() { "PRODUCT COMPANY", "ENTERPRISE", "FINTECH" }, new() { "FinTech", "IT Services & Consulting" },
                "Siruseri (SIPCOT IT Park)", "Plot No. G4, SIPCOT IT Park, Siruseri, Chennai 603103",
                12.8272, 80.2205, 1991, "2,500+", "Active",
                new() { "C", "C++", "Java", "Oracle", "Switch Architecture", "PCI DSS" },
                new() { "Payments Switch", "FinTech", "Cards", "Banking" });

            // ==========================================
            // 5. IT SERVICES, CONSULTING & INDIAN GIANTS
            // ==========================================
            AddCompany("comp-42", "Tata Consultancy Services (TCS) Chennai", "tcs-chennai", "Global Technology Services & Digital Transformation Powerhouse",
                "TCS operates its largest delivery footprint in Chennai with campuses across Siruseri (one of Asia's largest IT parks) and Sholinganallur.",
                "https://www.tcs.com", "https://www.tcs.com/careers",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Consulting", "SaaS / Enterprise Software" },
                "Siruseri (SIPCOT IT Park)", "TCS Siruseri Campus, 1/1G, SIPCOT IT Park, Siruseri, Chennai 603103",
                12.8312, 80.2185, 1968, "50,000+", "Hiring Surge",
                new() { "Java", "Python", ".NET", "React", "AWS", "Azure", "Cloud" },
                new() { "IT Services", "Consulting", "Enterprise Tech", "Fortune 500" });

            AddCompany("comp-43", "Cognizant Technology Solutions", "cognizant-chennai", "Enterprise Cloud, Modernization & Business Transformation Titan",
                "Chennai is Cognizant's largest operational base with mega campuses across MEPZ Tambaram, OMR Sholinganallur, and DLF Cybercity.",
                "https://www.cognizant.com", "https://careers.cognizant.com/global/en",
                new() { "MNC", "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Consulting", "DeepTech & AI" },
                "OMR (IT Corridor)", "Cognizant TCO Campus, 5/535 Old Mahabalipuram Road, Thoraipakkam, Chennai 600096",
                12.9352, 80.2325, 1994, "50,000+", "Hiring Surge",
                new() { "Java", ".NET", "Python", "Angular", "Salesforce", "Snowflake" },
                new() { "IT Services", "Enterprise Cloud", "AI Services" });

            AddCompany("comp-44", "Infosys Chennai", "infosys-chennai", "Digital Services, Next-Gen Cloud & Enterprise Architecture",
                "Infosys Chennai operates landmark campuses inside Mahindra World City and Sholinganallur OMR, servicing banking and aerospace worldwide.",
                "https://www.infosys.com", "https://www.infosys.com/careers.html",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Consulting", "DeepTech & AI" },
                "OMR (IT Corridor)", "Infosys Campus, Rajiv Gandhi Salai, Sholinganallur, Chennai 600119",
                12.9025, 80.2295, 1981, "25,000+", "Active",
                new() { "Java", "C#", "React", "Python", "Azure", "Kubernetes" },
                new() { "IT Services", "Digital Platforms", "Top Employer" });

            AddCompany("comp-45", "HCL Technologies", "hcltech-chennai", "Digital Engineering, Cloud Native Architecture & Enterprise Solutions",
                "HCLTech operates state-of-the-art software R&D centers across Sholinganallur OMR and Ambattur Industrial Estate.",
                "https://www.hcltech.com", "https://www.hcltech.com/careers",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Consulting", "DeepTech & AI" },
                "OMR (IT Corridor)", "HCL Technologies, 138 Rajiv Gandhi Salai, Sholinganallur, Chennai 600119",
                12.9052, 80.2288, 1976, "20,000+", "Hiring Surge",
                new() { "Java", ".NET", "C++", "React", "SAP", "Cloud Infrastructure" },
                new() { "IT Services", "Product Engineering", "Global Tech" });

            AddCompany("comp-46", "Wipro Limited Chennai", "wipro-chennai", "Cognitive Computing, Hyper-Automation & Digital Services",
                "Wipro's Chennai campus at Sholinganallur delivers cloud architecture, cybersecurity services, and enterprise engineering globally.",
                "https://www.wipro.com", "https://careers.wipro.com/",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Consulting" },
                "OMR (IT Corridor)", "Wipro Campus, 105 Rajiv Gandhi Salai, Sholinganallur, Chennai 600119",
                12.9038, 80.2291, 1945, "15,000+", "Active",
                new() { "Java", "Spring", "Python", "Angular", "Cybersecurity", "Cloud" },
                new() { "IT Services", "Consulting", "Enterprise" });

            AddCompany("comp-47", "LTI Mindtree Chennai", "lti-mindtree", "Digital Transformation, Enterprise Solutions & Cloud Platforms",
                "LTIMindtree operates multiple delivery centers across Chennai specializing in digital integration, insurance tech, and retail logistics.",
                "https://www.ltimindtree.com", "https://www.ltimindtree.com/careers/",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Consulting" },
                "Porur & DLF Cybercity", "DLF Cybercity, Block 5, Manapakkam, Porur, Chennai 600089",
                13.0318, 80.1659, 1996, "10,000+", "Active",
                new() { "Java", ".NET", "Python", "React", "Snowflake", "Data Engineering" },
                new() { "IT Services", "Enterprise Cloud", "Consulting" });

            AddCompany("comp-48", "Hexaware Technologies", "hexaware", "AI-Led Digital Transformation, Automation & Cloud Migration",
                "Hexaware operates major technology delivery campuses across Siruseri SIPCOT IT Park, pioneering automation-first IT services.",
                "https://hexaware.com", "https://jobs.hexaware.com/",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Consulting", "DeepTech & AI" },
                "Siruseri (SIPCOT IT Park)", "Plot No. H5, SIPCOT IT Park, Navallur Post, Siruseri, Chennai 603103",
                12.8265, 80.2215, 1990, "15,000+", "Hiring Surge",
                new() { "Java", ".NET", "Python", "Cloud Migration", "GenAI", "React" },
                new() { "Automation", "IT Services", "Cloud" });

            AddCompany("comp-49", "Aspire Systems", "aspire-systems", "Global Technology Services & Product Engineering Partner",
                "Headquartered in Siruseri SIPCOT, Aspire Systems specializes in software product engineering, digital retail, and banking tech.",
                "https://www.aspiresys.com", "https://www.aspiresys.com/careers/",
                new() { "ENTERPRISE", "IT SERVICES", "PRODUCT COMPANY" }, new() { "IT Services & Consulting", "SaaS / Enterprise Software" },
                "Siruseri (SIPCOT IT Park)", "1/D-1, SIPCOT IT Park, Siruseri, Chennai 603103",
                12.8258, 80.2198, 1996, "4,000+", "Active",
                new() { "Java", ".NET", "Angular", "React", "Testing Automation", "AWS" },
                new() { "Product Engineering", "IT Services", "Chennai HQ" });

            // ==========================================
            // 6. HEALTHTECH & EDTECH
            // ==========================================
            AddCompany("comp-50", "Apollo 24|7 (Apollo Hospitals Digital)", "apollo-247", "India's Premier Digital Healthcare & Telemedicine Platform",
                "Apollo 24|7 is the consumer health-tech arm of Apollo Hospitals, delivering digital consults, diagnostic booking, and e-pharmacy.",
                "https://www.apollo247.com", "https://www.apollo247.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "HealthTech", "E-Commerce & Retail Tech" },
                "Guindy (SIDCO / Olympia)", "Olympia Platina, Guindy Industrial Estate, Chennai 600032",
                13.0122, 80.2089, 2020, "1,000+", "Active",
                new() { "React Native", "Node.js", "Python", "AWS", "PostgreSQL", "Microservices" },
                new() { "HealthTech", "Telemedicine", "Digital Health" });

            AddCompany("comp-51", "GUVI Geek Network", "guvi", "Vernacular Tech Learning & Developer Upskilling Platform",
                "GUVI (an HCL company) teaches programming, cloud, and data science in vernacular languages (Tamil, Hindi, Telugu), upskilling 3M+ developers.",
                "https://www.guvi.in", "https://www.guvi.in/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "EdTech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9916, 80.2431, 2014, "200-500", "Active",
                new() { "Python", "React", "Node.js", "AWS", "MongoDB" },
                new() { "EdTech", "Vernacular Learning", "IIT Madras Incubation" });

            AddCompany("comp-52", "Skill-Lync", "skill-lync", "Advanced Engineering Upskilling & Industry Simulation Edtech",
                "Skill-Lync equips mechanical, electrical, and computer science engineers with practical simulation coursework (EV, Autonomous Vehicles).",
                "https://skill-lync.com", "https://skill-lync.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "EdTech", "Automotive Tech & EV" },
                "Guindy (SIDCO / Olympia)", "BAID Hi-Tech Park, Thoraipakkam OMR / Guindy, Chennai",
                12.9412, 80.2356, 2015, "500-1,000", "Active",
                new() { "Python", "React", "Django", "ANSYS", "CAD Simulation" },
                new() { "EdTech", "Core Engineering", "EV Training" });

            // ==========================================
            // 7. AUTOMOTIVE, EV & CLEAN MOBILITY TECH
            // ==========================================
            AddCompany("comp-53", "Ather Energy Chennai R&D Center", "ather-energy-chennai", "EV Battery Pack, Telematics & Software Hub",
                "Ather Energy's Chennai software and battery testing labs engineer dashboard Linux OS, smart charging algorithms, and cloud fleet telemetry.",
                "https://www.atherenergy.com", "https://www.atherenergy.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "Automotive Tech & EV", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9911, 80.2427, 2013, "1,000+", "Active",
                new() { "C++", "Python", "Android Automotive", "Embedded Linux", "IoT", "AWS" },
                new() { "Electric Vehicles", "Smart Mobility", "CleanTech" });

            AddCompany("comp-54", "TVS Motor Digital & Connected Mobility", "tvs-motor-digital", "Smart Two-Wheeler Infotainment & Connected Telematics",
                "TVS Motor Company's digital innovation teams build TVS SmartXonnect, electric powertrain telemetry, and connected scooter mobile apps.",
                "https://www.tvsmotor.com", "https://www.tvsmotor.com/careers",
                new() { "ENTERPRISE", "PRODUCT COMPANY" }, new() { "Automotive Tech & EV", "DeepTech & AI" },
                "Guindy (SIDCO / Olympia)", "Harita House, Anna Salai & Guindy Tech Hub, Chennai 600032",
                13.0135, 80.2105, 1978, "5,000+", "Hiring Surge",
                new() { "Flutter", "Kotlin", "Embedded C", "Python", "IoT", "Azure" },
                new() { "EV", "Automotive", "Connected Mobility" });

            AddCompany("comp-55", "Raptee Energy", "raptee-energy", "High-Voltage Electric Motorcycles with On-board Intelligence",
                "Raptee Energy manufactures premium electric motorcycles featuring CCS2 car-charger compatibility and integrated vehicle software.",
                "https://raptee.com", "https://raptee.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "Automotive Tech & EV" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9919, 80.2436, 2019, "50-100", "Active",
                new() { "Embedded C", "Python", "CAN Bus", "Battery Management", "IoT" },
                new() { "Electric Motorcycles", "CleanTech", "IIT Madras" });

            // ==========================================
            // 8. E-COMMERCE, CONSUMER TECH & LOGISTICS
            // ==========================================
            AddCompany("comp-56", "Matrimony.com", "matrimony-com", "India's Pioneer Matchmaking & Consumer Internet Company",
                "Matrimony.com is a publicly listed consumer internet giant running BharatMatrimony, CommunityMatrimony, and WeddingBazaar.",
                "https://www.matrimony.com", "https://www.matrimony.com/careers",
                new() { "PRODUCT COMPANY", "ENTERPRISE" }, new() { "E-Commerce & Retail Tech", "DeepTech & AI" },
                "Perungudi & Kandanchavadi", "No. 94 TVH Beliciaa Towers, Tower II, MRC Nagar / Santhome Salai, Chennai",
                12.9655, 80.2472, 1997, "3,000+", "Active",
                new() { "PHP", "Java", "Python", "React", "AI Matchmaking", "MySQL" },
                new() { "Consumer Internet", "Public Listed", "Matchmaking" });

            AddCompany("comp-57", "CaratLane (A Tanishq Partnership)", "caratlane", "Omnichannel Fine Jewelry & AR Virtual Try-On Tech",
                "CaratLane is India's leading omnichannel fine jewelry technology brand with cutting-edge 3D CAD visualization and digital retail.",
                "https://www.caratlane.com", "https://www.caratlane.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "E-Commerce & Retail Tech" },
                "OMR (IT Corridor)", "Rutland Gate 4th Street, Nungambakkam & OMR Hub, Chennai",
                12.9785, 80.2415, 2008, "1,000+", "Active",
                new() { "Node.js", "React", "Python", "Three.js", "AWS", "PostgreSQL" },
                new() { "Omnichannel", "Retail Tech", "E-Commerce" });

            AddCompany("comp-58", "WayCool Foods & Products", "waycool", "Agri-Commerce Supply Chain & Intelligent Logistics Tech",
                "WayCool operates a next-generation tech-enabled food and agricultural supply chain connecting 100k+ farmers with retail.",
                "https://waycool.in", "https://waycool.in/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "E-Commerce & Retail Tech", "DeepTech & AI" },
                "Guindy (SIDCO / Olympia)", "Guindy Industrial Estate, Chennai 600032",
                13.0128, 80.2095, 2015, "1,500+", "Active",
                new() { "Python", "React", "Go", "IoT Cold Chain", "AWS" },
                new() { "AgriTech", "Supply Chain", "B2B Commerce" });

            AddCompany("comp-59", "Pickyourtrail", "pickyourtrail", "DIY Global Vacation Planning & Travel FinTech Platform",
                "Pickyourtrail provides a real-time customized international holiday booking platform with algorithmic itinerary construction.",
                "https://pickyourtrail.com", "https://pickyourtrail.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "E-Commerce & Retail Tech" },
                "Guindy (SIDCO / Olympia)", "Olympia Platina, Guindy Industrial Estate, Chennai 600032",
                13.0119, 80.2082, 2014, "200-500", "Active",
                new() { "Node.js", "React", "Python", "MongoDB", "AWS" },
                new() { "TravelTech", "E-Commerce", "Consumer Tech" });

            AddCompany("comp-60", "Sulekha.com", "sulekha", "Digital Local Services Matchmaking Platform",
                "Sulekha connects urban consumers with verified local service providers across home improvement, vocational training, and events.",
                "https://www.sulekha.com", "https://www.sulekha.com/careers",
                new() { "PRODUCT COMPANY", "ENTERPRISE" }, new() { "E-Commerce & Retail Tech" },
                "Taramani (Tidel Park & Ascendas)", "Ramanujan IT City, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9882, 80.2471, 2007, "500-1,000", "Active",
                new() { "Java", "Python", "React", "Solr", "AWS", "MySQL" },
                new() { "Local Services", "Consumer Tech", "Marketplace" });

            return list;
        }
    }
}
