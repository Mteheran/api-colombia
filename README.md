# API-ECUADOR

## Introduction
API Ecuador is a public RESTful API that enable users to obtain a variety of public information about the country "Ecuador".

## Project Support Features
* Minimal API's endpoints to get information about:
  - General information about the country.
  - Provinces.
  - Cities.
  - Presidents.
  - Tourist attractions.
* Swagger documentation 
* Does not requiere  authentication.
## Versions
* 1.0
## Installation Guide
* Clone this repository [here](https://github.com/andresvillenas/api-ecuador).
* The develop branch is the most stable branch at any given time, ensure you're working from it.
* [.NET CORE SDK ](https://dotnet.microsoft.com/en-us/download) must be installed in the machine.
* Update the connection string to your prefered database in the "appsettings.json" file.
* Run `dotnet build` command to generate the build of the project.

## Usage
Public page [api-ecuador.com](https://api-ecuador.com/) has useful information about the API and a brief description of the available endpoints and the same allows access to the endpoints described bellow.

* Swagger documentation can be found in the following [URL](https://api-ecuador.com/swagger/index.html)

## API Endpoints
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/city                   | Get the list of all the cities                                       |
| GET        | /api/{version}/city/{id}              | Get the information of a city by it is id                            |
| GET        | /api/{version}/city/name/{name}       | Get the information of a city by it is name                          |
| GET        | /api/{version}/country/Ecuador       | Get the information of Ecuador                                      |
| GET        | /api/{version}/Province             | Get the list of all the provinces in Ecuador                      |
| GET        | /api/{version}/Province/{id}        | Get the information of a province by it is id                      |
| GET        | /api/{version}/Province/name/{name} | Get the information of a province by it is name                    |
| GET        | /api/{version}/President              | Get the list of all the presidents in Ecuador                       |
| GET        | /api/{version}/President/{id}         | Get the information of a president by it is id                       |
| GET        | /api/{version}/President/name/{name}  | Get the information of a president by it is name                     |
| GET        | /api/{version}/President/year/{name}  | Get the information of a president that ruled during a specific year |
| GET        | /api/{version}/TouristicAttraction             | Get the list of all the touristic attractions in Ecuador                      |
| GET        | /api/{version}/TouristicAttraction/{id}        | Get the information of a touristic attraction by it is id                      |
| GET        | /api/{version}/TouristicAttraction/name/{name} | Get the information of a touristic attaction by it is name                    |

## Response Example 
* Content type: "application/json". Responses are JSON Objects. 
* Response header contains the HTTP CODE with the status. 
* Example:
 
```json
 {"id":1,"name":"Ecuador","description":"Ecuador, officially the Republic of Ecuador, is a country in South America with insular regions in North America—near Nicaragua's Caribbean coast—as well as in the Pacific Ocean. The Ecuadorn mainland is bordered by the Caribbean Sea to the north, Venezuela to the east and northeast, Brazil to the southeast, Ecuador and Peru to the south and southwest, the Pacific Ocean to the west, and Panama to the northwest. Ecuador is divided into 32 provinces and the Capital District of Bogotá, the country's largest city. It covers an area of 1,141,748 square kilometers (440,831 sq mi), and has a population of 52 million. Ecuador's cultural heritage—including language, religion, cuisine, and art—reflects its history as a Spanish colony, fusing cultural elements brought by immigration from Europe and the Middle East, with those brought by enslaved Africans, as well as with those of the various Indigenous civilizations that predate colonization. Spanish is the official state language, although English and 64 other languages are recognized regional languages.","stateCapital":"Bogotá","surface":1141748,"population":52235050,"languages":["Spanish","English"],"timeZone":"UTC-5","currency":"Ecuadorn Peso","currencyCode":"COP","isoCode":"CO","internetDomain":".co","phonePrefix":"+57","radioPrefix":"HK","aircraftPrefix":"HK"}
 ```
## Technologies Used
* [.NET Core 6.*](https://dotnet.microsoft.com/en-us/) is a free, cross-platform, open source developer platform for building many kinds of applications. .NET is built on a high-performance runtime that is used in production by many high-scale apps.
* [PostgreSQL](https://www.postgresql.org/) is a powerful, open source object-relational database system with over 35 years of active development that has earned it a strong reputation for reliability, feature robustness, and performance.
* [Microsoft Azure](https://azure.microsoft.com/en-us/resources/cloud-computing-dictionary/what-is-azure/) The Azure cloud platform is more than 200 products and cloud services designed to help you bring new solutions to life—to solve today’s challenges and create the future. Build, run, and manage applications across multiple clouds, on-premises, and at the edge, with the tools and frameworks of your choice.

## Contributors ✨
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-5-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->
Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):
<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/mteheran/"><img src="https://avatars.githubusercontent.com/u/3578356?v=4?s=100" width="100px;" alt="Miguel Teheran"/><br /><sub><b>Miguel Teheran</b></sub></a><br /><a href="https://github.com/andresvillenas/api-ecuador/mteheran/api-ecuador/commits?author=mteheran" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/rina-plata/"><img src="https://avatars.githubusercontent.com/u/55161289?v=4?s=100" width="100px;" alt="Rina Plata"/><br /><sub><b>Rina Plata</b></sub></a><br /><a href="https://github.com/andresvillenas/api-ecuador/mteheran/api-ecuador/commits?author=rinaplata" title="Documentation">📖</a> <a href="#tutorial-rinaplata" title="Tutorials">✅</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/veronicaguaman/"><img src="https://avatars.githubusercontent.com/u/70024610?v=4?s=100" width="100px;" alt="Veronica Guaman"/><br /><sub><b>Veronica Guaman</b></sub></a><br /><a href="https://github.com/andresvillenas/api-ecuador/mteheran/api-ecuador/commits?author=VeronicaGuaman" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/mariobot/"><img src="https://avatars.githubusercontent.com/u/1220191?v=4?s=100" width="100px;" alt="Mario Botero"/><br /><sub><b>Mario Botero</b></sub></a><br /><a href="https://github.com/andresvillenas/api-ecuador/mteheran/api-ecuador/commits?author=mariobot" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/alejandrohv/"><img src="https://avatars.githubusercontent.com/u/99321349?v=4?s=100" width="100px;" alt="Alejandro Herreño"/><br /><sub><b>Alejandro Herreño</b></sub></a><br /><a href="https://github.com/andresvillenas/api-ecuador/mteheran/api-ecuador/commits?author=Alejandrohv06" title="Documentation">📖</a> <a href="https://github.com/andresvillenas/api-ecuador/mteheran/api-ecuador/commits?author=Alejandrohv06" title="Code">💻</a></td>
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