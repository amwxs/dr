using System.Text.Json;

namespace Dr.Extensions.Logging.Abstractions.Test;
public class JsonExtensionsTest
{
    [Fact]
    public void ToJson_Should_ReturnsJsonString()
    {
        var obj = new { Name = "John", Age = 30 };
        string json = obj.ToJson();
        Assert.NotEmpty(json);
    }

    [Fact]
    public void ToJson_UsesJsonOptions_Should_ReturnsJsonString()
    {
        var obj = new { Name = "John", Age = 30 };
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string json = obj.ToJson(jsonOptions);

        Assert.True(json.Contains("name") && json.Contains("age"));
    }

    [Fact]
    public void FromJson_Should_ReturnsDeserializedObject()
    {
        string json = "{\"Name\":\"John\",\"Age\":30}";

        var result = json.FromJson<MyObject>();

        Assert.NotNull(result);
        Assert.Equal("John", result.Name);
        Assert.Equal(30, result.Age);
    }

    [Fact]
    public void FromJson_EmptyJisonString_Should_ReturnsNull()
    {

        string emptyJson = string.Empty;

        var result = emptyJson.FromJson<MyObject>();

        Assert.Null(result);
    }


    [Fact]
    public void FromJson_UsesJsonOptions_Should_EqualObjProperties()
    {
        string json = "{\"name\":\"John\",\"age\":30}";
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = json.FromJson<MyObject>(jsonOptions);

        Assert.NotNull(result);
        Assert.Equal("John", result.Name);
        Assert.Equal(30, result.Age);
    }
}
public class MyObject
{
    public string? Name { get; set; }
    public int Age { get; set; }
}