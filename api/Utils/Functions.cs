using System.Collections.Generic;

namespace api.Utils
{
    public class Functions
    {

        public static List<T> FilterObjectListPropertiesByKeyword<T>(List<T> records, string keyword)
        {
            var filteredResults = new List<T>();


            // TO DO: Filter the fields by a list.
            // TO DO: Speed up the process.
            var recordProperties = typeof(T).GetProperties()
                               .Where(x => x.PropertyType == typeof(string))
                               .Where(x => !x.Name.Contains("Images"))
                               .ToList();

            // The second condition is to avoid searching for small articles in large fields as that will return practically any texts.
            var filtered = (from record in records
                            from property in recordProperties
                            let value = (property.GetValue(record, null) as string)!.Trim().ToUpper().Normalize()
                            where (value == keyword || (keyword.Length > 3 && value.Length > 10 && value.Contains(keyword)))
                            select record);

            if (filtered is not null)
            {
                filteredResults = filtered.ToList();
            }
            return filteredResults;
        }

    }
}