# Marka - Field Operations Platform

**Marka** (Philippine word for "Pin") is a web-based field operations management platform for tracking and managing location-based data points.

## Project Overview

- **Platform:** Web-only (Mobile deferred to Year 2)
- **Developer:** Solo developer + Claude Code
- **Timeline:** 12 months to v1.0
- **Database:** Aiven PostgreSQL with PostGIS
- **Authentication:** Keycloak
- **Maps:** Google Maps API

## Current Status

- **Phase:** Month 1, Week 1 - Project Setup
- **Version:** 0.1.0-alpha
- **Last Updated:** December 21, 2025

## Project Structure

```
marka/
├── backend/              # .NET 8 Web API (to be created)
├── frontend/             # React + TypeScript + Vite (to be created)
├── docs/                 # Documentation
│   ├── QUICK_START.md          # Fast setup guide (START HERE!)
│   ├── WEEK_01_CHECKLIST.md    # Week 1 detailed tasks
│   ├── MARKA_PROJECT_PLAN.md   # 12-month solo developer roadmap
│   ├── NRBY2_BLUEPRINT.md      # Architecture reference
│   └── PROJECT_PLANNING.md     # Original team plan (reference)
├── scripts/              # Utility scripts (deployment, setup)
├── .gitignore
└── README.md
```

## Technology Stack

### Backend
- **.NET 8** (C#) - Web API
- **PostgreSQL 15+** with **PostGIS** (Aiven managed)
- **Entity Framework Core 8** - ORM
- **Keycloak** - Authentication (OAuth2/OIDC)
- **Serilog** - Logging

### Frontend
- **React 18** with **TypeScript**
- **Vite** - Build tool
- **Tailwind CSS** - Styling
- **React Query** - Server state management
- **React Router 6** - Routing
- **Google Maps API** - Mapping
- **React Hook Form + Zod** - Forms and validation
- **TanStack Table** - Data tables

### Infrastructure
- **Hosting:** Fly.io/Railway (Months 1-6) → AWS ECS (Months 7-12)
- **Database:** Aiven PostgreSQL (free tier)
- **File Storage:** Local (Months 1-6) → AWS S3 (Months 7-12)
- **CI/CD:** GitHub Actions

## Core Features (Year 1)

### MVP (Month 3)
- ✅ User authentication (Keycloak)
- ✅ Marka CRUD operations
- ✅ Google Maps visualization
- ✅ Photo upload
- ✅ Basic attributes (text, number, date)
- ✅ Search and filtering

### Beta (Month 6)
- Advanced attributes (8+ types)
- Assignments and task management
- Jurisdictions and territories
- CSV import/export
- Basic reporting

### v1.0 (Month 12)
- Dashboard with analytics
- Advanced filtering
- Access control and permissions
- Production-ready deployment
- Complete documentation

## Getting Started

### Prerequisites

- **.NET 8 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Node.js 20+** - [Download](https://nodejs.org/)
- **PostgreSQL 15+** with PostGIS (or Aiven account)
- **Docker** (for local Keycloak)
- **Git**

### Quick Start

```bash
# Clone the repository
git clone https://github.com/yourusername/marka.git
cd marka

# Backend setup (coming in Week 2-3)
cd backend
dotnet restore
dotnet run

# Frontend setup (coming in Week 7-8)
cd frontend
npm install
npm run dev
```

## Development Roadmap

| Milestone | Target | Status |
|-----------|--------|--------|
| **Week 1-2** | Project setup, infrastructure | 🟡 In Progress |
| **Week 3-4** | Keycloak authentication | ⚪ Planned |
| **Month 2** | Marka CRUD + Maps | ⚪ Planned |
| **Month 3** | Alpha Release | ⚪ Planned |
| **Month 6** | Beta Release | ⚪ Planned |
| **Month 12** | v1.0 Production | ⚪ Planned |

## Documentation

- **[QUICK_START.md](docs/QUICK_START.md)** - Fast setup guide ⭐ **START HERE!**
- **[WEEK_01_CHECKLIST.md](docs/WEEK_01_CHECKLIST.md)** - Week 1 detailed checklist
- **[MARKA_PROJECT_PLAN.md](docs/MARKA_PROJECT_PLAN.md)** - Complete 12-month development plan
- **[NRBY2_BLUEPRINT.md](docs/NRBY2_BLUEPRINT.md)** - Architecture reference
- **[PROJECT_PLANNING.md](docs/PROJECT_PLANNING.md)** - Original team plan (reference)

## Contributing

This is a solo developer project, but feedback and suggestions are welcome!

## License

MIT License - See LICENSE file for details

## Acknowledgments

- Inspired by the **NRBY2** platform architecture
- Philippine heritage: "Marka" = Pin/Mark

---

**Built with ❤️ using .NET, React, and PostgreSQL**
