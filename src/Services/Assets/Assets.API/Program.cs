using Assets.API.Models;
using Assets.Data.Context;
using Assets.Data.Entities;
using Assets.Data.Repositories;
using Assets.Services;
using Assets.Services.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AssetsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAssetRepository, EntityFrameworkAssetRepository>();
builder.Services.AddScoped<ILocationRepository, EntityFrameworkLocationRepository>();

builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<ILocationService, LocationService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// ==== Assets ====
var assetsApi = app.MapGroup("/assets");

assetsApi.MapGet("/{id}", async (Guid id, IAssetService assetService)
    => await assetService.GetByIdAsync(id) switch
    {
        var x when x is not null => Results.Ok(x.ToAssetResponse()),
        _ => Results.NotFound(),
    });

assetsApi.MapPost("/", async (CreateAssetRequest createRequest, IAssetService assetService)
    => Results.Ok((await assetService.CreateAsync(createRequest.ToAsset())).ToAssetResponse()));

assetsApi.MapPut("/{id}", async (Guid id, UpdateAssetRequest updateRequest, IAssetService assetService) => 
    await assetService.UpdateAsync(updateRequest.ToAsset()) switch
    {
        var x when x is not null => Results.Ok(x.ToAssetResponse()),
        _ => Results.NotFound(),
    });

assetsApi.MapDelete("/{id}", async (Guid id, IAssetService assetService) =>   
    {
        await assetService.DeleteAsync(id);
        return Results.NoContent();
    });


// ==== Locations ====
var locationsApi = app.MapGroup("/locations");

locationsApi.MapGet("/{id}", async (Guid id, ILocationService locationService) => 
    {
        var location = await locationService.GetByIdAsync(id);
        if (location is null)
            return Results.NotFound();

        var parent = location.ParentLocationId is not null ? await locationService.GetByIdAsync(location.ParentLocationId.Value) : null;
        var children = await locationService.GetByParentIdAsync(id);

        return Results.Ok(location.ToLocationResponse() with
        {
            Parent = location.ParentLocationId is not null ? new(location.ParentLocationId.Value, parent?.Name ?? "Unknown") : null,
            Children = [.. children.Select(x => new LocationReference(x.Id, x.Name))]
        });
    });

locationsApi.MapPost("/", async (CreateLocationRequest createRequest, ILocationService locationService) =>
    {
        var location = await locationService.CreateAsync(createRequest.ToLocation());
        return Results.Created($"/{location.Id}", location.ToLocationResponse());
    });

locationsApi.MapPut("/{id}", async (Guid id, UpdateLocationRequest updateRequest, ILocationService locationService) =>
    {    
        var updatedLocation = await locationService.UpdateAsync(updateRequest.ToLocation(id));

        if (updatedLocation is null)
            return Results.NotFound();

        var parentLocation = updateRequest.ParentLocationId is not null
            ? await locationService.GetByIdAsync(updateRequest.ParentLocationId.Value)
            : null;

        if (updateRequest.Children is not null)
        {
            await locationService.ReparentChildrenAsync(updatedLocation.Id, updateRequest.ParentLocationId, updateRequest.Children.ToArray());        
        }

        var children = await locationService.GetByParentIdAsync(id);

        var response = updatedLocation is null ? null : updatedLocation.ToLocationResponse() with
        {
            Parent = parentLocation is not null ? new(parentLocation.Id, parentLocation.Name) : null,
            Children = [.. children.Select(x => new LocationReference(x.Id, x.Name))]
        };

        return Results.Ok();
    });

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Run();