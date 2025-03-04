using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Services.SignedUrl;
using api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static api.Utils.Utilities;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignedUrlController(ISignedUrlService signedUrlService) : ControllerBase
    {
        private readonly ISignedUrlService _signedUrlService = signedUrlService;

        [Authorize]
        [HttpGet("generate/{kind}/{slug}")]
        public async Task<ActionResult> GenerateSignedUrl(FileKind kind, string slug)
        {
            var ctxUsername = Utilities.GetCtxUsername(HttpContext);
            if (ctxUsername is null)
            {
                return Unauthorized();
            }

            var signedUrl = await _signedUrlService.GenerateSignedUrlAsync(kind, ctxUsername, slug);
            if (signedUrl is null)
            {
                return NotFound();
            }

            return Ok(signedUrl);
        }

        [HttpGet("{signedUrl}")]
        public async Task<ActionResult> GetFile(Guid signedUrl)
        {
            var _filepath = await _signedUrlService.GetFilePathByUrlAsync(signedUrl);
            if (_filepath is null)
            {
                return NotFound();
            }

            var storageFolder = Path.Combine(Directory.GetCurrentDirectory(), "storage");
            string filePath = Path.Combine(storageFolder, _filepath);

            FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(fs, "audio/mpeg");  // TODO: Multiple MIME types
        }
    }
}