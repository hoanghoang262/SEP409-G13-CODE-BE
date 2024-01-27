


using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using CourseService.API.Common.ModelDTO;
using CourseService.API.Feartures.CourseFearture.Command.CreateCourse;
using CourseService.API.Feartures.CourseFearture.Queries;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Cloudinary _cloudinary;

        public CourseController(IMediator mediator, Cloudinary cloudinary)
        {
            _mediator = mediator;
            _cloudinary = cloudinary;   
        }


        [HttpPost]
        public  async Task<IActionResult> GetMess()
        {
            return Ok(await _mediator.Send(new MessageCommand()));
        }
        [HttpPost]
        public async Task<IActionResult> GetMesaaas(IFormFile video)
        {
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> AddCourse(CreateCourseCommand command)
        {
           
            return Ok(await _mediator.Send(command));
        }
        [HttpPost]
        public IActionResult UploadVideo( IFormFile video)
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
