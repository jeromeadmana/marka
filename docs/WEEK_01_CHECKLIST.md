# Week 1 Checklist - Project Initialization
**Month 1, Week 1** | **Target:** Project Setup & Architecture

---

## ✅ Completed

- [x] Create project folder structure (backend/, frontend/, docs/, scripts/)
- [x] Initialize Git repository
- [x] Create .gitignore for .NET + React
- [x] Create project README
- [x] Add project planning documents
- [x] Initial commit

---

## 🔲 To Complete This Week

### 1. Development Environment Setup

#### Local Tools Installation
- [ ] Verify .NET 8 SDK installed (`dotnet --version`)
- [ ] Verify Node.js 20+ installed (`node --version`)
- [ ] Install Docker Desktop (for Keycloak)
- [ ] Install IDE/Editor
  - [ ] Visual Studio 2022 (recommended for .NET) OR
  - [ ] VS Code with C# DevKit extension
- [ ] Install database tools
  - [ ] DBeaver or pgAdmin (for PostgreSQL)
  - [ ] Azure Data Studio (optional)
- [ ] Install Postman or Bruno (API testing)

#### Verify Installations
```bash
# Check .NET
dotnet --version  # Should be 8.0.x

# Check Node
node --version    # Should be 20.x or higher
npm --version

# Check Git
git --version

# Check Docker
docker --version
docker-compose --version
```

---

### 2. Cloud Services Setup

#### Aiven PostgreSQL
- [ ] Sign up for Aiven account (free tier) - https://aiven.io/
- [ ] Create new PostgreSQL service
  - [ ] Choose free tier (Hobbyist plan)
  - [ ] Select region closest to you
  - [ ] Enable PostGIS extension
- [ ] Note down connection details:
  - Host
  - Port
  - Database name
  - Username
  - Password
  - SSL mode (required)
- [ ] Test connection from local machine (DBeaver/pgAdmin)
- [ ] Save connection string securely

**Connection String Format:**
```
Host=<hostname>;Port=<port>;Database=<database>;Username=<user>;Password=<password>;SSL Mode=Require;Trust Server Certificate=true
```

