# Multi-Tenant Task Management System

## ⚙️ Configuration

1. **Update Connection String**

Open `appsettings.Development.json` or `appsettings.json` and replace:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SQL_SERVER;Database=TaskDb;Trusted_Connection=True;TrustServerCertificate=True;"
}

Run these commands in the terminal:
# Add Migration (if not already created)
dotnet ef migrations add InitialCreate -s Multi_Tenant_Task_Management_System.API

# Apply Migration
dotnet ef database update -s Multi_Tenant_Task_Management_System.API
