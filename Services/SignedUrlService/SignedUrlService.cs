using api.Db;
using Microsoft.EntityFrameworkCore;
using static api.Utils.Utilities;

namespace api.Services.SignedUrl
{
    public class SignedUrlService(ZvuchokContext context) : ISignedUrlService
    {
        private readonly ZvuchokContext _context = context;

        public async Task<string?> GenerateSignedUrlAsync(FileKind kind, string ctxUsername, string slug)
        {
            var sample = await _context.Samples.FirstOrDefaultAsync(s => s.Slug == slug);
            if (sample is null)
            {
                return null;
            }

            var signedUrlGuid = Guid.NewGuid();

            if (kind == FileKind.Audio)
            {
                var filepath = sample.SampleFilePath;
                var signedUrl = new Models.SignedUrl
                {
                    FilePath = sample.SampleFilePath,
                    Url = signedUrlGuid
                };

                _context.SignedUrls.Add(signedUrl);
                await _context.SaveChangesAsync();

                return signedUrl.Url.ToString();
            }

            if (kind == FileKind.Image)
            {
                return null;
                // var signedUrlGuid = Guid.NewGuid();
                // var filepath = sample.SampleFilePath;
                // var signedUrl = new Models.SignedUrl
                // {
                //     FilePath = sample.SampleFilePath,
                //     Url = signedUrlGuid
                // };
            }

            return null;
        }

        public async Task<string?> GetFilePathByUrlAsync(Guid url)
        {
            var _record = await _context.SignedUrls.FirstOrDefaultAsync(su => su.Url == url);
            if (_record is null)
            {
                return null;
            }

            return _record.FilePath;
        }
    }
}