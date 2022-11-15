
using System.Text.Json.Serialization;
using Infrastructure;
using Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(o => o.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistence(builder.Configuration);

builder.Services
    .AddControllersWithViews()
    .AddJsonOptions(o => o.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString)
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNameCaseInsensitive = true)
    .AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);


builder.Services.AddTransient<DbInitializer>();


var app = builder.Build();

await SeedDataAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


//Seed Data
async Task SeedDataAsync(IHost app)
{
    var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetRequiredService<DbInitializer>();
         
        await service.InitializeAsync();
    }
}