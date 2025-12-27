# Marka Platform - Architecture Documentation

## Table of Contents
1. [Overview](#overview)
2. [Current Implementation Status](#current-implementation-status)
3. [Permission-Based Role System](#permission-based-role-system)
4. [Database Schema](#database-schema)
5. [API Endpoints](#api-endpoints)
6. [Authentication & Authorization](#authentication--authorization)
7. [Future Roadmap](#future-roadmap)

---

## Overview

Marka is a multi-tenant field operations platform that allows organizations to manage location-based markers (markas), assign tasks, and track field operations. The platform supports web and mobile applications with a flexible, permission-based role system.

### Technology Stack
- **Backend**: .NET 10, Entity Framework Core, PostgreSQL
- **Frontend**: React 18, TypeScript, Vite, Tailwind CSS
- **Authentication**: JWT (JSON Web Tokens)
- **Maps**: Google Maps API

---

## Current Implementation Status

### ✅ Completed Features

1. **Multi-Tenant Infrastructure**
   - Customer model with contact information
   - Customer isolation for data
   - Soft delete support

2. **User Management**
   - User CRUD operations
   - Email/password authentication with BCrypt
   - JWT token-based authentication
   - User roles: SuperAdmin, CustomerAdmin, User

3. **Marka Management**
   - Create, read, update, delete markas
   - Google Maps integration
   - Location-based data (latitude/longitude)
   - Custom attributes system
   - Category and status filtering

4. **Permission System Models**
   - Permission model with categories
   - CustomRole model for customer-specific roles
   - RolePermission junction table
   - User-to-CustomRole relationship
   - Database migration created

5. **Controllers**
   - AuthController (login, register)
   - CustomersController (SuperAdmin only)
   - UsersController (role-based access)
   - MarkasController (CRUD operations)

### 🚧 In Progress

1. **Permission System Implementation**
   - Permission seeding ✅
   - RolesController for role management
   - PermissionsController for listing permissions
   - Permission-based authorization attributes
   - Update controllers to use permission checks

2. **Frontend**
   - Admin dashboard UI
   - Role management interface
   - User management interface
   - Customer management interface

### 📋 Planned Features

1. **Assignment System**
   - Create assignments linked to markas
   - Assign tasks to field users
   - Track assignment status and completion
   - Mobile app support for field workers

2. **Reporting & Analytics**
   - View reports by customer
   - Export functionality
   - Dashboard with metrics

3. **Mobile Application**
   - React Native or similar
   - Offline support
   - GPS tracking
   - Assignment completion

---

## Permission-Based Role System

### Architecture

The platform uses a **three-tier role system**:

1. **SuperAdmin** (Platform Level)
   - Full system access
   - Can create and manage customers
   - Can create other SuperAdmins
   - Not tied to any customer

2. **CustomerAdmin** (Customer Level)
   - Full access within their customer
   - Can create and manage custom roles
   - Can create and manage users
   - Can assign permissions to roles

3. **User** (Custom Role Based)
   - Assigned a custom role created by CustomerAdmin
   - Permissions defined by their custom role
   - Can access web, mobile, or both based on permissions

### Permission Categories

```typescript
Access Permissions:
- Access.Web - Access web application
- Access.Mobile - Access mobile application

Marka Permissions:
- Marka.View - View markas
- Marka.Create - Create new markas
- Marka.Edit - Edit existing markas
- Marka.Delete - Delete markas

Assignment Permissions:
- Assignment.View - View assignments
- Assignment.Create - Create assignments
- Assignment.Assign - Assign assignments to users
- Assignment.Complete - Mark assignments complete

Report Permissions:
- Reports.View - View reports
- Reports.Export - Export reports

User Management Permissions:
- Users.View - View other users
- Users.Create - Create new users
- Users.Edit - Edit user details
- Users.Delete - Delete users

Role Management Permissions:
- Roles.View - View roles
- Roles.Create - Create new roles
- Roles.Edit - Edit role permissions
- Roles.Delete - Delete roles

Customer Management Permissions (SuperAdmin only):
- Customers.View - View all customers
- Customers.Create - Create new customers
- Customers.Edit - Edit customer details
- Customers.Delete - Delete customers
```

### Example Use Cases

**Example 1: Field Agent Role**
```json
{
  "roleName": "Field Agent",
  "permissions": [
    "Access.Mobile",
    "Marka.View",
    "Assignment.View",
    "Assignment.Complete"
  ]
}
```
- Can only use mobile app
- Can view markas
- Can see and complete assignments
- Cannot create markas or manage users

**Example 2: Dispatcher Role**
```json
{
  "roleName": "Dispatcher",
  "permissions": [
    "Access.Web",
    "Marka.View",
    "Marka.Create",
    "Assignment.View",
    "Assignment.Create",
    "Assignment.Assign"
  ]
}
```
- Can use web application
- Can view and create markas
- Can create and assign assignments to field agents

**Example 3: Manager Role**
```json
{
  "roleName": "Manager",
  "permissions": [
    "Access.Web",
    "Access.Mobile",
    "Marka.View",
    "Marka.Create",
    "Marka.Edit",
    "Assignment.View",
    "Assignment.Create",
    "Assignment.Assign",
    "Reports.View",
    "Reports.Export",
    "Users.View"
  ]
}
```
- Can use both web and mobile
- Full marka management
- Full assignment management
- Can view reports and users

---

## Database Schema

### Core Tables

#### Customers
```sql
- Id (Guid, PK)
- Name (string, required, indexed)
- ContactName (string)
- ContactEmail (string)
- ContactPhone (string)
- IsActive (bool)
- IsDeleted (bool)
- DeletedAt (DateTime?)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
```

#### Users
```sql
- Id (Guid, PK)
- Email (string, required, unique, indexed)
- PasswordHash (string, required)
- FirstName (string)
- LastName (string)
- Role (enum: SuperAdmin=2, CustomerAdmin=1, User=0)
- CustomRoleId (Guid?, FK to CustomRoles) -- NULL for SuperAdmin/CustomerAdmin
- CustomerId (Guid, FK to Customers)
- IsActive (bool)
- IsDeleted (bool)
- DeletedAt (DateTime?)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
```

#### Permissions
```sql
- Id (Guid, PK)
- Code (string, required, unique, indexed) -- e.g., "Access.Web"
- Name (string, required) -- e.g., "Access Web Application"
- Description (string)
- Category (string, indexed) -- e.g., "Access", "Marka", "Assignment"
- IsActive (bool)
- CreatedAt (DateTime)
```

#### CustomRoles
```sql
- Id (Guid, PK)
- Name (string, required) -- e.g., "Field Agent", "hello1"
- Description (string)
- CustomerId (Guid, FK to Customers, indexed)
- IsActive (bool)
- IsDeleted (bool)
- DeletedAt (DateTime?)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
- Unique Index: (CustomerId, Name)
```

#### RolePermissions (Junction Table)
```sql
- Id (Guid, PK)
- CustomRoleId (Guid, FK to CustomRoles)
- PermissionId (Guid, FK to Permissions)
- CreatedAt (DateTime)
- Unique Index: (CustomRoleId, PermissionId)
```

#### Markas
```sql
- Id (Guid, PK)
- Name (string, required)
- Description (string)
- Latitude (double, required, indexed)
- Longitude (double, required, indexed)
- Address (string)
- Category (string, indexed)
- Status (string, indexed, default: "Active")
- CustomerId (Guid, FK to Customers, indexed)
- CreatedByUserId (Guid, FK to Users)
- DeletedAt (DateTime?)
- CreatedAt (DateTime)
- UpdatedAt (DateTime)
```

### Relationships

```
Customer (1) ----< (N) Users
Customer (1) ----< (N) CustomRoles
Customer (1) ----< (N) Markas

User (N) >---- (1) CustomRole [optional]
User (N) >---- (1) Customer

CustomRole (N) >---- (1) Customer
CustomRole (1) ----< (N) RolePermissions
CustomRole (1) ----< (N) Users

Permission (1) ----< (N) RolePermissions

RolePermission (N) >---- (1) CustomRole
RolePermission (N) >---- (1) Permission
```

---

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register new user
- `POST /api/auth/login` - Login and get JWT token
- `GET /api/auth/me` - Get current user info

### Customers (SuperAdmin only)
- `GET /api/customers` - List all customers
- `GET /api/customers/{id}` - Get customer details
- `POST /api/customers` - Create new customer
- `PUT /api/customers/{id}` - Update customer
- `DELETE /api/customers/{id}` - Soft delete customer

### Users
- `GET /api/users` - List users (filtered by role)
- `GET /api/users/{id}` - Get user details
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Soft delete user

### Markas
- `GET /api/markas` - List markas (customer-isolated)
- `GET /api/markas/{id}` - Get marka details
- `POST /api/markas` - Create new marka
- `PUT /api/markas/{id}` - Update marka
- `DELETE /api/markas/{id}` - Soft delete marka

### Roles (To be implemented)
- `GET /api/roles` - List custom roles for customer
- `GET /api/roles/{id}` - Get role details with permissions
- `POST /api/roles` - Create new custom role
- `PUT /api/roles/{id}` - Update role
- `DELETE /api/roles/{id}` - Soft delete role
- `POST /api/roles/{id}/permissions` - Assign permissions to role
- `DELETE /api/roles/{id}/permissions/{permissionId}` - Remove permission from role

### Permissions (To be implemented)
- `GET /api/permissions` - List all available permissions
- `GET /api/permissions/categories` - Get permissions grouped by category

---

## Authentication & Authorization

### JWT Token Structure
```json
{
  "sub": "user-guid",
  "email": "user@example.com",
  "role": "SuperAdmin|CustomerAdmin|User",
  "customerId": "customer-guid",
  "customRoleId": "role-guid", // Only for User role
  "exp": 1234567890
}
```

### Authorization Flow

1. **User Login**
   - User submits email/password
   - Backend verifies credentials
   - JWT token generated with user claims
   - Token includes: userId, email, role, customerId, customRoleId (if applicable)

2. **API Request**
   - Client sends JWT in Authorization header: `Bearer <token>`
   - Backend validates token
   - Extracts user claims

3. **Permission Check**
   - If SuperAdmin: Grant access (bypass permission checks)
   - If CustomerAdmin: Check endpoint-specific rules
   - If User: Load user's custom role → Load role's permissions → Check required permission

4. **Customer Isolation**
   - All queries filtered by CustomerId from JWT
   - Prevents cross-customer data access

### Permission Check Implementation (Planned)

```csharp
[RequirePermission(PermissionCodes.MarkaCreate)]
public async Task<ActionResult> CreateMarka([FromBody] CreateMarkaRequest request)
{
    // Permission already checked by attribute
    // Proceed with creation
}
```

---

## Future Roadmap

### Phase 1: Complete Permission System (Current Sprint)
- [ ] Create RolesController
- [ ] Create PermissionsController
- [ ] Implement permission-based authorization attributes
- [ ] Update existing controllers to use permission checks
- [ ] Add customer isolation to markas queries
- [ ] Create admin dashboard (frontend)
- [ ] Test complete role-based access

### Phase 2: Assignment System
- [ ] Design Assignment model
- [ ] Create assignment database schema
- [ ] Implement AssignmentsController
- [ ] Assignment status tracking
- [ ] Mobile app integration

### Phase 3: Reporting & Analytics
- [ ] Design report structure
- [ ] Implement reporting queries
- [ ] Create ReportsController
- [ ] Export functionality (PDF, Excel)
- [ ] Dashboard with charts

### Phase 4: Mobile Application
- [ ] Choose mobile framework (React Native, Flutter)
- [ ] Implement mobile authentication
- [ ] GPS tracking for field agents
- [ ] Offline mode with sync
- [ ] Assignment completion workflow

### Phase 5: Advanced Features
- [ ] Real-time notifications (SignalR)
- [ ] File attachments for markas
- [ ] Photo upload for assignments
- [ ] Geofencing
- [ ] Route optimization

---

## Test Credentials

### SuperAdmin
- Email: admin@marka.com
- Password: password123
- Access: Full platform access

### Customer Admin (Acme Corporation)
- Email: jane.smith@acme.com
- Password: acme123
- Access: Full access to Acme Corporation

### Regular User (Acme Corporation)
- Email: bob.johnson@acme.com
- Password: user123
- Access: Limited access to Acme Corporation

---

## Notes for Developers

### Adding New Permissions

1. Add permission code to `PermissionCodes` class
2. Add permission to seed data in `SeedPermissions.cs`
3. Run migration to update database
4. Use `[RequirePermission]` attribute on controller actions

### Creating Custom Roles

CustomerAdmins can create custom roles through the Roles API:

```typescript
POST /api/roles
{
  "name": "Field Supervisor",
  "description": "Supervises field agents",
  "permissionIds": [
    "guid-for-access-mobile",
    "guid-for-marka-view",
    "guid-for-assignment-assign"
  ]
}
```

### Multi-Platform Access Control

Users can be restricted to specific platforms:
- **Web Only**: Only `Access.Web` permission
- **Mobile Only**: Only `Access.Mobile` permission
- **Both**: Both `Access.Web` and `Access.Mobile` permissions

This allows fine-grained control over where users can access the system.

---

**Last Updated**: 2025-12-27
**Version**: 1.0.0
