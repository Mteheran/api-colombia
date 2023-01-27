# API-COLOMBIA

## Introduction
API Colombia is a public RESTful API that enable users to obtain a variety of public information about the country "Colombia".

## Project Support Features
* Minimal API's endpoints to get information about:
  - General information about the country.
  - Departments/States.
  - Cities.
  - Presidents.
  - Tourist attractions.
* Swagger documentation 
* Does not requiere  authentication.
## Versions
* 1.0
## Installation Guide
* Clone this repository [here](https://github.com/Mteheran/api-colombia).
* The develop branch is the most stable branch at any given time, ensure you're working from it.
* [.NET CORE SDK ](https://dotnet.microsoft.com/en-us/download) must be installed in the machine.
* Update the connection string to your prefered database in the "appsettings.json" file.
* Run `dotnet build` command to generate the build of the project.

## Usage
Public URL: TBD
* Swagger documentation can be found in the URL: "{PUBLIC_URL}/swagger/index.html"

## API Endpoints
| HTTP Verbs | Endpoints                             | Action                                                               |
| ---------- | ------------------------------------- | -------------------------------------------------------------------- |
| GET        | /api/{version}/city                   | Get the list of all the cities                                       |
| GET        | /api/{version}/city/{id}              | Get the information of a city by it is id                            |
| GET        | /api/{version}/city/name/{name}       | Get the information of a city by it is name                          |
| GET        | /api/{version}/country/Colombia       | Get the information of Colombia                                      |
| GET        | /api/{version}/Department             | Get the list of all the departments in Colombia                      |
| GET        | /api/{version}/Department/{id}        | Get the information of a department by it is id                      |
| GET        | /api/{version}/Department/name/{name} | Get the information of a department by it is name                    |
| GET        | /api/{version}/President              | Get the list of all the presidents in Colombia                       |
| GET        | /api/{version}/President/{id}         | Get the information of a president by it is id                       |
| GET        | /api/{version}/President/name/{name}  | Get the information of a president by it is name                     |
| GET        | /api/{version}/President/year/{name}  | Get the information of a president that ruled during a specific year |
| GET        | /api/{version}/TouristicAttraction             | Get the list of all the touristic attractions in Colombia                      |
| GET        | /api/{version}/TouristicAttraction/{id}        | Get the information of a touristic attraction by it is id                      |
| GET        | /api/{version}/TouristicAttraction/name/{name} | Get the information of a touristic attaction by it is name                    |

## Response Example 
* Content type: "application/json". Responses are JSON Objects. 
* Response header contains the HTTP CODE with the status. 
* Example:
 
>{"id":1,"name":"Colombia","description":"Colombia, officially the Republic of Colombia, is a country in South America with insular regions in North America—near Nicaragua's Caribbean coast—as well as in the Pacific Ocean. The Colombian mainland is bordered by the Caribbean Sea to the north, Venezuela to the east and northeast, Brazil to the southeast, Ecuador and Peru to the south and southwest, the Pacific Ocean to the west, and Panama to the northwest. Colombia is divided into 32 departments and the Capital District of Bogotá, the country's largest city. It covers an area of 1,141,748 square kilometers (440,831 sq mi), and has a population of 52 million. Colombia's cultural heritage—including language, religion, cuisine, and art—reflects its history as a Spanish colony, fusing cultural elements brought by immigration from Europe and the Middle East, with those brought by enslaved Africans, as well as with those of the various Indigenous civilizations that predate colonization. Spanish is the official state language, although English and 64 other languages are recognized regional languages.","stateCapital":"Bogotá","surface":1141748,"population":52235050,"languages":["Spanish","English"],"timeZone":"UTC-5","currency":"Colombian Peso","currencyCode":"COP","isoCode":"CO","internetDomain":".co","phonePrefix":"+57","radioPrefix":"HK","aircraftPrefix":"HK"}
## Technologies Used
* [.NET Core 6.*](https://dotnet.microsoft.com/en-us/) is a free, cross-platform, open source developer platform for building many kinds of applications. .NET is built on a high-performance runtime that is used in production by many high-scale apps.
* [PostgreSQL](https://www.postgresql.org/) is a powerful, open source object-relational database system with over 35 years of active development that has earned it a strong reputation for reliability, feature robustness, and performance.
* [Microsoft Azure](https://azure.microsoft.com/en-us/resources/cloud-computing-dictionary/what-is-azure/) The Azure cloud platform is more than 200 products and cloud services designed to help you bring new solutions to life—to solve today’s challenges and create the future. Build, run, and manage applications across multiple clouds, on-premises, and at the edge, with the tools and frameworks of your choice.
## Authors
* [Miguel Teheran](https://www.linkedin.com/in/mteheran/)
* [Rina Plata](https://www.linkedin.com/in/rina-plata/)
* [Veronica Guaman](https://www.linkedin.com/in/veronicaguaman/)
* [Mario Botero](https://www.linkedin.com/in/mariobot/)
## License
*Open Source*