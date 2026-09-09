# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

- /

## [1.7.2] - 2026-09-09

### Fixed

- **`GET /api/v1/InvasiveSpecie/pagedList` ignored `sortBy` and `sortDirection`.** It was the only paginated route that never applied the sort: the parameters were accepted and the rows came back in insertion order, and an unknown `sortBy` was not rejected either. It now sorts and validates like every other `pagedList`, returning `400` for an unknown field or direction.

- **`GET /api/v1/TraditionalFairAndFestival` ignored `sortBy` and `sortDirection`.** The handler validated the parameters and then discarded the sort with a hard `OrderBy(Id)`, so a request for a name-sorted list silently came back in id order.

- **`GET /api/v1/PostalCode/city/{cityId}` accepted a non-positive id.** It was the one nested route without a `cityId <= 0` guard, so `0` or a negative id fell through to the empty-result branch and answered `404` where every comparable route answers `400`.

- **Swagger declared the wrong response type for four endpoints.** `GET /api/v1/Radio/{id}`, `GET /api/v1/Radio/name/{name}`, `GET /api/v1/NativeCommunity/{id}`, `GET /api/v1/NativeCommunity/name/{name}` and `GET /api/v1/InvasiveSpecie/pagedList` were documented as returning `City`. Generated clients built from the OpenAPI document were wrong for these routes; the document now names the real types.

### Removed

- **`PaginationModel.BindAsync`, which never ran.** The `pagedList` routes bind the model with `[AsParameters]`, which maps each property from the query string by name and does not call a type's `BindAsync`. The binder therefore described a contract the API does not have: the `?sortDir=` key it declared is not a parameter — **the key that works is `?sortDirection=`**, matching the property name — and its correction of a non-positive `?page=` to `1` never happened, so each route's own guard answers `400` instead. No endpoint behaviour changed; the misleading code is simply gone. `?pagesize=` and `?pageSize=` remain interchangeable, since query keys are matched case-insensitively.

### Changed

- Internal only: the test project was restructured (one seeder per resource, a scenario matrix applied to all 25 resources, 290 → 783 tests) and code coverage measurement was repaired — the coverlet filter was `[api.*]*`, which matches no assembly, so every run had been reporting 0%.

### Known issues

- **Empty-result responses are inconsistent between resources.** `/search/{keyword}` with no match answers `404` on Department, President, Radio, NativeCommunity, IndigenousReservation, ConstitutionArticle and UrbanCenter, and `200` with an empty array on the other thirteen. `/pagedList` past the last page answers `404` on thirteen resources and `200` on seven. `/name/{name}` with no match answers `404` only on President, and `200` with an empty array on the other thirteen. Single-item routes (`/{id}`, `/code/{code}`) are consistent and correctly answer `404`. The current behaviour of every one of these endpoints is now pinned by tests; unifying them would be a breaking change and is deliberately left for a major release.

[1.7.2]: https://github.com/Mteheran/api-colombia/releases/tag/v1.7.2

## [1.7.1] - 2026-09-03

### Fixed

- **Volcano text and elevation defects inherited from the SGC feed.** A review of the 24 volcano records against the SGC's own sources surfaced defects that the dataset had copied verbatim from the upstream feed. The description of the *Complejo Volcánico Chiles-Cerro Negro* was cut off mid-word (`...se clasifican como andesitas de d`) — the truncation is present in the SGC feed itself — and has been completed from the SGC's page for the complex. Its elevation moved from 4470 m (the Cerro Negro summit, which the feed publishes for the whole complex) to **4748 m**, the Chiles summit and the high point of the complex, the figure the same SGC page gives. *Volcán Las Ánimas* said "una altura de 4300 msnm" in its description while its `elevation` field read 4200; the text now matches the field. Spelling and grammar were corrected in eleven descriptions (`Vulcanolgía` → `Vulcanología`, `horblenda` → `hornblenda`, `geomorfologicamente` → `geomorfológicamente`, `picroclásticos` → `piroclásticos`, `estratovolcano` → `estratovolcán`, `edifico` → `edificio`, `fillitas` → `filitas`, `Bogota` → `Bogotá`, missing accents and a missing reflexive pronoun), along with unit notation (`Km` → `km`, `km2` → `km²`), stray "(figura N)" references to figures the API does not carry, and a zero-width space.

  The corrections are folded into `scripts/Volcano_Data.sql` so a fresh seed is already correct; `scripts/FixVolcanoText_Update.sql` applies them to databases seeded earlier and is idempotent.

  No endpoint behaviour changed: all seven volcano routes were exercised against production and the 24 records still match the SGC viewer exactly on name, coordinates, volcano type and alert level.

[1.7.1]: https://github.com/Mteheran/api-colombia/releases/tag/v1.7.1

## [1.7.0] - 2026-08-27

### Added

