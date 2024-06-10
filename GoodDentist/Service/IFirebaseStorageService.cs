using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;

namespace Services;

public interface IFirebaseStorageService
{
    Task<string> UploadFile(string name, IFormFile file, string imgFolderName);
    Task DeleteFileAndReference(string fileUrl);
    
}