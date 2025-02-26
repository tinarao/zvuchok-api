using api.Dto;
using api.Services.SampleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SampleController(ISampleService sampleService) : ControllerBase
    {
        private readonly ISampleService _sampleService = sampleService;

        [HttpGet]
        public async Task<ActionResult> GetAllSamples()
        {
            var samples = await _sampleService.GetAllSamplesAsync();
            return Ok(samples);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetSampleById(int id)
        {
            var sample = await _sampleService.GetSampleByIdAsync(id);
            if (sample is null)
            {
                return NotFound("Sample not found");
            }

            return Ok(sample);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Create(CreateSampleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity("Invalid request body");
            }

            var result = await _sampleService.CreateSampleAsync(dto, 1);

            if (result.Sample != null && result.StatusCode == 201)
            {
                return CreatedAtAction(nameof(GetSampleById), new { id = result.Sample.Id }, result.Sample);
            }

            return result.StatusCode switch
            {
                400 => BadRequest(result.Message),
                404 => NotFound(result.Message),
                _ => StatusCode(500),
            };
        }

        [HttpPatch("metadata/{token}")]
        [AllowAnonymous]
        public async Task<ActionResult> UpdateMetadata(UpdateSampleMetadataDTO dto, Guid token)
        {
            if (!ModelState.IsValid)
            {
                return UnprocessableEntity();
            }

            var updatedSample = await _sampleService.UpdateSampleMetadata(dto, token);
            if (updatedSample is null)
            {
                return NotFound("Sample does not exist");
            }

            return Ok(dto);
        }
    }
}