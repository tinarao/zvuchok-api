using api.Db;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController(ZvuchokContext context, ILogger<FileController> logger) : Controller
    {
        private readonly ZvuchokContext _context = context;
        private readonly ILogger<FileController> _logger = logger;

        [HttpGet("audio/{id}")]
        public async Task<ActionResult> ServeAudioFile(int id)
        {
            _logger.LogInformation("Serving audio file");
            var sample = await _context.Samples.FindAsync(id);
            if (sample is null)
            {
                return NotFound();
            }

            var storageFolder = Path.Combine(Directory.GetCurrentDirectory(), "storage");
            string filePath = Path.Combine(storageFolder, sample.SampleFilePath);

            FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fs, "audio/mpeg");  // TODO: Multiple MIME types
        }
    }
}