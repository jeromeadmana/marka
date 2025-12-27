# Marka - Quick Start Guide

## What We're Building Today (Week 1)

Setting up the foundation for the Marka platform - getting all tools, accounts, and initial projects ready.

---

## Step-by-Step Setup (Do in Order)

### 1. Verify Your Tools (5 minutes)

Open terminal/command prompt and run:

```bash
# Check .NET
dotnet --version
# Need: 8.0.x - If not installed: https://dotnet.microsoft.com/download/dotnet/8.0

# Check Node.js
node --version
# Need: 20.x+ - If not installed: https://nodejs.org/

# Check Git
git --version
# Need: Any recent version

# Check Docker
docker --version
# Need: Latest - If not installed: https://www.docker.com/products/docker-desktop/
```

---

### 2. Get Aiven PostgreSQL Database (15-20 minutes)

**Why:** We need a PostgreSQL database with PostGIS for storing markas (location data).

1. Go to https://aiven.io/
2. Sign up (email or GitHub)
3. Click "Create Service"
4. Select **PostgreSQL**
5. Choose **Free tier** (Hobbyist - 1 CPU, 1GB RAM)
6. Select region closest to you
7. Name it: `marka-dev`
8. Click "Create service"
9. Wait 2-3 minutes for provisioning
10. Once running, go to **Overview** tab
11. Copy connection details:
    - Host
    - Port
    - User
    - Password
    - Database name
12. **Enable PostGIS:**
    - Go to "Extensions" tab (or use SQL)
    - Enable `postgis` extension
    - Run: `CREATE EXTENSION IF NOT EXISTS postgis;`

**Save this connection string format:**
```
Host=<your-host>;Port=<port>;Database=defaultdb;Username=avnadmin;Password=<password>;SSL Mode=Require;Trust Server Certificate=true
```

**Test connection:**
- Download DBeaver (free): https://dbeaver.io/
- Create new PostgreSQL connection
- Paste your Aiven details
- Test connection - should succeed!

---

### 3. Get Google Maps API Key (10 minutes)

**Why:** We need Google Maps to display markas on a map.

1. Go to https://console.cloud.google.com/
2. Sign in with Google account
3. Create new project: "Marka"
4. Enable **Maps JavaScript API**:
   - Search "Maps JavaScript API" in search bar
   - Click "Enable"
5. Create credentials:
   - Go to "Credentials" (left sidebar)
   - Click "Create Credentials" → "API Key"
   - Copy the API key
6. Restrict the key (security):
   - Click "Edit API key"
   - Under "Application restrictions": Choose "HTTP referrers"
   - Add: `http://localhost:*` and `http://127.0.0.1:*`
   - Under "API restrictions": Restrict to "Maps JavaScript API"
   - Save

**Save your API key securely!**

---

### 4. Create GitHub Repository (5 minutes)

1. Go to https://github.com/
2. Sign in (or create account)
3. Click "New repository"
4. Name: `marka`
5. Choose Public or Private
6. **Don't** initialize with README (we have one!)
7. Click "Create repository"
8. Copy the remote URL (HTTPS or SSH)

**Link your local repo:**
```bash
cd c:\personal-source\marka
git remote add origin <your-github-url>
git branch -M main
git push -u origin main
```

---

### 5. Initialize Backend (.NET API) (20 minutes)

```bash
# Navigate to backend folder
cd c:\personal-source\marka\backend

# Create new .NET Web API
dotnet new webapi -n Marka.Api

# Go into project
cd Marka.Api

# Install packages (one by one to see progress)
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL.NetTopologySuite
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Serilog.AspNetCore
dotnet add package FluentValidation.AspNetCore

# Build to verify
dotnet build

# Run the API
dotnet run
```

**Should see:** API running at `https://localhost:7xxx`

Open browser to: `https://localhost:7xxx/swagger`

You should see Swagger UI with default WeatherForecast endpoint!

**Stop the server:** Press `Ctrl+C` in terminal

---

### 6. Initialize Frontend (React) (20 minutes)

```bash
# Navigate to frontend folder
cd c:\personal-source\marka\frontend

# Create Vite + React + TypeScript app
npm create vite@latest . -- --template react-ts

# Install dependencies
npm install

# Install additional packages
npm install react-router-dom @tanstack/react-query axios
npm install react-hook-form zod @hookform/resolvers
npm install date-fns @heroicons/react

# Install Tailwind CSS
npm install -D tailwindcss postcss autoprefixer
npx tailwindcss init -p
```

**Configure Tailwind:**

Edit `frontend/tailwind.config.js`:
```js
/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {},
  },
  plugins: [],
}
```

Edit `frontend/src/index.css`:
```css
@tailwind base;
@tailwind components;
@tailwind utilities;
```

**Run the dev server:**
```bash
npm run dev
```

**Should see:** App running at `http://localhost:5173`

Open browser - you should see the Vite + React default page!

**Stop the server:** Press `Ctrl+C` in terminal

---

### 7. Commit Everything (5 minutes)

```bash
cd c:\personal-source\marka

# Add all new files
git add .

# Commit
git commit -m "Week 1 complete: Backend and frontend initialized

- .NET 8 Web API with EF Core, PostgreSQL, Serilog
- React + TypeScript + Vite frontend
- Tailwind CSS for styling
- All core dependencies installed
- Development environment ready"

# Push to GitHub
git push
```

---

## ✅ Week 1 Complete!

You should now have:

- ✅ Aiven PostgreSQL database (with PostGIS)
- ✅ Google Maps API key
- ✅ GitHub repository
- ✅ .NET 8 API running locally
- ✅ React frontend running locally
- ✅ All tools installed
- ✅ Everything committed to Git

---

## 🎯 What's Next?

**Week 2:** Database schema design and Entity Framework setup

Check [docs/WEEK_01_CHECKLIST.md](WEEK_01_CHECKLIST.md) for the complete checklist.

---

## 💡 Tips

- **Stuck?** Ask Claude Code for help!
- **Save credentials:** Use a password manager (1Password, LastPass, Bitwarden)
- **Take breaks:** This is 2-3 hours of setup work
- **Test as you go:** Don't skip the verification steps
- **Document issues:** If something doesn't work, write it down

---

## 📞 Common Issues

### Can't connect to Aiven?
- Check firewall
- Verify SSL Mode=Require
- Make sure service is running (green status in Aiven)

### .NET build errors?
- Make sure .NET 8 SDK is installed (not just runtime)
- Try `dotnet restore` first
- Check for conflicting package versions

### npm install fails?
- Make sure Node.js 20+ is installed
- Clear npm cache: `npm cache clean --force`
- Delete node_modules and try again

### Git push fails?
- Check remote URL: `git remote -v`
- Make sure you're authenticated to GitHub
- Try HTTPS instead of SSH (or vice versa)

---

**Good luck! 🚀**
