---
name: finish-feature
description: >-
  Run the api-colombia feature-completion checklist: add/extend tests, run
  dotnet test, bump the version (semver), and update the CHANGELOG plus docs and
  README when public behavior changed. Use when finishing a new feature or an
  important change, or when the user says a change is "done" / "ready" and wants
  it properly tested, versioned, and documented before committing.
---

# Finish Feature — closing checklist

Use this after implementing a new feature or an important change in **api-colombia**, before committing. Work through the steps in order. Don't skip a step silently — if one doesn't apply, say so briefly and move on.

## 0. Understand what changed

- Run `git status` and `git diff` (and `git diff --staged`) to see the actual change and its scope.
- State in one or two sentences what behavior changed and whether it is **user-facing** (new/changed endpoint, response shape, limit, header) or **internal-only** (refactor, tooling). This drives how much of steps 4–5 apply.

## 1. Tests

Follow the repo conventions (see `CLAUDE.md` → "When adding or changing an endpoint", step 5). Tests live in `api.Tests/`.

- Add or extend tests that exercise the new behavior end-to-end. Integration tests use `CustomWebApplicationFactory` (`api.Tests/ApiRoutesTests/`).
- Cover the happy path **and** the notable edge cases / failure modes of what you changed.
- **Isolation gotcha:** each test class shares one `WebApplicationFactory` instance. If a test needs a lot of requests or its own app state (e.g. exhausting a rate limit), put it in its **own test class** so it gets its own factory instance and doesn't contaminate other tests. (See `ResourceRateLimitTests` / `HolidayRateLimitTests` for the pattern.)
- Keep the API `GET`-only and read-only — never add write operations.

## 2. Run the tests

- From the repo root or `api.Tests/`: `dotnet test`.
- **All tests must pass.** If anything fails, fix it (or the code) and re-run before continuing. Report the final pass count.

## 3. Bump the version

Edit `api/Const/Version.cs` (`VersionInfo.CurrentVersion`), which is shown in Swagger. Apply [semantic versioning](https://semver.org/):

| Change | Bump | Example |
| --- | --- | --- |
| New endpoint/feature, backward-compatible | **minor** | 1.1.0 → 1.2.0 |
| Bug fix, no API surface change | **patch** | 1.2.0 → 1.2.1 |
| Breaking change to an existing endpoint/response | **major** | 1.2.0 → 2.0.0 |

Internal-only refactors with no behavior change usually don't need a bump — if unsure, ask the user.

## 4. Update the CHANGELOG

Edit `CHANGELOG.md` (Keep a Changelog format):

- Add a new `## [<version>] - YYYY-MM-DD` section above the previous one, using **today's date**.
- Group notes under `### Added` / `### Changed` / `### Fixed` / `### Deprecated` / `### Removed` / `### Security` as appropriate.
- Add the inline release link right after the section, matching the existing style:
  `[<version>]: https://github.com/Mteheran/api-colombia/releases/tag/v<version>`
- Write for API consumers: what they can now do or must know, not internal mechanics.

## 5. Update documentation (only if user-facing)

If the change affects public behavior, update the relevant docs. Skip for internal-only changes.

- **VitePress docs** (`docs/`): most endpoint reference is generated from OpenAPI, but conceptual topics get their own page (e.g. `docs/mcp.md`, `docs/rate-limiting.md`) wired into `docs/.vitepress/config.ts` (nav + sidebar). Add or update a page when the concept isn't covered by the auto-generated reference.
- **Swagger metadata**: endpoint summaries/descriptions live in `api/Utils/Messages.cs` under `EndpointMetadata` — update there, not inline.
- **README.md and README_es.md**: update **both** (English + Spanish) if the change alters something documented there (feature list, usage, limits). Keep them in sync.
- **Landing / dashboard** (`api/wwwroot/index.html`, `metrics.html`): update if the change is worth surfacing to visitors. `index.html` uses i18n — add keys to `api/wwwroot/js/translations.json` for **es/en/pt**, not hardcoded text.

## 6. Wrap up

- Run `dotnet build` once more to confirm 0 errors.
- Give the user a short summary: what changed, tests result, new version, docs touched.
- **Do not commit, push, or open a PR unless the user asks.** When they do: branch first if on `main`, stage only the relevant files (never `.DS_Store`), and end the commit message with the project's `Co-Authored-By` trailer.
