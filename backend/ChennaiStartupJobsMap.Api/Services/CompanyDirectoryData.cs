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

            // ==========================================
            // 9. IT SERVICES, CONSULTING & TRANSFORMATION
            // ==========================================
            AddCompany("comp-61", "Cognizant Technology Solutions", "cognizant", "Global IT Services & Digital Engineering Pioneer",
                "Cognizant is an American multinational IT services giant with its largest global operational hub, delivery centers, and campuses in Chennai.",
                "https://www.cognizant.com", "https://careers.cognizant.com/global/en",
                new() { "MNC", "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation", "DeepTech & AI" },
                "OMR (IT Corridor)", "5/535 Old Mahabalipuram Road, Thoraipakkam & MEPZ SEZ, Chennai 600097",
                12.9412, 80.2378, 1994, "50,000+", "Hiring Surge",
                new() { "Java", "Python", "React", "Cloud", "Snowflake", "Azure", "AWS", "AI/ML" },
                new() { "IT Services", "Digital Engineering", "Fortune 500", "Cloud" });

            AddCompany("comp-62", "Tata Consultancy Services (TCS)", "tcs-chennai", "India's Flagship Global Tech Consulting Titan",
                "TCS Siruseri is the company's iconic butterfly campus housing over 25,000 engineers developing banking, aerospace, and AI platforms.",
                "https://www.tcs.com", "https://www.tcs.com/careers",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation", "DeepTech & AI" },
                "Siruseri (SIPCOT IT Park)", "SIPCOT IT Park, Siruseri, Old Mahabalipuram Road, Chennai 603103",
                12.8315, 80.2225, 1968, "50,000+", "Hiring Surge",
                new() { "Java", "Spring Boot", "Python", "Angular", "React", "AWS", "SAP", "Kubernetes" },
                new() { "IT Services", "Consulting", "Enterprise IT", "Siruseri" });

            AddCompany("comp-63", "Infosys Chennai", "infosys-chennai", "Next-Generation Digital Services & Consulting",
                "Infosys operates major development campuses in Sholinganallur and Mahindra World City, delivering enterprise digital core transformations.",
                "https://www.infosys.com", "https://www.infosys.com/careers.html",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation", "DeepTech & AI" },
                "OMR (IT Corridor)", "138 Old Mahabalipuram Road, Sholinganallur, Chennai 600119",
                12.9038, 80.2289, 1981, "25,000+", "Active",
                new() { "Java", ".NET", "Python", "Cloud", "React", "Oracle", "DevOps" },
                new() { "Digital Transformation", "IT Services", "Cloud" });

            AddCompany("comp-64", "Wipro Technologies Chennai", "wipro-chennai", "Cognitive Computing & Enterprise Cloud Solutions",
                "Wipro's Sholinganallur Center of Excellence drives enterprise cybersecurity, artificial intelligence, and cloud migrations.",
                "https://www.wipro.com", "https://careers.wipro.com/",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation" },
                "OMR (IT Corridor)", "ELCOT SEZ, Sholinganallur, Rajiv Gandhi Salai, Chennai 600119",
                12.9002, 80.2278, 1945, "20,000+", "Active",
                new() { "Java", "Python", "Azure", "React", "Salesforce", "Kubernetes" },
                new() { "IT Services", "Cloud Migration", "Enterprise" });

            AddCompany("comp-65", "HCLTech Chennai", "hcltech-chennai", "Supercharging Progress with Engineering R&D & Digital Tech",
                "HCLTech operates extensive innovation centers in Navalur, Sholinganallur, and Ambattur engineering hardware, chips, and enterprise software.",
                "https://www.hcltech.com", "https://www.hcltech.com/careers",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation", "DeepTech & AI" },
                "OMR (IT Corridor)", "ETA Techno Park, Navalur, Rajiv Gandhi Salai, Chennai 603103",
                12.8488, 80.2255, 1976, "25,000+", "Hiring Surge",
                new() { "C++", "Java", "Embedded Systems", "AWS", "Python", "IoT", "Cybersecurity" },
                new() { "Engineering Services", "R&D", "Cloud", "IT Services" });

            AddCompany("comp-66", "LTIMindtree Chennai", "ltimindtree", "Engineering at the Intersection of Physical & Digital Worlds",
                "LTIMindtree's Chennai development center at L&T Technology Park Manapakkam builds digital platforms, supply chain intelligence, and SaaS.",
                "https://www.ltimindtree.com", "https://www.ltimindtree.com/careers/",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation" },
                "Porur & DLF Cybercity", "L&T Technology Center, TC-1 Building, Mount Poonamallee Road, Manapakkam, Chennai 600089",
                13.0298, 80.1638, 1996, "15,000+", "Active",
                new() { "Java", "Microservices", "React", "Azure", "Snowflake", "Databricks" },
                new() { "Digital Consulting", "Data Engineering", "Enterprise" });

            AddCompany("comp-67", "Hexaware Technologies", "hexaware", "Automate Everything, Cloudify Everything, Transform Customer Experiences",
                "Hexaware's state-of-the-art campus in Siruseri SIPCOT IT Park powers automated enterprise cloud migration, GenAI, and QA automation.",
                "https://hexaware.com", "https://hexaware.com/careers/",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation", "DeepTech & AI" },
                "Siruseri (SIPCOT IT Park)", "H5, SIPCOT IT Park, Siruseri, Navalur Post, Chennai 603103",
                12.8335, 80.2238, 1990, "10,000+", "Active",
                new() { "Java", "Python", "Generative AI", "React", "AWS", "Azure", "Selenium" },
                new() { "Cloud Migration", "AI Automation", "IT Services" });

            AddCompany("comp-68", "Prodapt Solutions", "prodapt", "Connected Ecosystem Accelerator for DSPs and Digital Brands",
                "Prodapt is a global consulting and technology provider focused exclusively on telecommunications, digital media, and hyper-scale tech.",
                "https://prodapt.com", "https://prodapt.com/careers",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation", "SaaS / Enterprise Software" },
                "OMR (IT Corridor)", "Prince Info City II, 283/4 Rajiv Gandhi Salai, Kandanchavadi, Chennai 600096",
                12.9645, 80.2465, 1999, "5,000+", "Active",
                new() { "Java", "Python", "5G OpenRAN", "React", "Cloud Orchestration", "AWS" },
                new() { "Telecom Software", "5G", "Cloud", "DSP" });

            AddCompany("comp-69", "Aspire Systems", "aspire-systems", "Product Engineering & Software Craftsmanship Partner",
                "Aspire Systems specializes in enterprise product engineering, digital experience solutions, and autonomous testing for global ISVs.",
                "https://www.aspiresys.com", "https://www.aspiresys.com/careers/",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation" },
                "Siruseri (SIPCOT IT Park)", "1/D-1, SIPCOT IT Park, Siruseri, Chennai 603103",
                12.8275, 80.2185, 1996, "4,000+", "Active",
                new() { "Java", ".NET", "React", "Angular", "Python", "AWS", "Flutter" },
                new() { "Product Engineering", "Software Craftsmanship", "Fintech" });

            AddCompany("comp-70", "Virtusa Corporation", "virtusa", "Digital Business Transformation and Core Banking Modernization",
                "Virtusa's Chennai engineering centers build digital engineering solutions for Fortune 500 banks, insurance firms, and healthcare organizations.",
                "https://www.virtusa.com", "https://www.virtusa.com/careers",
                new() { "MNC", "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation", "FinTech" },
                "OMR (IT Corridor)", "34 IT Highway, Navalur, Rajiv Gandhi Salai, Chennai 603103",
                12.8465, 80.2262, 1996, "8,000+", "Active",
                new() { "Java", "Spring Boot", "React", "Microservices", "GCP", "Kubernetes" },
                new() { "Banking Modernization", "Digital Engineering", "FinTech" });

            AddCompany("comp-71", "Movate (formerly CSS Corp)", "movate", "Digital Technology & Customer Experience Transformation",
                "Movate creates human-centric tech services leveraging generative AI, automated IT infrastructure services, and customer experience ops.",
                "https://www.movate.com", "https://www.movate.com/careers/",
                new() { "ENTERPRISE", "IT SERVICES" }, new() { "IT Services & Digital Transformation" },
                "Ambattur Industrial Estate", "Ambit IT Park, Ambattur Industrial Estate, Chennai 600058",
                13.0885, 80.1612, 1996, "5,000+", "Active",
                new() { "Python", "Node.js", "React", "AI/ML", "Cloud Support", "AWS" },
                new() { "Customer Experience", "Cloud Services", "IT Infrastructure" });

            AddCompany("comp-72", "Sify Technologies", "sify-technologies", "Enterprise Cloud, Submarine Cables & Data Center Infrastructure",
                "Sify is India's pioneer telecom and cloud connectivity provider, operating hyper-scale data centers, subsea fiber cables, and network security.",
                "https://www.sifytechnologies.com", "https://www.sifytechnologies.com/careers/",
                new() { "ENTERPRISE", "PRODUCT COMPANY" }, new() { "Cybersecurity", "IT Services & Digital Transformation" },
                "Taramani (Tidel Park & Ascendas)", "Tidel Park, 2nd Floor, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9895, 80.2498, 1995, "4,000+", "Active",
                new() { "Linux", "SD-WAN", "Python", "Kubernetes", "OpenStack", "Network Security" },
                new() { "Data Centers", "Cloud", "Submarine Fiber", "Cybersecurity" });

            AddCompany("comp-73", "Redington Limited", "redington", "Global Technology Supply Chain & Digital Distribution Giant",
                "Redington is a Fortune India 500 company orchestrating end-to-end supply chain tech, cloud distribution, and 3D printing across 38 emerging markets.",
                "https://redingtongroup.com", "https://redingtongroup.com/careers/",
                new() { "ENTERPRISE" }, new() { "Supply Chain & Logistics Tech", "E-Commerce & Retail Tech" },
                "Guindy (SIDCO / Olympia)", "SPL Guindy House, 95 Mount Road, Guindy, Chennai 600032",
                13.0105, 80.2142, 1993, "5,000+", "Active",
                new() { "SAP", "Java", "Python", "Cloud Commerce", "Azure", "Logistics Tech" },
                new() { "Supply Chain", "Tech Distribution", "Cloud Marketplace" });

            // ==========================================
            // 10. FINTECH, BANKING & DEBT PLATFORMS
            // ==========================================
            AddCompany("comp-74", "M2P Fintech", "m2p-fintech", "API Infrastructure Unicorn for Banks and Neo-Fintechs",
                "M2P Fintech is Asia's largest API infrastructure provider enabling banks and fintechs to launch credit cards, core banking, and UPI.",
                "https://m2pfintech.com", "https://m2pfintech.com/careers/",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "FinTech", "DeepTech & AI" },
                "Guindy (SIDCO / Olympia)", "Guindy Industrial Estate, Guindy, Chennai 600032",
                13.0115, 80.2078, 2014, "1,000+", "Hiring Surge",
                new() { "Go", "Java", "React", "Kafka", "PostgreSQL", "AWS", "Fintech APIs" },
                new() { "API Banking", "Cards", "Payments Infrastructure", "Unicorn" });

            AddCompany("comp-75", "Yubi (CredAvenue)", "yubi-credavenue", "Enterprise Debt Marketplace & Credit Infrastructure Platform",
                "Yubi is a fintech unicorn revolutionizing enterprise debt markets, co-lending, and bond issuance via high-throughput matching algorithms.",
                "https://go-yubi.com", "https://go-yubi.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "FinTech" },
                "Guindy (SIDCO / Olympia)", "Prestige Polygon, Anna Salai, Rathna Nagar, Chennai 600035",
                13.0335, 80.2415, 2020, "1,000+", "Active",
                new() { "Python", "Node.js", "React", "Kafka", "AWS", "FastAPI" },
                new() { "Debt Market", "FinTech", "Credit Infrastructure", "Unicorn" });

            AddCompany("comp-76", "Northern Arc Capital", "northern-arc", "Financial Inclusivity & Digital Lending Platform",
                "Northern Arc leverages credit modeling, securitization algorithms, and capital marketplace platforms to enable funding for underbanked enterprises.",
                "https://www.northernarc.com", "https://www.northernarc.com/careers",
                new() { "ENTERPRISE", "PRODUCT COMPANY" }, new() { "FinTech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Kanagam Road, Taramani, Chennai 600113",
                12.9915, 80.2422, 2008, "500+", "Active",
                new() { "Python", "Java", "PostgreSQL", "React", "AWS", "Financial Modeling" },
                new() { "Financial Inclusion", "Credit Modeling", "Lending" });

            AddCompany("comp-77", "Kaleidofin", "kaleidofin", "Inclusive WealthTech Platform for Informal Sector Customers",
                "Kaleidofin is a fintech company tailoring financial solutions combining savings, credit scoring, and insurance for low-income households.",
                "https://kaleidofin.com", "https://kaleidofin.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "FinTech" },
                "Central Chennai / Anna Salai", "Alwarpet, Chennai 600018",
                13.0338, 80.2525, 2017, "200+", "Active",
                new() { "Python", "React Native", "PostgreSQL", "AWS", "Data Science" },
                new() { "Inclusion", "Neo-Banking", "WealthTech" });

            AddCompany("comp-78", "Financial Software and Systems (FSS)", "fssnet", "Global Payment Processing & Core ATM Switch Engine",
                "FSS powers retail payments, secure payment gateways, ATM switching, and merchant digital onboarding across 50+ global tier-1 banks.",
                "https://www.fsstech.com", "https://www.fsstech.com/careers",
                new() { "ENTERPRISE", "PRODUCT COMPANY" }, new() { "FinTech", "Cybersecurity" },
                "OMR (IT Corridor)", "G4, Mount Ponamallee Road & Rajiv Gandhi Salai, Navalur, Chennai 603103",
                12.8472, 80.2268, 1991, "3,000+", "Active",
                new() { "C", "C++", "Java", "Oracle", "Switch Architecture", "PCI-DSS" },
                new() { "Payment Switch", "Card Processing", "FinTech" });

            AddCompany("comp-79", "Computer Age Management Services (CAMS)", "cams-india", "Technology Driver of India's Mutual Fund Industry",
                "CAMS processes over 69% of India's mutual fund industry transactions, operating deep investor recordkeeping and real-time electronic KYC engines.",
                "https://www.camsonline.com", "https://www.camsonline.com/careers",
                new() { "ENTERPRISE", "PRODUCT COMPANY" }, new() { "FinTech" },
                "Central Chennai / Anna Salai", "Rayala Towers, 158 Anna Salai, Chennai 600002",
                13.0612, 80.2615, 1988, "4,000+", "Active",
                new() { "Java", ".NET", "Oracle", "Angular", "Spring Boot", "Microservices" },
                new() { "Mutual Funds", "Capital Markets", "Financial Tech" });

            // ==========================================
            // 11. DEEPTECH, SEMICONDUCTOR & ROBOTICS
            // ==========================================
            AddCompany("comp-80", "Mindgrove Technologies", "mindgrove-tech", "Indigenous RISC-V Fabless Semiconductor Silicon Startup",
                "Mindgrove designs cost-effective, high-performance edge compute RISC-V microcontrollers and System-on-Chips for IoT and automotive sensors.",
                "https://mindgrovetech.in", "https://mindgrovetech.in/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "Semiconductor & Hardware", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Kanagam Road, Taramani, Chennai 600113",
                12.9912, 80.2435, 2021, "50-100", "Active",
                new() { "RISC-V", "Verilog", "C++", "VHDL", "Linux Kernel", "Embedded Systems" },
                new() { "Semiconductors", "SoC", "RISC-V", "IIT Madras" });

            AddCompany("comp-81", "InCore Semiconductors", "incore-semiconductors", "RISC-V Processor Core IP & Configurable SoC Generators",
                "InCore designs customizable RISC-V processor cores, fault-tolerant IP blocks, and automated SoC composition platforms originated at IITM.",
                "https://incoresemi.com", "https://incoresemi.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "Semiconductor & Hardware", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9918, 80.2438, 2018, "50-100", "Active",
                new() { "Bluespec", "Verilog", "Python", "RISC-V", "Compilers", "Silicon Architecture" },
                new() { "Semiconductor IP", "RISC-V", "Processor Architecture" });

            AddCompany("comp-82", "Detect Technologies", "detect-technologies", "AI Computer Vision for Industrial Safety & Autonomous Drones",
                "Detect Technologies deploys real-time video analytics and automated inspection drones for petrochemical plants and heavy industrial facilities.",
                "https://detecttechnologies.com", "https://detecttechnologies.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "Module 2A, 3rd Floor, D Block, IITM Research Park, Taramani, Chennai 600113",
                12.9915, 80.2428, 2016, "200-500", "Active",
                new() { "Python", "PyTorch", "OpenCV", "TensorRT", "React", "ROS", "Drone AI" },
                new() { "Computer Vision", "Industrial AI", "Drones", "IIT Madras" });

            AddCompany("comp-83", "Planys Technologies", "planys-tech", "Underwater Inspection Robotics & Acoustic NDT Marine Systems",
                "Planys manufactures submerged Remotely Operated Vehicles (ROVs) equipped with AI ultrasonic sensors to inspect bridges, dams, and offshore ports.",
                "https://planystech.com", "https://planystech.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "DeepTech & AI", "Manufacturing & Industrial Tech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9922, 80.2441, 2015, "100-250", "Active",
                new() { "Embedded C", "Python", "Robotics", "ROS", "Acoustics", "Hydrodynamics" },
                new() { "Marine Robotics", "Underwater ROV", "DeepTech" });

            AddCompany("comp-84", "Solinas Integrity", "solinas-integrity", "Pipeline Robotics & Sanitation Automation DeepTech",
                "Solinas develops miniature crawlers and autonomous robots for pipeline health monitoring, water loss mitigation, and hazardous cleaning.",
                "https://solinas.in", "https://solinas.in/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "DeepTech & AI", "CleanTech & Renewable Energy" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9914, 80.2432, 2018, "50-100", "Active",
                new() { "Python", "C++", "Robotics", "Embedded Linux", "IoT Sensors" },
                new() { "Robotics", "CleanTech", "Pipeline AI", "Sanitation" });

            AddCompany("comp-85", "Aerostrovilos Energy", "aerostrovilos", "Decentralized Micro Gas Turbines for Clean Energy Generation",
                "Aerostrovilos builds multi-fuel micro gas turbines engineered with IIT Madras combustion research for zero-emission microgrids and heavy EV chargers.",
                "https://aerostrovilos.com", "https://aerostrovilos.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "CleanTech & Renewable Energy", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9917, 80.2435, 2017, "50-100", "Active",
                new() { "Thermodynamics", "Simulink", "CFD", "Embedded Control", "IoT" },
                new() { "CleanTech", "Gas Turbines", "EV Charging", "Clean Power" });

            AddCompany("comp-86", "Tan90 Thermal Solutions", "tan90-thermal", "Thermal Energy Storage & Passive Cold-Chain Logistics Tech",
                "Tan90 develops proprietary Phase Change Material (PCM) portable cold storage units reducing diesel reefer reliance for food and vaccine transport.",
                "https://tan90.in", "https://tan90.in/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "CleanTech & Renewable Energy", "Supply Chain & Logistics Tech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9916, 80.2433, 2019, "50-100", "Active",
                new() { "Thermal Dynamics", "IoT Telematics", "Python", "Cold Chain Tech" },
                new() { "CleanTech", "Thermal Battery", "Cold Chain" });

            AddCompany("comp-87", "XYMA Analytics", "xyma-analytics", "High-Temperature Ultrasonic Waveguide Sensors for Heavy Industry",
                "XYMA produces continuous ultrasonic multipoint waveguide sensors for real-time temperature and viscosity telemetry in steel and refinery furnaces.",
                "https://xyma.in", "https://xyma.in/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "DeepTech & AI", "Manufacturing & Industrial Tech" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9913, 80.2429, 2019, "50-100", "Active",
                new() { "Ultrasonic Signal Processing", "Python", "Edge AI", "Embedded C" },
                new() { "Industrial IoT", "Ultrasonic Sensors", "DeepTech" });

            // ==========================================
            // 12. EDTECH, LEARNING & TALENT PLATFORMS
            // ==========================================
            AddCompany("comp-88", "GUVI Geek Networks", "guvi", "Vernacular Tech Learning & Upskilling Platform (An HCL Group Company)",
                "GUVI provides high-quality programming and software engineering curriculum in vernacular Indian languages, backed by interactive coding sandbox engines.",
                "https://www.guvi.in", "https://www.guvi.in/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "EdTech" },
                "Taramani (Tidel Park & Ascendas)", "IITM Research Park, Kanagam Road, Taramani, Chennai 600113",
                12.9911, 80.2426, 2014, "250-500", "Active",
                new() { "Python", "React", "Node.js", "Docker", "AWS", "Compiler Sandboxes" },
                new() { "EdTech", "Vernacular", "HCL Group", "Coding Sandbox" });

            AddCompany("comp-89", "Skill-Lync", "skill-lync", "Engineering Simulation, EV & Mechanical Upskilling Platform",
                "Skill-Lync trains mechanical, electrical, and computer engineers with industry-grade software tools like ANSYS, MATLAB, and Autonomous Driving stacks.",
                "https://skill-lync.com", "https://skill-lync.com/careers",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "EdTech" },
                "Taramani (Tidel Park & Ascendas)", "BA Continuum / Tidel Park Road, Taramani, Chennai 600113",
                12.9875, 80.2455, 2015, "500-1,000", "Active",
                new() { "Python", "React", "Node.js", "CAD/CAE", "AWS", "PostgreSQL" },
                new() { "EdTech", "Engineering", "Simulation", "Upskilling" });

            AddCompany("comp-90", "Veranda Learning Solutions", "veranda-learning", "Comprehensive Tech-Enabled Exam Prep & Career Training",
                "Veranda Learning operates end-to-end digital test prep platforms across banking, civil services, software coding, and commerce qualifications.",
                "https://www.verandalearning.com", "https://www.verandalearning.com/careers",
                new() { "ENTERPRISE", "PRODUCT COMPANY" }, new() { "EdTech" },
                "Central Chennai / Anna Salai", "Old No. 54, New No. 34, Nungambakkam High Road, Chennai 600034",
                13.0602, 80.2415, 2018, "1,000+", "Active",
                new() { "React", "Node.js", "AWS", "Python", "Video Streaming Architecture" },
                new() { "EdTech", "Public Listed", "Career Training" });

            // ==========================================
            // 13. HEALTHTECH & BIOTECH
            // ==========================================
            AddCompany("comp-91", "Karkinos Healthcare", "karkinos-healthcare", "Decentralized Oncology Network & Cancer Care Platform",
                "Karkinos Healthcare builds digital health platforms for early oncology detection, genomic profiling, and remote patient navigation.",
                "https://karkinos.in", "https://karkinos.in/careers/",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "HealthTech & BioTech", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9918, 80.2432, 2020, "250-500", "Active",
                new() { "Python", "React", "PostgreSQL", "FHIR / HL7", "Genomics", "AWS" },
                new() { "Oncology", "Digital Health", "HealthTech", "Genomics" });

            AddCompany("comp-92", "Apollo TeleHealth & Digital Health", "apollo-telehealth", "Pioneer Telemedicine, Virtual Clinics & Hospital AI",
                "Apollo TeleHealth connects rural and urban patients to doctors via clinical telemedicine, AI symptom triage, and health kiosks.",
                "https://www.apollotelehealth.com", "https://www.apollotelehealth.com/careers",
                new() { "ENTERPRISE", "PRODUCT COMPANY" }, new() { "HealthTech & BioTech" },
                "Central Chennai / Anna Salai", "19 Bishop Gardens, Raja Annamalaipuram & Greams Road, Chennai 600028",
                13.0185, 80.2575, 1999, "2,000+", "Active",
                new() { "WebRTC", "Python", "Java", "React Native", "PostgreSQL", "Medical AI" },
                new() { "Telemedicine", "Digital Health", "Apollo Hospitals" });

            AddCompany("comp-93", "Kauvery Hospitals Digital", "kauvery-digital", "Hospital Information Systems & Smart Clinical Monitoring",
                "Kauvery Hospitals' software engineering wing builds next-gen Electronic Medical Record (EMR) systems, remote ICU monitors, and patient apps.",
                "https://www.kauveryhospital.com", "https://www.kauveryhospital.com/careers/",
                new() { "ENTERPRISE" }, new() { "HealthTech & BioTech" },
                "Central Chennai / Anna Salai", "No. 81, TTK Road, Alwarpet, Chennai 600018",
                13.0365, 80.2512, 1999, "1,000+", "Active",
                new() { "Node.js", "React", "Flutter", "PostgreSQL", "HL7", "AWS" },
                new() { "Hospital Digital", "EMR", "HealthTech" });

            AddCompany("comp-94", "CareStack (GoodX Software)", "carestack", "Cloud Dental Practice Management & Patient Experience Platform",
                "CareStack develops comprehensive cloud software for multi-location dental practices, automating scheduling, billing, and clinical charting.",
                "https://carestack.com", "https://carestack.com/careers",
                new() { "PRODUCT COMPANY", "SAAS" }, new() { "HealthTech & BioTech", "SaaS / Enterprise Software" },
                "Taramani (Tidel Park & Ascendas)", "Ascendas Tech Park, CSIR Road, Taramani, Chennai 600113",
                12.9872, 80.2458, 2015, "500+", "Active",
                new() { "C#", ".NET", "React", "Azure", "Microservices", "SQL Server" },
                new() { "Dental SaaS", "HealthTech", "Cloud Practice" });

            // ==========================================
            // 14. AUTOMOTIVE TECH, EV & CONNECTED MOBILITY
            // ==========================================
            AddCompany("comp-95", "Valeo India Tech Centre", "valeo-chennai", "Autonomous Driving Systems & Thermal EV Management R&D",
                "Valeo's Global Tech Center in Navalur designs smart sensor fusion algorithms, ultrasonic ADAS, and high-efficiency EV inverters.",
                "https://www.valeo.com", "https://www.valeo.com/en/careers/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Automotive Tech & EV", "DeepTech & AI" },
                "OMR (IT Corridor)", "Pacifica Tech Park, Rajiv Gandhi Salai, Navalur, Chennai 603103",
                12.8468, 80.2258, 1997, "3,000+", "Active",
                new() { "C++", "Python", "Embedded Linux", "AUTOSAR", "Computer Vision", "CANoe" },
                new() { "Automotive", "ADAS", "Autonomous Driving", "EV" });

            AddCompany("comp-96", "Visteon Corporation Chennai", "visteon-chennai", "Digital Cockpit Electronics & Curved Smart Displays",
                "Visteon's Chennai technical center engineers integrated digital cockpit software, instrument clusters, and cybersecurity gateways for smart cars.",
                "https://www.visteon.com", "https://visteon.com/careers/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Automotive Tech & EV" },
                "Guindy (SIDCO / Olympia)", "Olympia Technology Park, Guindy Industrial Estate, Chennai 600032",
                13.0122, 80.2085, 2000, "2,000+", "Active",
                new() { "C++", "Android Automotive", "Qt/QML", "RTOS", "Embedded C", "AUTOSAR" },
                new() { "Connected Cars", "Digital Cockpit", "Automotive" });

            AddCompany("comp-97", "Bosch Global Software Technologies (BGSW)", "bosch-bgsw-chennai", "Connected Vehicles, Cloud Telematics & Smart Factory AI",
                "BGSW Chennai engineering centers lead software development for electronic stability control, connected powertrain telemetry, and IoT.",
                "https://www.bosch-softwaretechnologies.com", "https://www.bosch-softwaretechnologies.com/en/careers/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Automotive Tech & EV", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "Ramanujan IT City, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9875, 80.2468, 1997, "4,000+", "Active",
                new() { "Embedded C", "Python", "Java", "Azure IoT", "AUTOSAR", "C++" },
                new() { "Automotive", "Connected Mobility", "Bosch", "Smart Factory" });

            AddCompany("comp-98", "Daimler India Commercial Vehicles (DICV Tech)", "daimler-dicv-chennai", "Connected Truck Telematics & Autonomous Commercial Freight",
                "DICV develops the Truckonnect intelligent fleet telematics system, predictive maintenance AI, and electric commercial vehicles.",
                "https://www.daimler-truck.com", "https://www.bharatbenz.com/careers",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Automotive Tech & EV" },
                "Porur & DLF Cybercity", "SIPCOT Industrial Growth Centre, Oragadam & Porur Corridor, Chennai 602105",
                12.8425, 79.9515, 2009, "4,000+", "Active",
                new() { "Java", "Python", "Embedded C", "IoT Telematics", "AWS", "Big Data" },
                new() { "BharatBenz", "Fleet Telematics", "Connected Trucks" });

            // ==========================================
            // 15. SUPPLY CHAIN, SEMICONDUCTOR & HARDWARE
            // ==========================================
            AddCompany("comp-99", "Freightify", "freightify", "Rate Management & Digital Ocean Freight Forwarding SaaS",
                "Freightify automates freight quotation, shipping schedule lookups, and container tracking for 300+ international ocean freight forwarders.",
                "https://www.freightify.com", "https://www.freightify.com/careers",
                new() { "PRODUCT COMPANY", "SAAS", "STARTUP" }, new() { "Supply Chain & Logistics Tech", "SaaS / Enterprise Software" },
                "Perungudi & Kandanchavadi", "OMR, Perungudi, Chennai 600096",
                12.9642, 80.2458, 2016, "100-250", "Active",
                new() { "Node.js", "React", "Python", "MongoDB", "AWS", "Microservices" },
                new() { "Freight Forwarding", "Logistics SaaS", "Maritime Tech" });

            AddCompany("comp-100", "Pando Enterprise Technologies", "pando-ai", "AI-Powered Supply Chain Visibility & Logistics Network Platform",
                "Pando is a global leader in networked logistics software, optimizing freight dispatch, fulfillment tracking, and freight audit for Fortune 500s.",
                "https://pando.ai", "https://pando.ai/careers",
                new() { "PRODUCT COMPANY", "SAAS", "STARTUP" }, new() { "Supply Chain & Logistics Tech", "SaaS / Enterprise Software" },
                "OMR (IT Corridor)", "Prince Infocity, Rajiv Gandhi Salai, Kandanchavadi, Chennai 600096",
                12.9652, 80.2468, 2017, "150-300", "Active",
                new() { "Python", "Go", "React", "PostgreSQL", "AWS", "Machine Learning" },
                new() { "Supply Chain", "Logistics Visibility", "Enterprise SaaS" });

            AddCompany("comp-101", "Flex (Flextronics) Global Business Services", "flex-chennai", "Advanced Electronics Manufacturing & Embedded Hardware Design",
                "Flex operates massive engineering and supply chain centers in DLF Cybercity and Sriperumbudur, manufacturing electronics for Apple, HP, and Cisco.",
                "https://flex.com", "https://flex.com/careers",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Semiconductor & Hardware", "Manufacturing & Industrial Tech" },
                "Porur & DLF Cybercity", "DLF Cybercity, 1/124 Shivaji Gardens, Mount Poonamallee Road, Porur, Chennai 600089",
                13.0315, 80.1652, 1969, "10,000+", "Active",
                new() { "Embedded C", "C++", "PCB Design", "FPGA", "Python", "Supply Chain Analytics" },
                new() { "Electronics Manufacturing", "Hardware Engineering", "MNC" });

            AddCompany("comp-102", "Qualcomm India Chennai R&D", "qualcomm-chennai", "5G Modem Firmware, RF Silicon & Automotive Telematics SoC",
                "Qualcomm's Chennai R&D facility designs wireless connectivity microcode, Wi-Fi 7 silicon, and Snapdragon automotive communication processors.",
                "https://www.qualcomm.com", "https://www.qualcomm.com/company/careers",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Semiconductor & Hardware", "DeepTech & AI" },
                "Taramani (Tidel Park & Ascendas)", "Ramanujan IT City, Rajiv Gandhi Salai, Taramani, Chennai 600113",
                12.9885, 80.2475, 1985, "1,500+", "Active",
                new() { "C", "C++", "Python", "RTOS", "5G NR", "ASIC Design", "Verilog" },
                new() { "Semiconductor", "5G", "Snapdragon", "SoC" });

            AddCompany("comp-103", "Applied Materials India", "applied-materials-chennai", "Nanomanufacturing Tech & Semiconductor Fab Equipment Center",
                "Applied Materials' Center of Excellence at IIT Madras collaborates on advanced materials science, atomic layer deposition, and semiconductor inspection.",
                "https://www.appliedmaterials.com", "https://www.appliedmaterials.com/en-in/careers",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Semiconductor & Hardware" },
                "Taramani (Tidel Park & Ascendas)", "IIT Madras Research Park, Taramani, Chennai 600113",
                12.9912, 80.2425, 1967, "500+", "Active",
                new() { "C++", "Python", "Control Systems", "Physics Simulation", "MATLAB" },
                new() { "Semiconductor Equipment", "Materials Science", "Nanotechnology" });

            AddCompany("comp-104", "Western Digital Chennai", "western-digital-chennai", "Enterprise Flash Storage Controller & NVMe Firmware R&D",
                "Western Digital's Chennai engineering site leads firmware development for enterprise solid-state drives (SSDs), NAND flash, and cloud storage.",
                "https://www.westerndigital.com", "https://careers.westerndigital.com/",
                new() { "MNC", "GCC", "ENTERPRISE" }, new() { "Semiconductor & Hardware" },
                "Taramani (Tidel Park & Ascendas)", "Ramanujan IT City, Cambridge Building, Taramani, Chennai 600113",
                12.9878, 80.2468, 1970, "1,000+", "Active",
                new() { "C", "C++", "NVMe", "PCIe", "Python", "Flash Memory", "RTOS" },
                new() { "Storage Tech", "SSD Firmware", "Semiconductor" });

            AddCompany("comp-105", "Mad Street Den (Vue.ai)", "vue-ai", "Enterprise Computer Vision & Omnichannel Generative AI",
                "Mad Street Den's Vue.ai platform uses computer vision and neural networks to power catalog management, 3D on-model imagery, and retail AI.",
                "https://vue.ai", "https://vue.ai/careers/",
                new() { "PRODUCT COMPANY", "STARTUP" }, new() { "DeepTech & AI", "E-Commerce & Retail Tech" },
                "Central Chennai / Anna Salai", "Prestige Polygon, 471 Anna Salai, Teynampet, Chennai 600018",
                13.0335, 80.2425, 2013, "250-500", "Active",
                new() { "Python", "PyTorch", "OpenCV", "TensorFlow", "React", "AWS", "Generative AI" },
                new() { "Computer Vision", "Generative AI", "Retail AI", "Startup" });

            return list;
        }
    }
}
