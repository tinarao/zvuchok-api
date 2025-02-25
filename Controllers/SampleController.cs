using api.Services.SampleService;
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

    }
}