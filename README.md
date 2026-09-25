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

The project runs on .NET 8. To install the SDK, please check Microsoft's official page here https://dotnet.microsoft.com/en-us/download/dotnet/8.0 


The app has two launch profiles, defined in `RecordShop/Properties/launchSettings.json`:

| Profile       | Environment   | Database                  |
|---------------|---------------|---------------------------|
| `Development` | Development   | SQLite in-memory (seeded) |
| `Production`  | Production    | MySQL                     |

### Development (default)

```bash
dotnet run --launch-profile Development
```

Then open http://localhost:5125. Running `dotnet run` with no profile also uses `Development`.

To get a JWT token, send `POST /api/auth/token` with the dev credentials using a website like [Postman](https://www.postman.com). This will authenticate you login, allowing you to use HTTP requests that need Authentication.

Use the following loging credentials to log in (Or you can change the username and password in Controllers/AuthControllers.cs file)

**The credentials are just placeholders or intended to be temporary.**

```json
{ "username": "mohamed@waveform.com", "password": "password123" }
```

### Production

If you intend to run the project using MYSQL, follow the instructions below.

1. Copy `RecordShop/appsettings.Production.example.json` to `RecordShop/appsettings.Production.json`. Git ignores this file.
2. Fill in your MySQL connection string.
3. Set a real JWT key (at least 32 characters). The recommended way is an environment variable, so the key never sits in the project folder:
   - Bash: `export Jwt__Key="your-long-random-key"`
   - PowerShell: `$env:Jwt__Key = "your-long-random-key"`
   - Alternatively, replace the `Jwt:Key` placeholder in `appsettings.Production.json`.

   You can generate a key with `openssl rand -base64 48`. The app refuses to start in Production if the key is missing, too short, or still one of the placeholder keys.
4. Then run the project by typing in the following command in your terminal:

```bash
dotnet run --launch-profile Production
```

On startup, the app checks the MySQL connection. 


### Configuration files

| File                                  | Committed | Purpose                                   |
|---------------------------------------|-----------|-------------------------------------------|
| `appsettings.json`                    | Yes       | Shared settings: logging, JWT issuer/audience |
| `appsettings.Development.json`        | Yes       | SQLite in-memory connection string, dev-only JWT key |
| `appsettings.Production.example.json` | Yes       | Template for production settings          |
| `appsettings.Production.json`         | No        | Your real MySQL connection string and secrets |
