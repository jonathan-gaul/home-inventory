using Assets.API.Models;
using Assets.Data.Context;
using Assets.Data.Entities;
using Assets.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddScoped<IAssetRepository, InMemoryAssetRepository>();

builder.Services.AddDbContext<AssetsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAssetRepository, EntityFrameworkAssetRepository>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

var assetsApi = app.MapGroup("/assets");

assetsApi.MapGet("/{id}", async (Guid id, IAssetRepository repository)
    => await repository.GetAssetByIdAsync(id) switch
    {
        Asset asset => Results.Ok(asset.ToAssetResponse()),
        _ => Results.NotFound(),
    });

assetsApi.MapPost("/", async (CreateAssetRequest createRequest, IAssetRepository repository)
    =>
{
    var asset = createRequest.ToAsset();
    await repository.AddAssetAsync(asset);
    return Results.Created($"/{asset.Id}", asset.ToAssetResponse());
});

assetsApi.MapPut("/{id}", async (Guid id, UpdateAssetRequest updateRequest, IAssetRepository repository)
    =>
{
    var assetToUpdate = updateRequest.ToAsset(id);

    return await repository.UpdateAssetAsync(assetToUpdate) switch
    {
        Asset updatedAsset => Results.Ok(updatedAsset.ToAssetResponse()),
        _ => Results.NotFound(),
    };
});

assetsApi.MapDelete("/{id}", async (Guid id, IAssetRepository repository) =>
{
    await repository.DeleteAssetAsync(id);
    return Results.NoContent();
});


assetsApi.MapGet("/", async (IAssetRepository repository)
    => await repository.GetAllAssetsAsync() switch
        {
            IEnumerable<Asset> assets => Results.Ok(assets.Select(x => x.ToAssetResponse()).ToList()),
            _ => Results.NoContent(),
        });

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Run();