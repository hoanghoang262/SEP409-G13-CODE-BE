using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Image.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public ImageController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [HttpPost("upload")]
        public IActionResult UploadImage( IFormFile file)
        {
            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, file.OpenReadStream()),
                // Add additional upload parameters as needed
            };

            var uploadResult = _cloudinary.Upload(uploadParams);

            // Process uploadResult as needed
            var imageUrl = uploadResult.Url;

            return Ok(new { ImageUrl = imageUrl });
        }
    }
}
