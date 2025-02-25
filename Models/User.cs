using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{

    public enum Plan
    {
        Free,
        Advanced,
        Pro
    }

    public class User
    {
        public int Id { get; set; }
        public required string Username { get; set; }
        public required string NormalizedUsername { get; set; }
        public required string Slug { get; set; }
        public int Credits { get; set; } = 300;
        public string? Bio { get; set; }
        public string? AvatarPath { get; set; }

        public ICollection<SamplePack> CreatedSamplePacks { get; set; } = [];
        public ICollection<Sample> CreatedSamples { get; set; } = [];

        public ICollection<SamplePack> FavoriteSamplePacks { get; set; } = [];
        public ICollection<Sample> FavoriteSamples { get; set; } = [];
        public required UserCredentials Credentials { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedAt { get; set; }
    }

    [Owned]
    public class UserCredentials
    {
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public required string PasswordHash { get; set; }
    }
}
