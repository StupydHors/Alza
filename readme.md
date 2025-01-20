# Product API

A .NET Core Web API demonstrating simple CRUD operations for Alza.
Functionality is implemented using the CQRS pattern with MediatR.

## Prerequisites

- .NET 8.0
- Docker

## Quick Start

1. Start the database:
```bash
docker-compose up postgres
```

2. Run migrations:
```bash
dotnet ef database update -p Alza.Infrastructure -s Alza.Api
```

3. Run the application:
```bash
dotnet run --project Alza.Api
```

You can now access the API manually at `http://localhost:5258/swagger/index.html` through Swagger UI.
You can change application URL in `lauchSettings.json`. Note that I have not tested IIS or HTTPS.

## Database Options

### In-Memory Database
- Default database configuration
- This database is used for development purposes only and does not persist data

### PostgreSQL
Switch to postgres database:
- Setting `"UseInMemoryDatabase": false` in `appsettings.json` or `appsettings.Development.json`
- The connection string can be changed in `appsettings.json` or `appsettings.Development.json`

## Data Seeding
- By default, data seeding is turned on
- Only seeds data if the database is empty
- Setting `"TryGenerateBogusData": false` in `appsettings.json` or `appsettings.Development.json` turns it off

## Testing

Run tests:
```bash
dotnet test
```

Tests cover:
- API endpoints
- CQRS handlers
- Repository operations

Tests are implemented using xUnit and Moq.
Tests cover usually only the happy path, but some tests also cover error cases.

## Docker

There is also a docker image available for the API. It can be launched with:
```bash
docker-compose up alza-api
```

The containerized API can be accessed at `http://localhost:8080/swagger/index.html` through Swagger UI.
The port can be changed in `docker-compose.yml`.