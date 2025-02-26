using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Utils
{
    public enum StorageDirectories
    {
        Audio,
        Image
    }

    public class Storage
    {
        /// <summary>
        /// Saves the specified file to a designated directory within the storage folder.
        /// </summary>
        /// <param name="file">The file to be saved.</param>
        /// <param name="directory">The storage directory type where the file should be saved (e.g., Audio, Image).</param>
        /// <returns>The unique filename of the saved file.</returns>

        public async static Task<string> SaveFile(IFormFile file, StorageDirectories directory)
        {
            var storageDir = Path.Combine(Directory.GetCurrentDirectory(), "storage");
            if (!Directory.Exists(storageDir))
            {
                Directory.CreateDirectory(storageDir);
            }

            var filename = $"{Guid.NewGuid()}_{file.FileName}";
            var filepath = Path.Combine(storageDir, filename);

            using var stream = new FileStream(filepath, FileMode.Create);
            await file.CopyToAsync(stream);

            return filename;
        }
    }
}