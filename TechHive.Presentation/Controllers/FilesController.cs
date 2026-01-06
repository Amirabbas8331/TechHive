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
        IFormFile file,
        string email,
        string password)
    {
        var jwt = await _storage.GetTokenAsync(email, password);

        await using var stream = file.OpenReadStream();
        var path = await _storage.UploadAsync(stream, file.ContentType, file.FileName, email);

        return Ok(new { Path = path });
    }

    [HttpGet("download")]
    public async Task<IActionResult> Download(
        [FromQuery] string fileName,
        [FromQuery] string email,
        [FromQuery] string password)
    {
        var jwt = await _storage.GetTokenAsync(email, password);
        var (stream, contentType) = await _storage.DownloadAsync(fileName, email);

        return File(stream, contentType, fileName);
    }

    [HttpGet("signed-download")]
    public async Task<IActionResult> SignedDownload(
        [FromQuery] string fileName,
        [FromQuery] string email,
        [FromQuery] string password,
        [FromQuery] int expiresIn = 60)
    {
        var jwt = await _storage.GetTokenAsync(email, password);
        var url = await _storage.GetSignedUrlAsync(fileName, email, expiresIn);

        return Ok(new { Url = url });
    }
}
