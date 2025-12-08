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



builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDB"));
builder.Services.AddSingleton<ConnectionDB>();

builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();

builder.Services.AddHttpClient();

builder.Services.AddHttpClient("ProductAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:ProductAPI"]!);
});
builder.Services.AddHttpClient("CustomerAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:CustomerAPI"]!);
});
var app = builder.Build();
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
