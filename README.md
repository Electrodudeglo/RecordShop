# RecordShop API

A clean, modular ASP.NET Core Web API for managing music records.

This project demonstrates a full CRUD system using MVC patterns, a service layer, repository abstraction, and an EF Core in‑memory database. It’s designed to be simple, readable, and ideal for learning or extending into a larger application.

# Core Features

- Retrieve all music records
- Retrieve a single record by ID
- Add new records
- Update existing records
- Delete records
- JSON‑based seed data
- EF Core with SQLite in‑memory provider


# Why This Project Exists
  
To provide a friendly, approachable example of how to structure a real ASP.NET Core API with clean separation of concerns and testable components.


# Running the Project

**Prerequisites:** .NET 8 SDK

The app has two launch profiles, defined in `RecordShop/Properties/launchSettings.json`:

| Profile       | Environment   | Database                  |
|---------------|---------------|---------------------------|
| `Development` | Development   | SQLite in-memory (seeded) |
| `Production`  | Production    | MySQL                     |

### Development (default)

```bash
dotnet run --project RecordShop --launch-profile Development
```

Then open http://localhost:5125/swagger. Running `dotnet run` with no profile also uses `Development`.

To get a JWT token, send `POST /api/auth/token` with the dev credentials:

```json
{ "username": "mohamed@waveform.com", "password": "password123" }
```

### Production

1. Copy `RecordShop/appsettings.Production.example.json` to `RecordShop/appsettings.Production.json`. Git ignores this file.
2. Fill in your MySQL connection string and a real JWT key (at least 32 characters).
3. Run:

```bash
dotnet run --project RecordShop --launch-profile Production
```

On startup the app checks the MySQL connection. If it can't reach the database, it stops with:

```
❌ Database connection failed: ...
❌ Application startup aborted — cannot reach database.
```

### Configuration files

| File                                  | Committed | Purpose                                   |
|---------------------------------------|-----------|-------------------------------------------|
| `appsettings.json`                    | Yes       | Shared settings: logging, JWT issuer/audience, dev JWT key |
| `appsettings.Development.json`        | Yes       | SQLite in-memory connection string        |
| `appsettings.Production.example.json` | Yes       | Template for production settings          |
| `appsettings.Production.json`         | No        | Your real MySQL connection string and secrets |
