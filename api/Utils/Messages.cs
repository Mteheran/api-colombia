namespace api.Utils
{
    public static class Messages
    {
        public struct EndpointMetadata
        {
            public struct CountryEndpoint
            {
                public const string MESSAGE_COUNTRY_SUMMARY = "General info about Ecuador";
                public const string MESSAGE_COUNTRY_DESCRIPTION = "This endpoint returns the information about Ecuador like TimeZone, Languages, Currency, etc...";
            }

            public struct ProvinceEndpoint
            {
                public const string MESSAGE_PROVINCE_LIST_SUMMARY = "List of provinces in Ecuador";
                public const string MESSAGE_PROVINCE_LIST_DESCRIPTION = "This endpoint returns the provinces in Ecuador including a general info like description, phone prefix, capital city, etc...";
                public const string MESSAGE_PROVINCE_BYID_SUMMARY = "Province information by Id";
                public const string MESSAGE_PROVINCE_BYID_DESCRIPTION = "This endpoint returns the information for the province with the provided id";
                public const string MESSAGE_PROVINCE_BYNAME_SUMMARY = "Province information by name";
                public const string MESSAGE_PROVINCE_BYNAME_DESCRIPTION = "This endpoint returns the information for the province with the provided name";
                public const string MESSAGE_PROVINCE_SEARCH_SUMMARY = "Search provinces by keyword ";
                public const string MESSAGE_PROVINCE_SEARCH_DESCRIPTION = "This endpoint returns a list of provinces any of the following fields(Name, Description, PhonePrefix) match the provided keyword ";
            }
            public struct CityEndpoint
            {
                public const string MESSAGE_CITY_LIST_SUMMARY = "List of cities in Ecuador";
                public const string MESSAGE_CITY_LIST_DESCRIPTION = "This endpoint returns the cities in Ecuador including general info like description, province, Surface city, etc...";
                public const string MESSAGE_CITY_BYID_SUMMARY = "City information by Id";
                public const string MESSAGE_CITY_BYID_DESCRIPTION = "This endpoint returns the information for the city with the provided id";
                public const string MESSAGE_CITY_BYNAME_SUMMARY = "City information by name";
                public const string MESSAGE_CITY_BYNAME_DESCRIPTION = "This endpoint returns the information for the city with the provided name";
                public const string MESSAGE_CITY_SEARCH_SUMMARY = "Search cities by keyword ";
                public const string MESSAGE_CITY_SEARCH_DESCRIPTION = "This endpoint returns a list of cities any of the following fields(Name, Description, PostalCode) match the provided keyword ";
            }
            public struct PresidentEndpoint
            {
                public const string MESSAGE_PRESIDENT_LIST_SUMMARY = "List of presidents in Ecuador";
                public const string MESSAGE_PRESIDENT_LIST_DESCRIPTION = "This endpoint returns the presidents in Ecuador including general info like political party, city, start period, etc...";
                public const string MESSAGE_PRESIDENT_BYID_SUMMARY = "President information by Id";
                public const string MESSAGE_PRESIDENT_BYID_DESCRIPTION = "This endpoint returns the information for the president with the provided id";
                public const string MESSAGE_PRESIDENT_BYNAME_SUMMARY = "President information by name";
                public const string MESSAGE_PRESIDENT_BYNAME_DESCRIPTION = "This endpoint returns the information for the president with the provided name";
                public const string MESSAGE_PRESIDENT_BYYEAR_SUMMARY = "President of the provided year";
                public const string MESSAGE_PRESIDENT_BYYEAR_DESCRIPTION = "This endpoint returns the president or presidents in the provided year";
                public const string MESSAGE_PRESIDENT_SEARCH_SUMMARY = "Search presidents by keyword ";
                public const string MESSAGE_PRESIDENT_SEARCH_DESCRIPTION = "This endpoint returns a list of presidents any of the following fields(Name, Description, PoliticalParty,LastName) match the provided keyword ";
            }

            public struct TouristAttractionsEndpoint
            {
                public const string MESSAGE_TOURIST_ATTRACTION_LIST_SUMMARY = "List of touristic attractions in Ecuador";
                public const string MESSAGE_TOURIST_ATTRACTION_LIST_DESCRIPTION = " This endpoint returns a list of touristic attractions including information about the city where they are located, the latitude and Longitude and image";
                public const string MESSAGE_TOURIST_ATTRACTION_BYNAME_SUMMARY = "Touristic attraction by name";
                public const string MESSAGE_TOURIST_ATTRACTION_BYNAME_DESCRIPTION = "This endpoint returns an specific touristic attraction by the provided name";
                public const string MESSAGE_TOURIST_ATTRACTION_BYID_SUMMARY = "Touristic attraction by id";
                public const string MESSAGE_TOURIST_ATTRACTION_BYID_DESCRIPTION = "This endpoint returns an specific touristic attraction by the provided id";
                public const string MESSAGE_TOURIST_ATTRACTION_SEARCH_SUMMARY = "Search touristic attractions by keyword ";
                public const string MESSAGE_TOURIST_ATTRACTION_SEARCH_DESCRIPTION = "This endpoint returns a list of touristic attractions any of the following fields(Name, Description,LastName,Latitude, Longitude) match the provided keyword ";
            }
        }
    }
}
