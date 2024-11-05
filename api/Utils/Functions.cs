using System.Collections.Generic;
using System.Globalization;
using System.Text;

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

        public static List<T> SortList<T>(List<T> list, PaginationModel paginationModel)
        {
            if (paginationModel is null || string.IsNullOrEmpty(paginationModel.SortBy)) return list;

            var property = typeof(T).GetProperty(paginationModel.SortBy);
            if (property is null) return list;

            list.Sort((x, y) => Comparer<object>.Default.Compare(property.GetValue(x), property.GetValue(y)));
            return list;
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