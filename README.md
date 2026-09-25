# RecordShop API

A clean, modular ASP.NET Core Web API for managing music records.

This project demonstrates a full CRUD system using MVC patterns, a service layer, repository abstraction, and an EF Core in‑memory database. It showcases my general skills in API development.

# Core Features

- Retrieve all music records
- Retrieve a single record by ID
- Add new records
- Update existing records
- Delete records
- JSON‑based seed data
- EF Core with SQLite in‑memory provider
- MYSQL Database connection checker and setup functions


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

#### Setting up MySQL

The Production profile needs a running MySQL 8 server with an existing `recordshop` database. Pick one of these options:

**Option A: Docker (quickest)**

```bash
docker run --name recordshop-mysql \
  -e MYSQL_ROOT_PASSWORD=yourpassword \
  -e MYSQL_DATABASE=recordshop \
  -p 3306:3306 -d mysql:8.0
```

This starts MySQL on `localhost:3306` and creates the `recordshop` database for you. Use `docker stop recordshop-mysql` and `docker start recordshop-mysql` to stop it and start it again later.

**Option B: Local install**

Install [MySQL Community Server 8](https://dev.mysql.com/downloads/mysql/), then create the database:

```sql
CREATE DATABASE recordshop;
```

The database has to exist before you start the app. The startup connection check connects to it directly and fails with "Unknown database" if it's missing.

#### Running in Production

1. Copy `RecordShop/appsettings.Production.example.json` to `RecordShop/appsettings.Production.json`. Git ignores this file.
2. Fill in your MySQL connection string, using the details from the setup above:

   ```json
   "DefaultConnection": "server=localhost;port=3306;database=recordshop;user=root;password=yourpassword;"
   ```
3. Set a real JWT key (at least 32 characters). The recommended way is an environment variable, so the key never sits in the project folder:
   - Bash: `export Jwt__Key="your-long-random-key"`
   - PowerShell: `$env:Jwt__Key = "your-long-random-key"`
   - Alternatively, replace the `Jwt:Key` placeholder in `appsettings.Production.json`.

   You can generate a key with `openssl rand -base64 48`. The app refuses to start in Production if the key is missing, too short, or still one of the placeholder keys.
4. Then run the project by typing in the following command in your terminal:

```bash
dotnet run --launch-profile Production
```

On startup, the app:

1. Checks the JWT settings. It stops if the key is missing, too short, or a placeholder.
2. Checks that it can connect to MySQL. It stops with `❌ Application startup aborted — cannot reach database.` if it can't.
3. Applies any pending migrations. On the first run, this creates the `MusicRecords` table.
4. Seeds the table from `Resources/MusicRecordData.json` if it's empty.


### Configuration files

| File                                  | Committed | Purpose                                   |
|---------------------------------------|-----------|-------------------------------------------|
| `appsettings.json`                    | Yes       | Shared settings: logging, JWT issuer/audience |
| `appsettings.Development.json`        | Yes       | SQLite in-memory connection string, dev-only JWT key |
| `appsettings.Production.example.json` | Yes       | Template for production settings          |
| `appsettings.Production.json`         | No        | Your real MySQL connection string and secrets |


# Database & Migrations

Development and Production create the database table in different ways:

| Mode        | Database          | How the table is created                                  | Uses `Migrations/`? |
|-------------|-------------------|-----------------------------------------------------------|---------------------|
| Development | SQLite in-memory  | `EnsureCreated()` builds it straight from the model        | No                  |
| Production  | MySQL             | `Migrate()` applies the migrations in `RecordShop/Migrations` | Yes             |

**Don't delete the `Migrations` folder.** Production depends on it to create the tables. The folder contains:

- `<timestamp>_InitialCreate.cs`: the migration itself. It creates the `MusicRecords` table.
- `<timestamp>_InitialCreate.Designer.cs`: metadata generated by EF Core. Don't edit it.
- `MyDbContextModelSnapshot.cs`: EF Core's picture of the current model, used to work out what changed when you add the next migration.

You don't need to run `dotnet ef database update`, because the app applies pending migrations itself when it starts in Production.

### Changing the model

If you change `MusicRecordModel` (for example, add a property), add a new migration so that Production picks up the change:

```bash
# One-time: install the EF Core CLI tools
dotnet tool install --global dotnet-ef --version 8.*

# From the repository root
dotnet ef migrations add <DescriptiveName> --project RecordShop
```

Development won't warn you if you forget this step: `EnsureCreated()` always builds the table from the current model, so everything looks fine locally.

The `dotnet ef` tools use `RecordShop/MyDbContextFactory.cs` to build the database context at design time. It has a placeholder MySQL connection string, and creating a migration doesn't need a running database. The app itself never uses this factory.
