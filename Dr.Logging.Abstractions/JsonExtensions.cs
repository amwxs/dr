using System.Text.Json;

namespace Dr.Logging.Abstractions
{
    public static class JsonExtensions
    {
        private static readonly JsonSerializerOptions _indentedJsonOptions = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string ToJson(this object obj, JsonSerializerOptions? jsonOptions = null)
        {
            jsonOptions ??= _jsonOptions;
            return JsonSerializer.Serialize(obj, jsonOptions);
        }

        public static string ToIndentedJson(this object obj, JsonSerializerOptions? jsonOptions = null)
        {
            jsonOptions ??= _indentedJsonOptions;
            return JsonSerializer.Serialize(obj, jsonOptions);
        }
    }
}
