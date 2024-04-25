using PokedexApi.Domain;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting application");

try
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddHttpClient();

    //register pokemon information service
    builder.Services.AddScoped<IPokemonInformationService, PokemonInformationService>();
    builder.Services.AddScoped<IMapper<PokemonResponse,PokemonInformation>, PokemonMapper>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapGet("/pokemon/{pokemonName}", (string pokemonName, IPokemonInformationService service) =>
        {
            return service.GetPokemonInformationAsync(pokemonName);
        })
        .WithOpenApi();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception");
}
finally
{
    await Log.CloseAndFlushAsync();
}
