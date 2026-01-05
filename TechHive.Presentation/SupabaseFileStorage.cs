using Supabase;
using System.Security.Claims;
using TechHive.Application.Common;

namespace TechHive.Presentation;

public class SupabaseFileStorage : IFileStorage
{
    private readonly Client _client;
    private readonly string _bucket;

    public SupabaseFileStorage(IConfiguration config)
    {
        _bucket = config["Supabase:Bucket"] ?? throw new ArgumentNullException("Bucket name missing in config");

        _client = new Client(
            config["Supabase:Url"],
            config["Supabase:ServiceKey"],
            new SupabaseOptions
            {
                AutoConnectRealtime = false,
                AutoRefreshToken = false
            }
        );

        // Initialize Supabase client
        _client.InitializeAsync().GetAwaiter().GetResult();
    }

    private string GetUserFolder(ClaimsPrincipal user)
    {
        var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
            throw new UnauthorizedAccessException("User not authenticated");

        return $"users/{userId}";
    }

    public async Task<string> UploadAsync(Stream fileStream, string contentType, string? fileName = null, ClaimsPrincipal user = null!)
    {
        if (fileStream == null || fileStream.Length == 0)
            throw new ArgumentException("File stream is empty", nameof(fileStream));

        fileName ??= $"{Guid.NewGuid()}.bin";

        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        ms.Position = 0;

        var folder = GetUserFolder(user);
        var path = $"{folder}/{fileName}";

        await _client.Storage
            .From(_bucket)
            .Upload(
                ms.ToArray(),
                path,
                new Supabase.Storage.FileOptions
                {
                    ContentType = contentType,
                    Upsert = true
                });

        return path;
    }

    public async Task<(Stream Stream, string ContentType)> DownloadAsync(string fileName, ClaimsPrincipal user)
    {
        var folder = GetUserFolder(user);
        var path = $"{folder}/{fileName}";

        var bytes = await _client.Storage
            .From(_bucket)
            .Download(supabasePath: path, onProgress: null, transformOptions: null);

        return (new MemoryStream(bytes), "application/octet-stream");
    }

    public async Task<string> GetSignedUrlAsync(string fileName, ClaimsPrincipal user, int expiresInSeconds = 60)
    {
        var folder = GetUserFolder(user);
        var path = $"{folder}/{fileName}";

        var url = await _client.Storage
            .From(_bucket)
            .CreateSignedUrl(path, expiresInSeconds);

        return url;
    }
}


