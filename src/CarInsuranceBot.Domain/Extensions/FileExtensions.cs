using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Domain.Extensions;
public static class FileExtensions
{
    public static bool CheckFileContenttype(this IFormFile file, string contentType)
    {
        return file.ContentType != contentType;
    }

    public static bool CheckFileLength(this IFormFile file, int length = 512)
    {
        return file.Length / 1024 > length;
    }

    public static async Task<string> CreateFileAsync(this IFormFile file, IWebHostEnvironment env, params string[] folders)
    {
        int lastIndex = file.FileName.LastIndexOf('.');

        string name = file.FileName.Substring(lastIndex);

        string fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid()}{name}";

        string fullPath = Path.Combine(env.WebRootPath);

        foreach (string folder in folders)
        {
            fullPath = Path.Combine(fullPath, folder);
        }

        fullPath = Path.Combine(fullPath, fileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        string fullpath = string.Empty;

        foreach (string folder in folders)
        {
            fullpath += folder;
        }

        return fullpath + '/' + fileName;
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
