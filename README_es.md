# API-COLOMBIA

## Introducción
API Colombia es una API pública RESTful que permite a los usuarios obtener una variedad de información pública sobre Colombia.

## Características de soporte del proyecto
* Minimal API's endpoints para obtener información sobre:
  - Información general sobre el país.
  - Departamentos/Estados.
  - Ciudades.
  - Presidentes.
  - Atracciones turísticas.
  - Áreas naturales y categorías.
  - Aeropuertos.
  - Días festivos.
  - Estaciones de radio.
  - Platos típicos.
  - Ferias y festivales tradicionales.
  - Resguardos indígenas.
  - Comunidades nativas.
  - Especies invasoras.
  - Artículos de la constitución.
  - Mapas.
* Documentación Swagger.
* No requiere autenticación.
* Pruebas de integración con base de datos en memoria.

## Versions
[Changelog - Versions](/CHANGELOG.md)


## Guía de instalación
* Clona este repositorio [aquí](https://github.com/Mteheran/api-colombia).
* La rama "develop" es la rama más estable en cualquier momento, asegúrate de estar trabajando en ella.
* Se debe instalar [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download) en la máquina.
* Actualiza la cadena de conexión con tu base de datos preferida en el archivo "appsettings.json".
* Ejecuta el comando `dotnet build` para generar la compilación del proyecto.

## Pruebas

El proyecto incluye pruebas de integración completas usando xUnit y una base de datos en memoria. Las pruebas verifican que todos los endpoints de la API funcionen correctamente.

### Ejecutar Pruebas

Para ejecutar todas las pruebas:
```bash
dotnet test
```

Para ejecutar pruebas con cobertura:
```bash
dotnet test /p:CollectCoverage=true
```

### Estructura de Pruebas

Las pruebas de integración se encuentran en el proyecto `api.Tests` y utilizan:
- **xUnit** como framework de pruebas
- **CustomWebApplicationFactory** para crear un servidor de pruebas con una base de datos en memoria
- **Microsoft.AspNetCore.Mvc.Testing** para pruebas de cliente HTTP
- **AutoFixture** para generación de datos de prueba

Cada endpoint tiene pruebas de integración correspondientes que verifican:
- Respuestas exitosas con datos correctos
- Funcionalidad de paginación
- Capacidades de ordenamiento
- Funcionalidad de búsqueda
- Manejo de errores (404, 400, etc.)

La base de datos de prueba se inicializa con datos de muestra para cada tipo de entidad, asegurando resultados de prueba consistentes.

## Uso
La página pública [api-colombia.com](https://api-colombia.com/) tiene información útil sobre la API y una breve descripción de los endpoints disponibles y la misma permite el acceso a los endpoints descritos a continuación.

* La documentación de swagger se puede encontrar en la siguiente [URL](https://api-colombia.com/swagger/index.html)

## API Endpoints

### Ciudades
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/city                   				| Obtiene la lista de todas las ciudades                               			|
| GET        | /api/{version}/city/{id}              				| Obtiene la información de una ciudad por su id                       			|
| GET        | /api/{version}/city/name/{name}       				| Obtiene la información de una ciudad por su nombre                   			|
| GET        | /api/{version}/city/search/{keyword}  				| Obtiene la información de una ciudad buscando por una palabra clave  			|
| GET        | /api/{version}/city/pagedList         				| Obtiene la lista de todas las ciudades paginadas                     			|

### País
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/country/Colombia       				| Obtiene la información de Colombia                                   			|

### Departamentos
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/Department             				| Obtiene la lista de todos los departamentos en Colombia              			|
| GET        | /api/{version}/Department/{id}        				| Obtiene la información de un departamento por su id                  			|
| GET        | /api/{version}/Department/name/{name} 				| Obtiene la información de un departamento por su nombre             			|
| GET        | /api/{version}/Department/search/{keyword} 			| Obtiene la información de un departamento que busca por una palabra clave 	|
| GET        | /api/{version}/Department/pagedList   				| Obtiene la lista de todos los departamentos paginados                			|

### Regiones
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/Region                 				| Obtiene la lista de todas las regiones en Colombia                   			|

### Presidentes
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/President              				| Obtiene la lista de todos los presidentes en Colombia                			|
| GET        | /api/{version}/President/{id}         				| Obtiene la información de un presidente por su id			        			|
| GET        | /api/{version}/President/name/{name} 				| Obtiene la información de un presidente por su nombre                			|
| GET        | /api/{version}/President/year/{year}  				| Obtiene la información de un presidente que gobernó durante un año específico	|
| GET        | /api/{version}/President/search/{keyword}      		| Obtiene la información de un presidente que busca por una palabra clave       |
| GET        | /api/{version}/President/pagedList   				| Obtiene la lista de todos los presidentes paginados                           |

### Atracciones Turísticas
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/TouristicAttraction             		| Obtiene la lista de todas las atracciones turísticas en Colombia     			|
| GET        | /api/{version}/TouristicAttraction/{id}        		| Obtiene la información de una atracción turística por su identificación 		|
| GET        | /api/{version}/TouristicAttraction/name/{name} 		| Obtiene la información de una atracción turística por su nombre         		|
| GET        | /api/{version}/TouristicAttraction/search/{keyword} 	| Obtiene la información de una atracción turística buscando por una palabra clave	|
| GET        | /api/{version}/TouristicAttraction/pagedList   		| Obtiene la lista de todas las atracciones turísticas paginadas                |

### Áreas Naturales
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/NaturalArea             				| Obtiene la lista de todas las áreas naturales en Colombia     			|
| GET        | /api/{version}/NaturalArea/{id}        				| Obtiene la información de un área natural por su id 		|
| GET        | /api/{version}/NaturalArea/name/{name} 				| Obtiene la información de un área natural por su nombre         		|
| GET        | /api/{version}/NaturalArea/search/{keyword} 		| Obtiene la información de un área natural buscando por una palabra clave	|
| GET        | /api/{version}/NaturalArea/pagedList   			| Obtiene la lista de todas las áreas naturales paginadas                |

### Categorías de Áreas Naturales
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/CategoryNaturalArea             		| Obtiene la lista de todas las categorías de áreas naturales     			|
| GET        | /api/{version}/CategoryNaturalArea/{id}        		| Obtiene la información de una categoría de área natural por su id 		|
| GET        | /api/{version}/CategoryNaturalArea/{id}/NaturalAreas | Obtiene la información de una categoría de área natural con sus áreas naturales por id         		|

### Aeropuertos
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/Airport             					| Obtiene la lista de todos los aeropuertos en Colombia     			|
| GET        | /api/{version}/Airport/{id}        					| Obtiene la información de un aeropuerto por su id 		|
| GET        | /api/{version}/Airport/name/{name} 					| Obtiene la información de un aeropuerto por su nombre         		|
| GET        | /api/{version}/Airport/search/{keyword} 				| Obtiene la información de un aeropuerto buscando por una palabra clave	|
| GET        | /api/{version}/Airport/pagedList   					| Obtiene la lista de todos los aeropuertos paginados                |

### Días Festivos
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/Holiday/year/{year}             		| Obtiene la lista de todos los días festivos para un año específico     			|
| GET        | /api/{version}/Holiday/year/{year}/month/{month}     | Obtiene la lista de todos los días festivos para un año y mes específicos 		|

### Estaciones de Radio
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/Radio             						| Obtiene la lista de todas las estaciones de radio en Colombia     			|
| GET        | /api/{version}/Radio/{id}        					| Obtiene la información de una estación de radio por su id 		|
| GET        | /api/{version}/Radio/name/{name} 					| Obtiene la información de una estación de radio por su nombre         		|
| GET        | /api/{version}/Radio/search/{keyword} 				| Obtiene la información de una estación de radio buscando por una palabra clave	|
| GET        | /api/{version}/Radio/pagedList   					| Obtiene la lista de todas las estaciones de radio paginadas                |

### Platos Típicos
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/TypicalDish             				| Obtiene la lista de todos los platos típicos en Colombia     			|
| GET        | /api/{version}/TypicalDish/{id}        				| Obtiene la información de un plato típico por su id 		|
| GET        | /api/{version}/TypicalDish/{id}/department 			| Obtiene la lista de platos típicos por id de departamento         		|
| GET        | /api/{version}/TypicalDish/name/{name} 				| Obtiene la información de un plato típico por su nombre         		|
| GET        | /api/{version}/TypicalDish/search/{keyword} 		| Obtiene la información de un plato típico buscando por una palabra clave	|
| GET        | /api/{version}/TypicalDish/pagedList   				| Obtiene la lista de todos los platos típicos paginados                |

### Ferias y Festivales Tradicionales
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/TraditionalFairAndFestival            | Obtiene la lista de todas las ferias y festivales tradicionales en Colombia     			|
| GET        | /api/{version}/TraditionalFairAndFestival/{id}        	| Obtiene la información de una feria o festival tradicional por su id 		|
| GET        | /api/{version}/TraditionalFairAndFestival/{id}/city 	| Obtiene la lista de ferias y festivales tradicionales por id de ciudad         		|
| GET        | /api/{version}/TraditionalFairAndFestival/name/{name} | Obtiene la información de una feria o festival tradicional por su nombre         		|
| GET        | /api/{version}/TraditionalFairAndFestival/search/{keyword} | Obtiene la información de una feria o festival tradicional buscando por una palabra clave	|
| GET        | /api/{version}/TraditionalFairAndFestival/pagedList | Obtiene la lista de todas las ferias y festivales tradicionales paginadas                |

### Resguardos Indígenas
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/IndigenousReservation             		| Obtiene la lista de todos los resguardos indígenas en Colombia     			|
| GET        | /api/{version}/IndigenousReservation/{id}        		| Obtiene la información de un resguardo indígena por su id 		|
| GET        | /api/{version}/IndigenousReservation/name/{name} 		| Obtiene la información de un resguardo indígena por su nombre         		|
| GET        | /api/{version}/IndigenousReservation/search/{keyword} | Obtiene la información de un resguardo indígena buscando por una palabra clave	|
| GET        | /api/{version}/IndigenousReservation/pagedList   	| Obtiene la lista de todos los resguardos indígenas paginados                |

### Comunidades Nativas
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/NativeCommunity             			| Obtiene la lista de todas las comunidades nativas en Colombia     			|
| GET        | /api/{version}/NativeCommunity/{id}        		| Obtiene la información de una comunidad nativa por su id 		|
| GET        | /api/{version}/NativeCommunity/name/{name} 			| Obtiene la información de una comunidad nativa por su nombre         		|
| GET        | /api/{version}/NativeCommunity/search/{keyword} 	| Obtiene la información de una comunidad nativa buscando por una palabra clave	|
| GET        | /api/{version}/NativeCommunity/pagedList   			| Obtiene la lista de todas las comunidades nativas paginadas                |

### Especies Invasoras
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/InvasiveSpecie             			| Obtiene la lista de todas las especies invasoras en Colombia     			|
| GET        | /api/{version}/InvasiveSpecie/{id}        			| Obtiene la información de una especie invasora por su id 		|
| GET        | /api/{version}/InvasiveSpecie/name/{name} 			| Obtiene la información de una especie invasora por su nombre         		|
| GET        | /api/{version}/InvasiveSpecie/search/{keyword} 		| Obtiene la información de una especie invasora buscando por una palabra clave	|
| GET        | /api/{version}/InvasiveSpecie/pagedList   			| Obtiene la lista de todas las especies invasoras paginadas                |

### Artículos de la Constitución
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/ConstitutionArticle             		| Obtiene la lista de todos los artículos de la constitución     			|
| GET        | /api/{version}/ConstitutionArticle/{id}        		| Obtiene la información de un artículo de la constitución por su id 		|
| GET        | /api/{version}/ConstitutionArticle/search/{keyword} | Obtiene la información de un artículo de la constitución buscando por una palabra clave	|
| GET        | /api/{version}/ConstitutionArticle/pagedList   		| Obtiene la lista de todos los artículos de la constitución paginados                |
| GET        | /api/{version}/ConstitutionArticle/byChapterNumber/{chapternumber} | Obtiene la lista de artículos de la constitución por número de capítulo                |

### Mapas
| Verbos HTTP| Endpoints                             				| Acción                                                              			|
| ---------- | ---------------------------------------------------- | ----------------------------------------------------------------------------- |
| GET        | /api/{version}/Map             						| Obtiene la lista de todos los mapas     			|
| GET        | /api/{version}/Map/{id}        						| Obtiene la información de un mapa por su id 		|

## Ejemplo de Respuesta
* Content type: "application/json".  Las respuestas son objetos JSON.
* El encabezado de respuesta contiene el código HTTP con el estado. 
* Ejemplo:
 
```json
 {"id":1,"name":"Colombia","description":"Colombia, officially the Republic of Colombia, is a country in South America with insular regions in North America—near Nicaragua's Caribbean coast—as well as in the Pacific Ocean. The Colombian mainland is bordered by the Caribbean Sea to the north, Venezuela to the east and northeast, Brazil to the southeast, Ecuador and Peru to the south and southwest, the Pacific Ocean to the west, and Panama to the northwest. Colombia is divided into 32 departments and the Capital District of Bogotá, the country's largest city. It covers an area of 1,141,748 square kilometers (440,831 sq mi), and has a population of 52 million. Colombia's cultural heritage—including language, religion, cuisine, and art—reflects its history as a Spanish colony, fusing cultural elements brought by immigration from Europe and the Middle East, with those brought by enslaved Africans, as well as with those of the various Indigenous civilizations that predate colonization. Spanish is the official state language, although English and 64 other languages are recognized regional languages.","stateCapital":"Bogotá","surface":1141748,"population":52235050,"languages":["Spanish","English"],"timeZone":"UTC-5","currency":"Colombian Peso","currencyCode":"COP","isoCode":"CO","internetDomain":".co","phonePrefix":"+57","radioPrefix":"HK","aircraftPrefix":"HK"}
 ```
## Tecnologías utilizadas
* [.NET 10](https://dotnet.microsoft.com/en-us/) es una plataforma de desarrollo gratuita, multiplataforma y de código abierto para crear muchos tipos de aplicaciones. .NET se basa en un tiempo de ejecución de alto rendimiento(high-performance runtime) que se usa en producción en muchas aplicaciones de gran escala.
* [PostgreSQL](https://www.postgresql.org/) s un potente sistema de base de datos relacional de objetos de código abierto con más de 35 años de desarrollo activo que le ha valido una sólida reputación por su fiabilidad, robustez de características y rendimiento.
* [Microsoft Azure](https://azure.microsoft.com/en-us/resources/cloud-computing-dictionary/what-is-azure/) La plataforma en la nube de Azure consta de más de 200 productos y servicios en la nube diseñados para ayudarle a dar vida a nuevas soluciones, para resolver los desafíos actuales y crear el futuro. Cree, ejecute y administre aplicaciones en múltiples nubes, en las instalaciones y en el perímetro, con las herramientas y los marcos de su elección.

## Contribuidores ✨
<!-- INSIGNIA-DE-TODOS-LOS-COLABORADORES:INICIO - No eliminar ni modificar esta sección -->
[![Todos los colaboradores](https://img.shields.io/badge/all_contributors-5-orange.svg?style=flat-square)](#contributors-)
<!-- IINSIGNIA-DE-TODOS-LOS-COLABORADORES:FIN -->
Gracias a estas maravillosas personas ([emoji key](https://allcontributors.org/docs/en/emoji-key)):
<!-- LISTA-DE-TODOS-LOS COLABORADORES:INICIO - No quitar ni modificar esta sección -->
<!-- prettier-ignore-inicio -->
<!-- markdownlint-desabilitado -->
<table>
  <tbody>
    <tr>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/mteheran/"><img src="https://avatars.githubusercontent.com/u/3578356?v=4?s=100" width="100px;" alt="Miguel Teheran"/><br /><sub><b>Miguel Teheran</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/mteheran/api-colombia/commits?author=mteheran" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/rina-plata/"><img src="https://avatars.githubusercontent.com/u/55161289?v=4?s=100" width="100px;" alt="Rina Plata"/><br /><sub><b>Rina Plata</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/mteheran/api-colombia/commits?author=rinaplata" title="Documentation">💻</a> <a href="#tutorial-rinaplata" title="Tutorials">✅</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/veronicaguaman/"><img src="https://avatars.githubusercontent.com/u/70024610?v=4?s=100" width="100px;" alt="Veronica Guaman"/><br /><sub><b>Veronica Guaman</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/mteheran/api-colombia/commits?author=VeronicaGuaman" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/mariobot/"><img src="https://avatars.githubusercontent.com/u/1220191?v=4?s=100" width="100px;" alt="Mario Botero"/><br /><sub><b>Mario Botero</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/mteheran/api-colombia/commits?author=mariobot" title="Code">💻</a></td>
      <td align="center" valign="top" width="14.28%"><a href="https://www.linkedin.com/in/alejandrohv/"><img src="https://avatars.githubusercontent.com/u/99321349?v=4?s=100" width="100px;" alt="Alejandro Herreño"/><br /><sub><b>Alejandro Herreño</b></sub></a><br /><a href="https://github.com/Mteheran/api-colombia/mteheran/api-colombia/commits?author=Alejandrohv06" title="Documentation">📖</a> <a href="https://github.com/Mteheran/api-colombia/mteheran/api-colombia/commits?author=Alejandrohv06" title="Code">💻</a></td>
    </tr>
  </tbody>
  <tfoot>
    <tr>
      <td align="center" size="13px" colspan="7">
        <img src="https://raw.githubusercontent.com/all-contributors/all-contributors-cli/1b8533af435da9854653492b1327a23a4dbd0a10/assets/logo-small.svg">
          <a href="https://all-contributors.js.org/docs/en/bot/usage">Agrega tus contribuciones</a>
        </img>
      </td>
    </tr>
  </tfoot>
</table>

<!-- markdownlint-restaurado -->
<!-- prettier-ignore-fin -->

<!-- LISTA-DE-TODOS-LOS COLABORADORES:FIN -->

Este proyecto sigue la especificación [all-contributors](https://github.com/all-contributors/all-contributors). Las contribuciones de cualquier tipo son bienvenidas!
## Licencia
Licencia MIT
