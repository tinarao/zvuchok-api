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

            if (kind == FileKind.Audio)
            {
                var signedUrlGuid = Guid.NewGuid();
                var filepath = sample.SampleFilePath;
                var signedUrl = new Models.SignedUrl
                {
                    FilePath = sample.SampleFilePath,
                    Url = signedUrlGuid
                };

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

        public Task<string?> GetFilePathByUrlAsync(string url)
        {
            throw new NotImplementedException();
        }
    }
}