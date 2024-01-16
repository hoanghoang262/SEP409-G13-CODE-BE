using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Image.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public VideoController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [HttpPost("upload")]
        public IActionResult UploadVideo(IFormFile video)
        {
            var uploadParams = new VideoUploadParams
            {
                File = new FileDescription(video.FileName, video.OpenReadStream()),
                // Các tham số tải lên khác nếu cần
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            // Process uploadResult as needed
            var videoUrl = uploadResult.Url;

            return Ok(new { VideoUrl = videoUrl });
        }
    }
}
