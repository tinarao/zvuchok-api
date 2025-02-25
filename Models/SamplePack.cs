using System.ComponentModel.DataAnnotations.Schema;
using static api.Utils.Utilities;

namespace api.Models
{
    public class SamplePack
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }
        public required string Description { get; set; }
        public AvailabilityStatus AvailabilityStatus { get; set; } = AvailabilityStatus.OnAnalysis;
        public List<Sample> Samples { get; set; } = [];
        public string? ArtworkPath { get; set; }

        public required User Author { get; set; }
        public required int AuthorId { get; set; }

        public int Likes { get; set; } = 0;


        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }
}
