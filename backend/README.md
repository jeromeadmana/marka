# Marka Backend API

.NET 10 Web API for the Marka field operations platform.

## Setup Instructions

### 1. Configure Database Connection

Edit `appsettings.Development.json` and replace the placeholders with your Aiven PostgreSQL credentials:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=YOUR_AIVEN_HOST;Port=YOUR_PORT;Database=defaultdb;Username=avnadmin;Password=YOUR_PASSWORD;SSL Mode=Require;Trust Server Certificate=true"
}
```

**Where to find these values in Aiven:**
- Go to your PostgreSQL service in Aiven dashboard
- Click on "Overview" tab
- Copy the connection details:
  - **Host:** service-name.a.aivencloud.com
  - **Port:** Usually shown (e.g., 12345)
  - **Database:** defaultdb
  - **Username:** avnadmin
  - **Password:** Click "Show password" to reveal

**Example connection string:**
```
Host=marka-dev-project.a.aivencloud.com;Port=23456;Database=defaultdb;Username=avnadmin;Password=AVNS_abc123xyz;SSL Mode=Require;Trust Server Certificate=true
```

### 2. Enable PostGIS Extension

In Aiven dashboard:
1. Go to your service
2. Click "Extensions" tab (or connect via SQL client)
3. Enable `postgis` extension

Or run this SQL:
```sql
CREATE EXTENSION IF NOT EXISTS postgis;
```

### 3. Test Connection

```bash
dotnet run
```

If successful, you should see:
- API starts at https://localhost:7xxx
- Swagger UI available at https://localhost:7xxx/swagger

## Project Structure

```
Marka.Api/
├── Controllers/       # API endpoints
├── Models/           # Database entities
├── Data/             # DbContext and migrations
├── Services/         # Business logic
├── Middleware/       # Custom middleware
├── Program.cs        # Application entry point
└── appsettings.json  # Configuration
```

## Next Steps

- [ ] Configure database connection
- [ ] Create DbContext
- [ ] Create first models (User, Customer, Marka)
- [ ] Generate first migration
- [ ] Test database connection
