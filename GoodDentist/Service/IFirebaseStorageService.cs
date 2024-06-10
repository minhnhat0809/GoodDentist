using Microsoft.AspNetCore.Http;

namespace Services;

public interface IFirebaseStorageService
{
    Task<string> UploadFile(string name, IFormFile file, string imgFolderName);
    Task DeleteFileAndReference(string fileUrl);
}