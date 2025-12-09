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

builder.Services.AddScoped<ISaleService, TomadaStore.SaleAPI.Services.v1.SaleService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

// --- 4. Configuração dos HttpClients (API de Produtos e Clientes) ---
var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

// Configuração ProductAPI
builder.Services.AddHttpClient("ProductAPI", client =>
{
    var url = builder.Configuration["Services:ProductAPI"];

    if (string.IsNullOrEmpty(url))
        throw new Exception("ERRO CRÍTICO: A configuração 'Services:ProductAPI' não foi encontrada no appsettings.json!");

    if (!url.EndsWith("/")) url += "/";

    client.BaseAddress = new Uri(url);
})
.ConfigurePrimaryHttpMessageHandler(() => handler);

// [ADICIONADO] Configuração CustomerAPI (Faltava isso!)
builder.Services.AddHttpClient("CustomerAPI", client =>
{
    var url = builder.Configuration["Services:CustomerAPI"];

    if (string.IsNullOrEmpty(url))
        throw new Exception("ERRO CRÍTICO: A configuração 'Services:CustomerAPI' não foi encontrada no appsettings.json!");

    if (!url.EndsWith("/")) url += "/";

    client.BaseAddress = new Uri(url);
})
.ConfigurePrimaryHttpMessageHandler(() => handler);


// --- 5. Configuração do RabbitMQ ---
builder.Services.AddSingleton<ConnectionFactory>(sp =>
{
    return new ConnectionFactory()
    {
        HostName = "localhost",
        UserName = "guest",
        Password = "guest",
    };
});
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
