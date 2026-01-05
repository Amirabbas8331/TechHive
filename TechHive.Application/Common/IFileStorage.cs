using System.Security.Claims;

namespace TechHive.Application.Common;

public interface IFileStorage
{
    Task<string> UploadAsync(Stream fileStream, string contentType, string? fileName, ClaimsPrincipal user);
    Task<(Stream Stream, string ContentType)> DownloadAsync(string fileName, ClaimsPrincipal user);
    Task<string> GetSignedUrlAsync(string fileName, ClaimsPrincipal user, int expiresInSeconds = 60);

}


