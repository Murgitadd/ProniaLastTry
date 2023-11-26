using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProniaPrime.Utilities.Extensions
{
    public static class FileValidator
    {

        public static bool ValidateType(this IFormFile file, string type = "image/")
        {
            if (file.ContentType.Contains(type)) return true;
            return false;
        }
        public static bool ValidateSize(this IFormFile file, int maxMb)
        {
            if (file.Length <= maxMb * 1024 * 1024) return true;
            return false;
        }
        public static async Task<string> CreateFile(this IFormFile file, string root, params string[] folder)
        {
            string originalFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string guidBasedFileName = ExtractGuidFileName(originalFileName);

            string fileFormat = GetFileFormat(originalFileName);

            string finalFileName = guidBasedFileName + fileFormat;

            string path = root;
            for (int i = 0; i < folder.Length; i++)
            {
                path = Path.Combine(path, folder[i]);
            }
            path = Path.Combine(path, finalFileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return finalFileName;

        }
        public static async void DeleteFile(this string fileName, string root, params string[] folders)
        {
            string path = root;
            for (int i = 0; i < folders.Length; i++)
            {
                path = Path.Combine(path, folders[i]);
            }
            path = Path.Combine(path, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static string ExtractGuidFileName(string fullFileName)
        {
            int underscoreIndex = fullFileName.IndexOf('_');

            if (underscoreIndex != -1)
            {
                string guidBasedFileName = fullFileName.Substring(0, underscoreIndex);

                return guidBasedFileName;
            }

            return fullFileName;

        }
        public static string GetFileFormat(string fullFileName)
        {
            int lastDotIndex = fullFileName.LastIndexOf('.');

            if (lastDotIndex != -1)
            {
                string fileFormat = fullFileName.Substring(lastDotIndex);

                return fileFormat;
            }
            return string.Empty;
        }

    }
}
