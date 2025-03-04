namespace api.Models
{
    public class SignedUrl
    {
        public int Id { get; set; }
        public required string FilePath { get; set; }
        public required Guid Url { get; set; }
    }
}