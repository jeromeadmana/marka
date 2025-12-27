# Marka - Solo Developer Project Plan
## Web-First Field Operations Platform

**Developer:** Solo (You + Claude Code)
**Project Duration:** 12 months
**Platform:** Web-only (Mobile deferred to Year 2)
**Core Entity:** "Marka" (Philippine word for "Pin")
**Last Updated:** December 21, 2025

---

## Table of Contents

1. [Executive Summary](#executive-summary)
2. [Project Scope & Constraints](#project-scope--constraints)
3. [Simplified Technology Stack](#simplified-technology-stack)
4. [Core Features (Prioritized)](#core-features-prioritized)
5. [12-Month Solo Developer Roadmap](#12-month-solo-developer-roadmap)
6. [Weekly Development Schedule](#weekly-development-schedule)
7. [Database Schema (Simplified)](#database-schema-simplified)
8. [Risk Management (Solo Developer)](#risk-management-solo-developer)
9. [Success Metrics](#success-metrics)
10. [Go-Live Checklist](#go-live-checklist)

---

## Executive Summary

### What We're Building

A **web-based field operations management platform** called **"Marka"** for tracking and managing location-based data points.

**Core Value Proposition:**
- Track physical locations (utility assets, inspection sites, service points, etc.)
- Assign custom attributes to each location
- Manage tasks and assignments
- View everything on an interactive map
- Generate reports and export data

### Key Constraints (Solo Developer Reality)

✅ **What we WILL build:**
- Web application only (responsive design for tablets)
- Authentication with Keycloak
- Marka CRUD with map visualization
- Custom attributes system
- Basic assignments and task tracking
- Simple reporting with CSV export
- Google Maps integration

❌ **What we WILL NOT build (Year 1):**
- Native mobile apps (iOS/Android)
- Offline capabilities
- Elasticsearch / advanced search
- Real-time notifications (SignalR)
- Messaging system
- Complex workflow automation
- Multi-language support

### Timeline & Milestones

| Milestone | Target Date | Key Features |
|-----------|-------------|--------------|
| **Alpha (Demo-able)** | Month 3 | Login, Marka CRUD, Map view |
| **Beta (Usable)** | Month 6 | Attributes, Assignments, Basic reports |
| **v1.0 (Production)** | Month 12 | Polish, security, full feature set |

---

## Project Scope & Constraints

### Solo Developer Realities

**Time Available:**
- Assume **20-30 hours/week** of focused development time
- Approximately **240-360 hours per month**
- **~3,600 hours total over 12 months**

**Productivity Factors:**
- No meetings, no coordination overhead (benefit!)
- Full context switching cost (drawback)
- Learning curve for new technologies
- Debugging takes longer alone
- No code reviews (need discipline)

**Realistic Velocity:**
- **Months 1-2:** Slow (learning, setup) - ~60% productivity
- **Months 3-6:** Medium (flow state) - ~80% productivity
- **Months 7-12:** High (experienced with stack) - ~90% productivity

### Scope Boundaries

**In Scope (Year 1):**
1. User authentication and authorization
2. Marka management (CRUD)
3. Custom attributes (8-10 types)
4. Map visualization (Google Maps)
5. Assignments and tasks
6. Jurisdictions (basic)
7. Simple reporting
8. CSV import/export
9. File/photo uploads
10. Basic admin dashboard

**Out of Scope (Deferred):**
1. Mobile apps (native iOS/Android)
2. Offline sync
3. Real-time notifications
4. In-app messaging
5. Advanced search (Elasticsearch)
6. Complex workflow automation
7. Scheduled background jobs
8. Multi-tenant/SaaS
9. API for third-party integrations
10. Advanced analytics

---

## Simplified Technology Stack

### Backend

| Component | Technology | Rationale |
|-----------|-----------|-----------|
| **Runtime** | .NET 8 (C#) | Mature, fast, great tooling |
| **Framework** | ASP.NET Core MVC + Web API | Full-stack framework, built-in auth |
| **Database** | **Aiven PostgreSQL** | Managed service, PostGIS support, free tier available |
| **ORM** | Entity Framework Core 8 | Type-safe, migrations, LINQ queries |
| **Geospatial** | PostGIS + NetTopologySuite | Industry standard for geo queries |
| **Authentication** | **Keycloak** | Open-source, standards-based (OAuth2/OIDC) |
| **File Storage** | **Local filesystem** (Month 1-6), AWS S3 (Month 7+) | Start simple, upgrade later |
| **Logging** | Serilog → Console + File | Simple, structured logging |
| **Testing** | xUnit + Moq | Standard .NET testing stack |

**Removed from NRBY2:**
- ❌ Elasticsearch (use PostgreSQL full-text search)
- ❌ Hangfire (no background jobs initially)
- ❌ Quartz (no scheduled jobs)
- ❌ SignalR (no real-time needed)
- ❌ Redis (no caching layer)
- ❌ Firebase (no mobile notifications)

### Frontend

| Component | Technology | Rationale |
|-----------|-----------|-----------|
| **Framework** | React 18 + TypeScript | Component-based, type-safe |
| **Build Tool** | Vite | Fast dev server, simple config |
| **Routing** | React Router 6 | Standard React routing |
| **Styling** | **Tailwind CSS** | Utility-first, rapid development, smaller bundle than Bootstrap |
| **State** | React Query + Context | Server state + minimal client state |
| **Forms** | React Hook Form + Zod | Lightweight, TypeScript-first validation |
| **Maps** | **Google Maps API** (@vis.gl/react-google-maps) | Required, well-documented |
| **Tables** | TanStack Table | Powerful, headless table library |
| **UI Components** | Headless UI (by Tailwind) | Unstyled, accessible components |
| **Date Handling** | date-fns | Lightweight, tree-shakeable |
| **HTTP Client** | Axios | Simple, interceptors for auth |
| **Icons** | Heroicons | Free, matches Tailwind aesthetic |

**Removed from NRBY2:**
- ❌ Bootstrap (Tailwind is faster for solo dev)
- ❌ FullCalendar (don't need calendar view initially)
- ❌ React Quill (defer rich text editor)
- ❌ Formik (React Hook Form is lighter)

### DevOps & Infrastructure

| Component | Technology | Rationale |
|-----------|-----------|-----------|
| **Database Hosting** | **Aiven PostgreSQL** | Free tier, managed, automatic backups |
| **App Hosting** | **Fly.io** or **Railway** (Month 1-6), AWS ECS (Month 7+) | Free tier, simple deployment |
| **File Storage** | Local disk (Month 1-6), AWS S3 (Month 7+) | Start simple |
| **CI/CD** | **GitHub Actions** | Free for public repos, simple YAML |
| **Monitoring** | **Built-in logging** (Month 1-6), CloudWatch (Month 7+) | Start simple |
| **Domain** | Namecheap or Route53 | ~$15/year |
| **SSL** | Let's Encrypt (free) | Auto-renewing certificates |

**Cost Estimate (Monthly):**
- Months 1-6: ~$0 (free tiers)
- Months 7-12: ~$50-100 (small AWS resources)

---

## Core Features (Prioritized)

### MoSCoW Prioritization (Web-Only)

#### MUST HAVE (MVP - Month 3)

| # | Feature | Description | Complexity | Priority |
|---|---------|-------------|------------|----------|
| 1 | **Authentication** | Keycloak login/logout, JWT tokens | High | CRITICAL |
| 2 | **User Management** | Create users, assign roles (Admin, User) | Medium | CRITICAL |
| 3 | **Marka CRUD** | Create, read, update, delete markas | Medium | CRITICAL |
| 4 | **Map View** | Display markas on Google Maps | Medium | CRITICAL |
| 5 | **Marka Details** | View single marka with all info | Low | HIGH |
| 6 | **Basic Attributes** | Attach 3 attribute types (text, number, date) | Medium | HIGH |
| 7 | **Photo Upload** | Attach 1+ photos to a marka | Medium | HIGH |
| 8 | **List View** | Table of all markas with sorting | Low | HIGH |
| 9 | **Search** | Search markas by name/description | Low | MEDIUM |
| 10 | **Basic Filters** | Filter by category, status | Low | MEDIUM |

**MVP Success Criteria:**
- Admin can log in
- Admin can create/edit markas with GPS coordinates
- Markas appear on Google Maps
- Photos can be uploaded
- Basic attributes can be added (text, number, date)

---

#### SHOULD HAVE (Beta - Month 6)

| # | Feature | Description | Complexity | Priority |
|---|---------|-------------|------------|----------|
| 11 | **Advanced Attributes** | 8+ attribute types (dropdown, multi-select, file, boolean) | Medium | HIGH |
| 12 | **Assignments** | Create tasks, assign to users | Medium | HIGH |
| 13 | **Jurisdictions** | Define geographic boundaries | High | MEDIUM |
| 14 | **User Roles & Permissions** | Fine-grained access control | Medium | MEDIUM |
| 15 | **CSV Export** | Export markas to CSV | Low | MEDIUM |
| 16 | **CSV Import** | Bulk import markas from CSV | Medium | MEDIUM |
| 17 | **Basic Reporting** | Filter + export markas | Low | MEDIUM |
| 18 | **Marka History** | View change log for markas | Low | LOW |

---

#### COULD HAVE (v1.0 - Month 12)

| # | Feature | Description | Complexity | Priority |
|---|---------|-------------|------------|----------|
| 19 | **Advanced Filtering** | Complex filter builder | Medium | LOW |
| 20 | **Saved Views** | Save filter configurations | Low | LOW |
| 21 | **Dashboard** | Summary stats and charts | Medium | LOW |
| 22 | **KML Import** | Import KML files | Medium | LOW |
| 23 | **Map Clustering** | Cluster markers when zoomed out | Low | LOW |
| 24 | **User Profiles** | Edit profile, change password | Low | LOW |
| 25 | **Audit Logs** | Track all user actions | Low | LOW |
| 26 | **Email Notifications** | Basic email alerts | Medium | LOW |

---

#### WON'T HAVE (Year 1)

- Mobile apps (iOS/Android)
- Offline sync
- Real-time updates (SignalR)
- Messaging system
- Elasticsearch
- Background job queue
- Scheduled reports
- Workflow automation (triggers, instructions)
- Multi-language (i18n)
- Multi-tenant (SaaS)

---

## 12-Month Solo Developer Roadmap

### Phase 1: Foundation (Months 1-3) - "Alpha Release"

**Goal:** Get something demo-able with core features working

---

#### Month 1: Setup & Authentication

**Week 1: Project Initialization**
- [ ] Create Git repository (GitHub)
- [ ] Initialize .NET 8 Web API project
- [ ] Initialize React + Vite + TypeScript project
- [ ] Set up monorepo structure or separate repos (decide)
- [ ] Configure .gitignore, README.md
- [ ] Install core dependencies
- [ ] Set up local development environment
- [ ] Create development checklist/task board

**Week 2: Database & Infrastructure**
- [ ] Sign up for Aiven PostgreSQL (free tier)
- [ ] Enable PostGIS extension on Aiven
- [ ] Create initial database schema (Users, Customers)
- [ ] Set up Entity Framework Core
- [ ] Create first migration
- [ ] Test database connection
- [ ] Set up Serilog logging
- [ ] Create appsettings.json structure (dev, staging, prod)

**Week 3: Keycloak Setup**
- [ ] Install Keycloak locally (Docker)
- [ ] Create "marka" realm
- [ ] Configure client for web app
- [ ] Set up users and roles
- [ ] Test OAuth2 flow manually (Postman)
- [ ] Document Keycloak configuration
- [ ] Create test users (admin, regular user)

**Week 4: Authentication Implementation**
- [ ] Implement JWT authentication in .NET API
- [ ] Create authentication endpoints (login, logout, refresh)
- [ ] Set up authorization policies
- [ ] Integrate Keycloak in React app
- [ ] Implement login page UI
- [ ] Create protected route wrapper
- [ ] Test authentication flow end-to-end
- [ ] Store JWT token properly (httpOnly cookies or memory)

**Month 1 Deliverables:**
✅ Project scaffolding complete
✅ Database running on Aiven
✅ Keycloak authentication working
✅ User can log in and see protected page

**Time Investment:** 80-100 hours

---

#### Month 2: Marka Core Features

**Week 5: Database Schema & Models**
- [ ] Design Marka entity (name, description, location, category, status)
- [ ] Create Marka, Attribute, AttributeValue models
- [ ] Add NetTopologySuite for PostGIS
- [ ] Create database migration
- [ ] Set up indexes (location, category, customer)
- [ ] Test geospatial queries (nearby markas)
- [ ] Seed test data (50 sample markas)

**Week 6: Marka API**
- [ ] Create MarkaController
- [ ] Implement GET /api/markas (list with pagination)
- [ ] Implement GET /api/markas/{id} (details)
- [ ] Implement POST /api/markas (create)
- [ ] Implement PUT /api/markas/{id} (update)
- [ ] Implement DELETE /api/markas/{id} (soft delete)
- [ ] Add input validation (FluentValidation)
- [ ] Write unit tests for MarkaService
- [ ] Document API with Swagger

**Week 7: Frontend - Marka List & Map**
- [ ] Set up Tailwind CSS
- [ ] Create layout components (Navbar, Sidebar, Container)
- [ ] Build marka list page (table view)
- [ ] Add pagination to table
- [ ] Implement sorting (name, created date)
- [ ] Create basic search input
- [ ] Set up API client (Axios with auth interceptors)
- [ ] Test CRUD operations from UI

**Week 8: Google Maps Integration**
- [ ] Get Google Maps API key
- [ ] Integrate @vis.gl/react-google-maps
- [ ] Create Map component
- [ ] Display markas as markers on map
- [ ] Show info window on marker click
- [ ] Link map view to list view (sync selection)
- [ ] Test with 100+ markers
- [ ] Add loading states

**Month 2 Deliverables:**
✅ Marka database schema complete
✅ Marka API endpoints working
✅ Marka list view (table)
✅ Google Maps showing markas
✅ Basic search and filter

**Time Investment:** 80-100 hours

---

#### Month 3: Marka Details & Attributes

**Week 9: Marka Detail Page**
- [ ] Create marka detail view (modal or page)
- [ ] Display all marka properties
- [ ] Add edit mode toggle
- [ ] Implement inline editing
- [ ] Add validation feedback
- [ ] Create delete confirmation modal
- [ ] Test update flow

**Week 10: Photo Upload**
- [ ] Set up file storage (local filesystem for now)
- [ ] Create file upload API endpoint
- [ ] Implement photo upload component (React Dropzone)
- [ ] Show photo thumbnails
- [ ] Add photo preview modal
- [ ] Handle multiple photos per marka
- [ ] Add image compression (client-side)
- [ ] Test with large files

**Week 11: Basic Attributes System**
- [ ] Create Attribute model (name, type, required)
- [ ] Implement AttributeValue storage (JSONB or EAV)
- [ ] Add 3 attribute types: text, number, date
- [ ] Create attribute configuration UI (admin)
- [ ] Build dynamic attribute input form
- [ ] Save attribute values to database
- [ ] Display attributes in marka detail
- [ ] Test attribute CRUD

**Week 12: Alpha Polish & Demo**
- [ ] Fix critical bugs
- [ ] Add loading skeletons
- [ ] Improve error messages
- [ ] Add empty states
- [ ] Create demo data set
- [ ] Write basic user documentation
- [ ] Deploy to Fly.io or Railway (staging)
- [ ] Record demo video

**Month 3 Deliverables (ALPHA):**
✅ Marka detail page with editing
✅ Photo upload working
✅ 3 attribute types functional
✅ Demo-able to stakeholders
✅ Deployed to staging

**Time Investment:** 80-100 hours

**🎉 ALPHA MILESTONE - Month 3 Complete**

---

### Phase 2: Beta Features (Months 4-6) - "Beta Release"

**Goal:** Make it actually usable with key operational features

---

#### Month 4: User Management & Advanced Attributes

**Week 13: User Management**
- [ ] Create User entity (if not already)
- [ ] Build user list page (admin only)
- [ ] Create user creation form
- [ ] Implement role assignment (Admin, User, Viewer)
- [ ] Add user edit/delete
- [ ] Sync users with Keycloak
- [ ] Test role-based access control

**Week 14: Advanced Attributes (Part 1)**
- [ ] Add dropdown/select attribute type
- [ ] Add multi-select attribute type
- [ ] Add boolean/checkbox attribute type
- [ ] Update attribute input form renderer
- [ ] Test all attribute types
- [ ] Add attribute validation rules

**Week 15: Advanced Attributes (Part 2)**
- [ ] Add file upload attribute type
- [ ] Add image attribute type (separate from general photos)
- [ ] Implement attribute sets (group related attributes)
- [ ] Allow attribute ordering
- [ ] Test complex attribute scenarios

**Week 16: Marka Categories & Status**
- [ ] Add category field (dropdown with predefined list)
- [ ] Add status field (Active, Inactive, Pending, Archived)
- [ ] Create category management UI
- [ ] Add filtering by category and status
- [ ] Update map markers to show different icons per category
- [ ] Test filtering

**Month 4 Deliverables:**
✅ User management (admin can create users)
✅ 8+ attribute types working
✅ Categories and statuses
✅ Improved filtering

**Time Investment:** 80-100 hours

---

#### Month 5: Assignments & Tasks

**Week 17: Assignment Model & API**
- [ ] Create Assignment entity (title, description, status, assignee, due date)
- [ ] Link assignments to markas
- [ ] Create AssignmentController
- [ ] Implement assignment CRUD endpoints
- [ ] Add assignment status workflow (Pending → In Progress → Completed)
- [ ] Write tests

**Week 18: Assignment UI (List & Create)**
- [ ] Create assignment list page
- [ ] Build assignment creation form
- [ ] Add assignment-marka linking UI
- [ ] Implement assignment search
- [ ] Add filtering (by assignee, status, due date)
- [ ] Test assignment creation flow

**Week 19: Assignment UI (Detail & Edit)**
- [ ] Create assignment detail view
- [ ] Add status update UI
- [ ] Implement due date picker
- [ ] Add notes/comments to assignments
- [ ] Allow reassigning to different user
- [ ] Add completion date tracking

**Week 20: Assignment Enhancements**
- [ ] Show assignment count on marka detail
- [ ] Add "My Assignments" view
- [ ] Create assignment metrics (completion rate)
- [ ] Add overdue assignment highlighting
- [ ] Test end-to-end assignment workflows

**Month 5 Deliverables:**
✅ Assignment system working
✅ Users can create and complete tasks
✅ Assignments linked to markas
✅ Basic assignment tracking

**Time Investment:** 80-100 hours

---

#### Month 6: Reporting & Bulk Operations

**Week 21: CSV Export**
- [ ] Implement CSV export API endpoint
- [ ] Add "Export to CSV" button to list view
- [ ] Include all marka fields in export
- [ ] Include attributes in export (flattened)
- [ ] Test with large datasets (1000+ records)
- [ ] Add date range filter for export

**Week 22: CSV Import**
- [ ] Create CSV import API endpoint (basic)
- [ ] Build upload UI
- [ ] Validate CSV format
- [ ] Show import preview
- [ ] Handle errors gracefully
- [ ] Test with sample CSV (100 records)
- [ ] Provide CSV template download

**Week 23: Basic Reporting**
- [ ] Create report builder UI
- [ ] Add filter configuration (category, status, date range, jurisdiction)
- [ ] Implement saved filters
- [ ] Add report preview (table view)
- [ ] Allow export filtered results
- [ ] Test various filter combinations

**Week 24: Beta Testing & Polish**
- [ ] Comprehensive bug testing
- [ ] Fix all critical and high bugs
- [ ] Performance optimization (slow queries)
- [ ] Add more user-friendly error messages
- [ ] Improve UI/UX based on feedback
- [ ] Update documentation
- [ ] Deploy beta to staging

**Month 6 Deliverables (BETA):**
✅ CSV import/export working
✅ Basic reporting with filters
✅ Saved filter views
✅ Beta ready for user testing
✅ 5-10 real users can test

**Time Investment:** 80-100 hours

**🎉 BETA MILESTONE - Month 6 Complete**

---

### Phase 3: Production Features (Months 7-9) - "Feature Complete"

**Goal:** Add remaining v1.0 features and prepare for production

---

#### Month 7: Jurisdictions & Access Control

**Week 25: Jurisdiction Model**
- [ ] Create Jurisdiction entity with PostGIS polygon
- [ ] Implement jurisdiction CRUD API
- [ ] Add spatial queries (markas within jurisdiction)
- [ ] Test geospatial queries
- [ ] Create sample jurisdictions

**Week 26: Jurisdiction UI**
- [ ] Build jurisdiction management page
- [ ] Implement map-based polygon drawing
- [ ] Create jurisdiction list view
- [ ] Add jurisdiction detail page
- [ ] Link markas to jurisdictions (automatic based on location)
- [ ] Test jurisdiction assignment

**Week 27: Access Control**
- [ ] Implement jurisdiction-based filtering
- [ ] Assign users to jurisdictions
- [ ] Filter markas by user's jurisdictions
- [ ] Add permission checks in API
- [ ] Test access control scenarios
- [ ] Document permission model

**Week 28: Infrastructure Upgrade**
- [ ] Migrate file storage to AWS S3
- [ ] Set up production AWS environment
- [ ] Configure RDS PostgreSQL (or continue Aiven)
- [ ] Set up CloudWatch logging
- [ ] Configure domain and SSL
- [ ] Test production infrastructure

**Month 7 Deliverables:**
✅ Jurisdiction management working
✅ Access control implemented
✅ Production infrastructure ready
✅ S3 file storage

**Time Investment:** 80-100 hours

---

#### Month 8: Dashboard & Analytics

**Week 29: Dashboard Design**
- [ ] Design dashboard layout
- [ ] Create summary cards (total markas, assignments, users)
- [ ] Add recent activity feed
- [ ] Show map overview
- [ ] Test dashboard performance

**Week 30: Charts & Visualizations**
- [ ] Add chart library (Recharts or Chart.js)
- [ ] Create markas by category chart (pie/bar)
- [ ] Add assignments by status chart
- [ ] Create activity over time chart (line)
- [ ] Make dashboard interactive

**Week 31: Advanced Filtering**
- [ ] Build complex filter builder UI
- [ ] Support AND/OR filter logic
- [ ] Add attribute-based filtering
- [ ] Implement filter persistence
- [ ] Test complex queries

**Week 32: User Experience Polish**
- [ ] Add keyboard shortcuts
- [ ] Implement breadcrumbs
- [ ] Add tooltips and help text
- [ ] Improve mobile/tablet responsiveness
- [ ] Add loading states everywhere
- [ ] Test on different screen sizes

**Month 8 Deliverables:**
✅ Dashboard with analytics
✅ Charts and visualizations
✅ Advanced filtering
✅ Better UX/UI

**Time Investment:** 80-100 hours

---

#### Month 9: Additional Features & Testing

**Week 33: Marka History/Timeline**
- [ ] Implement change tracking
- [ ] Create timeline view
- [ ] Show who changed what and when
- [ ] Add history comparison
- [ ] Test audit trail

**Week 34: KML Import (Optional)**
- [ ] Implement KML parsing
- [ ] Create KML upload UI
- [ ] Map KML features to markas
- [ ] Test with sample KML files
- [ ] (Skip if time is tight)

**Week 35: Map Enhancements**
- [ ] Add marker clustering
- [ ] Implement custom marker icons
- [ ] Add map legend
- [ ] Improve map performance
- [ ] Add map search/geocoding

**Week 36: Comprehensive Testing**
- [ ] Write integration tests
- [ ] Security audit (manual)
- [ ] Performance testing
- [ ] Cross-browser testing
- [ ] Fix all medium+ priority bugs

**Month 9 Deliverables:**
✅ History tracking
✅ Enhanced maps
✅ Comprehensive testing complete
✅ Ready for production prep

**Time Investment:** 80-100 hours

---

### Phase 4: Production Launch (Months 10-12) - "v1.0 Release"

**Goal:** Polish, secure, optimize, and launch to production

---

#### Month 10: Security & Optimization

**Week 37: Security Hardening**
- [ ] Enable HTTPS everywhere
- [ ] Add CSRF protection
- [ ] Implement rate limiting
- [ ] Configure CORS properly
- [ ] Validate all inputs (XSS, SQL injection)
- [ ] Add security headers
- [ ] Review OWASP Top 10

**Week 38: Performance Optimization**
- [ ] Add database indexes for slow queries
- [ ] Optimize API response times
- [ ] Implement pagination everywhere
- [ ] Add caching headers
- [ ] Minify and optimize frontend bundle
- [ ] Test with 10,000+ markas

**Week 39: Error Handling & Logging**
- [ ] Add global error handler
- [ ] Implement user-friendly error pages (404, 500)
- [ ] Set up error tracking (Sentry free tier)
- [ ] Improve logging (structured logs)
- [ ] Add request correlation IDs
- [ ] Test error scenarios

**Week 40: User Settings & Profiles**
- [ ] Create user profile page
- [ ] Allow password change
- [ ] Add user preferences (map defaults, etc.)
- [ ] Implement email notifications (basic)
- [ ] Test user workflows

**Month 10 Deliverables:**
✅ Security hardened
✅ Performance optimized
✅ Error handling robust
✅ User settings

**Time Investment:** 80-100 hours

---

#### Month 11: Documentation & Polish

**Week 41: User Documentation**
- [ ] Write user guide (getting started)
- [ ] Create feature documentation
- [ ] Record video tutorials (5-10 short videos)
- [ ] Write FAQ
- [ ] Create keyboard shortcut reference

**Week 42: Admin Documentation**
- [ ] Document deployment process
- [ ] Write database backup/restore guide
- [ ] Create troubleshooting guide
- [ ] Document common issues
- [ ] Create runbook for production incidents

**Week 43: UI/UX Final Polish**
- [ ] Fix all UI glitches
- [ ] Improve color scheme consistency
- [ ] Add animations (subtle)
- [ ] Test accessibility (keyboard navigation, screen readers)
- [ ] Get feedback from 3-5 users
- [ ] Implement feedback

**Week 44: Final Testing**
- [ ] End-to-end testing (all features)
- [ ] Regression testing
- [ ] Load testing (100 concurrent users)
- [ ] Browser testing (Chrome, Firefox, Safari, Edge)
- [ ] Tablet testing (iPad, Android tablet)
- [ ] Fix all remaining bugs

**Month 11 Deliverables:**
✅ Complete documentation
✅ UI polished
✅ All major bugs fixed
✅ Ready for production deployment

**Time Investment:** 80-100 hours

---

#### Month 12: Production Launch

**Week 45: Pre-Launch Preparation**
- [ ] Final deploy to production
- [ ] Configure production database (backups, monitoring)
- [ ] Set up CloudWatch alarms
- [ ] Create production checklist
- [ ] Prepare rollback plan
- [ ] Smoke test production

**Week 46: Soft Launch**
- [ ] Launch to 10-20 initial users
- [ ] Monitor closely for 48 hours
- [ ] Gather immediate feedback
- [ ] Fix critical issues quickly
- [ ] Adjust based on real usage

**Week 47: Full Launch**
- [ ] Announce launch (if public)
- [ ] Onboard remaining users
- [ ] Provide support
- [ ] Monitor metrics (uptime, errors, performance)
- [ ] Create support documentation

**Week 48: Post-Launch Support**
- [ ] Monitor production daily
- [ ] Fix bugs as reported
- [ ] Gather feature requests
- [ ] Analyze usage metrics
- [ ] Plan Year 2 features

**Month 12 Deliverables (v1.0 LAUNCH):**
✅ Production deployed
✅ Users onboarded
✅ Monitoring active
✅ Support process in place
✅ Year 2 roadmap drafted

**Time Investment:** 80-100 hours

**🎉 v1.0 LAUNCH - Month 12 Complete**

---

### Weeks 49-52: Post-Launch & Year 2 Planning

**Week 49-50: Stabilization**
- [ ] Fix production bugs
- [ ] Optimize based on real usage
- [ ] Improve documentation
- [ ] Gather user feedback

**Week 51: Retrospective**
- [ ] Document lessons learned
- [ ] Celebrate wins!
- [ ] Identify what worked and what didn't
- [ ] Update technical debt backlog

**Week 52: Year 2 Planning**
- [ ] Prioritize Year 2 features
- [ ] Consider: mobile app, offline sync, advanced search?
- [ ] Budget planning
- [ ] Consider hiring help for Year 2?

---

## Weekly Development Schedule

### Typical Week (Solo Developer)

**Assumptions:**
- **Available time:** 20-30 hours/week
- **Work pattern:** Evenings (3-4 hours) + weekends (8-12 hours)

**Monday-Friday (Evenings):** 15-20 hours total
- Focus on coding (features, bug fixes)
- Minimize distractions
- Use Claude Code for assistance

**Saturday-Sunday:** 8-12 hours total
- Larger features requiring focus
- Testing and debugging
- Documentation
- Planning next week

### Time Allocation (Typical Week)

| Activity | Hours/Week | Percentage |
|----------|-----------|------------|
| **Feature Development** | 12-18 hours | 60% |
| **Bug Fixes** | 2-4 hours | 10% |
| **Testing** | 2-4 hours | 10% |
| **Documentation** | 1-2 hours | 5% |
| **Planning/Design** | 2-3 hours | 10% |
| **Learning/Research** | 1-2 hours | 5% |

### Productivity Tips (Solo Developer)

1. **Focus on one feature at a time** - No context switching
2. **Use Claude Code heavily** - For boilerplate, debugging, code review
3. **Write tests as you go** - Easier than retrofitting
4. **Document while fresh** - Don't defer documentation
5. **Deploy early and often** - CI/CD from day one
6. **Take breaks** - Avoid burnout, you're in this for 12 months
7. **Celebrate small wins** - Finished a feature? Take note!
8. **Don't perfect everything** - Ship and iterate
9. **Track time** - Know where your time goes
10. **Set boundaries** - Don't work 7 days/week indefinitely

---

## Database Schema (Simplified)

### Core Tables

#### **users**
```sql
CREATE TABLE users (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    email VARCHAR(255) UNIQUE NOT NULL,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    keycloak_id UUID UNIQUE NOT NULL,
    role VARCHAR(50) NOT NULL, -- 'Admin', 'User', 'Viewer'
    customer_id UUID REFERENCES customers(id),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW(),
    is_active BOOLEAN DEFAULT true
);
```

#### **customers** (Simple multi-tenancy)
```sql
CREATE TABLE customers (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT NOW(),
    is_active BOOLEAN DEFAULT true
);
```

#### **markas** (Core entity - renamed from "smartpins")
```sql
CREATE TABLE markas (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    description TEXT,
    location GEOGRAPHY(POINT, 4326) NOT NULL, -- PostGIS
    address TEXT,
    category VARCHAR(100), -- 'Utility Pole', 'Building', 'Sensor', etc.
    status VARCHAR(50) DEFAULT 'Active', -- 'Active', 'Inactive', 'Pending', 'Archived'
    customer_id UUID REFERENCES customers(id),
    jurisdiction_id UUID REFERENCES jurisdictions(id),
    created_by UUID REFERENCES users(id),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW(),
    deleted_at TIMESTAMP NULL -- soft delete
);

-- Indexes
CREATE INDEX idx_markas_location ON markas USING GIST (location);
CREATE INDEX idx_markas_category ON markas (category);
CREATE INDEX idx_markas_status ON markas (status) WHERE deleted_at IS NULL;
CREATE INDEX idx_markas_customer ON markas (customer_id);
```

#### **attributes** (Field definitions)
```sql
CREATE TABLE attributes (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(100) NOT NULL,
    label VARCHAR(255) NOT NULL,
    type VARCHAR(50) NOT NULL, -- 'text', 'number', 'date', 'dropdown', 'boolean', 'file', etc.
    required BOOLEAN DEFAULT false,
    options JSONB, -- for dropdown/multi-select
    validation_rules JSONB,
    display_order INTEGER DEFAULT 0,
    customer_id UUID REFERENCES customers(id),
    created_at TIMESTAMP DEFAULT NOW()
);
```

#### **attribute_values** (Actual data)
```sql
CREATE TABLE attribute_values (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    attribute_id UUID REFERENCES attributes(id),
    marka_id UUID REFERENCES markas(id),
    value TEXT, -- Store as text, cast based on attribute.type
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW(),
    UNIQUE(attribute_id, marka_id)
);

CREATE INDEX idx_attribute_values_marka ON attribute_values (marka_id);
```

#### **assignments**
```sql
CREATE TABLE assignments (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    title VARCHAR(255) NOT NULL,
    description TEXT,
    status VARCHAR(50) DEFAULT 'Pending', -- 'Pending', 'In Progress', 'Completed', 'Blocked'
    assigned_to UUID REFERENCES users(id),
    assigned_by UUID REFERENCES users(id),
    marka_id UUID REFERENCES markas(id), -- optional
    due_date TIMESTAMP,
    completed_at TIMESTAMP,
    notes TEXT,
    customer_id UUID REFERENCES customers(id),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_assignments_assignee ON assignments (assigned_to);
CREATE INDEX idx_assignments_status ON assignments (status);
```

#### **jurisdictions**
```sql
CREATE TABLE jurisdictions (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    name VARCHAR(255) NOT NULL,
    boundary GEOGRAPHY(POLYGON, 4326), -- PostGIS
    parent_id UUID REFERENCES jurisdictions(id), -- hierarchical
    customer_id UUID REFERENCES customers(id),
    created_at TIMESTAMP DEFAULT NOW(),
    updated_at TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_jurisdictions_boundary ON jurisdictions USING GIST (boundary);
```

#### **media** (Photos and files)
```sql
CREATE TABLE media (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    filename VARCHAR(255) NOT NULL,
    file_path TEXT NOT NULL, -- S3 key or local path
    file_type VARCHAR(100), -- MIME type
    file_size BIGINT,
    entity_type VARCHAR(50), -- 'marka', 'assignment', 'attribute_value'
    entity_id UUID NOT NULL,
    uploaded_by UUID REFERENCES users(id),
    created_at TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_media_entity ON media (entity_type, entity_id);
```

#### **marka_history** (Audit trail)
```sql
CREATE TABLE marka_history (
    id UUID PRIMARY KEY DEFAULT gen_random_uuid(),
    marka_id UUID REFERENCES markas(id),
    field_name VARCHAR(100),
    old_value TEXT,
    new_value TEXT,
    changed_by UUID REFERENCES users(id),
    changed_at TIMESTAMP DEFAULT NOW()
);

CREATE INDEX idx_marka_history_marka ON marka_history (marka_id, changed_at DESC);
```

#### **user_jurisdictions** (Many-to-many)
```sql
CREATE TABLE user_jurisdictions (
    user_id UUID REFERENCES users(id),
    jurisdiction_id UUID REFERENCES jurisdictions(id),
    PRIMARY KEY (user_id, jurisdiction_id)
);
```

### Total Tables: ~10 core tables

**Much simpler than NRBY2's 90+ tables!**

---

## Risk Management (Solo Developer)

### Top Risks

| Risk | Probability | Impact | Mitigation |
|------|-------------|--------|------------|
| **1. Burnout** | HIGH | CRITICAL | Set realistic goals, take weekends off, celebrate wins |
| **2. Scope creep** | HIGH | HIGH | Strict prioritization, defer non-essentials ruthlessly |
| **3. Technical blocker (stuck on problem)** | MEDIUM | HIGH | Use Claude Code, Stack Overflow, ChatGPT; timebox research (2 hours max) |
| **4. Keycloak complexity** | MEDIUM | MEDIUM | Allocate extra time (2 weeks), consider Auth0 backup |
| **5. Life events (illness, emergencies)** | MEDIUM | HIGH | Build 2-week buffer into schedule, no hard deadlines |
| **6. Losing motivation** | MEDIUM | HIGH | Join dev communities, share progress publicly, find accountability partner |
| **7. Database performance issues** | LOW | MEDIUM | Index properly from start, test with realistic data early |
| **8. Google Maps API costs** | LOW | MEDIUM | Monitor usage, set billing alerts, use lazy loading |
| **9. Security vulnerability** | LOW | CRITICAL | Follow OWASP guidelines, use HTTPS, validate inputs, get external review |
| **10. User adoption low** | MEDIUM | MEDIUM | Get early feedback, iterate on UX, make it delightful to use |

### If Behind Schedule

**Month 3 (Alpha):**
- Cut: Advanced attributes, keep only text/number/date
- Cut: Photo upload, add later

**Month 6 (Beta):**
- Cut: CSV import, keep only export
- Cut: Jurisdictions, add later
- Focus: Core CRUD + assignments

**Month 12 (Launch):**
- Cut: KML import
- Cut: Dashboard charts
- Cut: Advanced filtering
- Ship: Stable core features only

### Burnout Prevention Plan

- **Work max 30 hours/week average**
- **Take at least 1 full day off per week**
- **No coding 2 weeks in December (holidays)**
- **Celebrate every month milestone**
- **Share progress with community (Twitter, Reddit, etc.)**
- **Don't compare to teams with 10+ developers**
- **This is a marathon, not a sprint**

---

## Success Metrics

### Month 3 (Alpha)
- ✅ Can log in with Keycloak
- ✅ Can create 10 markas via UI
- ✅ Markas appear on Google Maps
- ✅ Can add basic attributes (text, number, date)
- ✅ Photos can be uploaded
- ✅ Search works
- ✅ Deployed to staging
- ✅ Demo-able to 2-3 people

### Month 6 (Beta)
- ✅ 5-10 real users testing
- ✅ 100+ markas created by users
- ✅ 20+ assignments completed
- ✅ Advanced attributes working (8+ types)
- ✅ CSV export functional
- ✅ Reports generated
- ✅ Zero critical bugs
- ✅ Positive user feedback (80%+ satisfaction)

### Month 12 (v1.0 Launch)
- ✅ 50+ active users (if internal) or 200+ (if public)
- ✅ 1,000+ markas created
- ✅ 500+ assignments completed
- ✅ 99% uptime over 30 days
- ✅ < 2 second page load times
- ✅ < 200ms API response times (p90)
- ✅ All core features working
- ✅ Documentation complete
- ✅ Users can self-onboard

### Technical Metrics (Continuous)

| Metric | Target |
|--------|--------|
| **Test Coverage (Backend)** | 60%+ |
| **API Response Time (p90)** | < 200ms |
| **Page Load Time** | < 2s |
| **Database Query Time (p90)** | < 50ms |
| **Zero Data Loss** | 100% |
| **Security Vulnerabilities** | 0 critical/high |

---

## Go-Live Checklist

### 1 Month Before (Week 44)
- [ ] Production infrastructure provisioned (AWS or continue Fly.io)
- [ ] Domain registered and DNS configured
- [ ] SSL certificates set up (Let's Encrypt)
- [ ] Database backups tested (restore test)
- [ ] Monitoring configured (CloudWatch or similar)
- [ ] Error tracking set up (Sentry)
- [ ] All features code complete
- [ ] Documentation 90% complete
- [ ] Security review done

### 2 Weeks Before (Week 46)
- [ ] Deploy to production environment
- [ ] Smoke test production
- [ ] Load test with realistic data (1000+ markas)
- [ ] Browser testing (Chrome, Firefox, Safari, Edge)
- [ ] Tablet testing
- [ ] Fix all critical bugs
- [ ] User documentation complete
- [ ] Video tutorials recorded

### 1 Week Before (Week 47)
- [ ] Final deployment
- [ ] Data migration (if applicable)
- [ ] User accounts created
- [ ] Onboarding emails prepared
- [ ] Support process defined
- [ ] Rollback plan documented
- [ ] Launch checklist ready

### Launch Day (Week 48 - Day 1)
- [ ] Send launch announcement
- [ ] Onboard first 10-20 users
- [ ] Monitor logs closely
- [ ] Be available for support
- [ ] Track metrics (errors, performance)
- [ ] Fix critical issues immediately

### Week After Launch (Week 48)
- [ ] Daily monitoring
- [ ] Respond to user feedback
- [ ] Fix bugs as reported
- [ ] Adjust infrastructure if needed
- [ ] Document common issues
- [ ] Celebrate! 🎉

---

## Year 2 Roadmap (Tentative)

**What might come in Year 2:**

### High Priority
1. **Mobile app** (iOS + Android with Ionic)
2. **Offline sync** for mobile
3. **Advanced search** (Elasticsearch)
4. **Real-time notifications** (SignalR + Firebase)
5. **Messaging system**

### Medium Priority
6. Background jobs (Hangfire)
7. Scheduled reports
8. Workflow automation (triggers, instructions)
9. Advanced analytics dashboard
10. Multi-language support (i18n)

### Low Priority
11. Multi-tenant/SaaS capabilities
12. API for third-party integrations
13. White-labeling
14. Advanced geofencing
15. IoT device integration

**Decision Point:** After Month 12, evaluate:
- User feedback (what do they need most?)
- Usage patterns (how are they using it?)
- Consider hiring a junior developer to help
- Consider contractor for mobile app

---

## Tools & Resources

### Development Tools
- **IDE:** Visual Studio 2022 or VS Code + C# DevKit
- **Database:** DBeaver or pgAdmin for PostgreSQL
- **API Testing:** Postman or Bruno
- **Git Client:** GitHub Desktop or command line
- **Design:** Figma (free tier)

### Learning Resources
- **.NET Docs:** https://learn.microsoft.com/en-us/aspnet/core/
- **React Docs:** https://react.dev/
- **Tailwind CSS:** https://tailwindcss.com/
- **PostGIS:** https://postgis.net/documentation/
- **Keycloak Docs:** https://www.keycloak.org/documentation

### Community Support
- **Stack Overflow**
- **Reddit:** r/dotnet, r/reactjs
- **Discord:** Reactiflux, C# Discord
- **Claude Code:** Your AI pair programmer!

---

## Final Thoughts

### This is Doable!

**12 months, solo developer, web-only = Realistic**

You're building a **focused, production-ready web application** that solves a real problem. By deferring mobile and keeping the scope tight, you can ship something valuable in 12 months.

### Keys to Success

1. **Ruthless prioritization** - Say no to feature creep
2. **Ship early, iterate often** - Alpha at 3 months gives you 9 months to improve
3. **Use Claude Code** - Let AI handle boilerplate and debugging
4. **Celebrate small wins** - Every completed feature is progress
5. **Take care of yourself** - Sustainable pace wins the marathon
6. **Focus on users** - Build what they need, not what's cool
7. **Keep it simple** - Boring technology works

### Remember

- **You don't need Elasticsearch** - PostgreSQL full-text search is fine
- **You don't need real-time** - Polling every 30 seconds works
- **You don't need microservices** - Monolith is perfect for solo dev
- **You don't need perfect code** - Working code beats perfect code
- **You don't need all features** - Core features well-done beats 100 half-done features

**Ship v1.0 in 12 months. Iterate in Year 2.**

You've got this! 🚀

---

**Document Version:** 1.0 (Solo Developer Edition)
**Project:** Marka (Field Operations Platform)
**Developer:** Solo (You + Claude Code)
**Next Review:** End of Month 1
