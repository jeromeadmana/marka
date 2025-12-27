# NRBY2 Project Blueprint
## Field Operations & Logistics Management Platform

**Version:** 2.0
**Last Updated:** December 21, 2025
**Purpose:** This document serves as a comprehensive blueprint for creating similar field operations management applications based on the NRBY2 architecture.

---

## Table of Contents

1. [Executive Overview](#executive-overview)
2. [System Architecture](#system-architecture)
3. [Technology Stack](#technology-stack)
4. [Core Features & Capabilities](#core-features--capabilities)
5. [Data Models & Schema](#data-models--schema)
6. [Platform Components](#platform-components)
7. [Implementation Guide](#implementation-guide)
8. [Security & Authentication](#security--authentication)
9. [DevOps & Deployment](#devops--deployment)
10. [Best Practices & Patterns](#best-practices--patterns)

---

## Executive Overview

### What is NRBY2?

NRBY2 is a **comprehensive field operations and logistics management platform** designed for organizations that need to:
- Track and manage geographic points of interest (Smartpins)
- Assign tasks to field teams
- Collect custom data through configurable attributes
- Manage jurisdictions and hierarchical territories
- Enable offline-capable mobile field work
- Generate reports and analytics
- Provide real-time notifications and updates

### Key Value Propositions

1. **Multi-Platform Presence:** Web portal for administrators, mobile app for field workers
2. **Offline-First Mobile:** Works without internet connectivity using local SQLite
3. **Geospatial Intelligence:** PostGIS-powered location analytics and mapping
4. **Configurable Workflows:** Custom attributes, instructions, and triggers
5. **Enterprise-Grade Auth:** Keycloak-based SSO with role-based access control
6. **Real-Time Updates:** SignalR and Firebase push notifications
7. **Scalable Architecture:** Microservices-ready with Elasticsearch, background jobs, and cloud storage

### Use Cases

- **Utilities & Infrastructure:** Track utility poles, manholes, substations
- **Field Services:** Assign and track service visits, installations, repairs
- **Asset Management:** Monitor equipment, facilities, inventory locations
- **Environmental Monitoring:** Track sensors, sampling locations, incidents
- **Construction:** Manage job sites, inspections, punch lists
- **Emergency Services:** Incident tracking, resource allocation

---

## System Architecture

### High-Level Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                        CLIENT LAYER                              │
├─────────────────────┬───────────────────────┬───────────────────┤
│   Web Portal        │   Mobile App (iOS)    │  Mobile App       │
│   (React + Vite)    │   (Ionic/Capacitor)   │  (Android)        │
└──────────┬──────────┴───────────┬───────────┴──────────┬────────┘
           │                      │                       │
           └──────────────────────┼───────────────────────┘
                                  │
                          ┌───────▼────────┐
                          │  Keycloak Auth │
                          └───────┬────────┘
                                  │
           ┌──────────────────────┼───────────────────────┐
           │                      │                       │
┌──────────▼──────────┐  ┌───────▼────────┐  ┌──────────▼─────────┐
│  API Gateway/Load   │  │   SignalR      │  │  Firebase FCM      │
│  Balancer           │  │   WebSockets   │  │  Push Notifications│
└──────────┬──────────┘  └───────┬────────┘  └────────────────────┘
           │                     │
┌──────────▼─────────────────────▼──────────────────────────────────┐
│                    ASP.NET CORE WEB API                           │
├───────────────────────────────────────────────────────────────────┤
│  Controllers (37+) │ Services (24+) │ Background Jobs (Hangfire) │
└─────┬──────────────┴────────┬───────────────────┬────────────────┘
      │                       │                   │
┌─────▼─────────┐  ┌─────────▼──────────┐  ┌────▼──────────┐
│  PostgreSQL   │  │   Elasticsearch    │  │  AWS S3       │
│  + PostGIS    │  │   (Search Index)   │  │  (Media)      │
└───────────────┘  └────────────────────┘  └───────────────┘
```

### Component Breakdown

#### 1. **Web Portal (Admin Interface)**
- **Purpose:** Administrative dashboard for managing smartpins, users, reports, and system configuration
- **Users:** Administrators, supervisors, back-office staff
- **Key Functions:** Bulk operations, reporting, user management, system settings

#### 2. **Mobile App (Field Interface)**
- **Purpose:** Field worker interface for creating/updating smartpins, completing assignments
- **Users:** Field technicians, inspectors, service workers
- **Key Functions:** Offline data capture, photo uploads, GPS tracking, assignment completion

#### 3. **Backend API**
- **Purpose:** Central business logic, data access, and integration layer
- **Responsibilities:** Authentication, authorization, data validation, business rules, external integrations

#### 4. **Authentication Service**
- **Purpose:** Centralized identity and access management
- **Technology:** Keycloak with custom SPI and theme
- **Features:** SSO, MFA, role-based access, multi-realm support

#### 5. **Data Storage Layer**
- **PostgreSQL:** Primary relational database with PostGIS for geospatial data
- **Elasticsearch:** Full-text search and analytics
- **AWS S3:** Media and file storage
- **Redis (Optional):** Caching and session management

#### 6. **Background Processing**
- **Hangfire:** Job queue for async operations (imports, exports, notifications)
- **Quartz:** Scheduled jobs (reports, cleanup, synchronization)

---

## Technology Stack

### Backend API

| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| **Runtime** | .NET | 8.0 | Application framework |
| **Language** | C# | 12.0 | Primary programming language |
| **Web Framework** | ASP.NET Core | 8.0 | REST API and web services |
| **ORM** | Entity Framework Core | 8.0.4 | Database access and migrations |
| **Database** | PostgreSQL | 14+ | Primary data store |
| **Geospatial** | PostGIS + NetTopologySuite | 8.0.2 | Geographic data and queries |
| **Search** | Elasticsearch | 7.17.0 | Full-text search indexing |
| **Job Scheduling** | Quartz | 3.13.1 | Scheduled background jobs |
| **Job Queue** | Hangfire | 1.8.14 | Async task processing |
| **Real-time** | SignalR | Built-in | WebSocket communication |
| **Authentication** | JWT Bearer | Built-in | Token-based auth |
| **Cloud Storage** | AWS S3 SDK | Latest | File and media storage |
| **Push Notifications** | Firebase Admin SDK | 3.0.0 | Mobile push notifications |
| **Email** | MailKit | 4.6.0 | SMTP email delivery |
| **Geocoding** | Google Geocoding API | - | Address to coordinates |
| **Image Processing** | SkiaSharp | 3.116.1 | Image manipulation |
| **CSV Handling** | CsvHelper | 33.0.1 | CSV import/export |
| **Logging** | Serilog | Latest | Structured logging |

### Web Frontend

| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| **Framework** | React | 18.2.0 | UI library |
| **Language** | TypeScript | 5.7.2 | Type-safe JavaScript |
| **Build Tool** | Vite | 5.0.8 | Fast dev server and bundler |
| **Compiler** | SWC | - | Fast TypeScript/JSX compiler |
| **Styling** | SCSS + Bootstrap | 5.3.2 | CSS framework and preprocessing |
| **State Management** | React Query + SWR | 5.25.0 / 2.2.5 | Server state and caching |
| **Forms** | Formik + Yup | 2.4.5 / 1.4.0 | Form handling and validation |
| **Maps** | Google Maps API | 2.19.3 | Interactive mapping |
| **Tables** | TanStack Table | 8.12.0 | Advanced data tables |
| **Calendar** | FullCalendar | 6.1.11 | Calendar and scheduling UI |
| **Date Handling** | date-fns | 3.3.1 | Date manipulation |
| **Rich Text** | React Quill | 2.0.0 | WYSIWYG editor |
| **Authentication** | keycloak-js | 25.0.0 | Keycloak integration |
| **Notifications** | react-hot-toast | 2.4.1 | Toast notifications |
| **File Operations** | jszip, file-saver | Latest | File handling |
| **Image Processing** | browser-image-compression | Latest | Client-side image optimization |

### Mobile App

| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| **Framework** | Ionic | 7.0.0 | Mobile UI framework |
| **Base Library** | React | 18.2.0 | UI component library |
| **Language** | TypeScript | 5.7.2 | Type-safe development |
| **Native Bridge** | Capacitor | 6.0.0 | Native device API access |
| **Build Tool** | Vite | 5.0.0 | Fast bundler |
| **Local Database** | SQLite (Capacitor) | Latest | Offline data storage |
| **Maps** | Google Maps API | 2.19.3 | Mobile mapping |
| **Authentication** | ionic-appauth | 2.1.0 | OAuth/OIDC for mobile |
| **Localization** | i18next | 24.2.3 | Multi-language support |
| **Forms** | Formik + Yup | 2.4.5 / 1.3.3 | Mobile forms |
| **Image Processing** | browser-image-compression | Latest | Photo optimization |
| **Signature Capture** | react-signature-pad-wrapper | 3.4.0 | Digital signatures |
| **Gestures** | react-swipeable | 7.0.2 | Touch interactions |
| **Audio** | Capacitor Native Audio | Latest | Sound feedback |
| **Background Location** | Capacitor plugins | Latest | GPS tracking |
| **Push Notifications** | Firebase + Capacitor | Latest | Mobile notifications |
| **Testing** | Cypress + Vitest | Latest | E2E and unit testing |

### Authentication

| Component | Technology | Version | Purpose |
|-----------|-----------|---------|---------|
| **Identity Provider** | Keycloak | 24.0+ | OpenID Connect / OAuth 2.0 |
| **Custom SPI** | Java | 11+ | Custom authentication flows |
| **Theme** | React | 18.2.0 | Custom login UI |
| **Protocol** | JWT | - | Token format |

### DevOps & Infrastructure

| Component | Technology | Purpose |
|-----------|-----------|---------|
| **Containerization** | Docker | Application packaging |
| **Orchestration** | AWS ECS or Kubernetes | Container management |
| **CI/CD** | GitHub Actions | Automated deployments |
| **Database Hosting** | AWS RDS PostgreSQL | Managed database |
| **File Storage** | AWS S3 | Object storage |
| **CDN** | CloudFront | Static asset delivery |
| **Monitoring** | Graylog / Serilog | Log aggregation |
| **Code Quality** | Biome, CSharpier | Linting and formatting |

---

## Core Features & Capabilities

### 1. Smartpin Management (Core Entity)

**Concept:** A "Smartpin" is a geographic point with custom attributes, representing any location-based entity (utility pole, building, sensor, etc.)

**Features:**
- **CRUD Operations:** Create, read, update, delete smartpins
- **Geospatial Queries:** Find nearby smartpins, within polygons, radius searches
- **Hierarchical Organization:** Group smartpins into Region Contexts (parent-child relationships)
- **Custom Attributes:** Attach configurable fields (text, numbers, dates, dropdowns, images)
- **Assignment Tracking:** Link smartpins to user assignments
- **History/Timeline:** Audit trail of all changes
- **Bulk Operations:** Import/export thousands of smartpins via CSV
- **Map Visualization:** Display on Google Maps with clustering
- **Search & Filter:** Advanced filtering by attributes, location, status
- **Jurisdiction Assignment:** Assign smartpins to geographic territories

**Implementation Notes:**
- Store coordinates as PostGIS geometry (Point type)
- Index frequently queried fields (status, category, jurisdiction)
- Use Elasticsearch for full-text search across attributes
- Implement soft delete for data retention

### 2. Custom Attributes System

**Concept:** Configurable fields that can be attached to smartpins, allowing administrators to define data collection schemas without code changes

**Features:**
- **Attribute Types:** Text, number, date, dropdown, multi-select, image, file, boolean
- **Attribute Sets:** Group related attributes together
- **Context-Specific Attributes:** Different attribute sets for different smartpin categories
- **Validation Rules:** Required fields, min/max values, regex patterns
- **Conditional Logic:** Show/hide attributes based on other field values
- **Media Attachments:** Photos, videos, PDFs attached to attributes
- **Default Values:** Pre-populate attributes
- **Ordering & Grouping:** Control UI layout

**Implementation Notes:**
- Entity-Attribute-Value (EAV) pattern or JSONB columns in PostgreSQL
- Store attribute definitions in `attributes` table
- Store attribute values in junction table or JSONB field
- Validate attribute values against schema on API side
- Generate dynamic forms in frontend based on attribute definitions

### 3. Assignments & Task Management

**Concept:** Distribute work to field teams with tracking and completion workflows

**Features:**
- **Create Assignments:** Assign tasks to users or groups
- **Link to Smartpins:** Associate assignments with specific locations
- **Status Tracking:** Pending, in progress, completed, blocked
- **Notes & Comments:** Communication between assigners and assignees
- **Attachments:** Link documents, photos, forms
- **Due Dates:** Set deadlines and send reminders
- **Reassignment:** Transfer assignments between users
- **Completion Requirements:** Define what constitutes completion
- **Mobile Workflow:** Complete assignments from mobile app

**Implementation Notes:**
- `assignments` table with foreign keys to users and smartpins
- Status enum or state machine pattern
- Notification triggers on assignment creation and status changes
- Filter assignments by user, jurisdiction, date range

### 4. Jurisdictions & Territories

**Concept:** Define geographic boundaries for organizing work and controlling access

**Features:**
- **Polygon Definitions:** Draw jurisdictions on map (GeoJSON/WKT)
- **Hierarchical Structure:** Parent-child jurisdiction relationships
- **Access Control:** Users only see/edit smartpins in their jurisdictions
- **Assignment Routing:** Auto-assign work based on jurisdiction
- **Reporting:** Generate reports by jurisdiction
- **Boundary Visualization:** Display jurisdictions on map

**Implementation Notes:**
- Store boundaries as PostGIS polygon/multipolygon
- Use ST_Contains() for spatial queries
- Join users to jurisdictions via role assignments
- Implement row-level security based on jurisdiction access

### 5. User & Access Management

**Concept:** Multi-tenant user management with fine-grained permissions

**Features:**
- **User Roles:** Admin, supervisor, field worker, viewer
- **Groups:** Organize users into teams or departments
- **Permissions:** Granular access control (create, read, update, delete) per entity type
- **Jurisdiction Assignment:** Limit user access to specific territories
- **Supervisor Hierarchy:** Managers can view subordinate work
- **Customer/Tenant Isolation:** Multi-tenant data separation
- **User Profiles:** Contact info, preferences, notification settings
- **Audit Logging:** Track user actions

**Implementation Notes:**
- Keycloak manages authentication
- Backend enforces authorization via JWT claims
- `app_roles` table defines permissions
- Many-to-many relationships: users ↔ groups ↔ roles ↔ jurisdictions
- Filter queries based on user's allowed jurisdictions

### 6. Reporting & Analytics

**Concept:** Generate custom reports with flexible filtering and export

**Features:**
- **Custom Views:** Save filter configurations for reuse
- **Dynamic Filters:** Filter by any attribute, date range, status, jurisdiction
- **Export Formats:** PDF, Excel, CSV
- **Scheduled Reports:** Email reports on a schedule (daily, weekly)
- **Visualizations:** Charts, graphs, heatmaps
- **Aggregations:** Count, sum, average, group by
- **Report Builder:** Visual query builder for non-technical users
- **Historical Data:** Time-series analysis

**Implementation Notes:**
- `reports` table stores report definitions
- Use LINQ/EF Core for dynamic query building
- Generate PDFs server-side (libraries: iTextSharp, QuestPDF)
- Cache frequently run reports
- Use Hangfire for scheduled report generation

### 7. Offline-Capable Mobile App

**Concept:** Field workers operate without reliable internet connectivity

**Features:**
- **Local Database:** SQLite stores downloaded smartpins
- **Background Sync:** Auto-sync when connection available
- **Conflict Resolution:** Merge changes when multiple edits occur
- **Photo Queue:** Upload photos when online
- **Offline Maps:** Cache map tiles for offline viewing
- **GPS Tracking:** Record location even offline
- **Offline Forms:** Complete assignments without network
- **Sync Status Indicators:** Show what's synced vs. pending

**Implementation Notes:**
- Capacitor SQLite plugin for local storage
- Download user's jurisdiction data on login
- Queue API calls when offline (IndexedDB or SQLite queue)
- Implement last-write-wins or timestamp-based conflict resolution
- Show sync status in UI (synced, pending, conflict)
- Use service workers for offline web app support (optional)

### 8. Real-Time Notifications

**Concept:** Instant alerts for important events

**Features:**
- **Push Notifications:** Firebase Cloud Messaging (FCM) for mobile
- **In-App Notifications:** Toast messages in web app
- **Email Notifications:** MailKit for email alerts
- **WebSocket Updates:** SignalR for real-time web updates
- **Notification Types:** Assignment created, status changed, message received
- **User Preferences:** Control which notifications to receive
- **Read/Unread Tracking:** Mark notifications as read
- **Notification History:** View past notifications

**Implementation Notes:**
- Firebase Admin SDK for sending push notifications
- SignalR hubs for web real-time updates
- Store notification preferences in user settings
- `mobile_registrations` table for FCM device tokens
- Send notifications via background jobs (Hangfire) for reliability

### 9. Messaging System

**Concept:** In-app messaging between users

**Features:**
- **Direct Messages:** User-to-user communication
- **Group Messages:** Broadcast to teams
- **Attachments:** Send files, images
- **Read Receipts:** Know when messages are read
- **Message History:** Searchable message archive
- **Notification Integration:** Notify recipients of new messages

**Implementation Notes:**
- `messages` table with sender/recipient foreign keys
- SignalR for instant message delivery in web app
- Push notifications for mobile message alerts
- Paginate message history
- Store attachments in S3, reference by message ID

### 10. Bulk Data Operations

**Concept:** Import/export thousands of records efficiently

**Features:**
- **CSV Import:** Upload CSV with smartpin data
- **KML Import:** Import geospatial KML files
- **Data Validation:** Pre-import validation with error reporting
- **Progress Tracking:** Show upload/import progress
- **Batch Processing:** Handle large files without timeout
- **Export to CSV/Excel:** Download data in bulk
- **Template Download:** Provide CSV templates for import

**Implementation Notes:**
- Use background jobs (Hangfire) for imports
- CsvHelper library for parsing CSV
- NetTopologySuite for KML parsing
- Store job status in `jobs` table
- Provide real-time progress via SignalR or polling
- Validate data before committing to database

### 11. Instruction Engine & Triggers

**Concept:** Automate workflows based on events

**Features:**
- **Instruction Templates:** Define reusable task instructions
- **Trigger Conditions:** Execute actions when conditions met (e.g., status change)
- **Boolean Expressions:** Complex rule evaluation
- **Automated Assignments:** Auto-create assignments based on triggers
- **Scheduled Actions:** Time-based automation
- **Notification Triggers:** Send alerts on events

**Implementation Notes:**
- `instructions` and `triggers` tables
- Boolean expression evaluator service
- Quartz scheduler for time-based triggers
- Event-driven architecture: publish events on entity changes
- Subscribe to events and evaluate trigger conditions

---

## Data Models & Schema

### Core Entity Relationships

```
users ──┬─── assignments ─── smartpins
        │                        │
        ├─── messages            ├─── attributes_values
        │                        │
        ├─── user_groups         ├─── region_assignments
        │                        │
        └─── mobile_registrations├─── tag_regions ─── tags
                                 │
customers ─── jurisdictions ─────┤
        │                        │
        └─── app_roles           └─── region_contexts (hierarchy)
                 │
                 └─── permissions

attribute_sets ─── attributes ─── attribute_values
                                        │
                                        └─── smartpins

reports ─── report_filter_search
       │
       └─── scheduled_reports

instructions ─── triggers ─── trigger_notifications

persisted_media ─── assets_metadata
```

### Key Tables & Fields

#### **users**
- `id` (PK, UUID)
- `email` (unique)
- `first_name`, `last_name`
- `customer_id` (FK → customers)
- `keycloak_id` (unique, UUID)
- `role_id` (FK → app_roles)
- `created_at`, `updated_at`
- `is_active` (boolean)

#### **smartpins (regions)**
- `id` (PK, UUID)
- `name`
- `description`
- `location` (PostGIS geometry - Point)
- `address`
- `category` (enum or string)
- `status` (enum: active, inactive, pending, archived)
- `customer_id` (FK → customers)
- `jurisdiction_id` (FK → jurisdictions)
- `region_context_id` (FK → region_contexts) - for hierarchy
- `created_by` (FK → users)
- `created_at`, `updated_at`
- `deleted_at` (soft delete)
- `metadata` (JSONB - flexible additional data)

#### **attributes**
- `id` (PK, UUID)
- `name`
- `label` (display name)
- `type` (enum: text, number, date, dropdown, file, image, boolean)
- `required` (boolean)
- `options` (JSONB - for dropdown/multi-select)
- `validation_rules` (JSONB)
- `attribute_set_id` (FK → attribute_sets)
- `order` (integer - display order)

#### **attribute_values**
- `id` (PK, UUID)
- `attribute_id` (FK → attributes)
- `smartpin_id` (FK → smartpins)
- `value` (text or JSONB for complex types)
- `created_at`, `updated_at`

#### **assignments**
- `id` (PK, UUID)
- `title`
- `description`
- `status` (enum: pending, in_progress, completed, blocked)
- `assigned_to` (FK → users)
- `assigned_by` (FK → users)
- `smartpin_id` (FK → smartpins) - optional
- `due_date` (timestamp)
- `completed_at` (timestamp)
- `created_at`, `updated_at`
- `notes` (text)

#### **jurisdictions**
- `id` (PK, UUID)
- `name`
- `boundary` (PostGIS geometry - Polygon/MultiPolygon)
- `parent_id` (FK → jurisdictions) - for hierarchy
- `customer_id` (FK → customers)
- `created_at`, `updated_at`

#### **app_roles**
- `id` (PK, UUID)
- `name`
- `description`
- `permissions` (JSONB or separate permissions table)
- `customer_id` (FK → customers)

#### **customers**
- `id` (PK, UUID)
- `name`
- `subdomain` (unique - for multi-tenant routing)
- `settings` (JSONB - customer-specific config)
- `is_active` (boolean)
- `created_at`, `updated_at`

#### **messages**
- `id` (PK, UUID)
- `sender_id` (FK → users)
- `recipient_id` (FK → users)
- `subject`
- `body` (text)
- `is_read` (boolean)
- `created_at`

#### **persisted_media**
- `id` (PK, UUID)
- `filename`
- `file_path` (S3 key)
- `file_type` (MIME type)
- `file_size` (bytes)
- `uploaded_by` (FK → users)
- `entity_type` (e.g., 'smartpin', 'assignment')
- `entity_id` (UUID - FK to related entity)
- `created_at`

#### **jobs**
- `id` (PK, UUID)
- `job_type` (enum: import, export, report, notification)
- `status` (enum: queued, running, completed, failed)
- `progress` (integer 0-100)
- `result` (JSONB - output data or error messages)
- `created_by` (FK → users)
- `created_at`, `updated_at`, `completed_at`

### Indexes for Performance

```sql
-- Geospatial indexes
CREATE INDEX idx_smartpins_location ON smartpins USING GIST (location);
CREATE INDEX idx_jurisdictions_boundary ON jurisdictions USING GIST (boundary);

-- Foreign key indexes
CREATE INDEX idx_smartpins_customer ON smartpins (customer_id);
CREATE INDEX idx_smartpins_jurisdiction ON smartpins (jurisdiction_id);
CREATE INDEX idx_assignments_user ON assignments (assigned_to);
CREATE INDEX idx_attribute_values_smartpin ON attribute_values (smartpin_id);

-- Query optimization indexes
CREATE INDEX idx_smartpins_status ON smartpins (status) WHERE deleted_at IS NULL;
CREATE INDEX idx_assignments_status_due ON assignments (status, due_date);
CREATE INDEX idx_users_customer_active ON users (customer_id, is_active);

-- Full-text search (if not using Elasticsearch)
CREATE INDEX idx_smartpins_search ON smartpins USING GIN (to_tsvector('english', name || ' ' || description));
```

---

## Platform Components

### Backend API Structure

```
api/src/
├── Controllers/              # REST API endpoints
│   ├── v1/                  # Legacy API version
│   └── v2/                  # Current API version
│       ├── SmartpinsController.cs
│       ├── AttributesController.cs
│       ├── AssignmentsController.cs
│       ├── UsersController.cs
│       ├── JurisdictionsController.cs
│       ├── ReportController.cs
│       ├── MediaController.cs
│       └── ... (30+ more)
├── Services/                # Business logic
│   ├── SmartpinService.cs
│   ├── ReportService.cs
│   ├── ElasticsearchService.cs
│   ├── NotificationService.cs
│   ├── EmailService.cs
│   ├── S3Service.cs
│   ├── ThumbnailService.cs
│   └── ...
├── Models/                  # Data models
│   ├── Entities/           # Database entities
│   ├── DTOs/               # Data transfer objects
│   └── ViewModels/         # API response models
├── Data/
│   ├── AppDbContext.cs     # EF Core context
│   └── Migrations/         # Database migrations
├── Hubs/                   # SignalR hubs
│   └── NotificationHub.cs
├── BackgroundJobs/         # Hangfire jobs
│   ├── ImportJob.cs
│   ├── ReportGenerationJob.cs
│   └── NotificationJob.cs
├── ScheduledJobs/          # Quartz schedulers
│   └── DailyReportJob.cs
├── Middleware/             # Custom middleware
│   ├── ExceptionHandlingMiddleware.cs
│   ├── TenantMiddleware.cs
│   └── LoggingMiddleware.cs
├── Extensions/             # Extension methods
├── Validators/             # FluentValidation validators
├── Helpers/                # Utility classes
├── Configuration/          # Settings and configuration
└── Program.cs              # Application entry point
```

### Web Frontend Structure

```
webapp/src/
├── components/              # React components
│   ├── dashboard/          # Map and main dashboard
│   │   ├── Dashboard.tsx
│   │   ├── MapView.tsx
│   │   ├── SmartpinMarker.tsx
│   │   └── FilterPanel.tsx
│   ├── smartpins/          # Smartpin management
│   │   ├── SmartpinList.tsx
│   │   ├── SmartpinDetail.tsx
│   │   ├── SmartpinForm.tsx
│   │   └── SmartpinTimeline.tsx
│   ├── attributes/         # Attribute management
│   │   ├── AttributeList.tsx
│   │   ├── AttributeForm.tsx
│   │   └── AttributeSetManager.tsx
│   ├── assignments/        # Assignment views
│   ├── users/              # User management
│   ├── reports/            # Reporting interface
│   ├── bulk-upload/        # CSV/KML import
│   ├── settings/           # User settings
│   ├── common/             # Shared components
│   │   ├── Button.tsx
│   │   ├── Modal.tsx
│   │   ├── Table.tsx
│   │   ├── Input.tsx
│   │   └── Loading.tsx
│   └── layout/             # Layout components
│       ├── Header.tsx
│       ├── Sidebar.tsx
│       └── Footer.tsx
├── context/                # React Context providers
│   ├── AuthContext.tsx
│   ├── DashboardContext.tsx
│   ├── MapContext.tsx
│   └── SettingsContext.tsx
├── hooks/                  # Custom hooks
│   ├── useSmartpins.ts
│   ├── useAuth.ts
│   ├── useNotifications.ts
│   └── useMap.ts
├── services/               # API clients
│   ├── api.ts             # Axios instance
│   ├── smartpinApi.ts
│   ├── userApi.ts
│   └── authApi.ts
├── utils/                  # Utility functions
│   ├── dateFormat.ts
│   ├── geoUtils.ts
│   └── validators.ts
├── types/                  # TypeScript types
│   ├── smartpin.ts
│   ├── user.ts
│   └── api.ts
├── constants/              # Constants
│   ├── routes.ts
│   ├── apiEndpoints.ts
│   └── mapConfig.ts
├── assets/                 # Static assets
│   ├── images/
│   └── styles/
├── App.tsx                 # Main app component
├── main.tsx               # Entry point
└── vite.config.ts         # Vite configuration
```

### Mobile App Structure

```
mobile/src/
├── pages/                  # Page components
│   ├── Home.tsx
│   ├── SmartpinCreate.tsx
│   ├── SmartpinDetail.tsx
│   ├── SmartpinEdit.tsx
│   ├── AssignmentList.tsx
│   ├── Map.tsx
│   ├── Settings.tsx
│   ├── Login.tsx
│   └── ...
├── components/             # Reusable components
│   ├── SmartpinCard.tsx
│   ├── AttributeInput.tsx
│   ├── PhotoCapture.tsx
│   ├── LocationPicker.tsx
│   ├── SignaturePad.tsx
│   └── ...
├── services/               # Services
│   ├── api.ts             # HTTP client
│   ├── database.ts        # SQLite operations
│   ├── sync.ts            # Offline sync logic
│   ├── location.ts        # GPS services
│   ├── notifications.ts   # Push notifications
│   └── storage.ts         # Local storage
├── hooks/                  # Custom hooks
│   ├── useDatabase.ts
│   ├── useSync.ts
│   ├── useLocation.ts
│   └── useCamera.ts
├── context/                # Global state
│   ├── GlobalProvider.tsx
│   └── AuthContext.tsx
├── utils/                  # Utilities
│   ├── offline.ts
│   ├── validators.ts
│   └── imageUtils.ts
├── types/                  # TypeScript types
├── constants/              # Constants
├── theme/                  # Ionic theming
├── App.tsx
└── main.tsx
```

---

## Implementation Guide

### Phase 1: Foundation (Weeks 1-3)

#### Week 1: Project Setup & Infrastructure
1. **Initialize Projects**
   - Create .NET 8 Web API project
   - Set up React + Vite web app
   - Set up Ionic + React mobile app
   - Configure Git repository with .gitignore

2. **Database Setup**
   - Install PostgreSQL with PostGIS extension
   - Create initial database schema
   - Set up Entity Framework Core
   - Create first migration (Users, Customers tables)

3. **Development Environment**
   - Configure local development environment variables
   - Set up Docker Compose for local PostgreSQL + Elasticsearch
   - Configure HTTPS certificates
   - Set up hot reload for all projects

4. **Authentication Foundation**
   - Install and configure Keycloak (Docker container)
   - Create test realm and client
   - Integrate keycloak-js in web app
   - Integrate ionic-appauth in mobile app
   - Implement JWT validation in API

#### Week 2: Core Backend API
1. **Database Models**
   - Create entity classes: Smartpin, Attribute, AttributeSet, Assignment, Jurisdiction, AppRole
   - Configure EF Core relationships
   - Add PostGIS support (NetTopologySuite)
   - Generate and apply migrations

2. **Base API Controllers**
   - Create generic base controller with CRUD operations
   - Implement SmartpinsController with basic CRUD
   - Implement UsersController
   - Implement AttributesController

3. **Services Layer**
   - Create SmartpinService with business logic
   - Implement repository pattern (optional)
   - Add data validation
   - Implement error handling middleware

4. **Testing**
   - Set up unit test project (xUnit)
   - Write tests for SmartpinService
   - Set up integration tests for API endpoints

#### Week 3: Frontend Foundations
1. **Web App Structure**
   - Set up routing (React Router)
   - Create layout components (Header, Sidebar, Footer)
   - Implement authentication flow (login, logout, token refresh)
   - Create protected routes

2. **Component Library**
   - Build common components (Button, Input, Modal, Table, Loading)
   - Set up SCSS architecture
   - Integrate Bootstrap

3. **Dashboard Prototype**
   - Integrate Google Maps API
   - Display smartpins on map
   - Implement basic marker clustering
   - Create smartpin list view

4. **Mobile App Foundation**
   - Set up Ionic UI structure
   - Implement authentication flow
   - Create navigation structure
   - Test on iOS and Android simulators

### Phase 2: Core Features (Weeks 4-8)

#### Week 4: Smartpin Management
1. **Backend**
   - Implement advanced smartpin queries (spatial search, filters)
   - Add Elasticsearch integration
   - Implement smartpin timeline/history
   - Add soft delete support

2. **Web Frontend**
   - Create smartpin detail view
   - Build create/edit forms
   - Add image upload functionality
   - Implement search and filter UI

3. **Mobile**
   - Create smartpin creation flow
   - Integrate camera for photo capture
   - Implement GPS location picker
   - Add offline queue for smartpin creation

#### Week 5: Custom Attributes System
1. **Backend**
   - Finalize attribute value storage (JSONB or EAV)
   - Implement dynamic validation
   - Add attribute set context logic
   - Create API endpoints for attribute CRUD

2. **Frontend**
   - Build attribute configuration UI
   - Create dynamic form renderer based on attribute definitions
   - Implement attribute value editing
   - Add validation feedback

3. **Mobile**
   - Build mobile-friendly attribute input components
   - Implement native pickers (date, dropdown)
   - Add image/file attachment for attributes

#### Week 6: Assignments & Tasks
1. **Backend**
   - Create assignments API
   - Implement status transitions
   - Add assignment notifications
   - Link assignments to smartpins

2. **Web Frontend**
   - Build assignment list view
   - Create assignment detail page
   - Implement assignment creation form
   - Add filtering by user, status, date

3. **Mobile**
   - Create assignment list for field workers
   - Implement assignment completion workflow
   - Add offline assignment support
   - Display assignment map view

#### Week 7: Jurisdictions & Access Control
1. **Backend**
   - Implement jurisdiction-based filtering
   - Add role-based authorization
   - Create permission middleware
   - Implement multi-tenant isolation

2. **Frontend**
   - Build jurisdiction management UI
   - Implement boundary drawing on map
   - Add user-jurisdiction assignment
   - Filter data by user permissions

3. **Testing**
   - Test access control scenarios
   - Verify multi-tenant data isolation
   - Test spatial queries

#### Week 8: Reporting Foundation
1. **Backend**
   - Build dynamic query builder
   - Implement filter serialization
   - Create export services (CSV, Excel)
   - Add report templates

2. **Frontend**
   - Build report filter UI
   - Create custom view save functionality
   - Implement data table with sorting/pagination
   - Add export buttons

### Phase 3: Advanced Features (Weeks 9-12)

#### Week 9: Background Jobs & Notifications
1. **Backend**
   - Integrate Hangfire
   - Create bulk import job
   - Implement Firebase push notifications
   - Set up email service with MailKit

2. **Frontend**
   - Build bulk upload UI with progress bar
   - Integrate toast notifications
   - Add notification center

3. **Mobile**
   - Implement FCM integration
   - Add local notification support
   - Test background sync

#### Week 10: Offline Mobile Support
1. **Mobile**
   - Implement SQLite database layer
   - Build sync service (download jurisdiction data)
   - Implement offline queue for API calls
   - Add conflict resolution
   - Test offline scenarios thoroughly

2. **Backend**
   - Create sync endpoints optimized for mobile
   - Implement delta sync (only changed records)
   - Add conflict detection

#### Week 11: Real-Time Features
1. **Backend**
   - Integrate SignalR
   - Create notification hub
   - Broadcast smartpin updates

2. **Web Frontend**
   - Implement SignalR client
   - Show real-time updates on dashboard
   - Add live notification toasts

#### Week 12: Advanced Mapping & Geospatial
1. **Backend**
   - Implement KML import/export
   - Add advanced spatial queries (polygon intersections)
   - Create heatmap data endpoints

2. **Frontend**
   - Implement KML upload UI
   - Add drawing tools (polygons, lines)
   - Create heatmap visualization
   - Implement Smartlines feature

### Phase 4: Polish & Production (Weeks 13-16)

#### Week 13: Messaging & Collaboration
1. **Backend**
   - Create messaging API
   - Implement real-time message delivery

2. **Frontend**
   - Build messaging UI
   - Add message notifications

#### Week 14: Scheduled Tasks & Automation
1. **Backend**
   - Integrate Quartz scheduler
   - Build instruction engine
   - Implement trigger system
   - Create scheduled report jobs

#### Week 15: Deployment Preparation
1. **DevOps**
   - Create Dockerfiles for all services
   - Set up Docker Compose for staging
   - Configure CI/CD pipeline (GitHub Actions)
   - Set up AWS infrastructure (ECS, RDS, S3)

2. **Security**
   - Implement rate limiting
   - Add CORS configuration
   - Enable HTTPS enforcement
   - Security audit and penetration testing

#### Week 16: Testing & Launch
1. **Testing**
   - Comprehensive end-to-end testing
   - Load testing with realistic data
   - Mobile device testing (multiple devices)
   - User acceptance testing (UAT)

2. **Documentation**
   - API documentation (Swagger/OpenAPI)
   - User manuals
   - Admin guides
   - Deployment runbook

3. **Launch**
   - Deploy to production
   - Monitor logs and performance
   - Gather user feedback
   - Prepare hotfix pipeline

---

## Security & Authentication

### Authentication Flow

#### Web App Authentication
1. User accesses web app
2. App redirects to Keycloak login page
3. User enters credentials
4. Keycloak validates and returns authorization code
5. App exchanges code for JWT access token
6. App stores token in memory (not localStorage for security)
7. App includes token in Authorization header for API calls
8. API validates JWT signature and claims
9. Token refresh before expiration using refresh token

#### Mobile App Authentication
1. Mobile app initiates OAuth flow via ionic-appauth
2. Opens in-app browser to Keycloak
3. User authenticates
4. Keycloak redirects with authorization code
5. App exchanges code for tokens
6. Tokens stored in secure storage (Capacitor Secure Storage)
7. Auto-refresh tokens in background

### Authorization Strategy

#### Role-Based Access Control (RBAC)
- **Roles:** Admin, Supervisor, Field Worker, Viewer
- **Permissions:** Create, Read, Update, Delete (per entity type)
- Roles assigned to users in Keycloak
- JWT claims include user roles
- API validates required roles per endpoint

```csharp
[Authorize(Roles = "Admin,Supervisor")]
[HttpPost("api/smartpins")]
public async Task<IActionResult> CreateSmartpin(SmartpinDto dto) { ... }
```

#### Jurisdiction-Based Access
- Users assigned to jurisdictions
- Queries automatically filtered by user's jurisdictions
- Implemented via query filters in EF Core

```csharp
modelBuilder.Entity<Smartpin>()
    .HasQueryFilter(s => userJurisdictions.Contains(s.JurisdictionId));
```

#### Multi-Tenant Isolation
- Customer ID in JWT claims
- All queries filtered by customer ID
- Database-level isolation via RLS (Row-Level Security) optional

### Security Best Practices

1. **API Security**
   - Always validate JWT tokens
   - Use HTTPS only (enforce with middleware)
   - Implement rate limiting (AspNetCoreRateLimit)
   - Sanitize all inputs
   - Use parameterized queries (EF Core does this)
   - Validate file uploads (size, type, content)

2. **Frontend Security**
   - Store tokens in memory, not localStorage
   - Implement CSRF protection
   - Sanitize user inputs (React does basic XSS protection)
   - Use Content Security Policy (CSP) headers
   - Validate data on both client and server

3. **Mobile Security**
   - Use Capacitor Secure Storage for tokens
   - Implement certificate pinning for API calls
   - Encrypt local SQLite database
   - Obfuscate code before release

4. **Database Security**
   - Use environment variables for connection strings
   - Rotate database passwords regularly
   - Enable SSL for database connections
   - Implement database backups and encryption at rest

---

## DevOps & Deployment

### Containerization

#### Dockerfile for .NET API
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["API/API.csproj", "API/"]
RUN dotnet restore "API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
```

#### Dockerfile for React Web App
```dockerfile
FROM node:20-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist /usr/share/nginx/html
COPY nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

### Docker Compose for Local Development

```yaml
version: '3.8'

services:
  postgres:
    image: postgis/postgis:14-3.3
    environment:
      POSTGRES_DB: nrby_dev
      POSTGRES_USER: nrby_user
      POSTGRES_PASSWORD: dev_password
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:7.17.0
    environment:
      - discovery.type=single-node
      - "ES_JAVA_OPTS=-Xms512m -Xmx512m"
    ports:
      - "9200:9200"

  keycloak:
    image: quay.io/keycloak/keycloak:24.0
    environment:
      KEYCLOAK_ADMIN: admin
      KEYCLOAK_ADMIN_PASSWORD: admin
    ports:
      - "8080:8080"
    command: start-dev

  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"

  api:
    build:
      context: ./api
      dockerfile: Dockerfile
    environment:
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=nrby_dev;Username=nrby_user;Password=dev_password
      - ElasticsearchUrl=http://elasticsearch:9200
      - Keycloak__Authority=http://keycloak:8080/realms/nrby
    ports:
      - "5000:80"
    depends_on:
      - postgres
      - elasticsearch
      - keycloak

  webapp:
    build:
      context: ./webapp
      dockerfile: Dockerfile
    ports:
      - "3000:80"
    depends_on:
      - api

volumes:
  postgres_data:
```

### AWS Deployment Architecture

#### Infrastructure Components
- **Compute:** AWS ECS (Fargate) for containerized API and web app
- **Database:** AWS RDS PostgreSQL with PostGIS
- **File Storage:** AWS S3 for media files
- **CDN:** CloudFront for static asset delivery
- **Load Balancer:** Application Load Balancer (ALB)
- **Secrets:** AWS Secrets Manager for sensitive configuration
- **Monitoring:** CloudWatch for logs and metrics
- **Search:** AWS Elasticsearch Service (alternative to self-hosted)

#### CI/CD Pipeline (GitHub Actions)

```yaml
name: Deploy to Production

on:
  push:
    branches: [main]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3

      - name: Configure AWS credentials
        uses: aws-actions/configure-aws-credentials@v2
        with:
          aws-access-key-id: ${{ secrets.AWS_ACCESS_KEY_ID }}
          aws-secret-access-key: ${{ secrets.AWS_SECRET_ACCESS_KEY }}
          aws-region: us-east-1

      - name: Login to Amazon ECR
        id: login-ecr
        uses: aws-actions/amazon-ecr-login@v1

      - name: Build and push API image
        env:
          ECR_REGISTRY: ${{ steps.login-ecr.outputs.registry }}
          ECR_REPOSITORY: nrby-api
          IMAGE_TAG: ${{ github.sha }}
        run: |
          docker build -t $ECR_REGISTRY/$ECR_REPOSITORY:$IMAGE_TAG ./api
          docker push $ECR_REGISTRY/$ECR_REPOSITORY:$IMAGE_TAG

      - name: Deploy to ECS
        run: |
          aws ecs update-service --cluster nrby-cluster --service nrby-api-service --force-new-deployment
```

### Environment Configuration

#### Production Environment Variables (API)
```bash
# Database
ConnectionStrings__DefaultConnection=Host=prod-db.amazonaws.com;Database=nrby;Username=***;Password=***

# Keycloak
Keycloak__Authority=https://auth.yourdomain.com/realms/nrby
Keycloak__Audience=nrby-api

# AWS
AWS__S3__BucketName=nrby-media-prod
AWS__Region=us-east-1

# Elasticsearch
ElasticsearchUrl=https://search-nrby-prod.es.amazonaws.com

# Firebase
Firebase__ProjectId=nrby-prod
Firebase__CredentialsPath=/app/firebase-credentials.json

# Email
Email__SmtpHost=smtp.sendgrid.net
Email__SmtpPort=587
Email__FromAddress=noreply@yourdomain.com

# Google Maps
GoogleMaps__ApiKey=***

# Logging
Serilog__WriteTo__0__Name=Console
Serilog__WriteTo__1__Name=File
Serilog__WriteTo__1__Args__path=/app/logs/log-.txt
Serilog__WriteTo__2__Name=Graylog
Serilog__WriteTo__2__Args__hostnameOrAddress=logs.yourdomain.com
```

### Monitoring & Logging

1. **Application Logs**
   - Serilog with structured logging
   - Ship to Graylog or CloudWatch Logs
   - Include correlation IDs for request tracing

2. **Performance Monitoring**
   - Application Insights or New Relic
   - Track API response times
   - Monitor database query performance
   - Alert on error rate spikes

3. **Health Checks**
   - Implement ASP.NET Core health check endpoints
   - Monitor database connectivity
   - Check Elasticsearch status
   - Verify S3 access

4. **Metrics**
   - Track active users
   - Monitor smartpin creation rate
   - Assignment completion metrics
   - Mobile app usage statistics

---

## Best Practices & Patterns

### Backend Best Practices

1. **Use DTOs for API Contracts**
   - Never expose database entities directly
   - Use AutoMapper for entity-to-DTO mapping
   - Validate DTOs with FluentValidation

2. **Async/Await Throughout**
   - All API methods should be async
   - Use `async Task<IActionResult>` pattern
   - Avoid blocking calls (`.Result`, `.Wait()`)

3. **Proper Error Handling**
   ```csharp
   public class ErrorHandlingMiddleware
   {
       public async Task InvokeAsync(HttpContext context, RequestDelegate next)
       {
           try
           {
               await next(context);
           }
           catch (NotFoundException ex)
           {
               context.Response.StatusCode = 404;
               await context.Response.WriteAsJsonAsync(new { error = ex.Message });
           }
           catch (ValidationException ex)
           {
               context.Response.StatusCode = 400;
               await context.Response.WriteAsJsonAsync(new { errors = ex.Errors });
           }
           catch (Exception ex)
           {
               _logger.LogError(ex, "Unhandled exception");
               context.Response.StatusCode = 500;
               await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
           }
       }
   }
   ```

4. **Repository Pattern (Optional)**
   - Abstract data access behind repositories
   - Makes unit testing easier
   - Enables switching ORMs if needed

5. **Use Dependency Injection**
   - Register all services in `Program.cs`
   - Use scoped lifetime for DbContext
   - Use singleton for stateless services

### Frontend Best Practices

1. **Component Structure**
   - Keep components small and focused
   - Use custom hooks to extract logic
   - Prefer composition over inheritance

2. **State Management**
   - Use React Query/SWR for server state
   - Use Context API for global client state
   - Avoid prop drilling

3. **Performance Optimization**
   - Lazy load routes with `React.lazy()`
   - Memoize expensive computations (`useMemo`)
   - Debounce search inputs
   - Implement virtual scrolling for large lists

4. **Type Safety**
   - Define TypeScript interfaces for all API responses
   - Use strict mode in tsconfig.json
   - Avoid `any` type

5. **Error Boundaries**
   ```typescript
   class ErrorBoundary extends React.Component {
       state = { hasError: false };

       static getDerivedStateFromError(error) {
           return { hasError: true };
       }

       componentDidCatch(error, info) {
           logErrorToService(error, info);
       }

       render() {
           if (this.state.hasError) {
               return <ErrorFallback />;
           }
           return this.props.children;
       }
   }
   ```

### Mobile Best Practices

1. **Offline-First Design**
   - Always assume network is unreliable
   - Queue all write operations
   - Provide clear sync status indicators

2. **Battery & Performance**
   - Debounce GPS updates
   - Stop location tracking when app in background (unless required)
   - Compress images before upload
   - Limit SQLite query complexity

3. **User Experience**
   - Use native UI components (Ionic)
   - Provide haptic feedback
   - Show loading skeletons instead of spinners
   - Implement pull-to-refresh

4. **Testing on Real Devices**
   - Test on multiple Android versions
   - Test on various iOS devices
   - Test offline scenarios thoroughly
   - Test low battery mode behavior

### Database Best Practices

1. **Indexing Strategy**
   - Index foreign keys
   - Index columns used in WHERE clauses
   - Use partial indexes for filtered queries
   - Monitor slow query log

2. **Query Optimization**
   - Use `.AsNoTracking()` for read-only queries
   - Select only needed columns
   - Use pagination (`.Skip().Take()`)
   - Avoid N+1 queries (use `.Include()`)

3. **Migrations**
   - Never edit existing migrations
   - Test migrations on copy of production data
   - Create rollback scripts
   - Use transactions for multi-step migrations

4. **Backup Strategy**
   - Automated daily backups
   - Test restore process monthly
   - Keep backups for 30 days minimum
   - Store backups in separate region

---

## Customization & Extension Points

### How to Adapt This Blueprint

This blueprint is designed to be **context-agnostic**. You can adapt it for various use cases:

#### Example 1: Smart City Infrastructure Management
**Smartpins become:** Streetlights, traffic signals, benches, trash cans
**Attributes:** Maintenance status, installation date, manufacturer, wattage
**Assignments:** Repair requests, inspection tasks
**Jurisdictions:** City districts, neighborhoods

#### Example 2: Environmental Monitoring Network
**Smartpins become:** Sensor stations, sampling sites, monitoring points
**Attributes:** Sensor readings, water quality metrics, pollution levels
**Assignments:** Sample collection tasks, calibration schedules
**Jurisdictions:** Watersheds, monitoring zones

#### Example 3: Real Estate Management
**Smartpins become:** Properties, units, buildings
**Attributes:** Square footage, bedrooms, rent amount, lease dates
**Assignments:** Maintenance requests, showing appointments
**Jurisdictions:** Neighborhoods, property management zones

#### Example 4: Fleet & Asset Tracking
**Smartpins become:** Vehicles, equipment, tools
**Attributes:** VIN, license plate, maintenance records, fuel type
**Assignments:** Maintenance schedules, delivery routes
**Jurisdictions:** Service territories, depots

### Key Customization Points

1. **Rename "Smartpin"** to your domain entity (e.g., Asset, Site, Location, Property)
2. **Define custom attribute sets** for your specific data collection needs
3. **Configure map defaults** (center point, zoom level, marker icons)
4. **Customize workflows** with instruction templates and triggers
5. **Brand the UI** with your logo, colors, and terminology
6. **Add domain-specific services** (e.g., weather API integration for environmental monitoring)

---

## Glossary of Terms

- **Smartpin:** A geographic point with custom attributes (the core entity)
- **Region Context:** Hierarchical container for organizing smartpins
- **Attribute:** Custom field definition (e.g., "Serial Number" of type text)
- **Attribute Set:** Group of related attributes (e.g., "Electrical Equipment" attribute set)
- **Attribute Value:** Specific data stored for an attribute on a smartpin
- **Assignment:** Task assigned to a user, optionally linked to a smartpin
- **Jurisdiction:** Geographic boundary defining territories or zones
- **Customer:** Tenant organization in multi-tenant setup
- **App Role:** Role defining permissions (Admin, Supervisor, Field Worker)
- **Instruction:** Template defining reusable task steps
- **Trigger:** Automated action based on conditions (e.g., send notification when status changes)
- **Smartline:** Geographic line feature (e.g., roads, pipelines)
- **KML:** Keyhole Markup Language - geospatial data format

---

## Conclusion

This blueprint provides a comprehensive foundation for building a **production-grade field operations management platform**. The NRBY2 architecture demonstrates:

✅ **Scalable multi-tier architecture** with clear separation of concerns
✅ **Offline-capable mobile apps** for field workers
✅ **Flexible data model** adaptable to any industry
✅ **Enterprise authentication** with Keycloak SSO
✅ **Geospatial intelligence** with PostGIS and mapping
✅ **Real-time updates** via SignalR and Firebase
✅ **Background job processing** for async operations
✅ **Multi-tenant architecture** for SaaS deployment

### Next Steps

1. **Define your domain:** What are your "smartpins"? What attributes do they have?
2. **Choose your stack:** Follow this blueprint or adapt technologies to your team's expertise
3. **Start with MVP:** Implement Phase 1 foundation, then iterate
4. **Prioritize features:** Not all features are needed on day one
5. **Plan for scale:** Start simple, but architect for growth

### Success Metrics

- **Web app load time:** < 2 seconds
- **API response time:** < 200ms (90th percentile)
- **Mobile app offline capability:** 100% core features work offline
- **Uptime:** 99.9% availability
- **Data accuracy:** Zero data loss with sync conflicts properly resolved

---

**Document Version:** 1.0
**Based on:** NRBY2 Platform Analysis (December 2025)
**License:** Internal Use - Adapt as Needed

For questions or clarifications, refer to the original NRBY2 source code at `nrby2/nrby2/`.
