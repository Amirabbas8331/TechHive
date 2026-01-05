using Supabase;
using TechHive.Application.Common;

namespace TechHive.Presentation;

public class SupabaseFileStorage : IFileStorage
{
    private readonly Client _client;
    private readonly string _bucket;
    private readonly HttpClient _jwtClient;

    // Cache ساده برای JWT
    private string? _cachedToken;
    private DateTime _expiresAt;
    private string? _cachedUsername;
    private string? _cachedPassword;

    public SupabaseFileStorage(IConfiguration config, HttpClient jwtClient)
    {
        _jwtClient = jwtClient;
        var jwtApiUrl = config["JwtApi:BaseUrl"] ?? "https://localhost:7141/";
        _jwtClient.BaseAddress = new Uri(jwtApiUrl);


        _bucket = config["Supabase:Bucket"] ?? throw new ArgumentNullException("Bucket name missing in config");

        _client = new Supabase.Client(
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

    public async Task<string> GetJwtAsync(string username, string password)
    {
        if (_cachedToken != null
            && DateTime.UtcNow < _expiresAt
            && _cachedUsername == username
            && _cachedPassword == password)
        {
            return _cachedToken;
        }

        var requestBody = new { email = username, password };

        var loginResponse = await _jwtClient.PostAsJsonAsync("users/login", requestBody);

        if (!loginResponse.IsSuccessStatusCode)
        {
            var errorContent = await loginResponse.Content.ReadAsStringAsync();
            // اگر خطا "user not found" بود، پیام واضح بده
            if (errorContent.Contains("The user was not found", StringComparison.OrdinalIgnoreCase))
            {
                throw new UnauthorizedAccessException("کاربر یافت نشد. لطفاً ابتدا ثبت‌نام کنید.");
            }
            throw new HttpRequestException($"خطا در احراز هویت: {loginResponse.StatusCode} - {errorContent}");
        }

        var loginData = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

        if (loginData == null || string.IsNullOrEmpty(loginData.AccessToken))
            throw new Exception("Failed to get JWT from jwtapi: empty response or no access token");

        _cachedToken = loginData.AccessToken;
        _cachedUsername = username;
        _cachedPassword = password;
        _expiresAt = DateTime.UtcNow.AddSeconds(loginData.ExpiresIn - 10); // 10 ثانیه زودتر منقضی شود

        return _cachedToken;
    }

    private string GetUserFolder(string username)
    {
        if (string.IsNullOrEmpty(username))
            throw new UnauthorizedAccessException("Username is required");

        return $"users/{username}";
    }

    public async Task<string> UploadAsync(Stream fileStream, string contentType, string? fileName = null, string username = "default")
    {
        if (fileStream == null || fileStream.Length == 0)
            throw new ArgumentException("File stream is empty", nameof(fileStream));

        fileName ??= $"{Guid.NewGuid()}.bin";

        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms);
        ms.Position = 0;

        var folder = GetUserFolder(username);
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

    public async Task<(Stream Stream, string ContentType)> DownloadAsync(string fileName, string username)
    {
        var folder = GetUserFolder(username);
        var path = $"{folder}/{fileName}";

        var bytes = await _client.Storage
            .From(_bucket)
            .Download(supabasePath: path, transformOptions: null, onProgress: null);

        return (new MemoryStream(bytes), "application/octet-stream");
    }

    public async Task<string> GetSignedUrlAsync(string fileName, string username, int expiresInSeconds = 60)
    {
        var folder = GetUserFolder(username);
        var path = $"{folder}/{fileName}";

        var url = await _client.Storage
            .From(_bucket)
            .CreateSignedUrl(path, expiresInSeconds);

        return url;
    }

    private class LoginResponse
    {
        public string AccessToken { get; set; } = default!;
        public int ExpiresIn { get; set; }
    }
}