#### Google Cloud Platform (for Maps API)
- [ ] Create Google Cloud account (if don't have one)
- [ ] Create new project "Marka"
- [ ] Enable Google Maps JavaScript API
- [ ] Create API key
  - [ ] Restrict to localhost for development
  - [ ] Note: Will need to add production domain later
- [ ] Set up billing alert (optional but recommended)
- [ ] Save API key securely

#### GitHub Repository
- [ ] Create GitHub account (if don't have one)
- [ ] Create new repository "marka"
  - [ ] Public or Private (your choice)
  - [ ] Don't initialize with README (we already have one)
- [ ] Add remote to local repo
- [ ] Push initial commit

```bash
git remote add origin https://github.com/yourusername/marka.git
git branch -M main
git push -u origin main
```

---

### 3. Project Documentation

- [ ] Move planning docs to docs/ folder
  ```bash
  mkdir -p docs
  mv NRBY2_BLUEPRINT.md docs/
  mv MARKA_PROJECT_PLAN.md docs/
  mv PROJECT_PLANNING.md docs/
  ```

- [ ] Create docs/SETUP.md with setup instructions
- [ ] Create docs/DEVELOPMENT.md with development guidelines
- [ ] Create docs/ARCHITECTURE.md (initial draft)

---

### 4. Backend Project Initialization

- [ ] Navigate to backend folder
  ```bash
  cd backend
  ```

- [ ] Create .NET 8 Web API project
  ```bash
  dotnet new webapi -n Marka.Api
  cd Marka.Api
  ```

- [ ] Install core NuGet packages
  ```bash
  # Entity Framework Core + PostgreSQL
  dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
  dotnet add package Microsoft.EntityFrameworkCore.Design
  dotnet add package Microsoft.EntityFrameworkCore.Tools

  # PostGIS support
  dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite
  dotnet add package NetTopologySuite

  # Authentication
  dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer

  # Logging
  dotnet add package Serilog.AspNetCore
  dotnet add package Serilog.Sinks.Console
  dotnet add package Serilog.Sinks.File

  # Validation
  dotnet add package FluentValidation.AspNetCore

  # CORS
  # (built-in)
  ```

- [ ] Test build
  ```bash
  dotnet build
  dotnet run
  ```

- [ ] Verify API runs at https://localhost:7000 (or similar)
- [ ] Open Swagger UI in browser

---

### 5. Frontend Project Initialization

- [ ] Navigate to frontend folder
  ```bash
  cd frontend
  ```

- [ ] Create Vite + React + TypeScript project
  ```bash
  npm create vite@latest . -- --template react-ts
  ```

- [ ] Install core dependencies
  ```bash
  npm install

  # Routing
  npm install react-router-dom

  # State management
  npm install @tanstack/react-query
  npm install axios

  # Forms
  npm install react-hook-form zod @hookform/resolvers

  # UI/Styling
  npm install -D tailwindcss postcss autoprefixer
  npx tailwindcss init -p

  # Icons
  npm install @heroicons/react

  # Date handling
  npm install date-fns
  ```

- [ ] Configure Tailwind CSS
  - [ ] Update tailwind.config.js
  - [ ] Add Tailwind directives to index.css

- [ ] Test dev server
  ```bash
  npm run dev
  ```

- [ ] Verify app runs at http://localhost:5173

---

### 6. Code Repository Setup

- [ ] Create .editorconfig for consistent formatting
- [ ] Set up basic folder structure
  ```
  backend/
    Marka.Api/
      Controllers/
      Models/
      Services/
      Data/
      Middleware/
      Program.cs

  frontend/
    src/
      components/
      pages/
      services/
      utils/
      types/
      App.tsx
      main.tsx
  ```

- [ ] Add placeholder files (empty for now)

---

### 7. Documentation Creation

Create these files in `docs/`:

- [ ] **SETUP.md** - Environment setup instructions
- [ ] **DEVELOPMENT.md** - Development workflow, coding standards
- [ ] **ARCHITECTURE.md** - System architecture overview
- [ ] **DATABASE.md** - Database schema documentation (initial)
- [ ] **API.md** - API documentation (will populate later)

---

### 8. Git Configuration

- [ ] Create .gitattributes for line endings
  ```
  * text=auto
  *.cs text eol=lf
  *.tsx text eol=lf
  *.ts text eol=lf
  *.json text eol=lf
  *.md text eol=lf
  ```

- [ ] Commit backend setup
  ```bash
  git add backend/
  git commit -m "Initialize .NET 8 Web API project"
  ```

- [ ] Commit frontend setup
  ```bash
  git add frontend/
  git commit -m "Initialize React + Vite + TypeScript frontend"
  ```

- [ ] Push to GitHub
  ```bash
  git push
  ```

---

## 🎯 Week 1 Success Criteria

By end of Week 1, you should have:

- ✅ Development environment fully set up
- ✅ Aiven PostgreSQL database created and accessible
- ✅ Google Maps API key obtained
- ✅ .NET 8 Web API running locally
- ✅ React frontend running locally
- ✅ GitHub repository set up and synced
- ✅ Basic project documentation created
- ✅ All tools installed and verified

---

## 📝 Notes

### Time Estimate
- **Total:** 8-12 hours
- **Environment setup:** 2-3 hours
- **Cloud services:** 1-2 hours
- **Backend init:** 2-3 hours
- **Frontend init:** 2-3 hours
- **Documentation:** 1-2 hours

### Tips
- Don't rush - proper setup saves time later
- Test everything as you go
- Save all credentials securely (use password manager)
- Take screenshots during cloud setup for reference
- Ask Claude Code for help if stuck!

---

## Next Week Preview

**Week 2:** Database setup, Entity Framework migrations, initial models

---

**Status:** 🟡 In Progress
**Started:** December 21, 2025
**Target Completion:** December 27, 2025
