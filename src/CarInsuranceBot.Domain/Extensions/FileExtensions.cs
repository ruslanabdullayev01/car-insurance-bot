using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Domain.Extensions;
public static class FileExtensions
{
    public static bool CheckFileContenttype(this IFormFile file, string contentType)
    {
        return file.ContentType != contentType;
    }

    public static bool CheckFileLength(this IFormFile file, int length = 5120)
        => file.Length / 1024 > length;

    public static async Task<string> CreateFileAsync(this IFormFile file, IWebHostEnvironment env, string folder)
    {
        if (string.IsNullOrWhiteSpace(env.WebRootPath))
            throw new Exception("WebRootPath is null or empty. Make sure 'wwwroot' exists!");

        if (string.IsNullOrWhiteSpace(folder))
            throw new Exception("Folder argument is null or empty.");

        int lastIndex = file.FileName.LastIndexOf('.');
        string extension = file.FileName.Substring(lastIndex);
        string fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid()}{extension}";

        string directory = Path.Combine(env.WebRootPath, folder);
        Directory.CreateDirectory(directory);

        string fullPath = Path.Combine(directory, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine(folder, fileName).Replace("\\", "/");
    }

    public static void DeleteFile(string fileName, IWebHostEnvironment env, params string[] folders)
    {
        string fullPath = Path.Combine(env.WebRootPath);

        foreach (string folder in folders)
        {
            fullPath = Path.Combine(fullPath, folder);
        }

        fullPath = Path.Combine(fullPath, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }
}
