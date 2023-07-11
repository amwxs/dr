using Dr.Extensions.Logging.Abstractions;
using Dr.Extensions.Logging.RabbitMQ;
using Dr.Logging.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureLogging(c =>
{

    c.ClearProviders();
    c.AddDrLogger(o =>
    {
        o.IsConsolePrint = true;
        o.LogLevel.Add("Default", LogLevel.Information);
    }).AddRabbitMQSink();
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTrace();
app.UseAuthorization();

app.MapControllers();

app.Run();
