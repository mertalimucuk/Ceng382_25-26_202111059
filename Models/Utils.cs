using System.Text.Json;
using Week5Lab.Models;
using System.Collections.Generic;
using System.Linq;

namespace Week5Lab.Models
{
    public class Utils
    {
        private static Utils? _instance;
        public static Utils Instance => _instance ??= new Utils();

        // Tüm kolonları içeren varsayılan JSON export
        public string ExportToJson<T>(List<T> data)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(data, options);
        }

        // Seçili kolonlara göre JSON export (ClassInformation ile)
        public string ExportToJson(List<ClassInformation> data, List<string> selectedColumns)
        {
            var shapedData = data.Select(item =>
            {
                var dict = new Dictionary<string, object>();

                if (selectedColumns.Contains("ClassName"))
                    dict["ClassName"] = item.ClassName;

                if (selectedColumns.Contains("StudentCount"))
                    dict["StudentCount"] = item.StudentCount;

                if (selectedColumns.Contains("Description"))
                    dict["Description"] = item.Description;

                return dict;
            }).ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };
            return JsonSerializer.Serialize(shapedData, options);
        }
    }
}
