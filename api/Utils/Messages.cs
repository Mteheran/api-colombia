namespace api.Utils
{
    public static class Messages
    {
        public struct EndpointMetadata
        {

            public struct Swagger{
                public const string sortedBy =  "It can be sorted by any of the fields that have numerical, string, or date values (for example: Id, name, description, etc.).";
                public const string sortDirection = "Possible values: 'asc' or 'desc'.";
            }
            public struct RequestMessages
            {
                public const string BadRequest = "Bad request - Invalid sort parameter.";
            }

            public struct CountryEndpoint
            {
                public const string MESSAGE_COUNTRY_SUMMARY = "General info about Colombia";
                public const string MESSAGE_COUNTRY_DESCRIPTION = "This endpoint returns the information about Colombia like TimeZone, Languages, Currency, etc...";
            }

            public struct DepartmentEndpoint
            {
                public const string MESSAGE_DEPARTMENT_LIST_SUMMARY = "List of departments in Colombia";
                public const string MESSAGE_DEPARTMENT_LIST_DESCRIPTION = "This endpoint returns the departments in colombia including a general info like description, phone prefix, capital city, etc...";
                public const string MESSAGE_DEPARTMENT_BYID_SUMMARY = "Department information by Id";
                public const string MESSAGE_DEPARTMENT_BYID_DESCRIPTION = "This endpoint returns the information for the department with the provided id";
                public const string MESSAGE_DEPARTMENT_BYNAME_SUMMARY = "Department information by name";
                public const string MESSAGE_DEPARTMENT_BYNAME_DESCRIPTION = "This endpoint returns the information for the department with the provided name";
                public const string MESSAGE_DEPARTMENT_SEARCH_SUMMARY = "Search departments by keyword ";
                public const string MESSAGE_DEPARTMENT_SEARCH_DESCRIPTION = "This endpoint returns a list of departments any of the following fields(Name, Description, PhonePrefix) match the provided keyword ";
                public const string MESSAGE_DEPARTMENT_PAGEDLIST_SUMMARY = "List of departments using pagination - api/v1/department/pagedList?page=1&pagesize=10";
                public const string MESSAGE_DEPARTMENT_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of departments using pagination including page, pagesize, total records and data, example api/v1/department/pagedList?page=1&pagesize=10";
                public const string MESSAGE_DEPARTMENT_CITIES_SUMMARY = "List of cities by departmentId";
                public const string MESSAGE_DEPARTMENT_CITIES_DESCRIPTION = "This endpoint returns a list of cities filtered by departmentid";
                public const string MESSAGE_DEPARTMENT_NATURALAREAS_SUMMARY = "List of natural areas by departmentId";
                public const string MESSAGE_DEPARTMENT_NATURALAREAS_DESCRIPTION = "This endpoint returns a list of natural areas filtered by departmentid";


            }
            public struct CityEndpoint
            {
                public const string MESSAGE_CITY_LIST_SUMMARY = "List of cities in Colombia";
                public const string MESSAGE_CITY_LIST_DESCRIPTION = "This endpoint returns the cities in colombia including general info like description, department, Surface city, etc...";
                public const string MESSAGE_CITY_BYID_SUMMARY = "City information by Id";
                public const string MESSAGE_CITY_BYID_DESCRIPTION = "This endpoint returns the information for the city with the provided id";
                public const string MESSAGE_CITY_BYNAME_SUMMARY = "City information by name";
                public const string MESSAGE_CITY_BYNAME_DESCRIPTION = "This endpoint returns the information for the city with the provided name";
                public const string MESSAGE_CITY_SEARCH_SUMMARY = "Search cities by keyword ";
                public const string MESSAGE_CITY_SEARCH_DESCRIPTION = "This endpoint returns a list of cities any of the following fields(Name, Description, PostalCode) match the provided keyword ";
                public const string MESSAGE_CITY_PAGEDLIST_SUMMARY = "List of cities using pagination - api/v1/city/pagedList?page=1&pagesize=10";
                public const string MESSAGE_CITY_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of cities using pagination including page, pagesize, total records and data, example api/v1/city/pagedList?page=1&pagesize=10";

            }
            public struct PresidentEndpoint
            {
                public const string MESSAGE_PRESIDENT_LIST_SUMMARY = "List of presidents in Colombia";
                public const string MESSAGE_PRESIDENT_LIST_DESCRIPTION = "This endpoint returns the presidents in colombia including general info like political party, city, start period, etc...";
                public const string MESSAGE_PRESIDENT_BYID_SUMMARY = "President information by Id";
                public const string MESSAGE_PRESIDENT_BYID_DESCRIPTION = "This endpoint returns the information for the president with the provided id";
                public const string MESSAGE_PRESIDENT_BYNAME_SUMMARY = "President information by name";
                public const string MESSAGE_PRESIDENT_BYNAME_DESCRIPTION = "This endpoint returns the information for the president with the provided name";
                public const string MESSAGE_PRESIDENT_BYYEAR_SUMMARY = "President of the provided year";
                public const string MESSAGE_PRESIDENT_BYYEAR_DESCRIPTION = "This endpoint returns the president or presidents in the provided year";
                public const string MESSAGE_PRESIDENT_SEARCH_SUMMARY = "Search presidents by keyword ";
                public const string MESSAGE_PRESIDENT_SEARCH_DESCRIPTION = "This endpoint returns a list of presidents any of the following fields(Name, Description, PoliticalParty,LastName) match the provided keyword ";
                public const string MESSAGE_PRESIDENT_PAGEDLIST_SUMMARY = "List of presidents using pagination - api/v1/president/pagedList?page=1&pagesize=10";
                public const string MESSAGE_PRESIDENT_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of presidents using pagination including page, pagesize, total records and data, example api/v1/president/pagedList?page=1&pagesize=10";
            }

            public struct TouristAttractionsEndpoint
            {
                public const string MESSAGE_TOURIST_ATTRACTION_LIST_SUMMARY = "List of touristic attractions in Colombia";
                public const string MESSAGE_TOURIST_ATTRACTION_LIST_DESCRIPTION = " This endpoint returns a list of touristic attractions including information about the city where they are located, the latitude and Longitude and image";
                public const string MESSAGE_TOURIST_ATTRACTION_BYNAME_SUMMARY = "Touristic attraction by name";
                public const string MESSAGE_TOURIST_ATTRACTION_BYNAME_DESCRIPTION = "This endpoint returns an specific touristic attraction by the provided name";
                public const string MESSAGE_TOURIST_ATTRACTION_BYID_SUMMARY = "Touristic attraction by id";
                public const string MESSAGE_TOURIST_ATTRACTION_BYID_DESCRIPTION = "This endpoint returns an specific touristic attraction by the provided id";
                public const string MESSAGE_TOURIST_ATTRACTION_SEARCH_SUMMARY = "Search touristic attractions by keyword ";
                public const string MESSAGE_TOURIST_ATTRACTION_SEARCH_DESCRIPTION = "This endpoint returns a list of touristic attractions any of the following fields(Name, Description,LastName,Latitude, Longitude) match the provided keyword ";
                public const string MESSAGE_TOURIST_ATTRACTION_PAGEDLIST_SUMMARY = "List of touristic attraction using pagination - api/v1/TouristicAttraction/pagedList?page=1&pagesize=10";
                public const string MESSAGE_TOURIST_ATTRACTION_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of touristic attractions using pagination including page, pagesize, total records and data, example api/v1/TouristicAttraction/pagedList?page=1&pagesize=10";
            }

            public struct RegionEndpoint
            {
                public const string MESSAGE_REGION_LIST_SUMMARY = "List of regions in Colombia";
                public const string MESSAGE_REGION_LIST_DESCRIPTION = " This endpoint returns a list of regions in Colombia";
                public const string MESSAGE_REGION_BYID_SUMMARY = "Region information by Id";
                public const string MESSAGE_REGION_BYID_DESCRIPTION = "This endpoint returns the information for the region with the provided id";
                public const string MESSAGE_BYID_DEPARMENTS_SUMMARY = "Region information by Id including deparment's list";
                public const string MESSAGE_BYID_DEPARMENTS_DESCRIPTION = "This endpoint returns the information for the region with the provided id including deparments";
            }

            public struct CategoryNaturalAreaEndpoint
            {
                public const string MESSAGE_LIST_SUMMARY = "List of category natural areas in Colombia";
                public const string MESSAGE_LIST_DESCRIPTION = " This endpoint returns a list of category natural areas  in Colombia";
                public const string MESSAGE_BYID_SUMMARY = "Category natural area information by Id";
                public const string MESSAGE_BYID_DESCRIPTION = "This endpoint returns the information for the category natural area with the provided id";
            }

            public struct NaturalAreaEndpoint
            {
                public const string MESSAGE_LIST_SUMMARY = "List of natural areas in Colombia";
                public const string MESSAGE_LIST_DESCRIPTION = " This endpoint returns a list of natural areas  in Colombia";
                public const string MESSAGE_BYID_SUMMARY = "Natural area information by Id";
                public const string MESSAGE_BYID_DESCRIPTION = "This endpoint returns the information for the natural area with the provided id";
                public const string MESSAGE_BYNAME_SUMMARY = "Natural place by name";
                public const string MESSAGE_BYNAME_DESCRIPTION = "This endpoint returns an specific natural places by the provided name";
                public const string MESSAGE_SEARCH_SUMMARY = "Search natural places by keyword ";
                public const string MESSAGE_SEARCH_DESCRIPTION = "This endpoint returns a list of touristic attractions any of the following fields(Name, Description,LastName,Latitude, Longitude) match the provided keyword ";
                public const string MESSAGE_PAGEDLIST_SUMMARY = "List of natural places using pagination - api/v1/naturalarea/pagedList?page=1&pagesize=10";
                public const string MESSAGE_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of natural places in Colombia using pagination including page, pagesize, total records and data, example api/v1/naturalarea/pagedList?page=1&pagesize=10";
            }

            public struct MapEndpoint
            {
                public const string MESSAGE_LIST_SUMMARY = "List of Maps related to Colombia";
                public const string MESSAGE_LIST_DESCRIPTION = "Returns a list of maps including, natural areas, deparmets distribution, water, etc...";
                public const string MESSAGE_BYID_SUMMARY = "Map information by Id";
                public const string MESSAGE_BYID_DESCRIPTION = "This endpoint returns the information for the map with the provided id";
            }

            public struct InvasiveSpecieEndpoint
            {
                public const string MESSAGE_INVASIVE_SPECIE_LIST_SUMMARY = "List of invasive species in Colombia";
                public const string MESSAGE_INVASIVE_SPECIE_LIST_DESCRIPTION = "This endpoint returns the  invasive species in colombia including general info like commonNames, image, manage, etc...";
                public const string MESSAGE_INVASIVE_SPECIE_BYID_SUMMARY = "Invasive specie information by Id";
                public const string MESSAGE_INVASIVE_SPECIE_BYID_DESCRIPTION = "This endpoint returns the information for the  invasive specie with the provided id";
                public const string MESSAGE_INVASIVE_SPECIE_BYNAME_SUMMARY = "Invasive specie information by name";
                public const string MESSAGE_INVASIVE_SPECIE_BYNAME_DESCRIPTION = "This endpoint returns the information for the  invasive specie with the provided name";
                public const string MESSAGE_INVASIVE_SPECIE_SEARCH_SUMMARY = "Search  invasive species by keyword ";
                public const string MESSAGE_INVASIVE_SPECIE_SEARCH_DESCRIPTION = "This endpoint returns a list of  invasive species any of the following fields(Name, CommonName, Manage) match the provided keyword ";
                public const string MESSAGE_INVASIVE_SPECIE_PAGEDLIST_SUMMARY = "List of  invasive species using pagination - api/v1/invasivespecie/pagedList?page=1&pagesize=10";
                public const string MESSAGE_INVASIVE_SPECIE_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of  invasive species using pagination including page, pagesize, total records and data, example api/v1/invasivespecie/pagedList?page=1&pagesize=10";
            }

            public struct NativeCommunityEndpoint
            {
                public const string MESSAGE_NATIVE_COMMUNITY_LIST_SUMMARY = "List of native communities in Colombia";
                public const string MESSAGE_NATIVE_COMMUNITY_LIST_DESCRIPTION = "This endpoint returns the native communities in colombia including general info like name, description, images, etc...";
                public const string MESSAGE_NATIVE_COMMUNITY_BYID_SUMMARY = "Native communities information by Id";
                public const string MESSAGE_NATIVE_COMMUNITY_BYID_DESCRIPTION = "This endpoint returns the information for the native community with the provided id";
                public const string MESSAGE_NATIVE_COMMUNITY_BYNAME_SUMMARY = "Native community information by name";
                public const string MESSAGE_NATIVE_COMMUNITY_BYNAME_DESCRIPTION = "This endpoint returns the information for the  native community with the provided name";
                public const string MESSAGE_NATIVE_COMMUNITY_SEARCH_SUMMARY = "Search native communities by keyword ";
                public const string MESSAGE_NATIVE_COMMUNITY_SEARCH_DESCRIPTION = "This endpoint returns a list of  native communities any of the following fields(Name, Description, Languages) match the provided keyword ";
                public const string MESSAGE_NATIVE_COMMUNITY_PAGEDLIST_SUMMARY = "List of native communities using pagination - api/v1/nativecommunity/pagedList?page=1&pagesize=10";
                public const string MESSAGE_NATIVE_COMMUNITY_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of native communities using pagination including page, pagesize, total records and data, example api/v1/nativecommunity/pagedList?page=1&pagesize=10";
            }

            public struct IndigenousReservationEndpoint
            {
                public const string MESSAGE_INDIGENOUS_RESERVATION_LIST_SUMMARY = "List of Indigenous reservations in Colombia";
                public const string MESSAGE_INDIGENOUS_RESERVATION_LIST_DESCRIPTION = "This endpoint returns the Indigenous reservations in colombia including general info like name, description, images, etc...";
                public const string MESSAGE_INDIGENOUS_RESERVATION_BYID_SUMMARY = "Indigenous reservations information by Id";
                public const string MESSAGE_INDIGENOUS_RESERVATION_BYID_DESCRIPTION = "This endpoint returns the information for the indigenous reservation with the provided id";
                public const string MESSAGE_INDIGENOUS_RESERVATION_BYNAME_SUMMARY = "Indigenous reservation information by name";
                public const string MESSAGE_INDIGENOUS_RESERVATION_BYNAME_DESCRIPTION = "This endpoint returns the information for the  indigenous reservation with the provided name";
                public const string MESSAGE_INDIGENOUS_RESERVATION_SEARCH_SUMMARY = "Search Indigenous reservations by keyword ";
                public const string MESSAGE_INDIGENOUS_RESERVATION_SEARCH_DESCRIPTION = "This endpoint returns a list of  Indigenous reservations any of the following fields(Name, Description, Languages) match the provided keyword ";
                public const string MESSAGE_INDIGENOUS_RESERVATION_PAGEDLIST_SUMMARY = "List of Indigenous reservations using pagination - api/v1/IndigenousReservation/pagedList?page=1&pagesize=10";
                public const string MESSAGE_INDIGENOUS_RESERVATION_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of Indigenous reservations using pagination including page, pagesize, total records and data, example api/v1/IndigenousReservation/pagedList?page=1&pagesize=10";
            }          

            public struct AirportEndpoint
            {
                public const string MESSAGE_AIRPORT_LIST_SUMMARY = "List of Airports in Colombia";
                public const string MESSAGE_AIRPORT_LIST_DESCRIPTION = "This endpoint returns the Airports in colombia including general info like name, description, images, etc...";
                public const string MESSAGE_AIRPORT_BYID_SUMMARY = "Airports information by Id";
                public const string MESSAGE_AIRPORT_BYID_DESCRIPTION = "This endpoint returns the information for the airports with the provided id";
                public const string MESSAGE_AIRPORT_BYNAME_SUMMARY = "Airports information by name";
                public const string MESSAGE_AIRPORT_BYNAME_DESCRIPTION = "This endpoint returns the information for the airport with the provided name";
                public const string MESSAGE_AIRPORT_SEARCH_SUMMARY = "Search Airports by keyword ";
                public const string MESSAGE_AIRPORT_SEARCH_DESCRIPTION = "This endpoint returns a list of  Airports any of the following fields(Name, Description, city) match the provided keyword ";
                public const string MESSAGE_AIRPORT_PAGEDLIST_SUMMARY = "List of Airports using pagination - api/v1/airport/pagedList?page=1&pagesize=10";
                public const string MESSAGE_AIRPORT_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of Airports using pagination including page, pagesize, total records and data, example api/v1/airport/pagedList?page=1&pagesize=10";
            }

            public struct ConstitutionArticleEndpoint
            {
                public const string MESSAGE_CONSTITUTION_ARTICLE_LIST_SUMMARY = "List of Constitution Articles in Colombia";
                public const string MESSAGE_CONSTITUTION_ARTICLE_LIST_DESCRIPTION = "This endpoint returns the Constitution Articles in colombia including general info like title, chapter number, content, etc...";
                public const string MESSAGE_CONSTITUTION_ARTICLE_BYID_SUMMARY = "Constitution Articles information by Id";
                public const string MESSAGE_CONSTITUTION_ARTICLE_BYID_DESCRIPTION = "This endpoint returns the information for the airports with the provided id";
                public const string MESSAGE_CONSTITUTION_ARTICLE_BYNAME_SUMMARY = "Constitution Articles information by name";
                public const string MESSAGE_CONSTITUTION_ARTICLE_BYNAME_DESCRIPTION = "This endpoint returns the information for the airport with the provided name";
                public const string MESSAGE_CONSTITUTION_ARTICLE_SEARCH_SUMMARY = "Search Constitution Articles by keyword ";
                public const string MESSAGE_CONSTITUTION_ARTICLE_SEARCH_DESCRIPTION = "This endpoint returns a list of  Constitution Articles any of the following fields(title, chapter number, content) match the provided keyword ";
                public const string MESSAGE_CONSTITUTION_ARTICLE_PAGEDLIST_SUMMARY = "List of Constitution Articles using pagination - api/v1/ConstitutionArticles/pagedList?page=1&pagesize=10";
                public const string MESSAGE_CONSTITUTION_ARTICLE_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of Constitution Articles using pagination including page, pagesize, total records and data, example api/v1/ConstitutionArticles/pagedList?page=1&pagesize=10";
                public const string MESSAGE_CONSTITUTION_ARTICLE_BYCHAPTER_SUMMARY = "Return Constitution Articles by Chapter Number ";
                public const string MESSAGE_CONSTITUTION_ARTICLE_BYCHAPTER_DESCRIPTION = "This endpoint returns a list of Constitution Articles by the chapter number p´rovided ";

            }

            public struct RadioEndpoint
            {
                public const string MESSAGE_RADIO_LIST_SUMMARY = "List of Radios in Colombia";
                public const string MESSAGE_RADIO_LIST_DESCRIPTION = "This endpoint returns the radios in colombia including general info like name, url, frequency, etc...";
                public const string MESSAGE_RADIO_BYID_SUMMARY = "Radios information by Id";
                public const string MESSAGE_RADIO_BYID_DESCRIPTION = "This endpoint returns the information for the radio with the provided id";
                public const string MESSAGE_RADIO_BYNAME_SUMMARY = "Radios information by name";
                public const string MESSAGE_RADIO_BYNAME_DESCRIPTION = "This endpoint returns the information for the radio with the provided name";
                public const string MESSAGE_RADIO_SEARCH_SUMMARY = "Search Radios by keyword ";
                public const string MESSAGE_RADIO_SEARCH_DESCRIPTION = "This endpoint returns a list of Radios any of the following fields(Name, Frequency, URL, Streamers) match the provided keyword ";
                public const string MESSAGE_RADIO_PAGEDLIST_SUMMARY = "List of Radios using pagination - api/v1/radio/pagedList?page=1&pagesize=10";
                public const string MESSAGE_RADIO_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of Radios using pagination including page, pagesize, total records and data, example api/v1/radio/pagedList?page=1&pagesize=10";
            }

            public struct HolidayEndpoint
            {
                 public const string MESSAGE_HOLIDAY_BY_YEAR_LIST_SUMMARY = "List of holidays in Colombia by year";
                 public const string MESSAGE_HOLIDAY_BY_YEAR_LIST_DESCRIPTION = "This endpoint returns the list of holidays per year in Colombia";
                 public const string MESSAGE_HOLIDAY_BY_YEAR_MONTH_LIST_SUMMARY = "List of holidays in Colombia by year and moth"; 
                 public const string MESSAGE_HOLIDAY_BY_YEAR_MONTH_LIST_DESCRIPTION = "This endpoint returns the list of holidays per year and month in Colombia";
                 public const string BadRequestInvalidMonth = "Bad request - Invalid month parameter.";
                 public const string BadRequestInvalidYear = "Bad request - Invalid year parameter.";
            }

            public struct Holidays{
                public const string NEW_YEAR_DESCRIPTION = "Año Nuevo";
                public const string WISE_MEN_DESCRIPTION = "Día de los Reyes Magos";
                public const string SAINT_JOSEPH_DESCRIPTION = "Día de San José";
                public const string SUNDAY_PALMS_DESCRIPTION = "Domingo de Ramos";
                public const string HOLY_THURSDAY_DESCRIPTION = "Jueves Santo";
                public const string HOLY_FRIDAY_DESCRIPTION = "Viernes Santo";
                public const string RESURRECTION_DESCRIPTION = "Domingo de Pascua o Resurrección";
                public const string LABOR_DAY_DESCRIPTION = "Día del Trabajo"; 
                public const string ASCENSION_DESCRIPTION =  "Día de la Ascensión"; 
                public const string CORPUS_CHRISTI_DESCRIPTION =   "Día de Corpus Christi"; 
                public const string SACRED_HEART_JESUS_DESCRIPTION = "Día del Sagrado Corazón de Jesús";  
                public const string SAINT_PETER_PAUL_DESCRIPTION =  "Día de San Pedro y San Pablo";
                public const string INDEPENDENCE_DESCRIPTION =  "Día de la Independencia";
                public const string BOYACA_BATTLE_DESCRIPTION=  "Batalla de Boyacá";
                public const string ASSUMPTION_VIRGIN_DESCRIPTION =  "Asunción de la Virgen";
                public const string RACE_DESCRIPTION =  "Día de la Raza";  
                public const string ALL_SANTS_DESCRIPTION = "Día de todos los Santos"; 
                 public const string CARTAGENA_INDEPENDENCE_DESCRIPTION = "Independencia de Cartagena";
                 public const string INMACULATE_CONCEPTION_DESCRIPTION = "Inmaculada Concepción";
                 public const string CHRISTMAS_DESCRIPTION = "Navidad";
            }

            public struct TypicalDishEndpoint 
            {
                public const string MESSAGE_TYPICAL_DISH_LIST_SUMMARY = "List of typical dishes in Colombia";
                public const string MESSAGE_TYPICAL_DISH_LIST_DESCRIPTION = "This endpoint returns the typical dishes in colombia including a general info like description, image reference..";
                 public const string MESSAGE_TYPICAL_DISH_BYID_SUMMARY = "Typical dishes information by Id";
                public const string MESSAGE_TYPICAL_DISH_BYID_DESCRIPTION =   "This endpoint returns the typical dishes in colombia including a general info like description, image reference with the provided id";
                public const string MESSAGE_TYPICAL_DISH_DEPARTMENT_SUMMARY = "List of typical dishes by departmentId";
                public const string MESSAGE_TYPICAL_DISH_DEPARTMENT_DESCRIPTION = "This endpoint returns a list of typical dishes filtered by departmentid";
                public const string MESSAGE_TYPICAL_DISH_BYNAME_SUMMARY = "Typical dishes information by name";
                public const string MESSAGE_TYPICAL_DISH_BYNAME_DESCRIPTION = "This endpoint returns the information for the typical dishes with the provided name";
                public const string MESSAGE_TYPICAL_DISH_SEARCH_SUMMARY = "Search typical dish by keyword ";
                public const string MESSAGE_TYPICAL_DISH_SEARCH_DESCRIPTION = "This endpoint returns a list of typical dish any of the following fields(Name, Description) match the provided keyword ";
                public const string MESSAGE_TYPICAL_DISH_PAGEDLIST_SUMMARY = "List of typical dishes using pagination";
                public const string MESSAGE_TYPICAL_DISH_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of typical dishes using pagination including page, pagesize, total records and data, example api/v1/typicaldishes/pagedList?page=1&pagesize=10";

            }

            public struct TraditionalFairAndFestivalEndpoint{
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_LIST_SUMMARY = "List of traditional fair and festival in Colombia";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_LIST_DESCRIPTION = "This endpoint returns the traditional fair and festival in colombia including a general info like name, description, city...";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYID_SUMMARY = "Traditional fair and festival information by Id";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYID_DESCRIPTION =   "This endpoint returns the traditional fair and festival in colombia including a general info like name, description, city with the provided id";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYCITY_SUMMARY = "List of traditional fair and festival by City Id";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYCITY_DESCRIPTION = "This endpoint returns a list of traditional fair and festival filtered by City Id.";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYNAME_SUMMARY = "Traditional fair and festival information by name";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_BYNAME_DESCRIPTION = "This endpoint returns the information for the traditional fair and festival with the provided name";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_SEARCH_SUMMARY = "Search traditional fair and festival by keyword ";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_SEARCH_DESCRIPTION = "This endpoint returns a list of  traditional fair and festival any of the following fields(Name, Description) match the provided keyword ";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_PAGEDLIST_SUMMARY = "List of traditional fair and festival using pagination";
                public const string MESSAGE_TRADITIONAL_FAIR_AND_FESTIVAL_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of traditional fair and festival using pagination including page, pagesize, total records and data, example api/v1/typicaldishes/pagedList?page=1&pagesize=10";

            }

            public struct IntangibleHeritageEndpoint
            {
                public const string MESSAGE_INTANGIBLE_HERITAGE_LIST_SUMMARY = "List of intangible heritages in Colombia";
                public const string MESSAGE_INTANGIBLE_HERITAGE_LIST_DESCRIPTION = "This endpoint returns the intangible heritages in colombia including a general info like name, scope, inclusion year...";
                public const string MESSAGE_INTANGIBLE_HERITAGE_BYID_SUMMARY = "Intangible heritage information by Id";
                public const string MESSAGE_INTANGIBLE_HERITAGE_BYID_DESCRIPTION = "This endpoint returns the intangible heritage in colombia including a general info like name, scope, inclusion year with the provided id";
                public const string MESSAGE_INTANGIBLE_HERITAGE_DEPARTMENT_SUMMARY = "List of intangible heritages by departmentId";
                public const string MESSAGE_INTANGIBLE_HERITAGE_DEPARTMENT_DESCRIPTION = "This endpoint returns a list of intangible heritages filtered by departmentid";
                public const string MESSAGE_INTANGIBLE_HERITAGE_BYNAME_SUMMARY = "Intangible heritage information by name";
                public const string MESSAGE_INTANGIBLE_HERITAGE_BYNAME_DESCRIPTION = "This endpoint returns the information for the intangible heritage with the provided name";
                public const string MESSAGE_INTANGIBLE_HERITAGE_SEARCH_SUMMARY = "Search intangible heritage by keyword";
                public const string MESSAGE_INTANGIBLE_HERITAGE_SEARCH_DESCRIPTION = "This endpoint returns a list of intangible heritage any of the following fields(Name, Scope) match the provided keyword";
                public const string MESSAGE_INTANGIBLE_HERITAGE_PAGEDLIST_SUMMARY = "List of intangible heritages using pagination";
                public const string MESSAGE_INTANGIBLE_HERITAGE_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of intangible heritages using pagination including page, pagesize, total records and data, example api/v1/intangibleheritage/pagedList?page=1&pagesize=10";
            }

            public struct HeritageCityEndpoint
            {
                public const string MESSAGE_HERITAGE_CITY_LIST_SUMMARY = "List of heritage cities in Colombia";
                public const string MESSAGE_HERITAGE_CITY_LIST_DESCRIPTION = "This endpoint returns the heritage cities in Colombia including general info like name, description, city, department, and image.";
                public const string MESSAGE_HERITAGE_CITY_BYID_SUMMARY = "Heritage city information by Id";
                public const string MESSAGE_HERITAGE_CITY_BYID_DESCRIPTION = "This endpoint returns the heritage city information with the provided id";
                public const string MESSAGE_HERITAGE_CITY_BYNAME_SUMMARY = "Heritage city information by name";
                public const string MESSAGE_HERITAGE_CITY_BYNAME_DESCRIPTION = "This endpoint returns the heritage city information with the provided name";
                public const string MESSAGE_HERITAGE_CITY_SEARCH_SUMMARY = "Search heritage cities by keyword";
                public const string MESSAGE_HERITAGE_CITY_SEARCH_DESCRIPTION = "This endpoint returns a list of heritage cities any of the following fields(Name, Description, Image) match the provided keyword";
                public const string MESSAGE_HERITAGE_CITY_PAGEDLIST_SUMMARY = "List of heritage cities using pagination";
                public const string MESSAGE_HERITAGE_CITY_PAGEDLIST_DESCRIPTION = "This endpoint returns a list of heritage cities using pagination including page, pagesize, total records and data, example api/v1/heritagecity/pagedList?page=1&pagesize=10";
            }
        }
    }
}
