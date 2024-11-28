 using System.Globalization;
using System.Text; 
using System.Linq.Expressions;
using System.Reflection;

namespace api.Utils
{
    public class Functions
    {

        public static List<T> FilterObjectListPropertiesByKeyword<T>(List<T> records, string keyword)
        {
            var filteredResults = new List<T>();

            if(string.IsNullOrEmpty(keyword)) return filteredResults;

            string keywordFormatted = RemoveAccentMark(keyword);


            // TO DO: Filter the fields by a list.
            // TO DO: Speed up the process.
            var recordProperties = typeof(T).GetProperties()
                               .Where(x => x.PropertyType == typeof(string))
                               .Where(x => x.Name != null && !x.Name.Contains("Images"))
                               .ToList();

            // The second condition is to avoid searching for small articles in large fields as that will return practically any texts.
            var filtered = (from record in records
                            from property in recordProperties
                            let value = RemoveAccentMark((property.GetValue(record, null) as string)?.Trim().ToUpper().Normalize())
                            where value!= null && (value == keyword || (keyword.Length > 3 && value.Contains(keywordFormatted)))
                            select record);

            if (filtered is not null)
            {
                filteredResults = filtered.Distinct().ToList();
            }
            return filteredResults;
        }


        public static (IQueryable<T>, bool IsValidSort)  ApplySorting<T>(IQueryable<T> query, string sortBy, string sortOrder)
        { 
            if(string.IsNullOrEmpty(sortBy) || string.IsNullOrEmpty(sortOrder)){
                 return (query, true);
            }

            var property = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
           
            if (property == null || (!Enum.GetNames(typeof(SortDirection)).Contains(StringExtensions.FirstCharToUpper(sortOrder))))
            {
                return (query, false);
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyAccess = Expression.Property(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            var method = sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase) ? "OrderByDescending" : "OrderBy";

            var resultExpression = Expression.Call(
                typeof(Queryable),
                method,
                new Type[] { typeof(T), property.PropertyType },
                query.Expression,
                Expression.Quote(orderByExpression)
            );

            return (query.Provider.CreateQuery<T>(resultExpression), true);
        }

        static string RemoveAccentMark(string text)
        {
            if (text == null) return "";
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

            for (int i = 0; i < normalizedString.Length; i++)
            {
                char c = normalizedString[i];
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder
                .ToString()
                .Normalize(NormalizationForm.FormC);
        }
           
    }
}