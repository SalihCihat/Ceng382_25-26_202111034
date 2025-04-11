using System.Text.Json;
using System.Reflection;

namespace lab5.Helpers
{
    public class JsonExporter
    {
        private static JsonExporter? _instance;
        private static readonly object _lock = new();

        private JsonExporter() { }

        public static JsonExporter Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new JsonExporter();
                }
            }
        }

        public string ExportToJson<T>(IEnumerable<T> data, List<string>? selectedColumns = null)
        {
            if (selectedColumns == null || selectedColumns.Count == 0)
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }

            var filteredData = data.Select(item =>
            {
                var dict = new Dictionary<string, object?>();

                foreach (var prop in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (selectedColumns.Contains(prop.Name))
                    {
                        dict[prop.Name] = prop.GetValue(item);
                    }
                }

                return dict;
            });

            return JsonSerializer.Serialize(filteredData, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
