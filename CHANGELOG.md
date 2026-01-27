# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog],
and this project adheres to [Semantic Versioning].

## [Unreleased]

- /

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
