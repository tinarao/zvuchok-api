using System.ComponentModel.DataAnnotations;

namespace api.Dto
{
    public class CreateSampleDTO
    {
        [Required, MinLength(3), MaxLength(50)]
        public required string Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [Required, Range(1, 3600000)] // 1 second to 1 hour
        public required float DurationMs { get; set; }

        [Required, MinLength(1)]
        public required int[] Peaks { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string[] Tags { get; set; } = [];

        [StringLength(50, MinimumLength = 3)]
        public string[] Genres { get; set; } = [];

        [Required]
        public required IFormFile File { get; set; }
    }
}