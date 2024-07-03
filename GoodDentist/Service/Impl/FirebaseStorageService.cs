using System.Data.SqlTypes;
using Google;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;

namespace Services.Impl;

public class FirebaseStorageService : IFirebaseStorageService
{
    private readonly StorageClient _storageClient;
    private readonly string _bucketName;

    public FirebaseStorageService(IConfiguration configuration)
    {
        var credential = GoogleCredential.FromFile(configuration["Firebase:ServiceAccountKeyPath"]);
        _storageClient = StorageClient.Create(credential);
        _bucketName = configuration["Firebase:StorageBucket"];
    }

    public async Task<string> UploadFile(string name, IFormFile file, string imgFolderName)
    {
        var newGuid = Guid.NewGuid();
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        
        // Sanitize the file name
        var sanitizedFileName = SanitizeFileName(name);
        
        // Generate a unique file name
        var fileName = $"{sanitizedFileName}-{newGuid}";

        // Upload image to Firebase
        var img = await _storageClient.UploadObjectAsync(
            _bucketName, $"image/{imgFolderName}/{fileName}", file.ContentType, stream);

        // Image URI to get client image
        var photoUri = ConvertToFirebaseStorageUrl(img.MediaLink);
        return photoUri;
    }
    
    private string SanitizeFileName(string fileName)
    {
        // Replace spaces with hyphens and remove special characters
        return fileName.Replace(" ", "-")
            .Replace(":", "-")
            .Replace("/", "-")
            .Replace("\\", "-");
    }
    public async Task DeleteFileAndReference(string fileUrl)
    {
        // Extract the file path from the URL
        var uri = new Uri(fileUrl);
        var filePath = GetFilePathFromUrl(uri);

        // Delete the file from Firebase Storage
        try
        {
            await _storageClient.DeleteObjectAsync(_bucketName, filePath);
        }
        catch (GoogleApiException ex)
        {
            throw new Exception(ex.Message);
        }
    }
    private string ConvertToFirebaseStorageUrl(string mediaLink)
    {
        return mediaLink.Replace("https://storage.googleapis.com/download/storage/v1/b/", "https://firebasestorage.googleapis.com/v0/b/");
    }
    private string GetFilePathFromUrl(Uri uri)
    {
        // Adjust this method to match the format of your file URLs
        var bucketSegment = uri.LocalPath.IndexOf("/o/", StringComparison.Ordinal);
        if (bucketSegment < 0)
        {
            throw new ArgumentException("Invalid Firebase Storage URL format.");
        }

        // Extract the file path from the URL
        return Uri.UnescapeDataString(uri.LocalPath.Substring(bucketSegment + 3));
    }

    
}