using System.ComponentModel.DataAnnotations;

namespace api.Dto
{
    public class CreateSampleDTO
    {
        [Required, StringLength(256, MinimumLength = 3)]
        public required string Name { get; set; }

        public string? Description { get; set; }
        public string[] Tags { get; set; } = [];
        public string[] Genres { get; set; } = [];

        [Required]
        public required IFormFile File { get; set; }
    }

    public class UpdateSampleMetadataDTO
    {
        public required float[] Peaks { get; set; }
    }
}