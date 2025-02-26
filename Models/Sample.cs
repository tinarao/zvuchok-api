using System.ComponentModel.DataAnnotations.Schema;
using static api.Utils.Utilities;

namespace api.Models
{
    public class Sample
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public string? Description { get; set; }
        public float DurationMs { get; set; }
        public float[] Peaks { get; set; } = [];
        public required Guid UpdateMetadataToken { get; set; } // just to verify the server
        public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.OnAnalysis;
        public required string SampleFilePath { get; set; }
        public string[] Tags { get; set; } = [];
        public string[] Genres { get; set; } = [];
        public SamplePack? SamplePack { get; set; }
        public int? SamplePackId { get; set; }
        public User Author { get; set; } = null!;
        public required int AuthorId { get; set; }
        public int Downloads { get; set; } = 0;
        public int Likes { get; set; } = 0;
        public bool IsFeatured { get; set; } = false;
        public bool IsOnSale { get; set; } = false;
        public bool IsFree { get; set; } = true;

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }

    }
}
