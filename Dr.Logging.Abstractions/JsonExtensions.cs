using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace Dr.Logging.Abstractions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions _indentedJsonOptions = new()
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string ToJson(this object obj, JsonSerializerOptions? jsonOptions = null)
        {
            return JsonSerializer.Serialize(obj, jsonOptions);
        }

        public static string ToIndentedJson(this object obj, JsonSerializerOptions? jsonOptions = null)
        {
            jsonOptions ??= _indentedJsonOptions;
            return JsonSerializer.Serialize(obj, jsonOptions);
        }

        public static T? FromJson<T>(this string str, JsonSerializerOptions? jsonOptions = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(str, jsonOptions);
        }
    }
}
