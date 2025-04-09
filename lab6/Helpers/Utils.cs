using System.Text.Json;

namespace lab6.Helpers
{
    public sealed class Utils
    {
        private static readonly Lazy<Utils> lazy = new(() => new Utils());
        public static Utils Instance => lazy.Value;

        private Utils() { }

        public string ExportToJson<T>(IEnumerable<T> data, IEnumerable<string> selectedColumns = null)
        {
            if (selectedColumns == null || !selectedColumns.Any())
            {
                return JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            }
            else
            {
                var list = data.Select(item =>
                {
                    var dict = new Dictionary<string, object>();
                    var type = typeof(T);
                    foreach (var col in selectedColumns)
                    {
                        var prop = type.GetProperty(col);
                        if (prop != null)
                        {
                            dict[col] = prop.GetValue(item);
                        }
                    }
                    return dict;
                });
                return JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            }
        }
    }
}