- **Volcanoes dataset.** New `Volcano` resource with the 24 volcanoes of the Colombian territory, sourced from the *Servicio Geológico Colombiano* (SGC). Each record exposes its name, description, elevation in meters above sea level, coordinates, volcano type, SGC alert level, image, and the department and city it belongs to. Available at `GET /api/v1/Volcano` with the standard shapes (`/{id}`, `/name/{name}`, `/search/{keyword}`, `/pagedList`, plus sorting), and two sub-resource routes: `GET /api/v1/Department/{id}/volcanoes` and `GET /api/v1/City/{id}/volcanoes`. The resource is also exposed through the MCP server under the `volcano` key.

  Three elevations published by the SGC feed (Cerro Machín, Nevado del Ruiz and Paramillo del Quindío) were exactly their latitude x 1000 and have been corrected against the Smithsonian Global Volcanism Program and the SGC's own descriptions; two volcanoes on a departmental boundary were reassigned to the municipality their published coordinates fall in. Every correction is documented in `scripts/Volcano_Data.sql`.

[1.7.0]: https://github.com/Mteheran/api-colombia/releases/tag/v1.7.0

## [1.6.1] - 2026-08-24

### Fixed

- **Television channel search now finds short keywords.** `GET /api/v1/TelevisionChannel/search/{keyword}` returned an empty list for keywords of three characters or fewer unless they matched a channel name exactly, so common searches such as `RCN` found nothing even though channels like *Canal RCN* or *Noticias RCN* exist. The endpoint now matches the keyword anywhere in the channel name, ignoring case and accent marks, regardless of the keyword length.

[1.6.1]: https://github.com/Mteheran/api-colombia/releases/tag/v1.6.1

## [1.6.0] - 2026-08-14

### Added

- **Television channels dataset.** New `TelevisionChannel` resource exposing Colombian TV channels with their name, city, streaming URL and whether the channel is currently active. Available at `GET /api/v1/TelevisionChannel` with the standard shapes (`/{id}`, `/name/{name}`, `/search/{keyword}`, `/pagedList`, plus sorting), and a `GET /api/v1/City/{id}/televisionchannels` sub-resource to list the channels of a given city.

[1.6.0]: https://github.com/Mteheran/api-colombia/releases/tag/v1.6.0

## [1.5.0] - 2026-08-01

### Added

- **Metrics dashboard now speaks your language.** The public metrics dashboard (`/metrics`) is now available in **Spanish, English and Portuguese**. It auto-detects your browser language on first visit, a header selector lets you switch on the fly, and your choice is remembered (`localStorage`). Numbers, dates and month names are formatted for the chosen locale.

### Fixed

- **"Requests per month" chart on the metrics dashboard.** The bar chart was distorted (stretched labels and bars) because the SVG scaled its width and height independently. It now uses a uniform coordinate space with a baseline axis, readable value/month labels, and a year label shown only when the year changes.
- **Higher Education Institutions in the landing-page demo.** The interactive "Try the API" selector on the landing page now includes the `HigherEducationInstitution` resource added in 1.4.0.

[1.5.0]: https://github.com/Mteheran/api-colombia/releases/tag/v1.5.0

## [1.4.0] - 2026-08-01

### Added

- **Higher Education Institutions (IES).** New `HigherEducationInstitution` resource exposing Colombia's official higher-education institutions from SNIES (Ministerio de Educación, via datos.gov.co). Each record includes code, name, legal nature, academic character, address, phone, high-quality accreditation flag, website and its related city. Endpoints: `GET /api/v1/HigherEducationInstitution` (list, sortable), `/{id}`, `/name/{name}`, `/search/{keyword}` and `/pagedList`. City ids were validated against the API's own City data before loading.
- **Higher education institutions by city.** New sub-resource `GET /api/v1/City/{id}/highereducationinstitutions` returns the institutions located in a given city.

[1.4.0]: https://github.com/Mteheran/api-colombia/releases/tag/v1.4.0

## [1.3.0] - 2026-07-20

### Added

- **Light & dark theme for the dashboards.** The metrics dashboard (`/metrics`) and the MCP Inspector (`/mcp`) now support both a light and a dark theme, matching the landing page's Colombia-flag palette. A 🌙/☀️ toggle in each header switches theme, remembers your choice (`localStorage`), and honors your operating-system preference on first visit.
- **"Metrics" navigation link** on the landing page (desktop and mobile menus), opening the metrics dashboard. Translated for Spanish, English, and Portuguese.
- **Copy-URL button** on the MCP Inspector: a one-click icon (with tooltip) that copies the full MCP server URL (e.g. `https://api-colombia.com/api/v1/mcp`) to the clipboard for pasting into an MCP client.

### Changed

- The metrics and MCP dashboards now default to a light theme (previously dark-only), consistent with the landing page.

[1.3.0]: https://github.com/Mteheran/api-colombia/releases/tag/v1.3.0

## [1.2.0] - 2026-07-11

