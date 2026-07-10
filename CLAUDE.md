# CLAUDE.md

Guidance for Claude Code when working in this repository.

## What this is

**API Colombia** — a public, read-only RESTful API that serves general information about Colombia (geography, government, culture, nature, etc.). It is open, free, and requires **no authentication**. Every endpoint is a `GET`; there are no write operations anywhere in the codebase (CORS is even restricted to the `GET` method only).

- Live API: https://api-colombia.com/swagger/index.html
- Docs site: https://docs.api-colombia.com/

## Tech stack

- **.NET 10**, ASP.NET Core **Minimal APIs** (not MVC controllers).
- **Entity Framework Core 10** with **PostgreSQL** (`Npgsql.EntityFrameworkCore.PostgreSQL`).
- **Swashbuckle/Swagger** for OpenAPI docs (with annotations enabled).
- **Output caching** enabled globally (7-day expiry base policy).
- Tests: **xUnit** + `Microsoft.AspNetCore.Mvc.Testing` (integration tests via `WebApplicationFactory`) + `Microsoft.EntityFrameworkCore.InMemory` + AutoFixture + Moq.

## Solution layout

```
api-colombia/
├── api/                  # main Web API project (working dir)
│   ├── Program.cs        # app bootstrap + registers every route group
│   ├── DBContext.cs      # EF Core DbContext; NoTracking by default
│   ├── Models/           # entity/POCO classes (one per resource)
│   ├── Routes/           # one *Routes.cs static class per resource
│   ├── Data/Configs/     # EF Core IEntityTypeConfiguration per entity
│   ├── Migrations/       # EF Core migrations + seed .sql data files
│   ├── Utils/            # shared helpers (routing consts, sorting, search, pagination)
│   ├── Const/Version.cs  # API version string shown in Swagger
│   └── wwwroot/          # static landing page
├── api.Tests/            # integration + unit tests
└── docs/                 # VitePress documentation site (Node.js)
```

## Architecture & conventions

**Route registration.** Each resource has a `Routes/<Resource>Routes.cs` file exposing a static `Register<Resource>API(WebApplication app)` method. `Program.cs` calls every one of these after building the app. To add a resource, create the routes class and add its `Register...` call in `Program.cs`.

**Route pattern.** Routes are built from constants in `Utils/Util.cs`:
`API_ROUTE` (`api/`) + `API_VERSION` (`v1/`) + `<RESOURCE>_ROUTE`. So endpoints look like `api/v1/City`, `api/v1/Department/{id}/cities`, etc. Each group uses `.WithTags(...)` for Swagger grouping.

**Common endpoint shapes** most resources implement (not all have every one):
- `GET ""` — list all (supports `?sortBy=&sortDirection=asc|desc`)
- `GET /{id}` — by id (returns `400` for id ≤ 0, `404` if missing)
- `GET /name/{name}` — by name
- `GET /search/{keyword}` — keyword search across string fields
- `GET /pagedList?page=1&pageSize=10` — paginated (supports sorting)
- Sub-resource routes, e.g. `GET /{id}/cities`, `/{id}/naturalareas`

**Shared helpers in `Utils/`:**
- `Functions.ApplySorting<T>(query, sortBy, sortOrder)` — reflection-based dynamic sort; returns `(query, isValidSort)`. Invalid sort field/direction → return `Results.BadRequest(RequestMessages.BadRequest)`.
- `Functions.FilterObjectListPropertiesByKeyword<T>(records, keyword)` — accent-insensitive keyword search over string properties (used by `/search`).
- `PaginationModel` / `PaginationResponseModel<T>` — pagination request (`[AsParameters]`) and response envelope (`Page`, `PageSize`, `TotalRecords`, `Data`).
- `DateOnlyJsonConverter` — registered globally in `Program.cs` for `DateOnly` serialization.
- `Messages.EndpointMetadata` — **all Swagger summary/description strings live here** as `const`s, referenced via `SwaggerOperationAttribute`. Add new endpoint docs here, not inline.

**Data access.** `DBContext` uses `QueryTrackingBehavior.NoTracking` (read-only workload). Use `.Include(...)` for navigation properties when the endpoint should return related data. Entity mapping lives in `Data/Configs/<Entity>Config.cs` (applied in `DBContext.OnModelCreating`).

## Build / run / test

Run from the `api/` directory unless noted.

```bash
# Build
dotnet build

# Run the API (Development profile: https://localhost:7274 ; http://localhost:5204)
dotnet run

# Swagger UI once running
#   https://localhost:7274/swagger

# In DEBUG builds only, create the DB schema:
#   GET /db-creation   (see Routes/InfoRoutes.cs)

# Tests (from api.Tests/ or the repo)
dotnet test
```

**Database.** Connection string in `api/appsettings.json` (`ConnectionStrings:DefaultConnection`), overridable via the `DATABASE_CONNECTION` environment variable. PostgreSQL. Migrations live in `api/Migrations/`; seed data is in `.sql` files there.

**Docs site** (from `docs/`): `npm install`, then `npm run docs:dev` / `docs:build` / `docs:preview`.

## When adding or changing an endpoint

1. Add/adjust the model in `Models/` and its `Data/Configs/*Config.cs` if the schema changes (plus a migration).
2. Add the route in the resource's `Routes/*Routes.cs`, following the existing pattern (sorting via `ApplySorting`, `.Produces<T>(200)`, `.WithMetadata(new SwaggerOperationAttribute(...))`).
3. Put summary/description text in `Utils/Messages.cs` under `EndpointMetadata`.
4. Keep it `GET`-only and read-only.
5. Add integration tests in `api.Tests/ApiRoutesTests/`.

## Resources exposed (tags)

Country, Department, City, Region, President, TouristAttraction, CategoryNaturalArea, NaturalArea, Map, InvasiveSpecie, NativeCommunity, IndigenousReservation, Airport, ConstitutionArticle, Radio, Holiday, TypicalDish, TraditionalFairAndFestival, IntangibleHeritage, HeritageCity, PostalCode, UrbanCenter.
