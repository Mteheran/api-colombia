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
- Tests: **xUnit** + `Microsoft.AspNetCore.Mvc.Testing` (integration tests via `WebApplicationFactory`) + `Microsoft.EntityFrameworkCore.InMemory`.

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
│   └── wwwroot/          # static front-end: landing page, metrics & MCP dashboards, i18n
├── api.Tests/            # integration + unit tests
│   ├── Infrastructure/   # test host, shared collection, HTTP assertion helpers
│   ├── TestData/         # one seeder per resource + the shared CoreGraph
│   ├── Integration/      # Resources/, Mcp/, Platform/
│   └── Unit/             # helpers, converters, LINQ-translation tests
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

## Testing

- **One shared, seeded host.** The API is read-only, so `Infrastructure/ApiColombiaFactory` seeds an in-memory database once and every resource test shares it via `[Collection(SharedApiCollection.Name)]` + `IntegrationTestBase`. Nothing re-seeds between tests because nothing mutates data.
- **When a test needs its own host state** — recorded metrics, rate-limit counters, MCP sessions — use `IClassFixture<IsolatedFactory>` (or `RateLimitedFactory`, which drops the limit to a handful of requests) instead of the shared collection.
- **The rate limit is configurable.** `Program.cs` reads `RateLimiting:PermitLimit` (default 60), which is how the test host raises it out of the way and the rate-limit tests lower it.
- **Test parallelization is disabled** at assembly level (`Infrastructure/AssemblyBehavior.cs`). The shared host made the suite fast enough to expose an intermittent MCP handshake failure when the MCP class ran alongside the rest of the suite; the comment there records what was ruled out.
- **Seed data is one file per resource** under `TestData/`, hanging off `CoreGraph` (region → two departments → two cities). Add rows through a seeder and a `TotalRows` constant — never rely on a navigation property to smuggle an entity into the database, which is the accident `SeedIntegrityTests` exists to catch.
- **Coverage** is measured with `coverlet.runsettings` (which excludes the generated `Migrations/`). Branch coverage is the number that matters here: line coverage stays high just from happy paths, while the error branches are where the gaps hide.

## Versioning and the published OpenAPI document

Three things must agree, and tests enforce it:

1. `api/Const/Version.cs` (`VersionInfo.CurrentVersion`) — shown in Swagger.
2. The newest `## [x.y.z] - YYYY-MM-DD` heading in `CHANGELOG.md`.
3. `info.version` in `docs/public/openapi.json` — the document VitePress renders as the public endpoint reference.

`VersionConsistencyTests` checks (1) against (2) and validates the changelog's release links; `OpenApiDocumentTests` checks (3) and compares the whole checked-in document against the one the running app produces.

**After changing any endpoint, its Swagger metadata, or the version, regenerate the document:**

```bash
# from api.Tests/
UPDATE_OPENAPI=1 dotnet test --filter OpenApiDocumentTests
```

Then commit the regenerated `docs/public/openapi.json` alongside the change. Without this the public reference drifts — it had been stuck at 1.0.5 while the API was at 1.7.x, missing 44 endpoints across seven resources.

## Front-end (`wwwroot/`)

Plain static HTML/CSS/JS (no build step) served by ASP.NET Core static files. Key pages:

- `index.html` — public landing page. Served at `/` via `UseDefaultFiles()`.
- `metrics.html` — public metrics dashboard. Routed at `/metrics` (explicit `MapGet` in `Program.cs`), reads `/api/v1/metrics`.
- `mcp-inspector.html` — browser MCP tester. Routed at `/mcp` (explicit `MapGet` in `Program.cs`).

**Theming (light/dark).** All three pages support a light and a dark theme built on the same **Colombia-flag palette**; light is the default and a `dark` class on `<body>` switches to dark.

- **Landing page (`index.html`).** Ships `<body class="light">`; the theme toggle (`js/modal.js` `toggleColor`/`toggleColor2`) toggles the `dark` class. Theme rules live in `css/style.css` under `.dark ...` selectors. This page does **not** persist the choice.
- **Dashboards (`metrics.html`, `mcp-inspector.html`).** Self-contained pages that mirror the landing page with **CSS custom properties**: the light palette is declared on `:root` (the default) and overridden under `body.dark`. Each has a 🌙/☀️ toggle button in its header whose script: (1) reads the saved choice from `localStorage` — key `metrics-theme` / `mcp-theme` respectively — falling back to the OS `prefers-color-scheme`; (2) toggles `body.dark`; (3) persists the new choice. `body` has a `transition` on `background-color`/`color` for a smooth switch.

