

using CloudinaryDotNet;
using CourseService.API.Feartures.CourseFearture.Queries.CourseQueries;

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
        public async Task<IActionResult> GetAllCourses([FromQuery] GetAllCourseQuerry query)
        {
           
            var result = await _mediator.Send(query);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
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
        [HttpGet]
        public async Task<IActionResult> GetLessonById(int lessonId)
        {
            try
            {
                var query = new GetLessonByIdQuerry { LessonId = lessonId };
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting lesson by id: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetChapterById(int chapterId)
        {
            try
            {
                var query = new GetChapterByIdQuerry { ChapterId = chapterId };
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting lesson by id: {ex.Message}");
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetPracticeQuestionById(int practiceQuestionId)
        {
            try
            {
                var query = new GetPracticeQuestionByIdQuerry { PracticeQuestionId = practiceQuestionId };
                var result = await _mediator.Send(query);

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error getting practice question by id: {ex.Message}");
            }
        }


    }
}
