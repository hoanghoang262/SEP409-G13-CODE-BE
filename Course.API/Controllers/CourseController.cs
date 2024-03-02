

using CloudinaryDotNet;

using CourseService.API.Feartures.Queries.CourseQueries;
using MediatR;

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


        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            return Ok(await _mediator.Send(new GetAllCourseQuerry()));
        }
        [HttpGet]
        public async Task<IActionResult> GetCourseByUser(int Id)
        {
            return Ok(await _mediator.Send(new GetCourseByUserIdQuerry{UserId=Id}));
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseByCourseId(int Id)
        {
            return Ok(await _mediator.Send(new GetCourseByCourseIdQuerry { CourseId = Id }));
        }



    }
}
