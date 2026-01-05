using Microsoft.AspNetCore.Mvc;
using TechHive.Application.Common;

namespace TechHive.Presentation.Controllers;

public class FilesController : ControllerBase
{
    private readonly IFileStorage _storage;

    public FilesController(IFileStorage storage)
    {
        _storage = storage;
    }

    [HttpPost("upload")]
    [RequestSizeLimit(50 * 1024 * 1024)] // 50 MB
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("File is empty");

        await using var stream = file.OpenReadStream();
        var storedPath = await _storage.UploadAsync(stream, file.ContentType, file.FileName, User);

        return Ok(new { Path = storedPath });
    }

    [HttpGet("download/{fileName}")]
    public async Task<IActionResult> Download(string fileName)
    {
        var (stream, contentType) = await _storage.DownloadAsync(fileName, User);
        return File(stream, contentType, fileName);
    }

    [HttpGet("signed-download/{fileName}")]
    public async Task<IActionResult> SignedDownload(string fileName)
    {
        var url = await _storage.GetSignedUrlAsync(fileName, User, 120);
        return Ok(new { Url = url });
    }
}
