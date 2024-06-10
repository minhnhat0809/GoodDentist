using System.Data.SqlTypes;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

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
            _bucketName, $"{imgFolderName}/{fileName}", file.ContentType, stream);

        // Image URI to get client image
        var photoUri = img.MediaLink;

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
        var uri = new Uri(fileUrl);
        var filePath = uri.LocalPath.Substring("/download/storage/v1/b/good-dentist-67aba.appspot.com/o/".Length);
        // Delete the file from Firebase Storage
        await _storageClient.DeleteObjectAsync(_bucketName, filePath);
    }
   

}