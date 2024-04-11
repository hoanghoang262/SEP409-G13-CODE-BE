using CloudinaryDotNet;
using Contract.Service.Message;
using CourseService.API.Feartures.CourseFearture.Command.SyncCourse;
using CourseService.API.Feartures.CourseFearture.Queries.CourseQueries;
using CourseService.API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CourseService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Cloudinary _cloudinary;
        private readonly CourseContext context;

        public CourseController(IMediator mediator, Cloudinary cloudinary, CourseContext _context)
        {
            _mediator = mediator;
            _cloudinary = cloudinary;
            context = _context;
        }
        [HttpDelete]

        public IActionResult Delete1Course(int CourseId)
        {
            var courseToRemove =  context.Courses
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.Lessons)
                            .ThenInclude(l => l.TheoryQuestions)
                                .ThenInclude(ans => ans.AnswerOptions)
                     .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.LastExams)
                            .ThenInclude(l => l.QuestionExams)
                                .ThenInclude(ans => ans.AnswerExams)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.PracticeQuestions)
                            .ThenInclude(cq => cq.TestCases)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.Lessons)
                            .ThenInclude(l => l.TheoryQuestions)
                                .ThenInclude(ans => ans.AnswerOptions)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.PracticeQuestions)
                            .ThenInclude(cq => cq.TestCases)
                    .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.PracticeQuestions)
                         .ThenInclude(ch=>ch.TestCases)
                      .Include(c => c.Chapters)
                        .ThenInclude(ch => ch.LastExams)
                         .ThenInclude(ch => ch.QuestionExams)
                          .ThenInclude(ch=>ch.AnswerExams)
                    .FirstOrDefault(course => course.Id == CourseId);
            // Remove all related TheoryQuestions
            foreach (var chapter in courseToRemove.Chapters)
            {
                foreach (var lesson in chapter.Lessons)
                {
                    foreach (var theoryQuestion in lesson.TheoryQuestions)
                    {
                        // Remove related AnswerOptions of each TheoryQuestion
                        context.RemoveRange(theoryQuestion.AnswerOptions);
                    }
                    // Remove TheoryQuestions of each Lesson
                    context.RemoveRange(lesson.TheoryQuestions);
                }
            }

            // Remove all related PracticeQuestions and their TestCases
            foreach (var chapter in courseToRemove.Chapters)
            {
                foreach (var practiceQuestion in chapter.PracticeQuestions)
                {
                    // Remove related TestCases of each PracticeQuestion
                    context.RemoveRange(practiceQuestion.TestCases);
                }
                // Remove PracticeQuestions of each Chapter
                context.RemoveRange(chapter.PracticeQuestions);
            }

            // Remove all related LastExams and their QuestionExams and AnswerExams
            foreach (var chapter in courseToRemove.Chapters)
            {
                foreach (var lastExam in chapter.LastExams)
                {
                    foreach (var questionExam in lastExam.QuestionExams)
                    {
                        // Remove related AnswerExams of each QuestionExam
                        context.RemoveRange(questionExam.AnswerExams);
                    }
                    // Remove QuestionExams of each LastExam
                    context.RemoveRange(lastExam.QuestionExams);
                }
                // Remove LastExams of each Chapter
                context.RemoveRange(chapter.LastExams);
            }

            // Remove all Chapters
            context.RemoveRange(courseToRemove.Chapters);

            // Remove the course itself
            context.Remove(courseToRemove);

            // Save changes
            context.SaveChanges();

            return Ok("Check xem da xoa trong db chua");
        }
        [HttpGet]
        public async Task<IActionResult> GetCourseQuantity()
        {
            var result = await _mediator.Send(new GetCourseQuantityQuerry { });
            return Ok(result);
        }

        //[Authorize(Roles = "AdminSystem")]
        [HttpGet]
        public async Task<IActionResult> GetAllCourses([FromQuery] GetAllCourseQuerry query)
        {
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseByUser(int Id)
        {
            return Ok(await _mediator.Send(new GetCourseByUserIdQuerry { UserId = Id }));
        }

        [HttpGet]
        public async Task<IActionResult> GetCourseByCourseId(int Id, int userId)
        {
            return Ok(await _mediator.Send(new GetCourseByCourseIdQuerry { CourseId = Id, UserId = userId }));
        }

        [HttpGet]
        public async Task<IActionResult> GetChapterById(int chapterId, int userId)
        {
            try
            {
                var query = new GetChapterByIdQuerry { ChapterId = chapterId, UserId = userId };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(Message.MSG30);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonById(int lessonId)
        {
            try
            {
                var query = new GetLessonByIdQuerry { LessonId = lessonId };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(Message.MSG30);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPracticeQuestionById(int practiceQuestionId, int userId)
        {
            try
            {
                var query = new GetPracticeQuestionByIdQuerry { PracticeQuestionId = practiceQuestionId, UserId = userId };
                var result = await _mediator.Send(query);

                return Ok(result);
            }
            catch (Exception)
            {
                return BadRequest(Message.MSG30);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CompletedLesson(int userId, int lessonId)
        {
            var completed = new CompleteLesson
            {
                LessonId = lessonId,
                UserId = userId
            };

            context.CompleteLessons.Add(completed);
            await context.SaveChangesAsync();
            return Ok(completed);
        }
        [HttpGet]
        public async Task<IActionResult> GetExamQuestionDetail(int lastExamId)
        {
            var query = new GetExamQuestionDetailQuerry
            {
                LastExamId = lastExamId
            };
            var result = await _mediator.Send(query);

            return result;
        }
        [HttpGet]
        public async Task<IActionResult> GetQuantityOfCourses()
        {
            var query = new GetQuantityOfCourseQuerry();
            var result = await _mediator.Send(query);
            return result;
        }
    }
}
