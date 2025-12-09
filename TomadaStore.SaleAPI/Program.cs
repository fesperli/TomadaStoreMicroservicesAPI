using RabbitMQ.Client;
using TomadaStore.ProductAPI.Data;
using TomadaStore.SaleAPI.Repository;
using TomadaStore.SaleAPI.Services.Interfaces.v1;
using TomadaStore.SaleAPI.Services.v1;
using TomadaStore.SaleAPI.Services.v2;
using TomadaStore.SalesAPI.Data;
using TomadaStore.SalesAPI.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();



builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));
builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddScoped<ISaleService, TomadaStore.SaleAPI.Services.v1.SaleService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};
builder.Services.AddHttpClient("ProductAPI", client =>
{
    var url = builder.Configuration["Services:ProductAPI"]
              ?? throw new Exception("URL da ProductAPI não encontrada no appsettings.");
    client.BaseAddress = new Uri(url);
})
.ConfigurePrimaryHttpMessageHandler(() => handler); 

builder.Services.AddHttpClient("CustomerAPI", client =>
{
    var url = builder.Configuration["Services:CustomerAPI"]
              ?? throw new Exception("URL da CustomerAPI não encontrada no appsettings.");
    client.BaseAddress = new Uri(url);
})
.ConfigurePrimaryHttpMessageHandler(() => handler);
builder.Services.AddHttpClient();

builder.Services.AddHttpClient("ProductAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:ProductAPI"]!);
});
builder.Services.AddHttpClient("CustomerAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:CustomerAPI"]!);
});

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
