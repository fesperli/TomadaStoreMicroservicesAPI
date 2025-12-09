using RabbitMQ.Client;
using TomadaStore.SaleAPI.Repository;
using TomadaStore.SalesAPI.Data;
using TomadaStore.SalesAPI.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddHttpClient();

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDBSettings"));
    
builder.Services.AddSingleton<ConnectionDB>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

var handler = new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
};

builder.Services.AddHttpClient("ProductAPI", client =>
{
    var url = builder.Configuration["Services:ProductAPI"];
    if (string.IsNullOrEmpty(url))
        throw new Exception("ERRO: A URL 'Services:ProductAPI' não está no appsettings.json do Consumer!");
    if (!url.EndsWith("/")) url += "/";
    client.BaseAddress = new Uri(url);
}).ConfigurePrimaryHttpMessageHandler(() => handler);

builder.Services.AddHttpClient("CustomerAPI", client =>
{
    var url = builder.Configuration["Services:CustomerAPI"];

    if (string.IsNullOrEmpty(url))
        throw new Exception("ERRO: A URL 'Services:CustomerAPI' não está no appsettings.json do Consumer!");

    if (!url.EndsWith("/")) url += "/";

    client.BaseAddress = new Uri(url);
})
.ConfigurePrimaryHttpMessageHandler(() => handler);


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
