using Dr.Extensions.Logging.Abstractions;
using Dr.Management;
using Dr.Management.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(c =>
{
    c.ClearProviders()
    .AddDrLogger(c =>
    {
        c.AppId = "dr.logging.management";
        c.IsConsolePrint = true;
        c.LogLevel.Add("Default", LogLevel.Information);
    });
    //.AddRabbitMQSink();
});

builder.Services.AddMediatR(c =>
{
    c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});
builder.Services.Configure<ElasticOptioins>(builder.Configuration.GetSection("ElasticSearch"));
builder.Services.AddSingleton<IElsticSearchFactory, ElsticSearchFactory>();

builder.Services.AddControllers(c =>
{
    c.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var corsName = "dr";
builder.Services.AddCors(o =>
{
    o.AddPolicy(corsName, builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();
app.UseCors(corsName);
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();

app.MapControllers();

app.Run();
