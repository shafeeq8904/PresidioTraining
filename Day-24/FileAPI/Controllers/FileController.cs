using Microsoft.AspNetCore.Mvc;
using FileAPI.Interfaces;
using FileAPI.Models;

namespace FileAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile([FromForm] FileUploadDto dto)
    {
        if (dto.File == null || dto.File.Length == 0)
            return BadRequest("File is missing.");

        await _fileService.SaveFileAsync(dto.File);
        return Ok("File uploaded successfully.");
    }

    [HttpGet("{fileName}")]
    public async Task<IActionResult> GetFile(string fileName)
    {
        try
        {
            var fileBytes = await _fileService.GetFileBytesAsync(fileName);
            return File(fileBytes, "application/octet-stream", fileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound("File not found.");
        }
    }
}
