using Azure.Storage.Blobs;
using ImageProcessing.API.Models;
using ImageProcessing.Data.Context;
using ImageProcessing.Data.Entities;
using ImageProcessing.Data.Repositories;
using ImageProcessing.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<ImagesDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSingleton(x =>
    new BlobServiceClient(builder.Configuration["BlobStorage:ConnectionString"]));

builder.Services.AddScoped<IBlobStorageService>(sp =>
{
    var blobServiceClient = sp.GetRequiredService<BlobServiceClient>();
    var containerName = builder.Configuration["BlobStorage:ContainerName"] ?? "asset-images";
    return new AzureBlobStorageService(blobServiceClient, containerName);
});

builder.Services.AddScoped<IImageRepository, EntityFrameworkImageRepository>();

var cvEndpoint = builder.Configuration["ComputerVision:Endpoint"];
var cvApiKey = builder.Configuration["ComputerVision:ApiKey"];
if (string.IsNullOrEmpty(cvEndpoint) || string.IsNullOrEmpty(cvApiKey))
    throw new InvalidOperationException("ComputerVision settings are not configured");

builder.Services.AddScoped<IComputerVisionService>(sp =>
    new AzureComputerVisionService(cvEndpoint, cvApiKey));

var app = builder.Build();

var imagesApi = app.MapGroup("/images");

imagesApi.MapGet("/{id}/content", async (Guid id, IBlobStorageService storage, IImageRepository repository)
    =>
{
    var image = await repository.GetImageByIdAsync(id).ConfigureAwait(false);
    if (image == null)
        return Results.NotFound();

    var stream = await storage.DownloadAsync(image.Id!.Value);
    if (stream is null)
        return Results.NotFound();

    return Results.File(stream, image.ContentType);
});

app.MapPost("/{id}/analyse", async (
    Guid id,
    IImageRepository repository,
    IBlobStorageService blobStorage,
    IComputerVisionService computerVision,
    ILogger<Program> logger) =>
{
    // Get image metadata
    var image = await repository.GetImageByIdAsync(id).ConfigureAwait(false);
    if (image == null)
        return Results.NotFound();

    // Check if already analyzed
    if (image.AnalysisStatus == ImageAnalysisStatus.Analysed)
        return Results.Ok(new { message = "Image already analysed", image });

    if (image.AnalysisStatus == ImageAnalysisStatus.Analysing)
        return Results.Ok(new { message = "Image analysis already in progress" });

    try
    {
        // Update status to analyzing
        image.AnalysisStatus = ImageAnalysisStatus.Analysing;
        await repository.UpdateImageAsync(image).ConfigureAwait(false);

        logger.LogInformation("Starting analysis for image {ImageId}", id);

        // Download image from blob storage
        using var imageStream = await blobStorage.DownloadAsync(image.Id!.Value);

        if (imageStream is null)
            return Results.NotFound();

        // Analyze with Computer Vision
        var analysisResult = await computerVision.AnalyzeImageAsync(imageStream);

        //// Update metadata with results
        //image.Tags = System.Text.Json.JsonSerializer.Serialize(analysisResult.Tags);
        //image.DetectedObjects = System.Text.Json.JsonSerializer.Serialize(analysisResult.Objects);
        //image.OcrText = analysisResult.OcrText;
        //image.Caption = analysisResult.Caption;
        //image.AnalysisStatus = ImageAnalysisStatus.Completed;
        //image.AnalyzedAt = DateTime.UtcNow;

        //await repository.UpdateAsync(image);

        logger.LogInformation("Successfully analyzed image {ImageId}", id);

        return Results.Ok(new
        {
            message = "Analysis completed successfully",
            tags = analysisResult.Tags,
            objects = analysisResult.Objects,
            ocrText = analysisResult.OcrText,
            caption = analysisResult.Caption
        });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error analyzing image {ImageId}", id);

        image.AnalysisStatus = ImageAnalysisStatus.Failed;
        await repository.UpdateImageAsync(image);

        return Results.Problem("Analysis failed");
    }
});

imagesApi.MapGet("/{id}", async (Guid id, IImageRepository repository)
    => await repository.GetImageByIdAsync(id) switch
    {
        var image when image is not null => Results.Ok(image.ToResponse()),
        _ => Results.NotFound(),
    });

imagesApi.MapPost("/", async (
    IFormFile file,
    Guid assetId,
    IBlobStorageService storage,
    IImageRepository repository) =>
{
    // Validation
    if (file == null || file.Length == 0)
        return Results.BadRequest("No file uploaded");

    var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/heic" };
    if (!allowedTypes.Contains(file.ContentType.ToLower()))
        return Results.BadRequest("Invalid file type. Only JPEG, PNG, and HEIC are allowed.");

    const long maxFileSize = 10 * 1024 * 1024;
    if (file.Length > maxFileSize)
        return Results.BadRequest("File too large. Maximum size is 10MB.");

    var imageId = Guid.NewGuid();
    var extension = Path.GetExtension(file.FileName);
    var blobName = $"{imageId}{extension}";

    // Upload to blob storage
    using var stream = file.OpenReadStream();
    await storage.UploadAsync(imageId, file.ContentType, stream);

    // Save metadata to DB
    var image = new Image
    {
        Id = imageId,
        AssetId = assetId,
        ContentType = file.ContentType,
        CreatedAt = DateTime.UtcNow
    };

    await repository.AddImageAsync(image);

    return Results.Created($"/api/images/{imageId}", image.ToResponse());
})
.DisableAntiforgery(); // Required for file uploads

imagesApi.MapGet("/", async (
    [FromQuery] Guid[]? assetIds,
    IImageRepository repository) =>
{
    if (assetIds is null || assetIds.Length == 0)
        return Results.BadRequest("At least one assetId is required");

    var images = await repository.GetImagesByAssetIdAsync(assetIds);
    return Results.Ok(images.Select(x => x.ToResponse()));
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();


app.Run();
