using GrpcServices;
using CourseService;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using CourseService.API.Common.ModelDTO;
using Microsoft.EntityFrameworkCore;
using CourseService.API.Models;

namespace API.Feartures.CourseFearture.Queries
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
                     _context=context;
                     service=userIdCourseGrpcService;
                mapper = _mapper;
                
            }
            public async Task<IActionResult> Handle(GetCourseByCourseIdQuerry request, CancellationToken cancellationToken)
            {
                

                var courses = _context.Courses.Include(enroll=>enroll.Enrollments)
                              .Include(course => course.Chapters)
                              .ThenInclude(chapter => chapter.Lessons)
                              .ThenInclude(lesson => lesson.Questions)                         
                              .Where(course => course.Id == request.CourseId).ToList();

                var result = new
                {
                   
                    Courses = courses.Select(course => new
                    {
                        Id = course.Id,
                        Name = course.Name,
                        Description = course.Description,
                        Picture = course.Picture,
                        Tag = course.Tag,
                       
                        Chapters = course.Chapters.Select(chapter => new
                        {
                            Id = chapter.Id,
                            Name = chapter.Name,
                            CourseId = chapter.CourseId,
                            Part = chapter.Part,
                            IsNew = chapter.IsNew,
                            Lessons = chapter.Lessons.Select(lesson => new
                            {
                                Id = lesson.Id,
                                Title = lesson.Title,
                                VideoUrl = lesson.VideoUrl,
                                ChapterId = lesson.ChapterId,
                                Description = lesson.Description,
                                Duration = lesson.Duration,
                                Questions = lesson.Questions.Select(question => new
                                {
                                    Id = question.Id,
                                    VideoId = question.VideoId,
                                    ContentQuestion = question.ContentQuestion,
                                    AnswerA = question.AnswerA,
                                    AnswerB = question.AnswerB,
                                    AnswerC = question.AnswerC,
                                    AnswerD = question.AnswerD,
                                    CorrectAnswer = question.CorrectAnswer,
                                    Time = question.Time
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
