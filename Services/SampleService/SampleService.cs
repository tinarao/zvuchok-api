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

        public async Task<IReturnsDTO> CreateSampleAsync(CreateSampleDTO dto, int authorId)
        {
            var duplicate = await _context.Samples.FirstOrDefaultAsync(s => s.Name == dto.Name);
            if (duplicate != null)
            {
                return ReturnsFabric.BadRequest("Sample with this name already exists");
            }

            var user = await _context.Users.FindAsync(authorId);
            if (user is null)
            {
                return ReturnsFabric.NotFound("User not found");
            }

            var sampleName = GenerateSampleName(dto.Name, user.Username);
            var slug = new SlugHelper().GenerateSlug(sampleName);

            var filename = await Storage.SaveFile(dto.File, StorageDirectories.Audio);

            var sample = new Sample
            {
                Name = sampleName,
                Slug = slug,
                Description = dto.Description,
                DurationMs = dto.DurationMs,
                Genres = dto.Genres,
                SampleFilePath = filename,
                AuthorId = authorId
            };


            try
            {
                await _context.Samples.AddAsync(sample);
                await _context.SaveChangesAsync();

                return ReturnsFabric.Created(sample);
            }
            catch (Exception)
            {
                // Log!
                return ReturnsFabric.InternalServerError();
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
    }
}