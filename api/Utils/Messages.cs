namespace api.Utils
{
    public static class Messages
    {
        public struct EndpointMetadata
        {
            public const string MESSAGE_COUNTRY_SUMMARY = "General info about Colombia";
            public const string MESSAGE_COUNTRY_DESCRIPTION = "This endpoint returns the information about Colombia like TimeZone, Languages, Currency, etc...";

            public const string MESSAGE_DEPARMENT_LIST_SUMMARY = "List of deparments in Colombia";
            public const string MESSAGE_DEPARMENT_LIST_DESCRIPTION = "This endpoint returns the deparments in colombia including a general info like description, phone prefix, capital city, etc...";
            public const string MESSAGE_DEPARMENT_BYID_SUMMARY = "Deparment information by Id";
            public const string MESSAGE_DEPARMENT_BYID_DESCRIPTION = "This endpoint returns the information for the deparment with the id";
            public const string MESSAGE_DEPARMENT_BYNAME_SUMMARY = "Deparment information by name";
            public const string MESSAGE_DEPARMENT_BYNAME_DESCRIPTION = "This endpoint returns the information for the deparment with the name provided";
            public const string MESSAGE_DEPARMENT_SEARCH_SUMMARY = "Search departments by keyword ";
            public const string MESSAGE_DEPARMENT_SEARCH_DESCRIPTION = "This endpoint returns a list of departments any of the following fields(Name, Description, PhonePrefix) match the supplied keyword ";

            public const string MESSAGE_CITY_LIST_SUMMARY = "List of cities in Colombia";
            public const string MESSAGE_CITY_LIST_DESCRIPTION = "This endpoint returns the cities in colombia including a general info like description, deparment, Surface city, etc...";
            public const string MESSAGE_CITY_BYID_SUMMARY = "City information by Id";
            public const string MESSAGE_CITY_BYID_DESCRIPTION = "This endpoint returns the information for the city with the id";
            public const string MESSAGE_CITY_BYNAME_SUMMARY = "City information by name";
            public const string MESSAGE_CITY_BYNAME_DESCRIPTION = "This endpoint returns the information for the city with the name provided";
            public const string MESSAGE_CITY_SEARCH_SUMMARY = "Search cities by keyword ";
            public const string MESSAGE_CITY_SEARCH_DESCRIPTION = "This endpoint returns a list of cities any of the following fields(Name, Description, PostalCode) match the supplied keyword ";

            public const string MESSAGE_PRESIDENT_LIST_SUMMARY = "List of presidents in Colombia";
            public const string MESSAGE_PRESIDENT_LIST_DESCRIPTION = "This endpoint returns the presidents in colombia including a general info like political party, city, start period, etc...";
            public const string MESSAGE_PRESIDENT_BYID_SUMMARY = "President information by Id";
            public const string MESSAGE_PRESIDENT_BYID_DESCRIPTION = "This endpoint returns the information for the president with the id";
            public const string MESSAGE_PRESIDENT_BYNAME_SUMMARY = "President information by name";
            public const string MESSAGE_PRESIDENT_BYNAME_DESCRIPTION = "This endpoint returns the information for the president with the name provided";
            public const string MESSAGE_PRESIDENT_BYYEAR_SUMMARY = "President in the year provided";
            public const string MESSAGE_PRESIDENT_BYYEAR_DESCRIPTION = "This endpoint returns the president or presidents in the year provided";
            public const string MESSAGE_PRESIDENT_SEARCH_SUMMARY = "Search presidents by keyword ";
            public const string MESSAGE_PRESIDENT_SEARCH_DESCRIPTION = "This endpoint returns a list of presidents any of the following fields(Name, Description, PoliticalParty,LastName) match the supplied keyword ";

            public struct TouristAttractionsEndpoint
            {
                public const string MESSAGE_TOURIST_ATTRACTION_LIST_SUMMARY = "List of touristic attractions in Colombia";
                public const string MESSAGE_TOURIST_ATTRACTION_LIST_DESCRIPTION = " This endpoint returns a list of touristic attractions including information about the city where they are located, the latitude and Longitude and image";
                public const string MESSAGE_TOURIST_ATTRACTION_BYNAME_SUMMARY = "Touristic attraction by name";
                public const string MESSAGE_TOURIST_ATTRACTION_BYNAME_DESCRIPTION = "This endpoint returns an specific touristic attraction by the specified name";
                public const string MESSAGE_TOURIST_ATTRACTION_BYID_SUMMARY = "Touristic attraction by id";
                public const string MESSAGE_TOURIST_ATTRACTION_BYID_DESCRIPTION = "This endpoint returns an specific touristic attraction by the specified id";
                public const string MESSAGE_TOURIST_ATTRACTION_SEARCH_SUMMARY = "Search touristic attractions by keyword ";
                public const string MESSAGE_TOURIST_ATTRACTION_SEARCH_DESCRIPTION = "This endpoint returns a list of touristic attractions any of the following fields(Name, Description,LastName,Latitude, Longitude) match the supplied keyword ";

            }

        }

    }
}