**Shared dashboard palette** (CSS variables — light value / dark value). Reuse these exact tokens for any new dashboard-style page so the three pages stay visually consistent:

| Variable        | Light (`:root`) | Dark (`body.dark`) | Role |
|-----------------|-----------------|--------------------|------|
| `--bg`          | `#fdfdfd`       | `#22292a`          | page background |
| `--panel`       | `#ffffff`       | `#2b3335`          | card background |
| `--panel2`      | `#f1f3f4`       | `#323c3e`          | inset / input background |
| `--border`      | `#e6e9ea`       | `#3c4749`          | borders / dividers |
| `--text`        | `#22292a`       | `#fdfdfd`          | primary text |
| `--muted`       | `#565656`       | `#a7b0b1`          | secondary / labels |
| `--accent`      | `#b07d00`       | `#ffcd00`          | brand yellow (values, links, primary btn); darkened in light for contrast |
| `--accent2`     | `#2f7d63`       | `#43aa8b`          | green (success / links) |
| `--danger`      | `#c8102e`       | `#f26d6d`          | flag red (errors) |
| `--blue`        | `#003087`       | `#58a6ff`          | flag blue (info) — metrics only |
| `--header-grad` | `linear-gradient(90deg,#ffffff,#f1f3f4)` | `linear-gradient(90deg,#262e2f,#1e2526)` | header background |

Metrics-only helpers: `--code-bg` (`rgba(0,0,0,.05)` / `rgba(255,255,255,.08)`). MCP-only helpers: `--pre-bg` (`#f6f8fa` / `#1b2122`) for code blocks and `--on-accent` (`#22292a` / `#1a1200`) for text on the accent-colored primary button. The reference light/dark backgrounds and text (`#fdfdfd`/`#22292a`) and flag accents (yellow `#ffcd00`/`#ffbd07`, blue `#003087`, red `#c8102e`) match `index.html`.

**Internationalization (i18n).** The landing page supports **Spanish (`es`, default), English (`en`), and Portuguese (`pt`)**.
- All translatable UI strings live in `js/translations.json`, keyed as `{ "<lang>": { "<key>": "<text>" } }`. **Every key must exist in all three language blocks.**
- In markup, tag an element with `data-i18n-key="<key>"`; `js/i18n.js` replaces its `textContent` with the string for the active language on load and on language change.
- Language is chosen from `localStorage` (`language`) → browser language → `es` fallback, and switched via the header/mobile language selectors.
- **When adding or changing any user-facing text on the landing page, add the `data-i18n-key` and provide translations for `es`, `en`, and `pt`** — don't hardcode a single language. (The dashboards `metrics.html`/`mcp-inspector.html` are English-only and not wired to the i18n system — only their theming is shared with the landing page.)

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

# Tests with coverage (line + branch)
dotnet test --settings ./coverlet.runsettings --collect:"XPlat Code Coverage"
```

**Database.** Connection string in `api/appsettings.json` (`ConnectionStrings:DefaultConnection`), overridable via the `DATABASE_CONNECTION` environment variable. PostgreSQL. Migrations live in `api/Migrations/`; seed data is in `.sql` files there.

**Docs site** (from `docs/`): `npm install`, then `npm run docs:dev` / `docs:build` / `docs:preview`.

## When adding or changing an endpoint

1. Add/adjust the model in `Models/` and its `Data/Configs/*Config.cs` if the schema changes (plus a migration).
2. Add the route in the resource's `Routes/*Routes.cs`, following the existing pattern (sorting via `ApplySorting`, `.Produces<T>(200)`, `.WithMetadata(new SwaggerOperationAttribute(...))`).
3. Put summary/description text in `Utils/Messages.cs` under `EndpointMetadata`.
4. Keep it `GET`-only and read-only.
5. Add integration tests in `api.Tests/Integration/Resources/`, and a `TestData/<Resource>Seed.cs` with a `TotalRows` constant (`SeedIntegrityTests` asserts the database matches it). If the resource has the usual list/by-id/search/pagedList shape, add a row to `ResourceContractTests.Contracts` — that covers the whole scenario matrix — and keep the resource's own file for behaviour specific to it.

## Resources exposed (tags)

Country, Department, City, Region, President, TouristAttraction, CategoryNaturalArea, NaturalArea, Map, InvasiveSpecie, NativeCommunity, IndigenousReservation, Airport, ConstitutionArticle, Radio, Holiday, TypicalDish, TraditionalFairAndFestival, IntangibleHeritage, HeritageCity, PostalCode, UrbanCenter, HigherEducationInstitution, TelevisionChannel, Volcano.
