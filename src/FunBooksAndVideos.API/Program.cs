using Serilog;
using FunBooksAndVideos.API.Extensions;

Log.Logger = SerilogExtensions.CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

builder.UseSerilog();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
