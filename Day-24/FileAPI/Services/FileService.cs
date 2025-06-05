using FileAPI.Interfaces;

namespace FileAPI.Services;

public class FileService : IFileService
{
    private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "StoredFiles");

    public FileService()
    {
        if (!Directory.Exists(_storagePath))
        {
            Directory.CreateDirectory(_storagePath);
        }
    }

    public async Task SaveFileAsync(IFormFile file)
    {
        var filePath = Path.Combine(_storagePath, file.FileName);
        using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);
    }

    public async Task<byte[]> GetFileBytesAsync(string fileName)
    {
        var filePath = Path.Combine(_storagePath, fileName);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found.");
        }
        return await File.ReadAllBytesAsync(filePath);
    }
}