### Added

- **Per-IP rate limiting** on the highest-traffic public endpoints (**Holiday, City, Department**) to keep the service stable under heavy demand (2M+ requests/month). A shared sliding-window policy allows **60 requests per minute per IP**; requests over the limit receive **HTTP 429 Too Many Requests** with a `Retry-After` header.
    - The three groups share a single per-IP budget through one named policy (`Util.PublicRateLimitPolicy`).
    - `UseRateLimiter` runs before the output cache, so bursts are throttled even when a response could be served from cache — protecting outbound (egress) bandwidth.
    - Consumer tip: most of this data changes rarely, so **cache responses on the client side** (or honor the API's 7-day cache) instead of requesting them on every call.
    - Integration tests cover both the Holiday limit and the shared budget across the three resources.

[1.2.0]: https://github.com/Mteheran/api-colombia/releases/tag/v1.2.0

## [1.1.0] - 2026-07-09

### Added

- **MCP (Model Context Protocol) server** hosted alongside the REST API at `POST /api/v1/mcp` (Streamable HTTP, stateless). Lets AI agents consume the same public Colombia data through MCP tools without learning the REST surface.
    - Guidance tools: `list_colombia_resources`, `get_api_reference` — advertise the catalog and usage conventions (sorting, pagination, keyword search).
    - Generic data tools: `get_country_info`, `list_items`, `get_item_by_id`, `get_items_by_name`, `search_items`, `list_items_paged` — dispatch through a single `ResourceCatalog` covering all ~22 API resources.
    - Relational tools: `get_cities_by_department`, `get_natural_areas_by_department`, `get_touristic_attractions_by_department`.
    - Reuses the existing `Functions.ApplySorting` and `Functions.FilterObjectListPropertiesByKeyword` helpers so MCP responses match REST semantics.
    - Purely additive: no existing routes, models, or migrations changed.

[1.1.0]: https://github.com/Mteheran/api-colombia/releases/tag/v1.1.0

## [1.0.9] - 2026-01-26

### Added

- New HeritageCity model and endpoints for Colombia's heritage cities.
- HeritageCity table with fields: Id, Name, Description, CityId, DepartmentId, and Image.
- Complete REST API endpoints for HeritageCity including list, get by id, get by name, search, and paginated list.
- SQL seed script for HeritageCity data.

## [1.0.8] - 2026-01-25

### Added

- New IntangibleHeritage model and endpoints to manage Colombia's intangible cultural heritage information.
- IntangibleHeritage table with fields: Id, Name, DepartmentId, Scope, and InclusionYear.
- Complete REST API endpoints for IntangibleHeritage including list, get by id, get by department, get by name, search, and paginated list.

## [1.0.7] - 2026-01-07

### Added

- Optional `includeSunday` query parameter to holiday endpoints to include Sunday holidays (Palm Sunday and Easter Sunday) in the response.

## [1.0.6] - 2026-01-06

### Changed

- Project migration to .NET 10.
- Updated Entity Framework Core and other dependencies to version 10.
- Updated Swashbuckle.AspNetCore to version 10.1.0.

## [1.0.5] - 2024-12-18

### Changed

- Endpoints were added for holidays: one returning all holidays for a specific year and the other returning holidays for a specific month and year.

## [1.0.4] - 2024-10-25

### Changed

- All list-returning endpoints now support sorting by any field within their description, with options for both ascending and descending order.


## [1.0.3] - 2024-10-10

### Changed

- Deparment list now is returning the information about Capital city for each record

## [1.0.2] - 2024-10-10

### Added

- Adding ChangeLog, CodeOwners and Contributing file to improve documentation and create versions. 

### Changed

### Deprecated

### Removed

### Fixed

### Security

## [1.0.1] - 2022-03-01

- initial release

<!-- Links -->
[keep a changelog]: https://keepachangelog.com/en/1.0.0/
[semantic versioning]: https://semver.org/spec/v2.0.0.html

<!-- Versions -->
[unreleased]: https://github.com/Author/Repository/compare/v1.0.9...HEAD
[1.0.9]: https://github.com/Author/Repository/compare/v1.0.8...v1.0.9
[1.0.8]: https://github.com/Author/Repository/compare/v1.0.7...v1.0.8
[1.0.7]: https://github.com/Author/Repository/compare/v1.0.6...v1.0.7
[1.0.6]: https://github.com/Author/Repository/compare/v1.0.5...v1.0.6
[1.0.5]: https://github.com/Author/Repository/compare/v1.0.4...v1.0.5
[1.0.4]: https://github.com/Author/Repository/compare/v1.0.3...v1.0.4
[1.0.3]: https://github.com/Author/Repository/compare/v1.0.2...v1.0.3
[1.0.2]: https://github.com/Author/Repository/compare/v1.0.1...v1.0.2
[1.0.1]: https://github.com/Author/Repository/releases/tag/v1.0.1
