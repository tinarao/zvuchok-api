using api.Dto;
using api.Models;

namespace api.Services.SampleService
{
    public interface ISampleService
    {
        public Task<List<Sample>> GetAllSamplesAsync();
        public Task<Sample?> GetSampleByIdAsync(int id);
        public Task<IReturnsDTO> CreateSampleAsync(CreateSampleDTO dto, int authorId);
        public Task<Sample> UpdateSampleAsync(Sample sample);
        public Task<Sample?> UpdateSampleMetadata(UpdateSampleMetadataDTO dto);
        public Task<Sample> DeleteSampleAsync(int id);
    }
}