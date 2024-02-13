using GrpcServices;
using CourseService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseService.API.Common.ModelDTO;
using Microsoft.EntityFrameworkCore;
using CourseService.API.Models;

namespace CourseService.API.Feartures.CourseFearture.Queries
{
    public class GetCourseByCourseIdQuerry : IRequest<IActionResult>
    {
        public int CourseId { get; set; }

        public class GetCourseByUserHandler : IRequestHandler<GetCourseByCourseIdQuerry, IActionResult>
        {
            private readonly CourseContext _context;
            private readonly UserIdCourseGrpcService service;
            private readonly IMapper mapper;

            public GetCourseByUserHandler(CourseContext context, UserIdCourseGrpcService userIdCourseGrpcService, IMapper _mapper)
            {
                _context = context;
                service = userIdCourseGrpcService;
                mapper = _mapper;

            }
            public async Task<IActionResult> Handle(GetCourseByCourseIdQuerry request, CancellationToken cancellationToken)
            {


                var courses = _context.Courses.Include(enroll => enroll.Enrollments)
                              .Include(course => course.Chapters)
                              .ThenInclude(chapter => chapter.Lessons)
                              .ThenInclude(lesson => lesson.Questions)
                              .Where(course => course.Id == request.CourseId).ToList();

                var result = new
                {

                    Courses = courses.Select(course => new
                    {
                        course.Id,
                        course.Name,
                        course.Description,
                        course.Picture,
                        course.Tag,

                        Chapters = course.Chapters.Select(chapter => new
                        {
                            chapter.Id,
                            chapter.Name,
                            chapter.CourseId,
                            chapter.Part,
                            chapter.IsNew,
                            Lessons = chapter.Lessons.Select(lesson => new
                            {
                                lesson.Id,
                                lesson.Title,
                                lesson.VideoUrl,
                                lesson.ChapterId,
                                lesson.Description,
                                lesson.Duration,
                                Questions = lesson.Questions.Select(question => new
                                {
                                    question.Id,
                                    question.VideoId,
                                    question.ContentQuestion,
                                    question.AnswerA,
                                    question.AnswerB,
                                    question.AnswerC,
                                    question.AnswerD,
                                    question.CorrectAnswer,
                                    question.Time
                                }).ToList()
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };

                return new OkObjectResult(result);
            }
        }
    }
}
