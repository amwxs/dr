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
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
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
    }
}
