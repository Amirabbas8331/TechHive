

namespace TechHive.Application.Common;

public interface IFileStorage
{
    Task<string> UploadAsync(Stream fileStream, string contentType, string? fileName = null, string username = "default");
    Task<(Stream Stream, string ContentType)> DownloadAsync(string fileName, string username);

    Task<string> GetSignedUrlAsync(string fileName, string username, int expiresInSeconds = 60);
    Task<string> GetTokenAsync(string username, string password);

}


