using static api.Utils.Utilities;

namespace api.Services.SignedUrl
{
    public interface ISignedUrlService
    {
        public Task<string?> GenerateSignedUrlAsync(FileKind kind, string ctxUsername, string slug);
        public Task<string?> GetFilePathByUrlAsync(string url);
    }
}