using TomadaStore.SalesAPI.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<MongoDBSettings>(builder.Configuration.GetSection("MongoDB"));

// registrar ConnectionDB (ou registrar IMongoClient singleton e injetar)
builder.Services.AddSingleton<ConnectionDB>();
// Add services to the container.

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
