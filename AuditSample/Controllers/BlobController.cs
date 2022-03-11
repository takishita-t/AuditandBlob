using AuditSample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuditSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlobController : ControllerBase
    {
        private readonly IBlobService _blobService;

        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost("download")]
        public async Task<ActionResult<string>> Download(Download request)
        {
            await _blobService.DownloadAsync(request.DownloadBlobName, request.DownloadFileName, request.SavePath);
            return Ok("Download");
        }

        [HttpPost("upload")]
        public async Task<ActionResult<string>> Upload(Upload request)
        {
            await _blobService.UploadAsync(request.UploadContainerName, request.UploadFileName, request.UploadFilePath);
            return Ok("Upload");
        }

        [HttpPost("copy")]
        public async Task<ActionResult<string>> Copy(Copy request)
        {
            await _blobService.CopyAsync(request.CopyContainerName, request.CopyFromFileName, request.CopyToFileName);
            return Ok("copy");
        }
    }
}
