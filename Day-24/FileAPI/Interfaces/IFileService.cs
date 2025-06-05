using System.Threading.Tasks;

namespace FileAPI.Interfaces;

public interface IFileService
{
    Task<byte[]> GetFileBytesAsync(string fileName);
    Task SaveFileAsync(IFormFile file);
}
