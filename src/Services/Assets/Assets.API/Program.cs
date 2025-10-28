using Assets.API.Models;
using Assets.Data.Context;
using Assets.Data.Entities;
using Assets.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AssetsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAssetRepository, EntityFrameworkAssetRepository>();
builder.Services.AddScoped<ILocationRepository, EntityFrameworkLocationRepository>();

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

var locationsApi = app.MapGroup("/locations");

locationsApi.MapGet("/{id}", async (Guid id, ILocationRepository repository) => 
{
    var location = await repository.GetLocationByIdAsync(id);
    if (location is null)
        return Results.NotFound();

    var parent = location.ParentLocationId is not null ? await repository.GetLocationByIdAsync(location.ParentLocationId.Value) : null;
    var children = await repository.GetLocationsByParentIdAsync(id);

    return Results.Ok(location.ToLocationResponse() with
    {
        Parent = parent?.ToLocationReference(),
        Children = [.. children.Select(Mappings.ToLocationReference)]
    });
});

locationsApi.MapPost("/", async (CreateLocationRequest createRequest, ILocationRepository repository)
    =>
{
    var location = createRequest.ToLocation();
    await repository.AddLocationAsync(location);
    return Results.Created($"/{location.Id}", location.ToLocationResponse());
});

locationsApi.MapPut("/{id}", async (Guid id, UpdateLocationRequest updateRequest, ILocationRepository repository)
    =>
{    
    var updatedLocation = await repository.UpdateLocationAsync(updateRequest.ToLocation(id));

    if (updatedLocation is null)
        return Results.NotFound();

    var parentLocation = updateRequest.ParentLocationId is not null
        ? await repository.GetLocationByIdAsync(updateRequest.ParentLocationId.Value)
        : null;

    if (updateRequest.Children is not null)
    {
        // Update children if the field is present.
        var existingChildren = await repository.GetLocationsByParentIdAsync(id);

        var existingIds = existingChildren.Select(x => x.Id);
        var childIdsToAdd = updateRequest.Children.Except(existingIds);
        var newChildren = await repository.GetLocationsByIdAsync([.. childIdsToAdd]);

        var idsThatDontExist = updateRequest.Children.Where(x => !newChildren.Select(c => c.Id).Contains(x));

        if (idsThatDontExist.Any())
        {
            var idsText = string.Join(", ", idsThatDontExist);
            return Results.BadRequest($"The following children do not exist: {idsText}");
        }

        var childIdsToRemove = existingIds.Except(updateRequest.Children);

        // Children removed from this location will be moved to this location's parent if possible. 
        // Otherwise, return an error.
        if (childIdsToRemove.Any())
        {
            if (updateRequest.ParentLocationId is null)
            {
                var idsText = string.Join(", ", childIdsToRemove);
                return Results.BadRequest($"Cannot remove the following children as this location has no parent: {idsText}");
            }
            
            if (parentLocation is null)
            {
                var idsText = string.Join(", ", childIdsToRemove);
                return Results.BadRequest($"Cannot remove the following children as this location's parent ({updateRequest.ParentLocationId}) does not exist: {idsText}");
            }

            var childrenToRemove = await repository.GetLocationsByIdAsync([.. childIdsToRemove]);

            // We won't error on children that don't exist here in case the caller is trying to remove them.
            foreach (var child in childrenToRemove)            
                child.ParentLocationId = parentLocation.Id;

            await repository.UpdateLocationsAsync([.. childrenToRemove]);
        }

        // Set parent of new children to this location.
        if (childIdsToAdd.Any())
        {
            var childrenToAdd = await repository.GetLocationsByIdAsync([.. childIdsToAdd]);
            
            foreach (var child in childrenToAdd)
                child.ParentLocationId = id;

            await repository.UpdateLocationsAsync([.. childrenToAdd]);
        }        
    }

    var children = await repository.GetLocationsByParentIdAsync(id);

    var response = updatedLocation is null ? null : updatedLocation.ToLocationResponse() with
    {
        Parent = parentLocation?.ToLocationReference(),
        Children = [.. children.Select(Mappings.ToLocationReference)]
    };

    return Results.Ok();
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.Run();