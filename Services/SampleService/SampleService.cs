using api.Db;
using api.Dto;
using api.Models;
using api.Utils;
using Slugify;
using Microsoft.EntityFrameworkCore;

namespace api.Services.SampleService
{
    public class SampleService(ZvuchokContext context) : ISampleService
    {
        private readonly ZvuchokContext _context = context;

        public async Task<List<Sample>> GetAllSamplesAsync()
        {
            var samples = await _context.Samples.ToListAsync();
            return samples;
        }

        public async Task<Sample?> GetSampleByIdAsync(int id)
        {
            var sample = await _context.Samples.FindAsync(id);
            return sample;
        }

        public async Task<ReturnsDTOWithSample> CreateSampleAsync(CreateSampleDTO dto, int authorId)
        {
            var duplicate = await _context.Samples.FirstOrDefaultAsync(s => s.Name == dto.Name);
            if (duplicate != null)
            {
                return new ReturnsDTOWithSample
                {
                    Message = "Sample with this name already exists",
                    StatusCode = 400,
                };
            }

            var user = await _context.Users.FindAsync(authorId);
            if (user is null)
            {
                return new ReturnsDTOWithSample
                {
                    Message = "User not found",
                    StatusCode = 404,
                };
            }

            var sampleName = GenerateSampleName(dto.Name, user.Username);
            var slug = new SlugHelper().GenerateSlug(sampleName);

            var filename = await Storage.SaveFile(dto.File, StorageDirectories.Audio);
            var metadataToken = Guid.NewGuid();

            var sample = new Sample
            {
                Name = sampleName,
                Slug = slug,
                Description = dto.Description,
                Genres = dto.Genres,
                Tags = dto.Tags,
                SampleFilePath = filename,
                AuthorId = authorId,
                UpdateMetadataToken = metadataToken
            };

            try
            {
                await _context.Samples.AddAsync(sample);
                await _context.SaveChangesAsync();

                Utilities.SendSampleToAnalysisService(sample);

                return new ReturnsDTOWithSample
                {
                    Message = "Sample created successfully",
                    StatusCode = 201,
                    Sample = sample
                };
            }
            catch (Exception)
            {
                // Log!
                return new ReturnsDTOWithSample
                {
                    Message = "Sample creation failed",
                    StatusCode = 500
                };
            }
        }

        public async Task<Sample> UpdateSampleAsync(Sample sample)
        {
            throw new NotImplementedException();
        }

        public async Task<Sample> DeleteSampleAsync(int id)
        {
            throw new NotImplementedException();
        }



        private static string GenerateSampleName(string initialName, string username)
        {
            return $"{username.ToUpper()}_{initialName.ToUpper()}";
        }

        public async Task<Sample?> UpdateSampleMetadata(UpdateSampleMetadataDTO dto, Guid token)
        {
            var sample = await _context.Samples.FirstOrDefaultAsync(u => u.UpdateMetadataToken == token);
            if (sample is null)
            {
                return null;
            }

            sample.Peaks = dto.Peaks;
            await _context.SaveChangesAsync();

            return sample;
        }
    }
}