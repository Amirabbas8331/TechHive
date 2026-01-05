using Microsoft.AspNetCore.Mvc;
using TechHive.Application.Common;

namespace TechHive.Presentation.Controllers;

public class FilesController : ApiController
{
    private readonly IFileStorage _storage;

    public FilesController(IFileStorage storage)
    {
        _storage = storage;
    }
    [HttpPost("upload")]
    [RequestSizeLimit(50 * 1024 * 1024)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload(
        IFormFile file,           // بدون هیچ attribute
        string username,          // این‌ها هم به صورت form field می‌آیند
        string password)
    {
        var jwt = await _storage.GetJwtAsync(username, password);

        await using var stream = file.OpenReadStream();
        var path = await _storage.UploadAsync(stream, file.ContentType, file.FileName, username);

        return Ok(new { Path = path });
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download(
        [FromQuery] string fileName,
        [FromQuery] string username,
        [FromQuery] string password)
    {
        var jwt = await _storage.GetJwtAsync(username, password);
        var (stream, contentType) = await _storage.DownloadAsync(fileName, username);

        return File(stream, contentType, fileName);
    }

    [HttpGet("signed-download")]
    public async Task<IActionResult> SignedDownload(
        [FromQuery] string fileName,
        [FromQuery] string username,
        [FromQuery] string password,
        [FromQuery] int expiresIn = 60)
    {
        var jwt = await _storage.GetJwtAsync(username, password);
        var url = await _storage.GetSignedUrlAsync(fileName, username, expiresIn);

        return Ok(new { Url = url });
    }
}
