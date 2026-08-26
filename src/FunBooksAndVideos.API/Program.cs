using Serilog;
using FunBooksAndVideos.API.Extensions;
using FunBooksAndVideos.API.Endpoints;
using FunBooksAndVideos.Application.Features;
using FunBooksAndVideos.Application.Services;
using FunBooksAndVideos.Domain.Services;
using FunBooksAndVideos.Persistence.Configuration;
using FunBooksAndVideos.Persistence.Seed;

Log.Logger = SerilogExtensions.CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.UseSerilog();

builder.Services.AddOpenApi();
builder.Services.AddCentralExceptionHandling();
builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(typeof(FeatureAssemblyMarker).Assembly));
builder.Services.AddScoped<PurchaseOrderValidationService>();
builder.Services.AddScoped<MembershipActivationService>();
builder.Services.AddScoped<ShippingSlipService>();
builder.Services.AddPersistence(
    builder.Configuration.GetValue<string>("Persistence:DatabaseName") ?? "FunBooksAndVideos");
builder.Services.AddScoped<ISeedDataProvider, SeedDataInitializer>();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var seedDataProvider = scope.ServiceProvider.GetRequiredService<ISeedDataProvider>();
    await seedDataProvider.SeedAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCorrelationId();
app.UseCentralExceptionHandling();
app.MapPurchaseOrderEndpoints();

app.Run();
