using RabbitMQ.Client;
using TomadaStore.SaleAPI.Repository;
using TomadaStore.SalesAPI.Data;
using TomadaStore.SalesAPI.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient();
builder.Services.AddSingleton<ConnectionDB>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

builder.Services.AddSingleton<ConnectionFactory>(sp =>
{
    return new ConnectionFactory()
    {
        HostName = "localhost",
        UserName = "guest",
        Password = "guest"
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
