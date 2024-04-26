using FluentValidation;
using PokedexApi.Domain;
using PokedexApi.Domain.Interfaces;
using PokedexApi.Domain.Models;
using PokedexApi.Infrastructure.Client;
using PokedexApi.Infrastructure.DTO;
using Serilog;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json");

builder.Services.AddSerilog();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Logger = logger;

Log.Information("Starting application");

try
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpClient();

    builder.Services.ConfigureHttpJsonOptions(options =>
    {
        options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

    //register pokemon information service
    builder.Services.AddScoped<IPokemonSpeciesClient, PokemonSpeciesClient>();
    builder.Services.AddScoped<IValidator<string>, PokemonNameValidator>();
    builder.Services.AddScoped<IMapper<PokemonResponse,PokemonInformation>, PokemonMapper>();
    builder.Services.AddScoped<IPokemonInformationService, PokemonInformationService>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    var pokemomRouteBuilder = app.MapGroup("/pokemon");

    pokemomRouteBuilder.MapGet("/{pokemonName}", async (string pokemonName, IPokemonInformationService service) =>
    {
        var result = await service.GetPokemonInformationAsync(pokemonName);
        return result.ToMinimalApiResult();
    })
    .WithOpenApi();

    pokemomRouteBuilder.MapGet("/translated/{pokemonName}", async (string pokemonName) =>
    {
        var client = new YodaTranslatorClient(new HttpClient());
        var translation = await client.TranslatetextAsync(pokemonName);
        return translation;
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
