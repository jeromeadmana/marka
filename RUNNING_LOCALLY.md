# Running Marka Locally

Quick guide to run both backend and frontend servers on your local machine.

---

## 🚀 Quick Start

### Option 1: Two Terminals (Recommended)

**Terminal 1 - Backend:**
```bash
cd c:\personal-source\marka\backend\Marka.Api
dotnet run
```

**Terminal 2 - Frontend:**
```bash
cd c:\personal-source\marka\frontend
npm run dev
```

### Option 2: Background Mode

**Start Backend in Background:**
```bash
cd c:\personal-source\marka\backend\Marka.Api
start /B dotnet run
```

**Start Frontend Normally:**
```bash
cd c:\personal-source\marka\frontend
npm run dev
```

---

## 📍 URLs

Once both servers are running:

| Service | URL | Description |
|---------|-----|-------------|
| **Frontend** | http://localhost:5173 | React app (Vite) |
| **Backend API** | http://localhost:5229 | .NET Web API |
| **Swagger UI** | http://localhost:5229/swagger | API documentation (if configured) |
| **OpenAPI** | http://localhost:5229/openapi/v1.json | API schema |

---

## ✅ Verify Everything Works

### 1. Check Backend API
Open browser to: http://localhost:5229/weatherforecast

You should see JSON weather data.

### 2. Check Markas API
Open browser to: http://localhost:5229/api/markas

You should see an empty array `[]` (no markas created yet).

### 3. Check Frontend
Open browser to: http://localhost:5173

You should see the default Vite React page.

---

## 🛠️ Troubleshooting

### Backend won't start

**Error:** `Connection string not found`
- **Fix:** Make sure `backend/Marka.Api/appsettings.Development.json` has your Aiven connection string

**Error:** `Port 5229 already in use`
- **Fix:** Kill the existing process or change port in `launchSettings.json`

### Frontend won't start

**Error:** `Port 5173 already in use`
- **Fix:** Frontend will auto-increment to 5174, or kill existing process

**Error:** `Module not found`
- **Fix:** Run `npm install` in frontend folder

### Database connection fails

**Error:** `Connection refused` or `Timeout`
- **Fix:** Check Aiven service is running
- **Fix:** Verify connection string is correct
- **Fix:** Check firewall/network settings

---

## 🔧 Development Workflow

### Making Backend Changes

1. Edit C# files in `backend/Marka.Api/`
2. Save files
3. dotnet will auto-reload (hot reload)
4. Check terminal for compilation errors

### Making Frontend Changes

1. Edit TypeScript/React files in `frontend/src/`
2. Save files
3. Vite will auto-reload in browser (HMR - Hot Module Replacement)
4. Check browser console for errors

### Database Changes

1. Edit models in `backend/Marka.Api/Models/`
2. Create migration: `dotnet ef migrations add MigrationName`
3. Apply migration: `dotnet ef database update`
4. Restart backend

---

## 📝 Useful Commands

### Backend

```bash
# Build project
dotnet build

# Run project
dotnet run

# Run with watch (auto-reload)
dotnet watch run

# Create migration
dotnet ef migrations add MigrationName

# Apply migration
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

### Frontend

```bash
# Install dependencies
npm install

# Run dev server
npm run dev

# Build for production
npm run build

# Preview production build
npm run preview

# Lint code
npm run lint
```

---

## 🎯 Next Steps

Once both servers are running:

1. **Test the API** - Use Postman or curl to create a test marka
2. **Build the UI** - Start creating React components
3. **Integrate** - Connect frontend to backend API

---

## 💡 Pro Tips

1. **Keep both terminals visible** - Easy to spot errors
2. **Use VS Code** - Open project root, split terminal
3. **Browser DevTools** - F12 to see network requests
4. **Check console** - Both terminal and browser console for errors
5. **Git often** - Commit working code frequently

---

**Last Updated:** December 21, 2025
