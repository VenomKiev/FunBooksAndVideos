using FunBooksAndVideos.API.Endpoints;
using FunBooksAndVideos.API.Extensions;
using FunBooksAndVideos.Application.Features;
using FunBooksAndVideos.Application.Features.PurchaseOrders.Commands;
using FunBooksAndVideos.Application.Interfaces;
using FunBooksAndVideos.Domain.Services;
using FunBooksAndVideos.Persistence.Extensions;
using FunBooksAndVideos.Persistence.Seed;
using Serilog;

Log.Logger = SerilogExtensions.CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.UseSerilog();

builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName!.Replace('+', '.'));
});

builder.Services.AddCentralExceptionHandling();

builder.Services.AddMediatR(configuration =>
    configuration.RegisterServicesFromAssembly(typeof(CreatePurchaseOrderCommandHandler).Assembly));

builder.Services.AddApplicationServices();
builder.Services.AddDomainServices();

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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCorrelationId();
app.UseCentralExceptionHandling();
app.MapPurchaseOrderEndpoints();

app.Run();
