# API-COLOMBIA

[![Build and deploy ASP.Net Core app to Azure Web App - ApiColombia](https://github.com/Mteheran/api-colombia/actions/workflows/main_apicolombia.yml/badge.svg)](https://github.com/Mteheran/api-colombia/actions/workflows/main_apicolombia.yml)

[![Build and test - ApiColombia](https://github.com/Mteheran/api-colombia/actions/workflows/pull_request_apicolombia.yml/badge.svg)](https://github.com/Mteheran/api-colombia/actions/workflows/pull_request_apicolombia.yml)

![Coverage](https://img.shields.io/badge/coverage-50%25-brightgreen)

## Introduction
API Colombia is a public RESTful API that enables users to access a wide range of public information about the country of Colombia.

📚 **[VitePress](https://docs.api-colombia.com/)** | **[Swagger](https://api-colombia.com/swagger/index.html)**  | 🤖 **[Ask AI about API-Colombia](https://www.deepgithub.com/Mteheran/api-colombia)** 


Read this document in [Español](/README_es.md)

## Project Support Features
* Minimal API's endpoints to get information about:
  - General information about the country.
  - Departments/States.
  - Cities.
  - Presidents.
  - Tourist attractions.
  - Natural Areas and Categories.
  - Airports.
  - Holidays.
  - Radio stations.
  - Typical dishes.
  - Traditional fairs and festivals.
  - Intangible heritage.
  - Indigenous reservations.
  - Native communities.
  - Invasive species.
  - Constitution articles.
  - Maps.
* Swagger documentation 
* Does not require authentication.

## Versions

[Changelog - Versions](/CHANGELOG.md)

## Installation Guide
* Clone this repository [here](https://github.com/Mteheran/api-colombia).
* The develop branch is the most stable branch at any given time, please make sure you're working from it.
* [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download) must be installed in the machine.
* Update the connection string to your preferred database in the "appsettings.json" file.
* Run the `dotnet build` command to generate the build of the project.

## Documentation
The documentation site is built with VitePress and lives in the `docs/` folder.

Prerequisites:
- Node.js 18+ installed.

Setup:
- `cd docs`
- `npm install`

Local development:
- `npm run docs:dev`
Starts the VitePress dev server with hot-reload.

Build static site:
- `npm run docs:build`
The generated site will be in `docs/.vitepress/dist`.

Preview built site:
- `npm run docs:preview`
Useful to simulate a production-like server locally.

Note: Run all the commands above inside the `docs/` directory.

## Testing

The project includes comprehensive integration tests using xUnit and an in-memory database. The tests verify that all API endpoints work correctly.

### Running Tests

To run all tests:
```bash
dotnet test
```

To run tests with coverage:
```bash
dotnet test /p:CollectCoverage=true
```

### Test Structure

The integration tests are located in the `api.Tests` project and use:
- **xUnit** as the testing framework
- **CustomWebApplicationFactory** to create a test server with an in-memory database
- **Microsoft.AspNetCore.Mvc.Testing** for HTTP client testing
- **AutoFixture** for test data generation

Each endpoint has corresponding integration tests that verify:
- Successful responses with correct data
- Pagination functionality
- Sorting capabilities
- Search functionality
- Error handling (404, 400, etc.)

The test database is seeded with sample data for each entity type, ensuring consistent test results.

## Usage
The public page [api-colombia.com](https://api-colombia.com/) has useful information about the API and a brief description of the available endpoints and the same allows access to the endpoints described below.

* Full documentation site: [docs.api-colombia.com](https://docs.api-colombia.com/)
* Swagger documentation can be found at the following [URL](https://api-colombia.com/swagger/index.html)
* 🤖 Ask AI about API-Colombia: [DeepGitHub](https://www.deepgithub.com/Mteheran/api-colombia)

## Projects, Demos, POCs and Samples

| Repo | Url   | Description         |
| ---------- | ------------------------------------- | ------------------------------------------------ |
| https://github.com/Mteheran/invasivespecie-colombia | https://especiesinvasoras.api-colombia.com/ | A portal to consult all invasive species in Colombia |
| https://github.com/lightbluetitan/colombiapi | https://github.com/lightbluetitan/colombiapi | ColombiaAPI for R, an R package that consumes the data exposed by api-colombia |
| https://github.com/JuanPabloDiaz/turismo | https://turismo.colombia.jpdiaz.dev | Interactive tourism map of Colombia showcasing tourist destinations across the country |
| https://github.com/JuanPabloDiaz/colombia | https://colombia.jpdiaz.dev | A web app that offers an interactive and comprehensive exploration of Colombia's geography, culture, biodiversity, and institutions through maps, multimedia, and detailed data from api-colombia.com |
| https://github.com/duvan-gallego/mcp-api-colombia | - | MCP (Model Context Protocol) server for API Colombia, enabling AI assistants to access Colombia data |
| https://github.com/crexative/colombia-mcp-server | - | Colombia MCP Server - Another MCP implementation for integrating API Colombia with AI tools |

## API Endpoints

### Cities
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/city                   | Get the list of all cities                                           |
| GET        | /api/{version}/city/{id}              | Get the information of a city by it is id                            |
| GET        | /api/{version}/city/name/{name}       | Get the information of a city by it is name                          |
| GET        | /api/{version}/city/search/{keyword}  | Get the information of a city searching by keyword                   |
| GET        | /api/{version}/city/pagedList         | Get the list of all cities paginated                                 |

### Country
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/country/Colombia       | Get the information of Colombia                                      |

### Departments
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/Department             | Get the list of all departments in Colombia                          |
| GET        | /api/{version}/Department/{id}        | Get the information of a department by it is id                      |
| GET        | /api/{version}/Department/name/{name} | Get the information of a department by it is name                    |
| GET        | /api/{version}/Department/search/{keyword} | Get the information of a department searching by keyword        |
| GET        | /api/{version}/Department/pagedList   | Get the list of all department paginated                             |

### Regions
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/Region                 | Get the list of all regions in Colombia                              |

### Presidents
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/President              | Get the list of all presidents in Colombia                           |
| GET        | /api/{version}/President/{id}         | Get the information of a president by it is id                       |
| GET        | /api/{version}/President/name/{name}  | Get the information of a president by it is name                     |
| GET        | /api/{version}/President/year/{year}  | Get the information of a president that ruled during a specific year |
| GET        | /api/{version}/President/search/{keyword}      | Get the information of a president searching by keyword                        |
| GET        | /api/{version}/President/pagedList    | Get the list of all presidents paginated                             |

### Tourist Attractions
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/TouristicAttraction             | Get the list of all touristic attractions in Colombia                          |
| GET        | /api/{version}/TouristicAttraction/{id}        | Get the information of a touristic attraction by it is id                      |
| GET        | /api/{version}/TouristicAttraction/name/{name} | Get the information of a touristic attaction by it is name                     |
| GET        | /api/{version}/TouristicAttraction/search/{keyword} | Get the information of a touristic attaction searching by keyword         |
| GET        | /api/{version}/TouristicAttraction/pagedList   | Get the list of all touristic attractions paginated                            |

### Natural Areas
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/NaturalArea             | Get the list of all natural areas in Colombia                          |
| GET        | /api/{version}/NaturalArea/{id}        | Get the information of a natural area by it is id                      |
| GET        | /api/{version}/NaturalArea/name/{name} | Get the information of a natural area by it is name                     |
| GET        | /api/{version}/NaturalArea/search/{keyword} | Get the information of a natural area searching by keyword         |
| GET        | /api/{version}/NaturalArea/pagedList   | Get the list of all natural areas paginated                            |

### Category Natural Areas
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/CategoryNaturalArea             | Get the list of all category natural areas                          |
| GET        | /api/{version}/CategoryNaturalArea/{id}        | Get the information of a category natural area by it is id                      |
| GET        | /api/{version}/CategoryNaturalArea/{id}/NaturalAreas | Get the information of a category natural area with its natural areas by id                     |

### Airports
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/Airport             | Get the list of all airports in Colombia                          |
| GET        | /api/{version}/Airport/{id}        | Get the information of an airport by it is id                      |
| GET        | /api/{version}/Airport/name/{name} | Get the information of an airport by it is name                     |
| GET        | /api/{version}/Airport/search/{keyword} | Get the information of an airport searching by keyword         |
| GET        | /api/{version}/Airport/pagedList   | Get the list of all airports paginated                            |

### Holidays
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/Holiday/year/{year}             | Get the list of all holidays for a specific year, optional parameter includeSunday                          |
| GET        | /api/{version}/Holiday/year/{year}/month/{month}        | Get the list of all holidays for a specific year and month, optional parameter includeSunday                        |

### Radio Stations
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/Radio             | Get the list of all radio stations in Colombia                          |
| GET        | /api/{version}/Radio/{id}        | Get the information of a radio station by it is id                      |
| GET        | /api/{version}/Radio/name/{name} | Get the information of a radio station by it is name                     |
| GET        | /api/{version}/Radio/search/{keyword} | Get the information of a radio station searching by keyword         |
| GET        | /api/{version}/Radio/pagedList   | Get the list of all radio stations paginated                            |

### Typical Dishes
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/TypicalDish             | Get the list of all typical dishes in Colombia                          |
| GET        | /api/{version}/TypicalDish/{id}        | Get the information of a typical dish by it is id                      |
| GET        | /api/{version}/TypicalDish/{id}/department | Get the list of typical dishes by department id                     |
| GET        | /api/{version}/TypicalDish/name/{name} | Get the information of a typical dish by it is name                     |
| GET        | /api/{version}/TypicalDish/search/{keyword} | Get the information of a typical dish searching by keyword         |
| GET        | /api/{version}/TypicalDish/pagedList   | Get the list of all typical dishes paginated                            |

### Traditional Fairs and Festivals
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/TraditionalFairAndFestival             | Get the list of all traditional fairs and festivals in Colombia                          |
| GET        | /api/{version}/TraditionalFairAndFestival/{id}        | Get the information of a traditional fair and festival by it is id                      |
| GET        | /api/{version}/TraditionalFairAndFestival/{id}/city | Get the list of traditional fairs and festivals by city id                     |
| GET        | /api/{version}/TraditionalFairAndFestival/name/{name} | Get the information of a traditional fair and festival by it is name                     |
| GET        | /api/{version}/TraditionalFairAndFestival/search/{keyword} | Get the information of a traditional fair and festival searching by keyword         |
| GET        | /api/{version}/TraditionalFairAndFestival/pagedList   | Get the list of all traditional fairs and festivals paginated                            |

### Intangible Heritage
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/IntangibleHeritage             | Get the list of all intangible heritages in Colombia                          |
| GET        | /api/{version}/IntangibleHeritage/{id}        | Get the information of an intangible heritage by it is id                      |
| GET        | /api/{version}/IntangibleHeritage/{id}/department | Get the list of intangible heritages by department id                     |
| GET        | /api/{version}/IntangibleHeritage/name/{name} | Get the information of an intangible heritage by it is name                     |
| GET        | /api/{version}/IntangibleHeritage/search/{keyword} | Get the information of an intangible heritage searching by keyword         |
| GET        | /api/{version}/IntangibleHeritage/pagedList   | Get the list of all intangible heritages paginated                            |

### Indigenous Reservations
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/IndigenousReservation             | Get the list of all indigenous reservations in Colombia                          |
| GET        | /api/{version}/IndigenousReservation/{id}        | Get the information of an indigenous reservation by it is id                      |
| GET        | /api/{version}/IndigenousReservation/name/{name} | Get the information of an indigenous reservation by it is name                     |
| GET        | /api/{version}/IndigenousReservation/search/{keyword} | Get the information of an indigenous reservation searching by keyword         |
| GET        | /api/{version}/IndigenousReservation/pagedList   | Get the list of all indigenous reservations paginated                            |

### Native Communities
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/NativeCommunity             | Get the list of all native communities in Colombia                          |
| GET        | /api/{version}/NativeCommunity/{id}        | Get the information of a native community by it is id                      |
| GET        | /api/{version}/NativeCommunity/name/{name} | Get the information of a native community by it is name                     |
| GET        | /api/{version}/NativeCommunity/search/{keyword} | Get the information of a native community searching by keyword         |
| GET        | /api/{version}/NativeCommunity/pagedList   | Get the list of all native communities paginated                            |

### Invasive Species
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/InvasiveSpecie             | Get the list of all invasive species in Colombia                          |
| GET        | /api/{version}/InvasiveSpecie/{id}        | Get the information of an invasive specie by it is id                      |
| GET        | /api/{version}/InvasiveSpecie/name/{name} | Get the information of an invasive specie by it is name                     |
| GET        | /api/{version}/InvasiveSpecie/search/{keyword} | Get the information of an invasive specie searching by keyword         |
| GET        | /api/{version}/InvasiveSpecie/pagedList   | Get the list of all invasive species paginated                            |

### Constitution Articles
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/ConstitutionArticle             | Get the list of all constitution articles                          |
| GET        | /api/{version}/ConstitutionArticle/{id}        | Get the information of a constitution article by it is id                      |
| GET        | /api/{version}/ConstitutionArticle/search/{keyword} | Get the information of a constitution article searching by keyword         |
| GET        | /api/{version}/ConstitutionArticle/pagedList   | Get the list of all constitution articles paginated                            |
| GET        | /api/{version}/ConstitutionArticle/byChapterNumber/{chapternumber}   | Get the list of constitution articles by chapter number                            |

### Maps
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/Map             | Get the list of all maps                          |
| GET        | /api/{version}/Map/{id}        | Get the information of a map by it is id                      |

## Response Example 
* Content type: "application/json". Responses are JSON Objects. 
* Response header contains the HTTP CODE with the status. 
* Example:
 
```json
{
  "id": 1,
  "name": "Colombia",
  "description": "Colombia, officially the Republic of Colombia, is a country in South America with insular regions in North America—near Nicaragua's Caribbean coast—as well as in the Pacific Ocean. The Colombian mainland is bordered by the Caribbean Sea to the north, Venezuela to the east and northeast, Brazil to the southeast, Ecuador and Peru to the south and southwest, the Pacific Ocean to the west, and Panama to the northwest. Colombia is divided into 32 departments and the Capital District of Bogotá, the country's largest city. It covers an area of 1,141,748 square kilometers (440,831 sq mi), and has a population of 52 million. Colombia's cultural heritage—including language, religion, cuisine, and art—reflects its history as a Spanish colony, fusing cultural elements brought by immigration from Europe and the Middle East, with those brought by enslaved Africans, as well as with those of the various Indigenous civilizations that predate colonization. Spanish is the official state language, although English and 64 other languages are recognized regional languages.",
  "stateCapital": "Bogotá",
  "surface": 1141748,
  "population": 52235050,
  "languages": [
    "Spanish",
    "English"
  ],
  "timeZone": "UTC-5",
  "currency": "Colombian Peso",
  "currencyCode": "COP",
  "isoCode": "CO",
  "internetDomain": ".co",
  "phonePrefix": "+57",
  "radioPrefix": "HK",
  "aircraftPrefix": "HK"
}
```
## Technologies Used
* [.NET 10](https://dotnet.microsoft.com/en-us/) is a free, cross-platform, open source developer platform for building many kinds of applications. .NET is built on a high-performance runtime that is used in production by many high-scale apps.
* [PostgreSQL](https://www.postgresql.org/) is a powerful, open source object-relational database system with over 35 years of active development that has earned it a strong reputation for reliability, feature robustness, and performance.
* [Microsoft Azure](https://azure.microsoft.com/en-us/resources/cloud-computing-dictionary/what-is-azure/) The Azure cloud platform is more than 200 products and cloud services designed to help you bring new solutions to life—to solve today's challenges and create the future. Build, run, and manage applications across multiple clouds, on-premises, and at the edge, with the tools and frameworks of your choice.

## Contributors ✨
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-7-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->
Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):
<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/mteheran/"><img src="https://avatars.githubusercontent.com/u/3578356?v=4?s=100" width="100px;" alt="Miguel Teheran"/><br /><sub><b>Miguel Teheran</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/commits?author=mteheran" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/rina-plata/"><img src="https://avatars.githubusercontent.com/u/55161289?v=4?s=100" width="100px;" alt="Rina Plata"/><br /><sub><b>Rina Plata</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/commits?author=rinaplata" title="Code">💻</a> <a href="#tutorial-rinaplata" title="Tutorials">✅</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/veronicaguaman/"><img src="https://avatars.githubusercontent.com/u/70024610?v=4?s=100" width="100px;" alt="Veronica Guaman"/><br /><sub><b>Veronica Guaman</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/commits?author=VeronicaGuaman" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/mariobot/"><img src="https://avatars.githubusercontent.com/u/1220191?v=4?s=100" width="100px;" alt="Mario Botero"/><br /><sub><b>Mario Botero</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/mteheran/api-colombia/commits?author=mariobot" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/alejandrohv/"><img src="https://avatars.githubusercontent.com/u/99321349?v=4?s=100" width="100px;" alt="Alejandro Herreño"/><br /><sub><b>Alejandro Herreño</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/commits?author=Alejandrohv06" title="Documentation">📖</a> <a href="https://github.com/Mteheran/api-colombia/commits?author=Alejandrohv06" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/angelo-dario-cardona-tobon-5671602a/"><img src="https://avatars.githubusercontent.com/u/13419303?v=4" width="100px;" alt="Angelo Cardona"/><br /><sub><b>Angelo Cardona</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/commits?author=leangele" title="Documentation">📖</a> <a href="https://github.com/Mteheran/api-colombia/commits?author=leangele" title="Code">💻</a></td>
  </tr>
  <tr>
    <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/hamilgdev/"><img src="https://avatars.githubusercontent.com/u/37517478?v=4" width="100px;" alt="Hamilton Galeano"/><br /><sub><b>Hamilton Galeano</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/commits?author=hamilgdev" title="Documentation">📖</a> </td>
    <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/leomaris-reyes-1b598661/"><img src="https://avatars.githubusercontent.com/u/35940594?v=4" width="120px;" alt="Leomarys Reyes"/><br /><sub><b>Leomaris Reyes</b></sub></a><br /><a href="https://github.com/LeomarisReyes" title="Code">💻</a></td>
    <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/enzonotario/"><img src="https://avatars.githubusercontent.com/u/10469299?v=4" width="120px;" alt="Enzo Notario"/><br /><sub><b>Enzo Notario</b></sub></a><br /><a href="https://github.com/enzonotario" title="Code">💻</a></td>
  </tr>
  </tbody>
  <tfoot>
    <tr>
      <td align="center" size="13px" colspan="7">
        <img src="https://raw.githubusercontent.com/all-contributors/all-contributors-cli/1b8533af435da9854653492b1327a23a4dbd0a10/assets/logo-small.svg">
          <a href="https://all-contributors.js.org/docs/en/bot/usage">Add your contributions</a>
        </img>
      </td>
    </tr>
  </tfoot>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!

## License
MIT License
