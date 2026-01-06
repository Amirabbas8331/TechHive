using Microsoft.Extensions.Caching.Distributed;
using Supabase;
using System.Text;
using System.Text.Json;
using TechHive.Application.Common;

namespace TechHive.Presentation;

public class SupabaseFileStorage : IFileStorage
{
    private readonly Client _client;
    private readonly string _bucket;
    private readonly HttpClient _jwtClient;
    private readonly IDistributedCache _cache;
    private const string CacheKeyPrefix = "JwtToken_";

    public SupabaseFileStorage(IConfiguration config, HttpClient jwtClient, IDistributedCache cache)
    {
        _cache = cache;
        _jwtClient = jwtClient;
        var jwtApiUrl = config["JwtApi:BaseUrl"] ?? "https://localhost:7141/";
        _jwtClient.BaseAddress = new Uri(jwtApiUrl);


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

        _client.InitializeAsync().GetAwaiter().GetResult();
    }

    public async Task<string> GetTokenAsync(string email, string password)
    {
        string cacheKey = CacheKeyPrefix + email;

        byte[]? cachedData = await _cache.GetAsync(cacheKey);
        if (cachedData != null)
        {
            string json = Encoding.UTF8.GetString(cachedData);
            var cached = JsonSerializer.Deserialize<CachedTokenData>(json);

            if (cached != null && DateTime.UtcNow < cached.ExpiresAt)
            {
                return cached.Token;
            }
        }

        var requestBody = new { email, password };
        var loginResponse = await _jwtClient.PostAsJsonAsync("users/login", requestBody);

        if (!loginResponse.IsSuccessStatusCode)
        {
            var errorContent = await loginResponse.Content.ReadAsStringAsync();
            if (errorContent.Contains("The user was not found", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("User Not Found");
            }
            throw new HttpRequestException($"Exception in Authorization: {loginResponse.StatusCode} - {errorContent}");
        }

        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
        if (loginData == null || string.IsNullOrEmpty(loginData.AccessToken))
            throw new Exception("Failed to get JWT from jwtapi: empty response or no access token");

        var newCached = new CachedTokenData
        {
            Token = loginData.AccessToken,
            ExpiresAt = DateTime.UtcNow.AddSeconds(loginData.ExpiresIn - 10),
            Role = loginData.role
        };

        string jsonToCache = JsonSerializer.Serialize(newCached);
        byte[] bytesToCache = Encoding.UTF8.GetBytes(jsonToCache);

        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = newCached.ExpiresAt
        };

        await _cache.SetAsync(cacheKey, bytesToCache, options);

        return loginData.AccessToken;
    }


    private string GetUserFolder(string email)
    {
        if (string.IsNullOrEmpty(email))
            throw new UnauthorizedAccessException("Username is required");

        return $"users/{email}";
    }

    public async Task<string> UploadAsync(Stream fileStream, string contentType, string? fileName = null, string email = "default")
    {
        if (fileStream == null || fileStream.Length == 0)
            throw new ArgumentException("File stream is empty", nameof(fileStream));

        fileName ??= $"{Guid.NewGuid()}.bin";

        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        ms.Position = 0;

        var folder = GetUserFolder(email);
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

    public async Task<(Stream Stream, string ContentType)> DownloadAsync(string fileName, string email)
    {
        var folder = GetUserFolder(email);
        var path = $"{folder}/{fileName}";

        var bytes = await _client.Storage
            .From(_bucket)
            .Download(supabasePath: path, transformOptions: null, onProgress: null);

        return (new MemoryStream(bytes), "application/octet-stream");
    }

    public async Task<string> GetSignedUrlAsync(string fileName, string email, int expiresInSeconds = 60)
    {
        var folder = GetUserFolder(email);
        var path = $"{folder}/{fileName}";

        var url = await _client.Storage
            .From(_bucket)
            .CreateSignedUrl(path, expiresInSeconds);

        return url;
    }

    public class LoginResponse
    {
        public string AccessToken { get; set; } = default!;
        public int ExpiresIn { get; set; }
        public string role { get; set; } = default!;
    }
    private class CachedTokenData
    {
        public string Token { get; set; } = default!;
        public DateTime ExpiresAt { get; set; }
        public string Role { get; set; } = default!;

    }
}
