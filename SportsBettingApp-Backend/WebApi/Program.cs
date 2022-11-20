
using Application.Common.Interfaces;
using Application.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Infrastructure;
using Infrastructure.Data;
using Nop.Data;
using System.Text.Json.Serialization;

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

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        
        //repositories
        builder.RegisterGeneric(typeof(EfRepository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
    });

builder.Services.AddScoped<DataService>();
builder.Services.AddScoped<BettingService>();
builder.Services.AddScoped<WalletService>();

builder.Services.AddTransient<DbInitializer>();
var app = builder.Build();

SeedData(app);

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
static void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    using var scope = scopedFactory.CreateScope();
    var service = scope.ServiceProvider.GetRequiredService<DbInitializer>();

    service.Initialize();
}